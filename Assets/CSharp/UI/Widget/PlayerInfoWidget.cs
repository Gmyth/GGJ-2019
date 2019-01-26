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

        UpdatePlayerName(playerInfo.Name);
        readiness.SetActive(false);
    }

    public void ToggleReadiness()
    {
        readiness.SetActive(!readiness.activeSelf);
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
