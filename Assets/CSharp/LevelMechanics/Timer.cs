using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    public float ElapsedTime = 0f;

    // Update is called once per frame
    void Update ()
    {
        ElapsedTime += Time.deltaTime;
        
        if (ElapsedTime > 20)
        {
            LevelEventManager.Instance.RollEvent();
            ElapsedTime = 0;
        }
	}
}
