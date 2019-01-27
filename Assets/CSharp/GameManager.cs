using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The state of the game
/// </summary>
public enum GameState : int
{
    Start = 0,
    MainMenu,
    GameGuide,
    MatchSetup,
    Match,
    MatchResult,
    End,
}

/// <summary>
/// A FSM for the whole game at the highest level
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private float matchDuration = 120f;

    private List<PlayerInfo> playerInfos;
    private LevelData levelData;
    private Player[] players;
    private List<Pillow> pillows;
    private SortedList<int, PlayerRecord> lastMatchResult;

    private float matchTimeLeft;
    public float MatchTimeLeft
    {
        get
        {
            return matchTimeLeft;
        }

        private set
        {
            if (value != matchTimeLeft)
            {
                matchTimeLeft = value;
                OnMatchTimeLeftChange.Invoke(matchTimeLeft);
            }
        }
    }

    /// <summary>
    /// The unique instance
    /// </summary>
    public static GameManager Singleton { get; private set; }

    /// <summary>
    /// An event triggered whenever the state of the game changes
    /// </summary>
    public EventOnDataChange2<GameState> OnCurrentGameStateChange { get; private set; }
    public EventOnDataChange<float> OnMatchTimeLeftChange { get; private set; }

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
                        MatchTimeLeft = matchDuration;
                        break;
                }

                OnCurrentGameStateChange.Invoke(currentGameState, currentGameState);
            }
            else
            {
                // Before leaving the previous state
                switch (currentGameState)
                {
                    case GameState.MainMenu:
                        UIManager.Singleton.Close("MainMenu");
                        break;

                    case GameState.GameGuide:
                        UIManager.Singleton.Close("GameGuide");
                        break;

                    case GameState.MatchSetup:
                        UIManager.Singleton.Close("MatchSetup");
                        break;

                    case GameState.Match:
                        UIManager.Singleton.Close("HUD");
                        lastMatchResult = new SortedList<int, PlayerRecord>();
                        Destroy(levelData.gameObject);
                        foreach (Player player in players)
                        {
                            lastMatchResult.Add(player.Score * 10000 + player.Id, new PlayerRecord(player));
                            Destroy(player.gameObject);
                        }
                        foreach (Pillow pillow in pillows)
                            Destroy(pillow.gameObject);
                        levelData = null;
                        players = null;
                        pillows = null;
                        MatchTimeLeft = 0;
                        break;

                    case GameState.MatchResult:
                        UIManager.Singleton.Close("MatchResult");
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

                OnCurrentGameStateChange.Invoke(previousGameState, currentGameState);

                switch (currentGameState)
                {
                    case GameState.MainMenu:
                        playerInfos = null;
                        UIManager.Singleton.Open("MainMenu");
                        AudioManager.Instance.PlayBGM();
                        UIManager.Singleton.Open("MainMenu", UIManager.UIMode.Default, previousGameState == GameState.GameGuide ? 1 : 0);
                        break;

                    case GameState.GameGuide:
                        UIManager.Singleton.Open("GameGuide");
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

                            MatchTimeLeft = matchDuration;
                            StartCoroutine(MatchCountdown());
                        }
                        break;

                    case GameState.MatchResult:
                        UIManager.Singleton.Open("MatchResult", UIManager.UIMode.Default, lastMatchResult);
                        break;

                    case GameState.End:
                        Application.Quit();
                        break;
                }
            }
        }
    }

    private GameManager() {}

    public void ShowGameGuide()
    {
        if (currentGameState == GameState.MainMenu)
            CurrentGameState = GameState.GameGuide;
    }

    public void SetUpNewMatch()
    {
        if (currentGameState == GameState.MainMenu)
            CurrentGameState = GameState.MatchSetup;
    }

    public void StartNewMatch()
    {
        CurrentGameState = GameState.Match;
    }

    public void ReturnToMainMenu()
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

    private IEnumerator MatchCountdown()
    {
        while (MatchTimeLeft > 0)
        {
            yield return null;
            MatchTimeLeft -= Time.deltaTime;
        }

        MatchTimeLeft = 0;

        if (CurrentGameState == GameState.Match)
            CurrentGameState = GameState.MatchResult;

        yield break;
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
        OnCurrentGameStateChange = new EventOnDataChange2<GameState>();
        OnMatchTimeLeftChange = new EventOnDataChange<float>();

        CurrentGameState = GameState.MainMenu;
    }
}
