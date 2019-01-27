using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemSpeedChange : MonoBehaviour {
    float currCountdownValue;
    GameObject player = null;
    [SerializeField] private float duration = 10f;
    [SerializeField] private float speedfactor = 1f;
    void Awake()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            player = other.gameObject;
        }
        player.GetComponent<Player>().SetSpeed(speedfactor);
        StartCoroutine(StartCountdown(duration));
        
    }
    public IEnumerator StartCountdown(float countdownValue = 10)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;         
        }
        player.GetComponent<Player>().ResetSpeed();
    }
}
