using UnityEngine;

public class PlayerInfo
{
    public readonly int id;

    private string name;
    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            if (value != name)
            {
                name = value;
                OnNameChange.Invoke(name);
            }
        }
    }

    public EventOnDataChange<string> OnNameChange { get; private set; }

    private PlayerInfo() {}

    public PlayerInfo(int id, string name)
    {
        this.id = id;
        this.name = name;

        OnNameChange = new EventOnDataChange<string>();
    }
}

public class Player : MonoBehaviour
{
    /// <summary>
    /// The name of the player, which is set at the beginning of the game
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// The name of the player, which is set at the beginning of the game
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// The score that has been earned by the player
    /// </summary>
    public int Score { get; private set; }

    /// <summary>
    /// The number of pillows carried by the player
    /// </summary>
    public int Pillow { get; private set; }

    private Player()
    {
        Id = 0;
        Name = "";
        Score = 0;
        Pillow = 1;
    }

    public void Initialize(PlayerInfo playerInfo)
    {
        Id = playerInfo.id;
        Name = playerInfo.Name;
        Score = 0;
        Pillow = 1;
    }
}
