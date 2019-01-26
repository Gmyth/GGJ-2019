using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MigrateEvent : LevelEvent {

    [SerializeField] private Transform spawnPoint;

    [SerializeField] private GameObject yakPrefab;
    private Yak[] yaks;

    [SerializeField] private float createTimePeriod;

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
            go.GetComponent<Yak>().SetMigrateDirection(spawnPoint.forward);
            go.transform.LookAt(spawnPoint.position + spawnPoint.forward);
            go.SetActive(true);
            yield return new WaitForSeconds(createTimePeriod);
        }
    }

}
