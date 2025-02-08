using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    public static GamePauseUI Instance { get; private set; }

    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button menuBtn;
    [SerializeField] private Button OptionBtn;
    private void Awake()
    {
        Instance = this;

        resumeBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.PauseGame();
        });

        menuBtn.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        OptionBtn.onClick.AddListener(() =>
        {
            OptionUI.Instance.Show();
        });

        
    }
    private void Start()
    {
        GameManager.Instance.OnPause += Instance_OnPause;
        GameManager.Instance.OnUnPause += Instance_OnUnPause;
        Hide();
    }

    private void Instance_OnUnPause(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void Instance_OnPause(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
