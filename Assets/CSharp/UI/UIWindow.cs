using UnityEngine;

public abstract class UIWindow : MonoBehaviour
{
    public virtual void OnOpen(params object[] args) {}
    public virtual void OnClose() {}

    public virtual void Redraw() {}
    public virtual void UpdateAll() {}

    public virtual void Close()
    {
        UIManager.Singleton.Close(GetType().Name);
    }
}
