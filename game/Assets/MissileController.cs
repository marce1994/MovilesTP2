using UnityEngine;

public class MissileController : MonoBehaviour
{
    Vector3[] path;
    int current = 0;
    TrailRenderer trailRenderer;

    private void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    void ResetTrail()
    {
        //for (int i = 0; i < trailRenderer.positionCount; i++)
        //    trailRenderer.SetPosition(i, Vector3.zero);
    }

    public void SetPath(Vector3[] path)
    {
        ResetTrail();
        this.path = path;
        current = 0;
    }

    public void Destroy()
    {
        path = null;
        ObjectPooler.Instance.RecicleGameObject("Missile", this.gameObject);
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
