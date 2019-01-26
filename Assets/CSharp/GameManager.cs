using UnityEngine;

/// <summary>
/// The state of the game
/// </summary>
public enum GameState : int
{
    Start = 0,
    MainMenu,
    MatchSetup,
    Match,
    End,
}

/// <summary>
/// A FSM for the whole game at the highest level
/// </summary>
public class GameManager : MonoBehaviour
{
    PlayerInfo[] players;

    /// <summary>
    /// The unique instance
    /// </summary>
    public static GameManager Singleton { get; private set; }

    /// <summary>
    /// An event triggered whenever the state of the game changes
    /// </summary>
    public EventOnDataChange2<GameState> onCurrentGameStateChange = new EventOnDataChange2<GameState>();

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
                    case GameState.MainMenu:
                        UIManager.Singleton.Open("MainMenu");
                        break;

                    case GameState.MatchSetup:
                        int numPlayers = Input.GetJoystickNames().Length;
                        players = new PlayerInfo[numPlayers];
                        for (int id = 0; id < numPlayers; id++)
                            players[id] = new PlayerInfo(id, "Player " + (id + 1));
                        UIManager.Singleton.Open("MatchSetup", UIManager.UIMode.Default, players);
                        break;

                    case GameState.End:
                        Application.Quit();
                        break;
                }
            }
        }
    }

    private GameManager() {}

    public void SetUpNewMatch()
    {
        CurrentGameState = GameState.MatchSetup;
    }

    public void StartNewMatch()
    {

    }

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
