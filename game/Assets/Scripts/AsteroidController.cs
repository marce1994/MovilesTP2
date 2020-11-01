using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    Vector3[] _pathVectors;
    private int current;
    private Vector3 touchPosition;

    private void Awake()
    {
        _pathVectors = new Vector3[] { };
        CalculatePath();

    }

    private void OnEnable()
    {
        CalculatePath();
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _pathVectors.Length; i++)
            Gizmos.DrawLine(_pathVectors[i], _pathVectors[Mathf.Min(i + 1, _pathVectors.Length -1)]);
        Gizmos.DrawSphere(touchPosition, 1);
    }

    private void FixedUpdate()
    {
        if (transform.position != _pathVectors[current])
        {
            if (current == 0)
                transform.position = _pathVectors[current];

            Vector3 pos = Vector3.MoveTowards(transform.position, _pathVectors[current], 20 * Time.deltaTime);
            transform.position = pos;
        }
        else current = (current + 1) % _pathVectors.Length;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Earth")
        {
            ObjectPooler.Instance.InstantiateFromPool("Explosion", transform.position, Quaternion.identity);
            ObjectPooler.Instance.RecicleGameObject("Asteroid", this.gameObject);
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
            //return (viewportPoint.z > 0(new Rect(0, 0, 1, 1)).Contains(viewportPoint));

            if (inside)
            {
                touchPosition = _pathVectors[i];
                break;
            }
        }
        current = 0;
    }
}
