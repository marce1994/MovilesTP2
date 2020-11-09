using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    private Image hpBar;
    private Text hpBarText;
    private Button[] buttons;
    private Text scoreText;

    private Text gameOverText;

    GameObject pauseMenu;
    GameObject gameOverMenu;
    GameObject tutorialMenu;

    public void SetPopulation(long count, long initialPopulation)
    {
        hpBarText.text = $"Pupulation: {count}";
        hpBar.fillAmount = (float)count / (float)initialPopulation;
    }

    IEnumerator PutTutorialPause()
    {
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0f;
    }

    private void Awake()
    {
        var images = GetComponentsInChildren<Image>();
        Text[] text = gameObject.GetComponentsInChildren<Text>(includeInactive: true);

        gameOverText = text.Single(x => x.name == "GameOverScore");
        scoreText = text.Single(x => x.name == "Score");
        hpBarText = text.Single(x => x.name == "PopulationText");
        hpBar = images.Single(x => x.name == "HpBar");

        buttons = GetComponentsInChildren<Button>(includeInactive: true);

        pauseMenu = gameObject.GetComponentsInChildren<Image>(includeInactive: true).Single(x => x.name == "PauseMenu").gameObject;
        gameOverMenu = gameObject.GetComponentsInChildren<Image>(includeInactive: true).Single(x => x.name == "GameOverMenu").gameObject;
        tutorialMenu = gameObject.GetComponentsInChildren<Image>(includeInactive: true).Single(x => x.name == "Tutorial").gameObject;

        Button play = buttons.Single(x => x.name == "Play");
        Button pause = buttons.Single(x => x.name == "Pause");
        Button mainMenu = buttons.Single(x => x.name == "MainMenu");
        Button settings = buttons.Single(x => x.name == "Settings");
        Button continue_play = buttons.Single(x => x.name == "Continue");

        Button tutorial_ok = buttons.Single(x => x.name == "Ok");
        Button game_over_main_menu = buttons.Single(x => x.name == "GOMainMenu");
        Button game_over_new_game = buttons.Single(x => x.name == "NewGame");

        tutorial_ok.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            tutorialMenu.SetActive(false);
        });

        game_over_new_game.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            RemoveListeners();
            SceneManager.LoadSceneAsync("GameScene");
        });

        game_over_main_menu.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            RemoveListeners();
            SceneManager.LoadSceneAsync("MainMenu");
        });

        mainMenu.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            RemoveListeners();
            SceneManager.LoadSceneAsync("MainMenu");
        });

        settings.onClick.AddListener(() =>
        {
            throw new NotImplementedException(); // TODO: Hacer un menu de configuraciones.
        });

        continue_play.onClick.AddListener(() =>
        {
            pauseMenu.SetActive(false);
            play.gameObject.SetActive(true);
            pause.gameObject.SetActive(false);
            Time.timeScale = 1f;
        });

        play.onClick.AddListener(() =>
        {
            pauseMenu.SetActive(true);
            pause.gameObject.SetActive(true);
            play.gameObject.SetActive(false);
            Time.timeScale = 0f;
        });

        pause.onClick.AddListener(() =>
        {
            pauseMenu.SetActive(false);
            play.gameObject.SetActive(true);
            pause.gameObject.SetActive(false);
            Time.timeScale = 1f;
        });

        StartCoroutine(PutTutorialPause()); // TODO: Cambiar por algo no feo
    }

    void RemoveListeners()
    {
        foreach (Button button in buttons)
            button.onClick.RemoveAllListeners();
    }

    internal void ShowGameOverMenu(long score)
    {
        gameOverMenu.SetActive(true);
        string hightScoreString = PlayerPrefs.GetString("HightScore");
        long hightScore;
        long.TryParse(hightScoreString, out hightScore);

        if (hightScore < score)
        {
            PlayerPrefs.SetString("HightScore", $"{ score }");
            hightScore = score;
        }

        gameOverText.text = $"Score: { score } \n Hight Score: { hightScore }";
    }

    internal void UpdateScore(long score)
    {
        scoreText.text = $"Score: {score}";
    }

    private new void OnDestroy()
    {
        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }

        base.OnDestroy();
    }
}