using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int level = 1;
    private int difficultyMultiplier = 1;
    private long population = 7824082000;
    private long originalPopulation = 7824082000;
    private long score = 0;

    public long Population { get { return population; } }

    public int Level { get { return level; } }

    public int DifficultyMultiplier { get { return difficultyMultiplier; } }

    internal void BeginLevel()
    {
        StartCoroutine(InstantiateAsteroid());
    }

    private void Start()
    {
        BeginLevel();
        AddScore(0);
        KillPopulation(0);
    }

    public void AddScore(long score_to_add)
    {
        score += score_to_add;
        UIManager.Instance.UpdateScore(score);
    }

    public void KillPopulation(long quantity)
    {
        population = (long)Mathf.Max(0, population - quantity * 30000000);
        UIManager.Instance.SetPopulation(population, originalPopulation);
        if (population == 0)
        {
            Time.timeScale = 0f;
            UIManager.Instance.ShowGameOverMenu(score);
        }
    }

    private IEnumerator InstantiateAsteroid()
    {
        for (; ; )
        {
            if (Time.timeScale <= 0) continue;

            Debug.Log($"level {level}, difficulty {difficultyMultiplier}");
            yield return new WaitForSeconds(3f / level);
            Vector3 spawnPoint = Random.insideUnitCircle.normalized * 25;
            GameObject asteroid = ObjectPooler.Instance.InstantiateFromPool($"Asteroid", spawnPoint, Quaternion.identity);
            AsteroidController controller = asteroid.GetComponent<AsteroidController>();
            controller.Initialize();
            level = 1 + (int)(Time.timeSinceLevelLoad / 20);
        }
    }

    private new void OnDestroy()
    {
        StopAllCoroutines();
        base.OnDestroy();
    }
}
