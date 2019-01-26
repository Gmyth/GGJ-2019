using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillow : MonoBehaviour {
    public bool Picked;
    public bool Throwed; // if the pillow is throwed, then do damage

    private GameObject holder;
	// Use this for initialization
	void Start () {
        Picked = false;
    }
	
	// Update is called once per frame
	void Update (){
        if (Picked) {
            // pillow follow
            transform.position = Vector3.Lerp(transform.position, holder.transform.position, Time.time);
        }
    }

    public void Pick(GameObject player){
        Picked = true;
        holder = player;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Throwed)
            {
                // doing dmg TODO
                Throwed = false;
            }
        }
    }
}
