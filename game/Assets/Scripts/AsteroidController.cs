using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AsteroidController : MonoBehaviour, ITouchable, IPooledObject
{
    private int current;
    private float touchMaxTime;
    private Vector3 touchPosition;
    private Vector3 touchPositionWorldSpace;

    private GameObject touchObject;

    public AnimationCurve touchScale;
    public AudioClip ExplosionClip;
    public AudioClip TouchSound; // TODO: Meter en algo como un sound manager o algo asi
    public AudioClip MissileSound; // TODO: Meter en algo como un sound manager o algo asi

    public long AsteroidDamage;
    public int speed = 20;

    public Vector3[] path { get; private set; }

    public Vector3[] reversePath
    {
        get { return path.Reverse().ToArray(); }
    }

    private void Awake()
    {
        touchObject = GetComponentsInChildren<SpriteRenderer>().Single(x => x.name == "Touch").gameObject;
    }

    private void OnDrawGizmos()
    {
        if (path == null) return;
        for (int i = 0; i < path.Length; i++)
            Gizmos.DrawLine(path[i], path[Mathf.Min(i + 1, path.Length - 1)]);
        Gizmos.DrawSphere(touchPosition, 1);
    }

    float touchCurrentTime = 0;

    private void Update()
    {
        if (transform.position != path[current])
        {
            if (current == 0)
                transform.position = path[current];

            Vector3 pos = Vector3.MoveTowards(transform.position, path[current], 20 * Time.deltaTime);
            transform.position = pos;
        }
        else current = (current + 1) % path.Length;

        UpdateTouchObject();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("Missile"))
        {
            path = null;
            var missileController = collision.gameObject.GetComponent<MissileController>();
            missileController.Destroy();
            ObjectPooler.Instance.InstantiateFromPool("Explosion", transform.position, Quaternion.identity);
            ObjectPooler.Instance.Recicle("Asteroid", this.gameObject);
            GameManager.Instance.AddScore((long)(AsteroidDamage * touchScale.Evaluate(touchCurrentTime / touchMaxTime)));
            AudioSource.PlayClipAtPoint(ExplosionClip, Camera.main.transform.position, 0.5f);
        }
        if (collision.name == "Earth")
        {
            path = null;
            Handheld.Vibrate();
            Camera.main.GetComponent<StressReceiver>().InduceStress(1);
            ObjectPooler.Instance.InstantiateFromPool("Explosion", transform.position, Quaternion.identity);
            ObjectPooler.Instance.Recicle("Asteroid", this.gameObject);
            GameManager.Instance.KillPopulation(AsteroidDamage);
            AudioSource.PlayClipAtPoint(ExplosionClip, Camera.main.transform.position, 0.5f);
        }
    }

    private void UpdateTouchObject()
    {
        if (touchCurrentTime > touchMaxTime)
            return;

        touchCurrentTime += Time.deltaTime;

        if (touchCurrentTime > touchMaxTime)
        {
            touchObject.SetActive(false);
        }
        else
        {
            touchObject.transform.position = touchPositionWorldSpace;
            touchObject.transform.localScale = Vector3.one * touchScale.Evaluate(touchCurrentTime / touchMaxTime);
        }
    }

    public void CalculatePath()
    {
        List<Vector3> pathVectors = new List<Vector3>();

        float dist = 25;
        float time = Random.Range(0f, 10f);
        float decreaseDistance = Random.Range(0.4f, 0.6f);
        float increaseTime = Random.Range(0.01f, 0.25f);

        while (dist > 1)
        {
            dist -= decreaseDistance;
            time += increaseTime;
            float x = Mathf.Sin(time) * dist;
            float y = Mathf.Cos(time) * dist;
            pathVectors.Add(new Vector3() { x = x, y = y, z = 0 });
        }

        path = pathVectors.ToArray();
        for (int i = 0; i < pathVectors.Count; i++)
        {
            var cameraPoint = Camera.main.WorldToScreenPoint(path[i]);
            var inside = Camera.main.rect.Contains(cameraPoint);
            if (inside)
            {
                touchPosition = path[i];
                break;
            }
        }

        current = 0;
        gameObject.transform.position = pathVectors[0];

        var suitable_positions = pathVectors.Where(x => x.magnitude > 5 && x.magnitude < 6);
        touchPositionWorldSpace = suitable_positions.ElementAt(Random.Range(0, suitable_positions.Count()));
    }

    public void OnClick()
    {
        if (Time.timeScale <= 0) return;
        touchObject.SetActive(false);
        GameObject missile = ObjectPooler.Instance.InstantiateFromPool("Missile", Vector3.zero, Quaternion.identity);
        MissileController missileController = missile.GetComponent<MissileController>();
        AudioSource.PlayClipAtPoint(TouchSound, Camera.main.transform.position, 0.1f);
        AudioSource.PlayClipAtPoint(MissileSound, Camera.main.transform.position, 0.2f);
        missileController.SetPath(reversePath);
    }

    public void OnInstantiate()
    {
        touchObject.transform.position = Vector3.one * 1000;
        path = new Vector3[] { };
        touchMaxTime = Random.Range(3f, 5f);
        touchObject.SetActive(true);
        touchCurrentTime = 0f;
        CalculatePath();
    }
}
