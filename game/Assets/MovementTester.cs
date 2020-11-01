using UnityEngine;

public class MovementTester : MonoBehaviour
{
    public GameObject asteroidPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
        {
            Vector3 spawnPos = Random.insideUnitSphere.normalized * 50;
            spawnPos.z = 0;
            ObjectPooler.Instance.InstantiateFromPool("Asteroid", spawnPos, Quaternion.identity);
        }

        var multiplier = 1;
        if (Input.GetKey(KeyCode.LeftShift))
            multiplier = 10;
        if (Input.GetKey(KeyCode.W))
            transform.position += Vector3.up * Time.deltaTime*multiplier;
        if (Input.GetKey(KeyCode.A))
            transform.position += Vector3.left * Time.deltaTime* multiplier;
        if (Input.GetKey(KeyCode.S))
            transform.position += Vector3.down * Time.deltaTime * multiplier;
        if (Input.GetKey(KeyCode.D))
            transform.position += Vector3.right * Time.deltaTime * multiplier;
    }
}
