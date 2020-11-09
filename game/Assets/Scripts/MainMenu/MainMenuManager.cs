using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 1f; // TODO: Fixear el bug de que se bugea y se congela todo

        Button[] buttons = GetComponentsInChildren<Button>(includeInactive: true);

        Button play = buttons.Single(x => x.name == "Play");
        Button logs = buttons.Single(x => x.name == "Logs");
        Button settings = buttons.Single(x => x.name == "Settings");
        Button quit = buttons.Single(x => x.name == "Quit");

        play.onClick.AddListener(() =>
        {
            PluginTest.Instance.Log("Play");
            buttons.RemoveListeners();
            SceneManager.LoadSceneAsync("GameScene");
        });

        logs.onClick.AddListener(() =>
        {
            PluginTest.Instance.Log("Logs");
            buttons.RemoveListeners();
            SceneManager.LoadSceneAsync("LogPlugin");
        });

        settings.onClick.AddListener(() =>
        {
            PluginTest.Instance.Log("Settings");
            buttons.RemoveListeners();
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        });

        quit.onClick.AddListener(() =>
        {
            PluginTest.Instance.Log("Quit");
            buttons.RemoveListeners();
            Application.Quit();
        });
    }
}
