using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadsterRotate : MonoBehaviour
{
    float earthDistance;
    private void Awake()
    {
        earthDistance = transform.position.magnitude;
    }

    void Update()
    {
        transform.position = transform.position.RotatePointAroundPivot(Vector3.zero, Quaternion.Euler(0, -30 * Time.deltaTime, 0));
    }
}
