using UnityEngine;

public struct ResourceUtility
{
    public static T GetPrefab<T>(string directory) where T : Object
    {
        return Resources.Load<T>("Prefabs/" + directory);
    }

    public static GameObject GetPrefab(string directory)
    {
        return GetPrefab<GameObject>(directory);
    }

    public static T GetUIPrefab<T>(string name) where T : Object
    {
        return Resources.Load<T>("Prefabs/UI/" + name);
    }

    public static Sprite GetCardTemplate(string name)
    {
        return Resources.Load<Sprite>("Sprites/Card/Template/" + name);
    }

    public static Sprite GetCardIllustration(string name)
    {
        return Resources.Load<Sprite>("Sprites/Card/Illustration/" + name);
    }

    public static GameObject GetCardEffect(string name)
    {
        return Resources.Load<GameObject>("Prefabs/CardEffect/" + name);
    }
}
