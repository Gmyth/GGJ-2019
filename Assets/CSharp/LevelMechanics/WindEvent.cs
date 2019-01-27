using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEvent : LevelEvent {

    [SerializeField] private Vector3 blowDirection;
    [SerializeField] private float windForce;

    private GameObject[] pillowObjects;

    private void Start()
    {
        pillowObjects = GameObject.FindGameObjectsWithTag("Pillow");
    }

    private void FixedUpdate()
    {
        //Wind blowing
    }

    public override void OnStart()
    {
        foreach (GameObject go in pillowObjects)
        {
            go.GetComponent<Pillow>().isInWind = true;
        }
        base.OnStart();
    }

    public override void OnEnd()
    {
        foreach (GameObject go in pillowObjects)
        {
            go.GetComponent<Pillow>().isInWind = false;
        }
        base.OnEnd();
    }

}
