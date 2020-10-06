using UnityEngine;

public class RotatePlanet : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(Vector3.forward, Time.deltaTime);
    }
}
