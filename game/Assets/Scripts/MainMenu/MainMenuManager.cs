using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 1f; // TODO: Fixear el bug de que se bugea y se congela todo

        Button[] buttons = GetComponentsInChildren<Button>();

        Button play = buttons.Single(x => x.name == "Play");
        Button logs = buttons.Single(x => x.name == "Logs");
        Button settings = buttons.Single(x => x.name == "Settings");
        Button quit = buttons.Single(x => x.name == "Quit");

        play.onClick.AddListener(() =>
        {
            Debug.Log("Play");
            RemoveListeners(buttons);
            SceneManager.LoadSceneAsync("GameScene");
        });
        logs.onClick.AddListener(() =>
        {
            Debug.Log("Logs");
            RemoveListeners(buttons);
            SceneManager.LoadSceneAsync("LogPlugin");
        });
        settings.onClick.AddListener(() =>
        {
            Debug.Log("Settings");
            RemoveListeners(buttons);
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        });
        quit.onClick.AddListener(() =>
        {
            Debug.Log("Quit");
            RemoveListeners(buttons);
            Application.Quit();
        });
    }

    private void RemoveListeners(Button[] buttons)
    {
        foreach (Button button in buttons)
            button.onClick.RemoveAllListeners();
    }
}
