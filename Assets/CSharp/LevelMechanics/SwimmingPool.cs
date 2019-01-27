using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimmingPool : MonoBehaviour {

    [SerializeField] private float reduceSpeedMultiplier;
    private bool SoundActivate = false;
    private bool isPlaying = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (SoundActivate == true)
        {
            if (!isPlaying)
            {
                AudioManager.Instance.PlaySoundEffect("WalkingInPool", true, false);
                isPlaying = true;
            }
        }
        else
        {
            AudioManager.Instance.StopSoundEffect("WalkingInPool", true);
            isPlaying = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            AudioManager.Instance.PlaySoundEffect("EnterWater");
            SoundActivate = true;           
            //Reduce player's speed
            other.GetComponent<Player>().SetSpeed(reduceSpeedMultiplier);
            //Slow down the pillow pitch speed

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            SoundActivate = false;
            //Reset player's speed
            other.GetComponent<Player>().ResetSpeed();
        }
    }

}
