using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yak : MonoBehaviour {

    [SerializeField] private float migrateSpeed;
    [SerializeField] private float bounceForce;

    private Vector3 migrateDirection;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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
        if (collision.gameObject.tag == "Player")
        {
            //Bounce back the player
            
        }

        if (collision.gameObject.tag == "Pillow")
        {
            //Stop or bounce back the pillow
            
        }

        if (collision.gameObject.tag == "Boarder")
        {
            StopMigrate();
        }

    }

    private void OnDestroy()
    {
        gameObject.SetActive(false);
    }

}
