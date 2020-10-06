using System.Collections.Generic;
using UnityEngine;

public class GravityEntity : MonoBehaviour
{
    const float G = 66.74f;
    public bool isStatic = false;

    public Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Random.insideUnitCircle * 100);
        GravityManager.Instance.AddElement(this);
    }

    public void UpdateGravity(IEnumerable<GravityEntity> entities) {
        foreach (var entity in entities)
        {
            if (entity != this)
                Attract(entity);
        }
    }

    void Attract(GravityEntity gravityEntity)
    {
        if (isStatic) return;

        Rigidbody2D rbToAttract = gravityEntity.rb;
        Vector2 direction = rb.position - rbToAttract.position;

        float forceMagniture = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(direction.magnitude, 2);
        Vector2 force = direction.normalized * forceMagniture;

        rb.AddForce(-force);

        if (transform.position.magnitude > 100)
            rb.AddForce(-transform.position.normalized);
    }
}
