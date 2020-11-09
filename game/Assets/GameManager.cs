using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private long score = 0;

    private const long originalPopulation = 7824082000;
    private const int difficultyMultiplier = 30000000;
    private const int spawnDistance = 25;

    public long Population { get; private set; } = 7824082000;
    public int Level { get; private set; } = 1;
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
        Population = (long)Mathf.Max(0, Population - quantity * difficultyMultiplier);
        UIManager.Instance.SetPopulation(Population, originalPopulation);

        if (Population == 0)
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

            PluginTest.Instance.Log($"level {Level}, difficulty {difficultyMultiplier}");

            yield return new WaitForSeconds(3f / Level);

            Vector3 spawnPoint = Random.insideUnitCircle.normalized * spawnDistance;

            ObjectPooler.Instance.InstantiateFromPool($"Asteroid", spawnPoint, Quaternion.identity);

            Level = 1 + (int)(Time.timeSinceLevelLoad / 20);
        }
    }

    private new void OnDestroy()
    {
        StopAllCoroutines();
        base.OnDestroy();
    }
}
