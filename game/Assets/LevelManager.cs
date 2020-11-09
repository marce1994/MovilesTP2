using System.Collections;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    private int level = 1;
    private int difficultyMultiplier = 1;
    private long population = 7824082000;

    public long Population { get { return population; } }

    public int Level { get { return level; } }

    public int DifficultyMultiplier { get { return difficultyMultiplier; } }

    internal void BeginLevel()
    {
        StartCoroutine(InstantiateAsteroid());
    }
    
    private IEnumerator InstantiateAsteroid()
    {
        for (;;)
        {
            if (Time.timeScale <= 0) continue;

            Debug.Log($"level {level}, difficulty {difficultyMultiplier}");
            yield return new WaitForSeconds(3f / level);
            Vector3 spawnPoint = Random.insideUnitCircle.normalized * 25;
            GameObject asteroid = ObjectPooler.Instance.InstantiateFromPool($"Asteroid", spawnPoint, Quaternion.identity);
            AsteroidController controller = asteroid.GetComponent<AsteroidController>();
            controller.Initialize();
            level = 1 + (int)(Time.timeSinceLevelLoad / 20);
            //level = 1 + (int)(Time.timeSinceLevelLoad / 20);
        }
    }

    private new void OnDestroy()
    {
        StopAllCoroutines();
        base.OnDestroy();
    }
}
