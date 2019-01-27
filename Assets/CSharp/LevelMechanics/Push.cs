using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Bouncer")
        {
            Vector3 pushDirection = new Vector3((collision.transform.parent.position.x - transform.position.x), 0, (collision.transform.parent.position.z - transform.position.z));
            collision.transform.parent.GetComponent<Rigidbody>().AddForce(pushDirection.normalized * 500);
            Debug.Log("Push: " + pushDirection.normalized);
        }

    }

}
