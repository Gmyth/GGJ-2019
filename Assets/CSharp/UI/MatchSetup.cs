using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchSetup : UIWindow
{
    [SerializeField] private Transform playerInfoList;
    [SerializeField] private Transform tooltipList;
    [SerializeField] private Text countdown;

    private PlayerInfoWidget[] playerInfoWidgets;
    private GameObject[] newPlayers;

    private List<PlayerInfo> playerInfos;
    private int numPlayers;
    private int maxNumPlayers;
    private HashSet<int> preparedPlayers = new HashSet<int>();

    private HashSet<string> controllerIds = new HashSet<string>();

    public override void OnOpen(params object[] args)
    {
        int numListItems = playerInfoList.childCount;

        playerInfoWidgets = new PlayerInfoWidget[numListItems];
        for (int i = 0; i < numListItems; i++)
            playerInfoWidgets[i] = playerInfoList.GetChild(i).GetComponent<PlayerInfoWidget>();

        newPlayers = new GameObject[numListItems];
        for (int i = 0; i < numListItems; i++)
            newPlayers[i] = tooltipList.GetChild(i).gameObject;

        playerInfos = (List<PlayerInfo>)args[0];
        numPlayers = 0;

        string[] avaliableJoysticks = Input.GetJoystickNames();
        maxNumPlayers = Mathf.Min(4, avaliableJoysticks.Length + 2);

        controllerIds.Add("_K1");
        controllerIds.Add("_K2");
        for (int id = 0; id < avaliableJoysticks.Length; id++)
        {
            controllerIds.Add("_J" + (id + 1));
        }

        foreach (string controllerId in controllerIds)
        {
            isSubmitButtonUp.Add(controllerId, true);
            isStartButtonUp.Add(controllerId, true);
        }

        for (int id = 0; id < numListItems; id++)
        {
            HidePlayerWidget(id);
            UpdateNewPlayerWidget(id);
        }

        countdown.text = "";
    }

    private void ShowPlayerWidget(int id)
    {
        playerInfoWidgets[id].Show();
    }

    private void HidePlayerWidget(int id)
    {
        playerInfoWidgets[id].Hide();
    }

    private void UpdateNewPlayerWidget(int id)
    {
        newPlayers[id].SetActive(numPlayers < maxNumPlayers && id == numPlayers);
    }

    private void UpdatePlayerWidget(int id)
    {
    }

    private void TogglePlayerReadiness(int id)
    {
        if (preparedPlayers.Contains(id))
        {
            preparedPlayers.Remove(id);
            playerInfos[id].IsReady = false;
            AudioManager.Instance.PlaySoundEffect("Unload");
        }
        else
        {
            preparedPlayers.Add(id);
            playerInfos[id].IsReady = true;
            AudioManager.Instance.PlaySoundEffect("Ready2");

            if (numPlayers > 1 && preparedPlayers.Count == numPlayers)
                countdownStartTime = TimeUtility.localTimeInMilisecond;
        }
    }

    private Dictionary<string, bool> isSubmitButtonUp = new Dictionary<string, bool>(6);
    private Dictionary<string, bool> isStartButtonUp = new Dictionary<string, bool>(6);

    private long countdownStartTime = 0;
    string temp = null;
    private void Update()
    {
        if (!(numPlayers > 1 && preparedPlayers.Count == numPlayers) && countdownStartTime != 0)
        {
            countdownStartTime = 0;
            countdown.text = "";
        }

        if (countdownStartTime > 0)
        {
            long duration = TimeUtility.localTimeInMilisecond - countdownStartTime;
            countdown.text = Mathf.RoundToInt(5 - duration / 1000f).ToString();          
            if(temp != countdown.text)
            {
                if(countdown.text == "0")
                    AudioManager.Instance.PlaySoundEffect("CountDownOver2", false, false);
                else
                    AudioManager.Instance.PlaySoundEffect("CountDown03", false, false);
                temp = countdown.text;                
            }
            if (duration > 5000)
            {
                GameManager.Singleton.StartNewMatch();
                return;
            }
        }

        if (numPlayers < maxNumPlayers)
        {
            foreach (string controllerId in controllerIds)
            {
                if (Input.GetAxis("Start" + controllerId) == 0)
                    isStartButtonUp[controllerId] = true;
                else if (isStartButtonUp[controllerId])
                {
#if UNITY_EDITOR
                    Debug.Log(LogUtility.MakeLogString("MatchSetup", "A new player has jointed the game. (" + controllerId + ")"));
#endif
                    AudioManager.Instance.PlaySoundEffect("Coin");

                    isStartButtonUp[controllerId] = false;
                    PlayerInfo playerInfo = new PlayerInfo(numPlayers, "Player " + (numPlayers + 1), controllerId);
                    playerInfos.Add(playerInfo);

                    playerInfoWidgets[numPlayers].Initialize(playerInfo);
                    ShowPlayerWidget(numPlayers);

                    UpdateNewPlayerWidget(numPlayers++);
                    UpdateNewPlayerWidget(numPlayers);

                    controllerIds.Remove(controllerId);
                    break;
                }
            }
        }
        
        foreach (PlayerInfo playerInfo in playerInfos)
        {
            if (Input.GetAxis("Submit" + playerInfo.controllerId) == 0)
                isSubmitButtonUp[playerInfo.controllerId] = true;
            else if (isSubmitButtonUp[playerInfo.controllerId])
            {
                isSubmitButtonUp[playerInfo.controllerId] = false;
                TogglePlayerReadiness(playerInfo.id);
            }
        }
    }
}
