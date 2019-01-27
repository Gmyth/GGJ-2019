using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEvent : MonoBehaviour {

    public string eventName;

    [SerializeField] protected float duration;

    virtual public void OnStart()
    {
        Debug.Log("Event: " + eventName + " Start!!!");
        StartCoroutine("CountDown");
    }

    virtual public void OnEnd()
    {
        Debug.Log("Event: " + eventName + " End...");
    }

    protected IEnumerator CountDown()
    {
        float t = duration;
        while (t > 0)
        {
            t -= 1;
            yield return new WaitForSecondsRealtime(1);
        }
        OnEnd();
    }

}
