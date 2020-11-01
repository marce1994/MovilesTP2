using System.Collections;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    private int level = 1;
    private int difficultyMultiplier = 1;

    private int population;

    public int Population { get { return population; } }

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
            Debug.Log($"level {level}, difficulty {difficultyMultiplier}");
            yield return new WaitForSeconds(3 / level);
            Vector3 spawnPoint = Random.insideUnitCircle.normalized * 25;
            ObjectPooler.Instance.InstantiateFromPool($"Asteroid", spawnPoint, Quaternion.identity);
        }
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        StopAllCoroutines();
    }
}
