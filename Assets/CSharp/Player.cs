using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public readonly int id;

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

    public EventOnDataChange<string> OnNameChange { get; private set; }

    private PlayerInfo() {}

    public PlayerInfo(int id, string name)
    {
        this.id = id;
        this.name = name;

        OnNameChange = new EventOnDataChange<string>();
    }
}

public class Player : MonoBehaviour
{
    /// <summary>
    /// The name of the player, which is set at the beginning of the game
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// The name of the player, which is set at the beginning of the game
    /// </summary>
    public string Name { get; private set; }


    private int score;
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
    [SerializeField] private float speed;
    // Record default speed
    private float defaultSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float gravity;


    [SerializeField] private float minThrowPower;
    [SerializeField] private float maxThrowPower;
    [SerializeField] private float powerToAddEachFrame;

    [SerializeField] private float powerThrowForward;
    [SerializeField] private float powerThrowUpper;
    
    
    [SerializeField] private GameObject model;

    private int numPillowHold;
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

    private List<Pillow> Pillows;
    private List<Pillow> Ammo;
    private float emPower;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private bool oldTriggerHeldPick;
    private bool oldTriggerHeldThrow;

    public EventOnDataChange<int> OnScoreChange { get; private set; }
    public EventOnDataChange<int> OnNumPillowsHeldChange { get; private set; }

    public void Initialize(PlayerInfo playerInfo)
    {
        Id = playerInfo.id;
        Name = playerInfo.Name;
        Score = 0;
        numPillowHold = 0;
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Ammo = new List<Pillow>();
        Pillows = new List<Pillow>();
        emPower = 0.0f;
        OnScoreChange = new EventOnDataChange<int>();
        OnNumPillowsHeldChange = new EventOnDataChange<int>();
        // let the gameObject fall down

        defaultSpeed = speed;
    }

    void FixedUpdate()
    {
        // all input interface
        if (controller.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = moveDirection * speed;
            Vector3 facingrotation = Vector3.Normalize(moveDirection);
            if (facingrotation != Vector3.zero)         //This condition prevents from spamming "Look rotation viewing vector is zero" when not moving.
                model.transform.forward = facingrotation;
           // model.transform.LookAt(new Vector3(0,moveDirection.y,0));
//            float heading = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
//            transform.rotation = Quaternion.Euler(0f, 0f, heading - 90);
            
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }
        bool newTriggerHeldPick = Input.GetAxis("Pick") > 0f;
        if(!oldTriggerHeldPick == newTriggerHeldPick)
        {
            if (NumPillowsHeld >= 2)
            {
                // exceed the number that one player can hold 
                ThrowDrop();
            }
            else
            {
                PickUp();
            }
        }
        oldTriggerHeldPick = newTriggerHeldPick;
        
        
        
        bool newTriggerHeldThrow = Input.GetAxis("Throw") > 0f;
        if (newTriggerHeldThrow != oldTriggerHeldThrow)
        {
            if (newTriggerHeldThrow && NumPillowsHeld > 0)
            {
                // start to empower the throw
                if (Ammo.Count != 0)
                {
                    Ammo[0].ReadyToGo();
                    emPower = minThrowPower;
                }
            }
            
        }

        if (emPower > 0.01f)
        {
            emPower = emPower < maxThrowPower ? emPower + powerToAddEachFrame : emPower; 
        }

        if (oldTriggerHeldThrow && !newTriggerHeldThrow) {
            /// throw
            if (Ammo.Count!=0)
            {
                Ammo[0].Throw( model.transform.forward, model.transform.up, (emPower - 0.08f + 1) * powerThrowForward, (emPower + 1) * powerThrowUpper);
                emPower = 0.0f;
                oldTriggerHeldThrow = false;
                Ammo.RemoveAt(0);
                NumPillowsHeld--;
            } 
          
        }
        else {
          oldTriggerHeldThrow = newTriggerHeldThrow;
        }
        
        // Apply gravity
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

        // Move the controller
        controller.Move(moveDirection * Time.deltaTime);
    }

    public void ThrowDrop()
    {

    }

    public void PickUp()
    {
        for (int i = 0; i < Pillows.Count; i++)
        {
            var temp = Pillows[i];
            if (temp.currentState==PillowState.Idle)
            {
                temp.Pick(gameObject);
                Ammo.Add(temp);
                NumPillowsHeld++;
                break;
            }
        }
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }

    public void ResetSpeed()
    {
        speed = defaultSpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pillow") {
            var temp = other.GetComponent<Pillow>();
            if (temp.currentState == PillowState.Throwed){
                // doing dmg TODO

            }else{
                // sign into queue for pick up
                if (temp.currentState == PillowState.Idle) { Pillows.Add(temp);}
            }
        }   
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Pillow")
        {
            var temp = other.GetComponent<Pillow>();
            if (Pillows.Contains(temp)) { 
                // sign out queue
                Pillows.Remove(temp);
            }
        }
    }

}