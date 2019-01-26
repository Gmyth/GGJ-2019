using System.Collections.Generic;
using UnityEngine;

public class MatchSetup : UIWindow
{
    [SerializeField] private Transform list;
    private PlayerInfoWidget[] playerWidgets;

    private PlayerInfo[] playerInfos;
    private int numPlayers;
    private HashSet<int> isPlayerReady = new HashSet<int>();

    public override void OnOpen(params object[] args)
    {
        int numListItems = list.childCount;

        playerWidgets = new PlayerInfoWidget[numListItems];
        for (int i = 0; i < numListItems; i++)
            playerWidgets[i] = list.GetChild(i).GetComponent<PlayerInfoWidget>();

        playerInfos = (PlayerInfo[])args;
        numPlayers = playerInfos.Length;

        int id = 0;
        while (id < numPlayers)
        {
            ShowPlayerWidget(id);
            UpdatePlayerWidget(id++);
        }

        while (id < 4)
            HidePlayerWidget(id++);

        isSubmitButtonUp = new bool[numPlayers];
    }

    private void ShowPlayerWidget(int id)
    {
        playerWidgets[id].Show();
    }

    private void HidePlayerWidget(int id)
    {
        playerWidgets[id].Hide();
    }

    
    private void UpdatePlayerWidget(int id)
    {
    }

    private void TogglePlayerReadiness(int id)
    {
        playerWidgets[id].ToggleReadiness();

        if (isPlayerReady.Contains(id))
        {
            isPlayerReady.Remove(id);
        }
        else
        {
            isPlayerReady.Add(id);

            if (isPlayerReady.Count == numPlayers)
                GameManager.Singleton.StartNewMatch();
        }
    }

    bool[] isSubmitButtonUp;

    private void Update()
    {
        for (int i = 0; i < numPlayers; i++)
        {
            if (Input.GetAxis("Submit" + playerInfos[i].controllerId) == 0)
                isSubmitButtonUp[i] = true;
            else if (isSubmitButtonUp[i])
            {
                isSubmitButtonUp[i] = false;
                TogglePlayerReadiness(i);
            }
        }
    }
}
