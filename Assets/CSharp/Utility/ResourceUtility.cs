using UnityEngine;

public struct ResourceUtility
{
    public static GameObject GetPrefab(string directory)
    {
        return GetPrefab<GameObject>(directory);
    }

    public static T GetPrefab<T>(string directory) where T : Object
    {
        return Resources.Load<T>("Prefabs/" + directory);
    }

    public static T GetUIPrefab<T>(string name) where T : Object
    {
        return Resources.Load<T>("Prefabs/UI/" + name);
    }

    public static Sprite GetSprite(string name)
    {
        return Resources.Load<Sprite>("Sprites/" + name);
    }
}
