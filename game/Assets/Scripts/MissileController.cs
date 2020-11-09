using UnityEngine;

public class MissileController : MonoBehaviour, IPooledObject
{
    Vector3[] path;
    int current = 0;

    public void OnInstantiate()
    {
        var trailRenderer = gameObject.GetComponent<TrailRenderer>();
        for (int i = 0; i < trailRenderer.positionCount; i++)
            trailRenderer.SetPosition(i, Vector3.zero);
    }

    public void SetPath(Vector3[] path)
    {
        this.path = path;
        current = 0;
    }

    public void Destroy()
    {
        path = null;
        ObjectPooler.Instance.Recicle("Missile", this.gameObject);
    }

    private void Update()
    {
        if (path == null) return;

        if (transform.position != path[current])
        {
            if (current == path.Length - 1)
            {
                this.Destroy();
                return;
            }

            Vector3 relative = transform.InverseTransformPoint(path[current]);
            float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
            gameObject.transform.Rotate(0, 0, -angle);

            Vector3 pos = Vector3.MoveTowards(transform.position, path[current], 20 * Time.deltaTime / current);
            transform.position = pos;
        }
        else current = (current + 1) % path.Length;
    }
}
