using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PillowState : int
{
    Idle = 0,
    Picked,
    Aimed,
    Throwed,
}
public class Pillow : MonoBehaviour {
    public PillowState currentState;// if the pillow is throwed, then do damage
    private GameObject holder;
	// Use this for initialization
	void Start () {
	    currentState = PillowState.Idle;
    }
	
	// Update is called once per frame
	void FixedUpdate (){
        // pillow follow
        if (currentState == PillowState.Picked && Vector3.Distance(transform.position, holder.transform.position) > 2f) {
            //print(Vector3.Distance(transform.position, holder.transform.position));
            transform.position = Vector3.Lerp(transform.position, holder.transform.position, 0.1f);
        }

	    if (currentState == PillowState.Throwed&&GetComponent<Rigidbody>().velocity.magnitude < 5f)
	    {
	        currentState = PillowState.Idle;
	    }

	    if (currentState == PillowState.Aimed)
	    {
	        transform.position = holder.transform.position +holder.transform.forward;
	    }
	}

    public void ReadyToGo(GameObject model)
    {
        // set the ball to the front of the 
        //transform.parent = model;
        holder = model;
        currentState = PillowState.Aimed;
    }

    public void Pick(GameObject player)
    {
        currentState = PillowState.Picked;
        holder = player;
    }

    public void Throw(Vector3 forward, Vector3 Up,float ForwardForce, float UpperForce)
    {
        currentState = PillowState.Throwed;
        GetComponent<Rigidbody>().AddForce(forward * 130);
        GetComponent<Rigidbody>().AddForce(Up * 20);
        holder = null;
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (currentState == PillowState.Throwed)
            {
                // doing dmg TODO
                currentState = PillowState.Idle;
            }
        }
    }
}
