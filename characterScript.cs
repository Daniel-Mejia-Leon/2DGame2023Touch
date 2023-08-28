using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterScript : MonoBehaviour
{
    Vector2 doNothingVector;
    Animator _animator;
    public controlsV2 joystickInput;
    
    SpriteRenderer sprite;
    [SerializeField] private float walkSpeed, jumpSpeed, minimumValueToStartMovingOnX, coyoteTime, coyoteTimeSet;
    public bool onGround, toRight, toLeft, toGround, jumpPressed;

    private string
        idle_anim_str = "idle",
        jump_anim_str = "jump",
        run_anim_str = "run",
        current_anim;

    [Header("All Sounds")]
    [SerializeField] private AudioSource itemTakenSound;
    [SerializeField] private AudioSource jumpSound;

    // BUGS
    // IF YOU MOVE AND YOU TAP THE TILEMAP TOO, THE CHARACTER WILL ACT WEIRD

    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        _animator = gameObject.GetComponent<Animator>();
        current_anim = idle_anim_str;
        jumpPressed = true;
    }

    // Update is called once per frame
    void Update()
    {
        float stateInfo = _animator.GetCurrentAnimatorStateInfo(0).speed;

        doNothingVector = new Vector2(transform.position.x, transform.position.y);

        RaycastHit2D[] raysDown = Physics2D.RaycastAll(gameObject.transform.position, Vector2.down, 2.8f);

        foreach (RaycastHit2D hit in raysDown)
        {
            if (hit.transform.tag == "ground")
            {
                toGround = true;
                Debug.DrawRay(transform.position, Vector2.down * hit.distance, Color.blue);
            }
            else { toGround = false; }
        }

        // CHARACTER MOVEMENT
        if (toRight && joystickInput.getAxisRaw > 0)
        {
            transform.position = doNothingVector;
        } 

        else if (toLeft && joystickInput.getAxisRaw < 0)
        {
            transform.position = doNothingVector;
        }
        else if (joystickInput.getAxisX > minimumValueToStartMovingOnX || joystickInput.getAxisX < -minimumValueToStartMovingOnX)
        {
            transform.position = new Vector2(transform.position.x + joystickInput.getAxisX * walkSpeed * Time.deltaTime, transform.position.y);
        }



        // SIMPLE FLIP OF CHARACTER SPRITE
        if (joystickInput.getAxisRaw > 0)
        {
            sprite.flipX = false;
        }
        else if (joystickInput.getAxisRaw < 0)
        {
            sprite.flipX = true;
        }

        // RUNNING ANIMATION IF INPUT.getAxisRaw HAS A VALUE
        if (onGround && joystickInput.getAxisX >= minimumValueToStartMovingOnX || onGround && joystickInput.getAxisX <= -minimumValueToStartMovingOnX)
        {
            float proportionalSpeed = joystickInput.getAxisX;
            setCurrentStateTo(run_anim_str);
            _animator.SetFloat("runSpeed", proportionalSpeed);

        }

        // CHARACTER JUMP // REMOVE THE DEPENDENCIE OF ONGROUND FORM INPUT.CS AND FIX IT WITH A COYOTE TIME
        if (joystickInput.touchedJumpButton)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector3(0f, jumpSpeed, 0f);
            joystickInput.touchedJumpButton = false;
            jumpSound.Play();
        }

        // ON AIR
        if (!onGround)
        {
            coyoteTime -= Time.deltaTime;
            setCurrentStateTo(jump_anim_str);
        }

        // ON GROUND IDLE
        if (onGround)
        {
            joystickInput.touchedJumpButton = false;
            coyoteTime = coyoteTimeSet;
        }
    

        

        // IDLE (DONT REMEMBER WHAT I DID HERE)

        // SECON PARAMETER (DELTED, REPLACED BY EXTERNAL INPUT .CS) = WHEN YOU RUN, JUMP, AND KEEP THE JUMP BUTTON PRESSED, AND THE SUDDENLY RELEASE THE MOVE BUTTON THE RUN ANIMATION WILL KEEP LOOPIN WITHOUT POSITION CHANGE
        // THIS MAKES SURE THAT IF THERE'S 1 TOUCH AND IT IS ON THE JUMP BUTTON THE ANIMATION WILL CHANGE TO IDLE 
        if (Input.touchCount <= 0 && onGround || Input.touchCount == 1 && onGround && _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == jump_anim_str
            || Input.touchCount == 1 && onGround && joystickInput.getAxisRaw == 0 && _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == run_anim_str 
            || joystickInput.getAxisX > -minimumValueToStartMovingOnX && joystickInput.getAxisX < minimumValueToStartMovingOnX && onGround) // || Input.touchCount == 1 && onGround) // REPLACED BY EXTERNAL INPUT .CS // && jumpPressed)
        {
            // REPLACED BY EXTERNAL INPUT .CS
            // jumpPressed = false;

            setCurrentStateTo(idle_anim_str);
        }

        

        // Debug.Log(stateInfo);
    }

    void setCurrentStateTo(string newState)
    {
        if (current_anim == newState){ return; }

        _animator.Play(newState);

        current_anim = newState;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("item"))
        {
            collision.GetComponent<CircleCollider2D>().enabled = false;
            collision.GetComponent<Animator>().SetBool("taken", true);
            itemTakenSound.Play();
        }
    }
}
