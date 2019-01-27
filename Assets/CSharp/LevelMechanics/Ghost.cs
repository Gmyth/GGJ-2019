
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {

    [SerializeField] private float migrateSpeed;
    [SerializeField] private GameObject particle;

    private Vector3 migrateDirection;
    [SerializeField] private int currentRouteIndex;
    [SerializeField] private int bound;

    private GhostEvent me;
    private bool Found;
    private List<Player> playerlist; // players be found

    // Use this for initialization
    void Start()
    {
        Found = false;
        playerlist = new List<Player>();
        currentRouteIndex = 2;
        particle.SetActive(false);
        me = LevelEventManager.Instance.GetComponent<GhostEvent>();
        transform.LookAt(me.MigratePoints[currentRouteIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf&&!Found)
        {
            transform.Translate(migrateDirection * migrateSpeed * Time.deltaTime, Space.World);
        }
    }

    public void SetMigrateDirection(Vector3 direction)
    {
        migrateDirection = direction;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            //Stop or bounce back the pillow
            collision.transform.parent.GetComponent<Player>().SetSpeed(0);
            playerlist.Add(collision.transform.parent.GetComponent<Player>());
            particle.SetActive(true);
            Found = true;
        }

        if (collision.tag == "RoutePoint1")
        {
            if (currentRouteIndex < me.MigratePoints.Length - 1)
            {
                if (collision.name == me.MigratePoints[currentRouteIndex].name)
                {
                    currentRouteIndex = currentRouteIndex >= bound - 1? 1: currentRouteIndex + 1;
                    migrateDirection = (me.MigratePoints[currentRouteIndex].position - transform.position).normalized;
                    transform.LookAt(me.MigratePoints[currentRouteIndex].position);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // no escape, just in case
        if (other.tag == "Player")
        {
            //Stop or bounce back the pillow
            other.transform.parent.GetComponent<Player>().ResetSpeed();
        }
    }

    public void OnDestroy()
    {
        // release all player
        foreach (Player player in playerlist) {
            player.ResetSpeed();
        }
        gameObject.SetActive(false);
    }
}
