using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("enter audio test");
        AudioManager.Instance.PlayEffect("Cow Marching and Mourn", false);
        AudioManager.Instance.StopEffect("Cow Marching and Mourn", true);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
