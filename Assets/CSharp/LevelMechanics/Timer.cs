using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    public float ElapsedTime = 0f;
    [SerializeField] private float GameLength = 120f;

    // Update is called once per frame
    void Update () {
        ElapsedTime += Time.deltaTime;
        if(Mathf.Round(ElapsedTime) == GameLength)
        {
            //end this game
        }
        if(Mathf.Round(ElapsedTime) == 10f)
        {
            ElapsedTime++;
            LevelEventManager.Instance.TriggerEventWithIndex(0);
        }
	}
}
