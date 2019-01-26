using UnityEngine;

public abstract class UIWidget : MonoBehaviour
{
    public virtual void Refresh(params object[] args) { }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
