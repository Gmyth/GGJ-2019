using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlayerInfo
{
    public readonly int id;
    public readonly string controllerId;

    private string name;
    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            if (value != name)
            {
                name = value;
                OnNameChange.Invoke(name);
            }
        }
    }

    private bool isReady;
    public bool IsReady
    {
        get
        {
            return isReady;
        }

        set
        {
            if (value != isReady)
            {
                isReady = value;
                OnReadinessChange.Invoke(isReady);
            }
        }
    }

    public EventOnDataChange<string> OnNameChange { get; private set; }
    public EventOnDataChange<bool> OnReadinessChange { get; private set; }

    private PlayerInfo() {}

    public PlayerInfo(int id, string name, string controllerId)
    {
        this.id = id;
        this.controllerId = controllerId;
        this.name = name;

        OnNameChange = new EventOnDataChange<string>();
        OnReadinessChange = new EventOnDataChange<bool>();
    }
}

public class Player : MonoBehaviour
{
    /// <summary>
    /// The name of the player, which is set at the beginning of the game
    /// </summary>
    public int Id { get; private set; }

    public string ControllerId { get; private set; }

    /// <summary>
    /// The name of the player, which is set at the beginning of the game
    /// </summary>
    public string Name { get; private set; }

    private int score = 0;
    /// <summary>
    /// The score that has been earned by the player
    /// </summary>
    public int Score
    {
        get
        {
            return score;
        }

        private set
        {
            if (value != score)
            {
                score = value;
                OnScoreChange.Invoke(score);
            }
        }
    }

    // Use this for initialization
    [SerializeField]private float speed;
    [SerializeField]private float jumpSpeed;
    [SerializeField]private float gravity;


    [SerializeField] private float minThrowPower;
    [SerializeField] private float maxThrowPower;
    [SerializeField] private float powerToAddEachFrame;

    [SerializeField] private float powerThrowForward;
    [SerializeField] private float powerThrowUpper;
    
    
    public GameObject model;

    [SerializeField] private Animator buttom;
    [SerializeField] private Animator top;

    [SerializeField] private Transform SlotLA;
    [SerializeField] private Transform SlotRA;

    private int numPillowHold = 0;
    /// <summary>
    /// The number of pillows carried by the player
    /// </summary
    public int NumPillowsHeld
    {
        get
        {
            return numPillowHold;
        }

        set
        {
            if (value != numPillowHold)
            {
                numPillowHold = value;
                OnNumPillowsHeldChange.Invoke(numPillowHold);
            }
        }
    }

    private PillowState currentPlayerState;
    private List<Pillow> Pillows = new List<Pillow>();
    private Queue<Pillow> Ammo = new Queue<Pillow>();
    private float emPower = 0;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;

    public EventOnDataChange<int> OnScoreChange { get; private set; }
    public EventOnDataChange<int> OnNumPillowsHeldChange { get; private set; }

    public void Initialize(PlayerInfo playerInfo)
    {
        Id = playerInfo.id;
        ControllerId = playerInfo.controllerId;
        Name = playerInfo.Name;
        Score = 0;

        // let the gameObject fall down
        gameObject.transform.position = new Vector3(0, 5, 0);
    }

    private bool isSubmitButtonUp = true;
    private bool isCancelButtonUp = true;

