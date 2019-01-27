using System.Collections.Generic;
using UnityEngine;

public class MatchResult : UIWindow
{
    [SerializeField] private Transform list;

    private PlayerRecordWidget[] playerRecordWidgets;

    public override void OnOpen(params object[] args)
    {
        int maxNumRecords = list.childCount;
        playerRecordWidgets = new PlayerRecordWidget[maxNumRecords];

        for (int i = 0; i < maxNumRecords; i++)
            playerRecordWidgets[i] = list.GetChild(i).GetComponent<PlayerRecordWidget>();

        SortedList<int, PlayerRecord> playerRecords = (SortedList<int, PlayerRecord>)args[0];
        Stack<PlayerRecord> s = new Stack<PlayerRecord>();

        foreach (KeyValuePair<int, PlayerRecord> pair in playerRecords)
            s.Push(pair.Value);

        PlayerRecord playerRecord;
        int previousScore = int.MaxValue;
        int rank = 4;
        int index = 0;

        while (s.Count > 0)
        {
            playerRecord = s.Pop();

            if (rank > 0 && playerRecord.playerScore != previousScore)
                rank--;

            previousScore = playerRecord.playerScore;

            playerRecordWidgets[index].UpdatePlayerRecord(playerRecord, rank);
            playerRecordWidgets[index++].Show();
        }

        while (index < maxNumRecords)
            playerRecordWidgets[index++].Hide();
    }
}
