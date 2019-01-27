using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yak : MonoBehaviour {

    [SerializeField] private float migrateSpeed;
    [SerializeField] private float bounceForce;

    private Vector3 migrateDirection;
    [SerializeField] private int currentRouteIndex;

    private MigrateEvent me;

    // Use this for initialization
    void Start()
    {
        currentRouteIndex = 2;

        me = LevelEventManager.Instance.GetComponent<MigrateEvent>();
        transform.LookAt(me.MigratePoints[currentRouteIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            transform.Translate(migrateDirection * migrateSpeed * Time.deltaTime, Space.World);
        }
    }

    public void StartMigrate(Vector3 direction)
    {

    }

    public void StopMigrate()
    {
        OnDestroy();
    }

    public void SetMigrateDirection(Vector3 direction)
    {
        migrateDirection = direction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Pillow")
        {
            //Stop or bounce back the pillow

        }

        if (collision.gameObject.tag == "Boarder")
        {
            StopMigrate();
        }

        if (collision.gameObject.tag == "PlayerCollider")
        {
            Vector3 pushDirection = new Vector3((collision.transform.parent.position.x - transform.position.x), 0, (collision.transform.parent.position.z - transform.position.z));
            collision.transform.parent.GetComponent<Rigidbody>().isKinematic = false;
            collision.transform.parent.GetComponent<Rigidbody>().AddForce(-pushDirection.normalized * bounceForce);
            //collision.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
            Debug.Log("Push: " + pushDirection.normalized);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent.GetComponent<Player>().PushBack();
            //Bounce back the player
            //other.gameObject.GetComponent<Rigidbody>().isKinematic = false;

            //StartCoroutine("Countdown", other.gameObject.GetComponent<Player>());

            //Vector3 pushDirection = new Vector3((other.transform.position.x - transform.position.x), 0, (other.transform.position.z - transform.position.z));
            
            //other.gameObject.GetComponent<Rigidbody>().AddForce(pushDirection.normalized * bounceForce);
            //other.gameObject.GetComponent<Rigidbody>().velocity = pushDirection.normalized * bounceForce;
            //other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            //Debug.Log(pushDirection.normalized);

            //other.gameObject.GetComponent<Player>().PushAway(pushDirection.normalized, bounceForce);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "RoutePoint")
        {         
            if (currentRouteIndex < me.MigratePoints.Length - 1) {
                if (other.gameObject.name == me.MigratePoints[currentRouteIndex].name)
                {
                    currentRouteIndex++;
                    migrateDirection = (me.MigratePoints[currentRouteIndex].position - transform.position).normalized;
                    transform.LookAt(me.MigratePoints[currentRouteIndex].position);
                }               
            }
        }
    }

    private IEnumerator Countdown(Player p)
    {
        yield return new WaitForSeconds(0.1f);
        p.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void OnDestroy()
    {
        gameObject.SetActive(false);
    }

}
