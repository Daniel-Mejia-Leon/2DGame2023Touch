using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterScript : MonoBehaviour
{
    Animator _animator;
    public joystickX joystickInput;
    SpriteRenderer sprite;
    [SerializeField] private float walkSpeed, jumpSpeed, coyoteTime, coyoteTimeSet;
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
        RaycastHit2D[] raysToRight = Physics2D.RaycastAll(gameObject.transform.position, Vector2.right, 1f);

        foreach (RaycastHit2D hit in raysToRight)
        {
            if (hit.transform.tag == "ground")
            {
                toRight = true;
                Debug.DrawRay(transform.position, Vector2.right * hit.distance, Color.red);
            } else { toRight = false; } 

        }

        RaycastHit2D[] raysToLeft = Physics2D.RaycastAll(gameObject.transform.position, Vector2.left, 1f);

        foreach (RaycastHit2D hit in raysToLeft)
        {
            if (hit.transform.tag == "ground")
            {
                toLeft = true;
                Debug.DrawRay(transform.position, Vector2.left * hit.distance, Color.blue);
            } else { toLeft = false; }
        }

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

        transform.position = new Vector2(transform.position.x + joystickInput.movementX * walkSpeed * Time.deltaTime, transform.position.y);

        // CHECK THIS LATER ON, BOTH BOOLS CANT BE CHECKED AT THE SAME TIME
        /*if (!toLeft)
        {
            transform.position = new Vector2(transform.position.x + joystickInput.movementX * walkSpeed * Time.deltaTime, transform.position.y);
        }

        if (!toRight)
        {
            transform.position = new Vector2(transform.position.x - joystickInput.movementX * walkSpeed * Time.deltaTime, transform.position.y);
        }
*/
        if (joystickInput.movementX > 0)
        {
            sprite.flipX = false;
        }
        else if (joystickInput.movementX < 0)
        {
            sprite.flipX = true;
        }

        if (onGround && joystickInput.movementX != 0)
        {
            setCurrentStateTo(run_anim_str);
        }

        if (joystickInput.touchedJumpButton)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector3(0f, jumpSpeed, 0f);
            joystickInput.touchedJumpButton = false;

        }

        if (onGround)
        {
            joystickInput.touchedJumpButton = false;
            coyoteTime = coyoteTimeSet;
        }
    

        if (!onGround)
        {
            coyoteTime -= Time.deltaTime;
            setCurrentStateTo(jump_anim_str);
        }

        // IDLE

        // SECON PARAMETER (DELTED, REPLACED BY EXTERNAL INPUT .CS) = WHEN YOU RUN, JUMP, AND KEEP THE JUMP BUTTON PRESSED, AND THE SUDDENLY RELEASE THE MOVE BUTTON THE RUN ANIMATION WILL KEEP LOOPIN WITHOUT POSITION CHANGE
        // THIS MAKES SURE THAT IF THERE'S 1 TOUCH AND IT IS ON THE JUMP BUTTON THE ANIMATION WILL CHANGE TO IDLE 
        if (Input.touchCount <= 0 && onGround || Input.touchCount == 1 && onGround && _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == jump_anim_str
            || Input.touchCount == 1 && onGround && joystickInput.movementX == 0 && _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == run_anim_str) // || Input.touchCount == 1 && onGround) // REPLACED BY EXTERNAL INPUT .CS // && jumpPressed)
        {
            // REPLACED BY EXTERNAL INPUT .CS
            // jumpPressed = false;

            setCurrentStateTo(idle_anim_str);
        }


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
