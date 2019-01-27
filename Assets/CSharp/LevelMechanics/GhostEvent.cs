using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEvent : LevelEvent {

    public Transform[] MigratePoints;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private GameObject ghost;

    [SerializeField] private float createTimePeriod;

    private GameObject ghostObj;

    private void Start()
    {
        MigratePoints = spawnPoint.GetComponentsInChildren<Transform>();
    }

    override public void OnStart()
    {
        //FindSpawnPoint();
        CreateGhost();
        base.OnStart();
    }

    override public void OnEnd()
    {
        ghostObj.GetComponent<Ghost>().OnDestroy();
        base.OnEnd();
    }



    private void CreateGhost()
    {
        ghostObj = Instantiate(ghost, MigratePoints[1].transform.position, Quaternion.identity);
        ghostObj.GetComponent<Ghost>().SetMigrateDirection(MigratePoints[2].position - MigratePoints[1].transform.position);
        //go.transform.LookAt(MigratePoints[1].position - spawnPoint.transform.position);
        ghostObj.SetActive(true);
    }
}
