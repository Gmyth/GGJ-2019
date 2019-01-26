﻿using UnityEngine;

/// <summary>
/// The state of the game
/// </summary>
public enum GameState : int
{
    Start = 0,
    MainMenu,
    Match,
    End,
}

/// <summary>
/// A FSM for the whole game at the highest level
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The unique instance
    /// </summary>
    public static GameManager Singleton { get; private set; }

    /// <summary>
    /// An event triggered whenever the state of the game changes
    /// </summary>
    public EventOnDataChange<GameState> onCurrentGameStateChange = new EventOnDataChange<GameState>();

    private GameState currentGameState;

    /// <summary>
    /// The current state of the game
    /// </summary>
    public GameState CurrentGameState
    {
        get
        {
            return currentGameState;
        }

        private set
        {


            // Reset current state
            if (value == currentGameState)
            {
#if UNITY_EDITOR
                LogUtility.PrintLogFormat("GameManager", "Reset {0}.", value);
#endif

                //switch (currentGameState)
                //{
                //}
            }
            else
            {
                // Before leaving the previous state
                //switch (currentGameState)
                //{
                //}

#if UNITY_EDITOR
                LogUtility.PrintLogFormat("GameManager", "Made a transition to {0}.", value);
#endif

                GameState previousGameState = CurrentGameState;
                currentGameState = value;

                // After entering the new state
                //switch (currentGameState)
                //{
                //}

                onCurrentGameStateChange.Invoke(previousGameState, currentGameState);

                switch (currentGameState)
                {
                    case GameState.End:
                        Application.Quit();
                        break;
                }
            }
        }
    }

    private GameManager() {}

    /// <summary>
    /// Quit the game
    /// </summary>
    public void QuitGame()
    {
        CurrentGameState = GameState.End;
    }

    private void Awake()
    {
        if (!Singleton)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (this != Singleton)
            Destroy(gameObject);
   }

    private void Start()
    {
        CurrentGameState = GameState.MainMenu;
    }
}