    void FixedUpdate()
    {
        // Picking up pillows
        if (Input.GetAxis("Pick" + ControllerId) == 0)
            isSubmitButtonUp = true;
        else if (isSubmitButtonUp)
        {
            isSubmitButtonUp = false;

            if (NumPillowsHeld >= 2) // exceed the number that one player can hold
                Drop();
            else
                PickUp();
        }
        //Attack
        if (Input.GetAxis("Attack" + ControllerId) > 0) {
            top.SetInteger("CurrentState", 4);
            StartCoroutine(AttackFinish());
            if (numPillowHold == 0)
            {
                // that is a punch
                SlotRA.GetComponent<SphereCollider>().enabled = true;
            }
            else {
                // a Pillow Sweep
                if(numPillowHold == 1)
                {
                    var temp = Ammo.Peek();
                    temp.Attack();
                }
            }
        }
            

        // Tossing pillows
        if (Input.GetAxis("Toss" + ControllerId) == 0)
        {
            if (!isCancelButtonUp && NumPillowsHeld > 0)
            {
                Pillow pillow = Ammo.Dequeue();
                pillow.Throw(model.transform.forward, model.transform.up, (emPower - 0.08f + 1) * powerThrowForward, (emPower + 1) * powerThrowUpper);
                pillow.transform.parent = transform.parent;

                top.SetInteger("CurrentState", 3);
                StartCoroutine(ThrowFinish());
                emPower = 0;
                NumPillowsHeld--;
            }

            isCancelButtonUp = true;
        }
        else if (isCancelButtonUp)
        {
            isCancelButtonUp = false;

            if (NumPillowsHeld > 0)
            {
                Pillow pillow = Ammo.Peek();
                pillow.ReadyToGo();

                if (numPillowHold == 1)
                {
                    pillow.transform.parent = SlotRA;
                    pillow.transform.localPosition = Vector3.zero;
                    pillow.transform.localRotation = Quaternion.identity;
                }

                emPower = minThrowPower;
                top.SetInteger("CurrentState", 1);
            }
        }

        if (!isCancelButtonUp && emPower > 0)
            emPower = Mathf.Min(emPower + powerToAddEachFrame * Time.fixedDeltaTime, maxThrowPower);

        if (controller.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            if (moveDirection.sqrMagnitude > 0.2f)
            {
                moveDirection = Quaternion.Euler(0, 45, 0) * transform.TransformDirection(moveDirection);
                moveDirection = moveDirection.normalized;

                top.SetFloat("Speed", speed);
                buttom.SetFloat("Speed", speed);
                model.transform.forward = moveDirection;
            }
            else
            {
                moveDirection = Vector3.zero;

                top.SetFloat("Speed", 0);
                buttom.SetFloat("Speed", 0);
            }   

            //if (Input.GetButton("Jump"))
            //{
            //    moveDirection.y = jumpSpeed;
            //}
        }

        // Apply gravity
        moveDirection.y = moveDirection.y - (gravity * Time.fixedDeltaTime);

        controller.Move(moveDirection * speed * Time.fixedDeltaTime);
    }

    public void Drop()
    {

    }

    public void PickUp()
    {
#if UNITY_EDITOR
        Debug.Log(LogUtility.MakeLogString("Player", name + " picked up a pillow."));
#endif

        if (Pillows.Count > 0)
        {
            top.SetInteger("CurrentState", 2);
            StartCoroutine(PickFinish());
            Pillow pillow = Pillows[0];
            pillow.transform.parent = NumPillowsHeld++ == 0 ? SlotLA : SlotRA;
            pillow.transform.localPosition = Vector3.zero;
            pillow.transform.localRotation = Quaternion.identity;

            pillow.Pick(gameObject);
            Ammo.Enqueue(pillow);
        }
    }
    public void Hurt(bool facing) {
        top.SetBool("Hurt", true);
        top.SetBool("HurtFromBack", !facing);
        StartCoroutine(Hurt());
    }
    IEnumerator Hurt()
    {
        yield return new WaitForSeconds(1f);
        top.SetBool("Hurt", false);
    }

    IEnumerator PickFinish() {
        yield return new WaitForSeconds(1f);
        top.SetInteger("CurrentState", 0);
    }

    IEnumerator AttackFinish()
    {
        yield return new WaitForSeconds(1f);
        SlotRA.GetComponent<SphereCollider>().enabled = false;
        top.SetInteger("CurrentState", 0);
    }

    IEnumerator ThrowFinish()
    {
        yield return new WaitForSeconds(1f);
        top.SetInteger("CurrentState", 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pillow")
        {
            Pillow pillow = other.GetComponent<Pillow>();

            switch (pillow.currentState)
            {
                case PillowState.Idle:
                    Pillows.Add(pillow); // Register the pillow
                    break;

                case PillowState.Throwed:
                    // doing dmg TODO
                    break;
            }
        }

        if (other.tag == "Punch")
        {
            //get punch by others
            // TODO deal damage
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Pillow")
        {
            Pillow pillow = other.GetComponent<Pillow>();

            if (Pillows.Contains(pillow))
                Pillows.Remove(pillow); // Deregister the pillow
        }

    }

    private void Awake()
    {
        Id = 0;
        ControllerId = "_J1";
        Name = "Tester";

        controller = GetComponent<CharacterController>();

        // let the gameObject fall down
        gameObject.transform.position = new Vector3(0, 5, 0);

        OnScoreChange = new EventOnDataChange<int>();
        OnNumPillowsHeldChange = new EventOnDataChange<int>();
    }
}
