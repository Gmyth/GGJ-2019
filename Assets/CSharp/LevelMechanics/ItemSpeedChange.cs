using System.Collections;
using UnityEngine;


public class ItemSpeedChange : MonoBehaviour
{
    [SerializeField] private float duration = 10f;
    [SerializeField] private float speedfactor = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player player = other.transform.parent.GetComponent<Player>();
            player.SetSpeed(speedfactor);
            player.StartCoroutine(StartCountdown(player, duration));

            gameObject.SetActive(false);
        }
    }

    public IEnumerator StartCountdown(Player player, float countdownValue = 10)
    {
        float currCountdownValue = countdownValue;

        while (currCountdownValue > 0)
        {
            yield return null;
            currCountdownValue -= Time.deltaTime;
        }

        player.ResetSpeed();
    }
}
