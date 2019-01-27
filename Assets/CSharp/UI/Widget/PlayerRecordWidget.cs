using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRecordWidget : UIWidget
{
    private static readonly Color[] colors = new Color[4] { new Color(1.0f, 1.0f, 1.0f),
                                                            new Color(0.8f, 0.4f, 0.2f),
                                                            new Color(0.6f, 0.6f, 0.6f),
                                                            new Color(1.0f, 0.8f, 0.2f)};

    [SerializeField] private Image playerPortrait;
    [SerializeField] private Text playerName;
    [SerializeField] private Text playerScore;

    public void UpdatePlayerRecord(PlayerRecord playerRecord, int rank)
    {
        playerPortrait.sprite = ResourceUtility.GetSprite("player" + playerRecord.playerId);
        playerName.text = playerRecord.playerName;
        playerScore.text = playerRecord.playerScore.ToString();

        UpdateStyle(rank);
    }

    public override void Hide()
    {
        GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        base.Hide();
    }

    private void UpdateStyle(int rank)
    {
        int r = Mathf.Max(3, 2 + rank);
        float size = 32 * r;

        GetComponent<RectTransform>().sizeDelta = new Vector2(0, size);
        playerPortrait.GetComponent<RectTransform>().sizeDelta = Vector2.one * size;

        playerName.fontSize = 16 * (r + 1);
        playerScore.fontSize = 16 * (r + 1);

        playerName.color = colors[rank];
        playerScore.color = colors[rank];
    }
}
