using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimmingPool : MonoBehaviour {

    [SerializeField] private float reducedSpeed;

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
            //Reduce player's speed
            other.GetComponent<Player>().SetSpeed(reducedSpeed);
            //Slow down the pillow pitch speed

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //Reset player's speed
            other.GetComponent<Player>().ResetSpeed();
        }
    }

}
