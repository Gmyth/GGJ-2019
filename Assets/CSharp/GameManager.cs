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
    private List<PlayerInfo> playerInfos;
    private LevelData levelData;
    private Player[] players;
    private List<Pillow> pillows;

    public long MatchStartTime { get; private set; }

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

                switch (currentGameState)
                {
                    case GameState.Match:
                        ResetPlayers();
                        ResetPillows();
                        MatchStartTime = TimeUtility.localTimeInMilisecond;
                        break;
                }
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

                    case GameState.Match:
                        UIManager.Singleton.Close("HUD");
                        Destroy(levelData);
                        foreach (Player player in players)
                            Destroy(player);
                        foreach (Pillow pillow in pillows)
                            Destroy(pillow);
                        levelData = null;
                        players = null;
                        pillows = null;
                        MatchStartTime = 0;
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
                            LoadLevel("Level");
                            SpawnPlayers();
                            SpawnPillows();

                            UIManager.Singleton.Open("HUD", UIManager.UIMode.Permenent, players);

                            MatchStartTime = TimeUtility.localTimeInMilisecond;
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

    public void QuitMatch()
    {
        CurrentGameState = GameState.MainMenu;
    }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void QuitGame()
    {
        CurrentGameState = GameState.End;
    }

    private void LoadLevel(string name)
    {
        levelData = Instantiate(ResourceUtility.GetPrefab<LevelData>(name), transform);
    }

    private void SpawnPlayers()
    {
        int numPlayers = playerInfos.Count;

        List<Transform> spawnDatas = new List<Transform>();

        for (int i = 0; i < levelData.PlayerSpawnDatas.childCount; i++)
            spawnDatas.Add(levelData.PlayerSpawnDatas.GetChild(i));

        players = new Player[numPlayers];
        Player player;
        Transform spawnData;
        int r;
        for (int id = 0; id < numPlayers; id++)
        {
            Random.InitState(TimeUtility.localTime + id);

            r = Random.Range(0, spawnDatas.Count);
            spawnData = spawnDatas[r];
            spawnDatas.RemoveAt(r);

            player = Instantiate(ResourceUtility.GetPrefab<Player>("Player" + id), spawnData.position, spawnData.rotation, transform);
            player.Initialize(playerInfos[id], spawnData);
            players[id] = player;
        }
    }

    private void ResetPlayers()
    {
        for (int id = 0; id < players.Length; id++)
            players[id].ResetAll();
    }

    private void SpawnPillows()
    {
        pillows = new List<Pillow>();

        Transform spawnDatas = levelData.PillowSpawnDatas.GetChild(0);
        Transform spawnData;
        Pillow pillow;

        for (int i = 0; i < spawnDatas.childCount; i++)
        {
            spawnData = spawnDatas.GetChild(i);
            pillow = Instantiate(ResourceUtility.GetPrefab<Pillow>("Pillow"), spawnData.position, spawnData.rotation, transform);
            pillow.spawnData = spawnData;
            pillows.Add(pillow);
        }

        spawnDatas = levelData.PillowSpawnDatas.GetChild(1);
        
        for (int i = 0; i < spawnDatas.childCount; i++)
        {
            Random.InitState(TimeUtility.localTime + i + 4);
            if (Random.Range(0, 100) < 50)
            {
                spawnData = spawnDatas.GetChild(i);
                pillow = Instantiate(ResourceUtility.GetPrefab<Pillow>("Pillow"), spawnData.position, spawnData.rotation, transform);
                pillow.spawnData = spawnData;
                pillows.Add(pillow);
            }
        }
    }

    private void ResetPillows()
    {
        foreach (Pillow pillow in pillows)
            pillow.ResetAll();
    }

    private void Awake()
    {
        if (!Singleton)
            Singleton = this;
        else if (this != Singleton)
            Destroy(gameObject);
   }

    private void Start()
    {
        CurrentGameState = GameState.MainMenu;
    }
}
