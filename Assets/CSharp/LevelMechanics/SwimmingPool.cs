using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimmingPool : MonoBehaviour {

    [SerializeField] private float reduceSpeedMultiplier;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Reduce player's speed
            other.transform.parent.GetComponent<Player>().SetSpeed(reduceSpeedMultiplier);
            //Slow down the pillow pitch speed

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //Reset player's speed
            other.transform.parent.GetComponent<Player>().ResetSpeed();
        }
    }

}
