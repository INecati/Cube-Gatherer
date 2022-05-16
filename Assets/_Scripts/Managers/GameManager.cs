using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : StaticInstance<GameManager>
{

    public static event Action OnGameSetup;
    public static event Action OnGameStarted;
    public static event Action OnGameEnded;
    public static event Action<bool> OnGamePaused;
    public bool IsGamePaused { get; private set; }
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        IsGamePaused = false;
    }
    private void Start()
    {
        StartGame();
    }
    public void StartGame()
    {
        Debug.Log("Game Started");
        OnGameSetup?.Invoke();
        OnGameStarted?.Invoke();
    }
    public void EndGame()
    {
        Debug.Log("Game Ended");
        OnGameEnded?.Invoke();
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        IsGamePaused = true;
        OnGamePaused?.Invoke(IsGamePaused);
    }
    public void UnPauseGame()
    {
        Time.timeScale = 1;
        IsGamePaused = false;
        OnGamePaused?.Invoke(IsGamePaused);
    }
    
}
