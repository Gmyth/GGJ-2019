using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour {

    [SerializeField] private float bounceForce;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 pushDirection = new Vector3((other.transform.position.x - transform.position.x), 0, (other.transform.position.z - transform.position.z));
            other.gameObject.GetComponent<Player>().PushAway(pushDirection.normalized, bounceForce);
        }
    }

}
