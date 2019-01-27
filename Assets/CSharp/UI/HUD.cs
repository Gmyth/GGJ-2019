using UnityEngine;
using UnityEngine.UI;

public class HUD : UIWindow
{
    [SerializeField] private Transform playerWidgetList;
    [SerializeField] private Text countdown;

    private Player[] players = null;
    private int numPlayers;
    private PlayerWidget[] playerWidgets;

    public override void OnOpen(params object[] args)
    {
        int maxNumPlayers = playerWidgetList.childCount;

        playerWidgets = new PlayerWidget[maxNumPlayers];
        for (int i = 0; i < maxNumPlayers; i++)
            playerWidgets[i] = playerWidgetList.GetChild(i).GetComponent<PlayerWidget>();

        players = (Player[])args;
        numPlayers = players.Length;

        int id = 0;

        while (id < numPlayers)
        {
            playerWidgets[id].Initialize(players[id]);
            playerWidgets[id++].Show();
        }

        while (id < maxNumPlayers)
            playerWidgets[id++].Hide();
    }

    private void Update()
    {
        countdown.text = Mathf.RoundToInt(120 - (TimeUtility.localTimeInMilisecond - GameManager.Singleton.MatchStartTime) / 1000f).ToString();
    }
}
