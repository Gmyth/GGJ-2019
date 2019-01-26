using UnityEngine;

public class HUD : UIWindow
{
    [SerializeField] private Transform playerWidgetList;

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
}
