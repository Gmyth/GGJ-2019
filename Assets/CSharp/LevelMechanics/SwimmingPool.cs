using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimmingPool : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            //Reduce the player's speed
            //Slow down the pillow pitch speed
        }
    }

}
