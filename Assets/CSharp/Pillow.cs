﻿using System.Collections;
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
    [SerializeField] private MeshCollider Collider;
    public PillowState currentState;// if the pillow is throwed, then do damage
    private GameObject holder;
	// Use this for initialization
	void Start () {
	    currentState = PillowState.Idle;
    }
	
	void FixedUpdate()
    {
        //// pillow follow
        //if (currentState == PillowState.Picked && Vector3.Distance(transform.position, holder.transform.position) > 2.5f)
        //{
        //    GetComponent<Rigidbody>().position = Vector3.Lerp(transform.position, holder.transform.position, 0.1f);
        //}

        if (currentState == PillowState.Throwed && GetComponent<Rigidbody>().velocity.magnitude < 5f)
	    {
	        currentState = PillowState.Idle;
	    }

	    if (currentState == PillowState.Aimed)
	    {
            GetComponent<Rigidbody>().position = holder.transform.position + holder.transform.forward + new Vector3(0, 2, 0);
	    }
	}

    public void ReadyToGo()
    {
        // set the ball to the front of the 
        //transform.parent = model;
        currentState = PillowState.Aimed;
        GetComponent<Rigidbody>().useGravity = false;
        //GetComponent<MeshCollider>().enabled = false;
        Collider.enabled = false;
    }

    public void Pick(GameObject player)
    {
        currentState = PillowState.Picked;

        GetComponent<Rigidbody>().isKinematic = true;

        holder = player;
    }

    public void Throw(Vector3 forward, Vector3 Up,float ForwardForce, float UpperForce)
    {
        currentState = PillowState.Throwed;

        GetComponent<Rigidbody>().isKinematic = false;

        GetComponent<Rigidbody>().AddForce(forward * ForwardForce + Up * UpperForce);

        holder = null;

        GetComponent<Rigidbody>().useGravity = true;
        // GetComponent<MeshCollider>().enabled = true;

        Collider.enabled = true; 
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
