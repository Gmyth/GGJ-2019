using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public readonly int id;
    public string name;

    private PlayerInfo() {}

    public PlayerInfo(int id, string name)
    {
        this.id = id;
        this.name = name;
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
        Name = playerInfo.name;
        Score = 0;
        Pillow = 1;
    }
}
