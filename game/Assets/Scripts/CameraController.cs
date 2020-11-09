using System.Collections;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    private Vector3 originalPosition;
    private Vector3 randomNewPosition;

    private void Awake()
    {
        originalPosition = transform.position;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            randomNewPosition = Random.insideUnitCircle* magnitude;
            randomNewPosition.z = originalPosition.z;

            transform.position = randomNewPosition;
            
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPosition;
    }
}
