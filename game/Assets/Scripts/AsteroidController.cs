using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AsteroidController : MonoBehaviour, ITouchable
{
    private int current;
    private float touchMaxTime;
    
    private Vector3[] _pathVectors;
    private Vector3 touchPosition;
    private Vector3 touchPositionWorldSpace;
    
    private GameObject touchObject;
    
    public AnimationCurve touchScale;
    public AudioClip ExplosionClip;
    
    public long AsteroidDamage;

    public Vector3[] path
    {
        get { return _pathVectors; }
    }

    public Vector3[] reversePath
    {
        get { return _pathVectors.Reverse().ToArray(); }
    }

    private void Awake()
    {
        touchObject = GetComponentsInChildren<SpriteRenderer>().Single(x => x.name == "Touch").gameObject;
        touchCurrentTime = 0f;
        Initialize();
    }

    public void Initialize()
    {
        touchObject.transform.position = Vector3.one * 1000;
        _pathVectors = new Vector3[] { };
        touchMaxTime = Random.Range(3f, 5f);
        touchObject.SetActive(true);
        touchCurrentTime = 0f;
        CalculatePath();
    }

    private void OnDrawGizmos()
    {
        if (_pathVectors == null) return;
        for (int i = 0; i < _pathVectors.Length; i++)
            Gizmos.DrawLine(_pathVectors[i], _pathVectors[Mathf.Min(i + 1, _pathVectors.Length - 1)]);
        Gizmos.DrawSphere(touchPosition, 1);
    }

    float touchCurrentTime = 0;
    private void Update()
    {
        if (transform.position != _pathVectors[current])
        {
            if (current == 0)
                transform.position = _pathVectors[current];

            Vector3 pos = Vector3.MoveTowards(transform.position, _pathVectors[current], 20 * Time.deltaTime);
            transform.position = pos;
        }
        else current = (current + 1) % _pathVectors.Length;


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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print($"collision {collision.name}");
        if (collision.name.Contains("Missile"))
        {
            _pathVectors = null;
            var missileController = collision.gameObject.GetComponent<MissileController>();
            missileController.Destroy();
            ObjectPooler.Instance.InstantiateFromPool("Explosion", transform.position, Quaternion.identity);
            ObjectPooler.Instance.RecicleGameObject("Asteroid", this.gameObject);
            AudioSource.PlayClipAtPoint(ExplosionClip, Camera.main.transform.position, 0.5f);
            switch (gameObject.name)
            {
                case "Asteroid 4":
                    GameManager.Instance.AddScore((long)(AsteroidDamage * touchScale.Evaluate(touchCurrentTime / touchMaxTime)));
                    break;
                case "Asteroid 3":
                    GameManager.Instance.AddScore((long)(AsteroidDamage * touchScale.Evaluate(touchCurrentTime / touchMaxTime)));
                    break;
                case "Asteroid 5":
                    GameManager.Instance.AddScore((long)(AsteroidDamage * touchScale.Evaluate(touchCurrentTime / touchMaxTime)));
                    break;
                default:
                    GameManager.Instance.AddScore((long)(AsteroidDamage * touchScale.Evaluate(touchCurrentTime / touchMaxTime)));
                    break;
            }
        }
        if (collision.name == "Earth")
        {
            _pathVectors = null;
            Handheld.Vibrate();
            Camera.main.GetComponent<StressReceiver>().InduceStress(1);
            ObjectPooler.Instance.InstantiateFromPool("Explosion", transform.position, Quaternion.identity);
            ObjectPooler.Instance.RecicleGameObject("Asteroid", this.gameObject);
            AudioSource.PlayClipAtPoint(ExplosionClip, Camera.main.transform.position, 0.5f);
            
            switch (gameObject.name)
            {
                case "Asteroid 4":
                    GameManager.Instance.KillPopulation(AsteroidDamage);
                    break;
                case "Asteroid 3":
                    GameManager.Instance.KillPopulation(AsteroidDamage);
                    break;
                case "Asteroid 5":
                    GameManager.Instance.KillPopulation(AsteroidDamage);
                    break;
                default:
                    GameManager.Instance.KillPopulation(AsteroidDamage);
                    break;
            }
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

        _pathVectors = pathVectors.ToArray();
        for (int i = 0; i < pathVectors.Count; i++)
        {
            var cameraPoint = Camera.main.WorldToScreenPoint(_pathVectors[i]);
            var inside = Camera.main.rect.Contains(cameraPoint);
            if (inside)
            {
                touchPosition = _pathVectors[i];
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
        var missile = ObjectPooler.Instance.InstantiateFromPool("Missile", Vector3.zero, Quaternion.identity);
        var missileController = missile.GetComponent<MissileController>();
        missileController.SetPath(reversePath);
    }
}
