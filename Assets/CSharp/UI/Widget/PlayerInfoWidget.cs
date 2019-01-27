using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoWidget : UIWidget
{
    [SerializeField] private Text playerName;
    [SerializeField] private GameObject readiness;

    private PlayerInfo playerInfo = null;

    public void Initialize(PlayerInfo playerInfo)
    {
        this.playerInfo = playerInfo;

        playerInfo.OnNameChange.AddListener(UpdatePlayerName);
        playerInfo.OnReadinessChange.AddListener(UpdateReadiness);

        UpdateAll();
    }

    private void UpdateAll()
    {
        UpdatePlayerName(playerInfo.Name);
        UpdateReadiness(false);
    }

    private void UpdateReadiness(bool isReady)
    {
        readiness.SetActive(isReady);
    }

    private void UpdatePlayerName(string name)
    {
        playerName.text = name;
    }

    private void OnDestroy()
    {
        if (playerInfo != null)
            playerInfo.OnNameChange.RemoveListener(UpdatePlayerName);
    }
}
