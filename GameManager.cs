using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public event EventHandler OnStateChange;
    public event EventHandler OnPause;
    public event EventHandler OnUnPause;
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private State state;
    
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer;        
    private float gamePlayingTimerMaxThisGame;
    private bool isGamepause = false;

    private void Awake()
    {
        Instance = this;
        state = new State();
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += Instance_OnPauseAction;
        GameInput.Instance.OnInteractAction += Instance_OnInteractAction;
        float gamePlayingTimerMax = PlayerPrefs.GetFloat("gamePlayingTimerMax");
        if (gamePlayingTimerMax == 0)
        {
            gamePlayingTimerMax = 30;
        }
        else
        {
            gamePlayingTimerMax = PlayerPrefs.GetFloat("gamePlayingTimerMax");
        }
        gamePlayingTimerMaxThisGame = gamePlayingTimerMax;
    }

    private void Instance_OnInteractAction(GameInput arg0, EventArgs arg1)
    {
        if (state == State.WaitingToStart)
        {
            state = State.CountdownToStart;
            OnStateChange?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Instance_OnPauseAction(object sender, EventArgs e)
    {
        PauseGame();
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
               
                break;
            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f)
                {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMaxThisGame;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f)
                {
                    state = State.GameOver;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
    }
    public bool IsPlayGame()
    {
        return state == State.GamePlaying;
    }
    public bool IsCountdownToStartActive()
    {
        return state == State.CountdownToStart;
    }
    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }
    public bool IsGameOver()
    {
        return state == State.GameOver;
    }
    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMaxThisGame);
    }
    public float GetGamePlayingTimerMax()
    {
        return gamePlayingTimerMaxThisGame;
    }
    public void SetGamePlayingTimerMax(float nextGameTimer)
    {
        PlayerPrefs.SetFloat("gamePlayingTimerMax", nextGameTimer);
    }

    public  void PauseGame()
    {
        isGamepause = !isGamepause;
        if (!isGamepause)
        {
            Time.timeScale = 0f;
            OnPause?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnUnPause?.Invoke(this, EventArgs.Empty);
        }
        
    }
}
