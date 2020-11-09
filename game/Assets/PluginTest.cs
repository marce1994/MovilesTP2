using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PluginTest : MonoBehaviour
{
    public GameObject logPrefab;

    const string pluginName = "com.example.logger.Logger";

    public class AlertViewCallback : AndroidJavaProxy
    {
        private readonly Action<int> alertHandler;

        public AlertViewCallback(Action<int> alertHandlerIn) : base(pluginName + "$AlertViewCallback")
        {
            alertHandler = alertHandlerIn;
        }

        public void onButtonTapped(int index)
        {
            //Debug.Log($"Button tapped: { index }");
            alertHandler?.Invoke(index);
        }
    }

    static AndroidJavaClass _pluginClass;
    static AndroidJavaObject _pluginInstance;

    public static AndroidJavaClass PluginClass
    {
        get
        {
            if (_pluginClass == null)
            {
                _pluginClass = new AndroidJavaClass(pluginName);
                AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
                _pluginClass.SetStatic("mainActivity", activity);
            }
            return _pluginClass;
        }
    }

    public static AndroidJavaObject PluginInstance
    {
        get
        {
            if (_pluginInstance == null)
                _pluginInstance = PluginClass.CallStatic<AndroidJavaObject>("getInstance");
            return _pluginInstance;
        }
    }

    private RectTransform contentRectTransform;

    private void Awake()
    {
        var buttons = gameObject.GetComponentsInChildren<Button>();

        buttons.Single(x => x.name == "Return").onClick.AddListener(() =>
        {
            SceneManager.LoadSceneAsync(0); // Esta va a ser la escena menu inicial...
        });

        buttons.Single(x => x.name == "Refresh").onClick.AddListener(() =>
        {
            ShowAllLogs();
        });

        buttons.Single(x => x.name == "Clean").onClick.AddListener(() =>
        {
            showAlertDialog(new string[] { "Alert dialog", "Do you want to clean all logs?", "close", "no", "yes" }, x =>
            {
                if (x == -1)
                    CleanLogs();
            });
        });

        contentRectTransform = GetComponentsInChildren<RectTransform>().Single(x => x.name == "Content");
    }

    private float timer = 0;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5)
        {
            timer -= 5;
            var elapsedTime = getElapsedTime();
            Log($"Ticks: {elapsedTime}");
        }
    }

    double getElapsedTime()
    {
        if (Application.platform == RuntimePlatform.Android)
            return PluginInstance.Call<double>("getElapsedTime");
        Debug.LogWarning($"Wrong platform");
        return 0;
    }

    private void Log(string log)
    {
        if (Application.platform == RuntimePlatform.Android)
            PluginInstance.Call("writeLog", log);
        else
            Debug.LogWarning($"Wrong platform");
    }

    Dictionary<int, Text> logGos = new Dictionary<int, Text>();

    private void CleanLogs()
    {
        foreach (var item in logGos)
        {
            Destroy(item.Value.gameObject);
        }

        logGos = new Dictionary<int, Text>();

        if (Application.platform == RuntimePlatform.Android)
            PluginInstance.Call("deleteLogs");
        else
            Debug.LogWarning("Wrong platform");

        ShowAllLogs();
    }

    public void ShowAllLogs()
    {
        string[] logs = new string[] { };

        if (Application.platform == RuntimePlatform.Android)
            logs = PluginInstance.Call<string[]>("getAllLogs");
        else
            Debug.LogWarning("Wrong platform");

        for (int i = 0; i < logs.Length; i++)
        {
            if (logGos.ContainsKey(i))
            {
                logGos[i].text = logs[i];
            }
            else
            {
                var goinst = Instantiate(logPrefab);
                goinst.transform.SetParent(contentRectTransform);
                var text = goinst.GetComponent<Text>();
                text.text = logs[i];
                logGos.Add(i, text);
            }
        }
    }

    void showAlertDialog(string[] strings, Action<int> handler = null)
    {
        if (strings.Length < 3)
        {
            Debug.LogError("Alert view requires at least 3 strings");
            return;
        }

        if (Application.platform == RuntimePlatform.Android)
            PluginInstance.Call("showAlertView", new object[] { strings, new AlertViewCallback(handler) });
        else
            Debug.LogWarning("AlertView not supported on this platform");
    }
}
