using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEventManager : MonoBehaviour {

    [SerializeField] private LevelEvent[] events;

    [SerializeField] private float rollTimePeriod;
    
	// Use this for initialization
	void Start () {
        TriggerEventWithName("Migrate");
        TriggerEventWithIndex(0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

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
