using System.Collections.Generic;
using UnityEngine;

public class MatchSetup : UIWindow
{
    [SerializeField] private Transform list;
    private UIWidget[] playerWidgets;

    private PlayerInfo[] players;
    private int numPlayers;
    private HashSet<int> isPlayerReady = new HashSet<int>();

    public override void OnOpen(params object[] args)
    {
        players = (PlayerInfo[])args;
        numPlayers = players.Length;

        int id = 0;
        while (id < numPlayers)
        {
            ShowPlayerWidget(id);
            UpdatePlayerWidget(id++);
        }

        while (id < 4)
            HidePlayerWidget(id);

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

    private void Start()
    {
        int numListItems = list.childCount;

        playerWidgets = new UIWidget[numListItems];
        for (int i = 0; i < numListItems; i++)
            playerWidgets[i] = list.GetChild(i).GetComponent<UIWidget>();
    }

    bool[] isSubmitButtonUp;

    private void Update()
    {
        for (int i = 0; i < numPlayers; i++)
        {
            if (Input.GetAxis("Submit" + i) == 0)
                isSubmitButtonUp[i] = true;
            else if (isSubmitButtonUp[i])
            {
                isSubmitButtonUp[i] = false;

                TogglePlayerReadiness(i);
            }
        }
    }
}
