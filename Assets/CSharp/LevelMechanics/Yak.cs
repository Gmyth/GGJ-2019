using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yak : MonoBehaviour {

    [SerializeField] private float migrateSpeed;
    [SerializeField] private float bounceForce;

    private Vector3 migrateDirection;
    [SerializeField] private int currentRouteIndex;

    private MigrateEvent me;

    private void Awake()
    {
        me = GameObject.Find("LevelEventManager").GetComponent<MigrateEvent>();
    }

    // Use this for initialization
    void Start()
    {
        currentRouteIndex = 1;
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

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Bounce back the player
            other.gameObject.GetComponent<Rigidbody>().AddForce(-other.transform.right * bounceForce);
            Debug.Log(-other.transform.right * bounceForce);
        }

        if (other.gameObject.tag == "RoutePoint")
        {
            currentRouteIndex++;
            if (currentRouteIndex < me.MigratePoints.Length) {
                migrateDirection = (me.MigratePoints[currentRouteIndex].position - transform.position).normalized;
                transform.LookAt(me.MigratePoints[currentRouteIndex].position);
            }
        }

    }

    private void OnDestroy()
    {
        gameObject.SetActive(false);
    }

}
