using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MigrateEvent : LevelEvent {

    public Transform[] MigratePoints;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private GameObject yakPrefab;

    [SerializeField] private float createTimePeriod;

    private void Start()
    {
        MigratePoints = spawnPoint.GetComponentsInChildren<Transform>();
    }

    override public void OnStart()
    {
        FindSpawnPoint();
        StartCoroutine("CreateYaks");
        base.OnStart();
    }

    override public void OnEnd()
    {
        StopCoroutine("CreateYaks");
        base.OnEnd();
    }

    private void FindSpawnPoint()
    {

    }

    private IEnumerator CreateYaks()
    {
        while (true)
        {
            GameObject go = Instantiate(yakPrefab, spawnPoint.position, Quaternion.identity);
            go.GetComponent<Yak>().SetMigrateDirection(MigratePoints[1].position - MigratePoints[0].transform.position);
            //go.transform.LookAt(MigratePoints[1].position - spawnPoint.transform.position);
            go.SetActive(true);
            yield return new WaitForSeconds(createTimePeriod);
        }
    }

    private void OnDrawGizmosSelected()
    {
        foreach (Transform t in MigratePoints)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(t.position, new Vector3(0.5f, 0.5f, 0.5f));
        }
    }

}
