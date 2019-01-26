using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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
        Name = "";
        Score = 0;
        Pillow = 1;
    }
}
