using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemSpeedChange : MonoBehaviour
{
    float currCountdownValue;
    Player player = null;

    [SerializeField] private float duration = 10f;
    [SerializeField] private float speedfactor = 1f;

    void Awake()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            player = other.GetComponent<Player>();
        }

        player.SetSpeed(speedfactor);
        player.StartCoroutine(StartCountdown(player, duration));
    }

    public IEnumerator StartCountdown(Player player, float countdownValue = 10)
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
