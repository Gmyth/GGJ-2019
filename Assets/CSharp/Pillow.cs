using System.Collections;
using UnityEngine;
public enum PillowState : int
{
    Idle = 0,
    Picked,
    Aimed,
    Throwed,
    Attacked,
}
public class Pillow : MonoBehaviour
{
    [SerializeField] private MeshCollider Collider;

    public PillowState currentState;// if the pillow is throwed, then do damage
    private Player holder;

    public bool isInWind;

    private Vector3 launchPoint;

    public Transform spawnData;

    public void ResetAll()
    {
        currentState = PillowState.Idle;
        holder = null;
        isInWind = false;

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        rigidbody.useGravity = true;

        transform.parent = GameManager.Singleton.transform;
        transform.localPosition = spawnData.position;
        transform.rotation = spawnData.rotation;
    }

    // Use this for initialization
    private void Start ()
    {
	    currentState = PillowState.Idle;
        isInWind = false;
    }
	
	void FixedUpdate()
    {
        //// pillow follow
        //if (currentState == PillowState.Picked && Vector3.Distance(transform.position, holder.transform.position) > 2.5f)
        //{
        //    GetComponent<Rigidbody>().position = Vector3.Lerp(transform.position, holder.transform.position, 0.1f);
        //}

        if (currentState == PillowState.Throwed && GetComponent<Rigidbody>().velocity.magnitude < 5f)
	    {
	        currentState = PillowState.Idle;
	    }

	    //if (currentState == PillowState.Aimed)
	    //{
            //GetComponent<Rigidbody>().position = holder.transform.position + holder.transform.forward + new Vector3(0, 2, 0);
	    //}
	}

    public void ReadyToGo()
    {
        // set the ball to the front of the 
        //transform.parent = model;
        currentState = PillowState.Aimed;
        GetComponent<Rigidbody>().useGravity = false;
        //GetComponent<MeshCollider>().enabled = false;
        Collider.enabled = false;
    }

    public void Pick(Player player)
    {
        currentState = PillowState.Picked;

        GetComponent<Rigidbody>().isKinematic = true;

        holder = player;
    }

    public void Throw(Vector3 forward, Vector3 Up,float ForwardForce, float UpperForce)
    {
        currentState = PillowState.Throwed;

        launchPoint = transform.position;

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
        rigidbody.velocity = forward * ForwardForce + Up * UpperForce;
        rigidbody.useGravity = true;

        // GetComponent<MeshCollider>().enabled = true;
        Collider.enabled = true; 

        if (isInWind)
        {
            StartCoroutine("FlyInTheWind");
        }
    }

    private IEnumerator FlyInTheWind()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<Rigidbody>().AddForce(Vector3.forward * 2500f);
    }

    public void Attack() {
        currentState = PillowState.Attacked;
        StartCoroutine(ReturnToIdle());
    }

    IEnumerator ReturnToIdle()
    {
        yield return new WaitForSeconds(1f);
        currentState = PillowState.Idle;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (currentState == PillowState.Throwed)
            {
                if (player != holder)
                {
                    Vector3 forward = transform.forward;
                    Vector3 orientation = (other.transform.position - transform.position).normalized;

                    float d = Vector3.Distance(other.transform.position, launchPoint);
                    if (Vector3.Dot(forward, orientation) < 0.7f)
                    {
                        player.Hurt(false);
                        player.Score += Mathf.FloorToInt(CalculateDamage(d));
                    }
                    else
                    {
                        player.Hurt(true);
                        player.Score += Mathf.FloorToInt(1.2f * CalculateDamage(d));
                    }
                    AudioManager.Instance.PlaySoundEffect("PillowNearFight", false);
                    currentState = PillowState.Idle;
                    holder = null;
                }
            }
            else if (currentState == PillowState.Attacked)
            {
                // doing melee damage
                if (player != holder)
                {
                    Vector3 forward = transform.forward;
                    Vector3 orientation = (other.transform.position - transform.position).normalized;

                    if (Vector3.Dot(forward, orientation) < 0.7f)
                    {
                        other.GetComponent<Player>().Hurt(false);
                        player.Score += 1;
                    }
                    else
                    {
                        other.GetComponent<Player>().Hurt(true);
                        player.Score += 2;
                    }

                    currentState = PillowState.Idle;
                }

                AudioManager.Instance.PlaySoundEffect("PillowNearFight", false);

            }
        }
        if (other.tag == "Terrian")
        {
            currentState = PillowState.Idle;
            holder = null;
        }
    }

    private float CalculateDamage(float d)
    {
        return Mathf.Max(0, d * d / 10 - 2) + 5;
    }
}
