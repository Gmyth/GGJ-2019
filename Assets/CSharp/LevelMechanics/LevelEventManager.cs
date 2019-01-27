using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEventManager: MonoBehaviour
{

    [SerializeField] private LevelEvent[] events;

    [SerializeField] private float rollTimePeriod;

    public static LevelEventManager Instance = null;

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    // Use this for initialization
    //   void Start () {

    //       //TriggerEventWithName("Migrate");
    //       //TriggerEventWithIndex(0);
    //}

    // Update is called once per frame

    private IEnumerator rollEvent()
    {
        while (true)
        {
            //Roll a random event
            yield return new WaitForSeconds(rollTimePeriod);
            
        }
    }

    public void TriggerEventWithName(string name)
    {
        foreach (LevelEvent e in events)
        {
            if (e.name == name)
            {
                e.OnStart();
            }
        }
    }

    public void TriggerEventWithIndex(int index)
    {
        events[index].OnStart();
    }

}
