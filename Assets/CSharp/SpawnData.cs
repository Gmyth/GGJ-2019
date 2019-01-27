using UnityEngine;

public class SpawnData
{
    public readonly Vector3 position;
    public readonly Quaternion rotation;

    private SpawnData() {}

    public SpawnData(Vector3 position)
    {
        this.position = position;
        rotation = Quaternion.identity;
    }

    public SpawnData(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}
