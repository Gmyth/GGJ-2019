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

        isStartButtonUp = new bool[numPlayers];

        GameManager.Singleton.OnMatchTimeLeftChange.AddListener(UpdateCountdown);
    }

    private void UpdateCountdown(float matchTimeLeft)
    {
        countdown.text = Mathf.RoundToInt(matchTimeLeft).ToString();
    }

    private bool[] isStartButtonUp;

    private void Update()
    {
        if (!UIManager.Singleton.IsInViewport("InMatchMenu"))
            for (int id = 0; id < players.Length; id++)
                if (Input.GetAxis("Start" + players[id].ControllerId) == 0)
                    isStartButtonUp[id] = true;
                else if (isStartButtonUp[id])
                {
                    isStartButtonUp[id] = false;
                    UIManager.Singleton.Open("InMatchMenu", UIManager.UIMode.Default, players[id].ControllerId);
                }
    }
}
