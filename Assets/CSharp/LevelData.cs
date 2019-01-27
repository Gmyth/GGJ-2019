using UnityEngine;

public class LevelData : MonoBehaviour
{
    [SerializeField] private Transform playerSpawnDatas;
    [SerializeField] private Transform pillowSpawnDatas;

    public Transform PlayerSpawnDatas
    {
        get
        {
            return playerSpawnDatas;
        }
    }

    public Transform PillowSpawnDatas
    {
        get
        {
            return pillowSpawnDatas;
        }
    }
}
