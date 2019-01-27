using System.Collections.Generic;
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
    List<PlayerInfo> playerInfos;
    Player[] players;

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
                switch (currentGameState)
                {
                    case GameState.MainMenu:
                        UIManager.Singleton.Close("MainMenu");
                        break;

                    case GameState.MatchSetup:
                        UIManager.Singleton.Close("MatchSetup");
                        break;
                }

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
                        {
                            playerInfos = new List<PlayerInfo>();

                            UIManager.Singleton.Open("MatchSetup", UIManager.UIMode.Default, playerInfos);
                        }
                        break;

                    case GameState.Match:
                        {
                            int numPlayers = playerInfos.Count;

                            List<SpawnData> spawnDatas = new List<SpawnData>();
                            spawnDatas.Add(new SpawnData(new Vector3(3, 2, 0), Quaternion.Euler(0, 90, 0)));
                            spawnDatas.Add(new SpawnData(new Vector3(10, 2, -7), Quaternion.Euler(0, 45, 0)));
                            spawnDatas.Add(new SpawnData(new Vector3(20, 2, -16), Quaternion.Euler(0, 45, 0)));
                            spawnDatas.Add(new SpawnData(new Vector3(25, 2, -24), Quaternion.Euler(0, 0, 0)));

                            players = new Player[numPlayers];
                            Player player;
                            SpawnData spawnData;
                            int i;
                            for (int id = 0; id < numPlayers; id++)
                            {
                                Random.InitState(TimeUtility.localTime + id);
                                
                                i = Random.Range(0, spawnDatas.Count);
                                spawnData = spawnDatas[i];
                                spawnDatas.RemoveAt(i);
                                Debug.Log(i);
                                player = Instantiate(ResourceUtility.GetPrefab<Player>("Player" + id), spawnData.position, spawnData.rotation, transform);
                                player.Initialize(playerInfos[id]);
                                players[id] = player;
                            }

                            Instantiate(ResourceUtility.GetPrefab<GameObject>("Level"), transform);

                            UIManager.Singleton.Open("HUD", UIManager.UIMode.Permenent, players);
                        }
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
        CurrentGameState = GameState.Match;
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
