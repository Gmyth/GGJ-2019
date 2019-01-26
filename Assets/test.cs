using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("enter audio test");
        //AudioManager.Instance.PlaySoundEffect("ThrowPillow", false);

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("1"))
        {
            Debug.Log("enter 1");
            AudioManager.Instance.PlaySoundEffect("ThrowPillow", false);
        }
        else if (Input.GetKeyDown("2"))
            AudioManager.Instance.PlaySoundEffect("Cow Marching and Mourn", true);
        else if (Input.GetKeyDown("3"))
            AudioManager.Instance.StopSoundEffect("Cow Marching and Mourn", true);
    }
}
