using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterScript : MonoBehaviour
{
    Animator _animator;
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

        for (int i = 0; i < Input.touchCount; i++)
        {
            Ray rayFromCamera = Camera.main.ScreenPointToRay(Input.touches[i].position);
            RaycastHit hit;
            if (!Physics.Raycast(rayFromCamera, out hit, 100)) { return; }

            else if (Physics.Raycast(rayFromCamera, out hit, 100))
            {
                // THIS IS TO AVOID GETTING STUCK IN THE ANIMATION OF RUNNING BY PRESSING FORWARD/BACKWARD + ANY OTHER PLACE THAT IS NOT TAGGED AS BELOW;
                if (i == 1 && !hit.transform.CompareTag("forwardButton") && !hit.transform.CompareTag("backwardButton") && !hit.transform.CompareTag("jumpButton"))
                {
                    continue;
                }

                // MOVING FORWARD
                if (hit.transform.CompareTag("forwardButton")) 
                {
                    sprite.flipX = false;
                    /*if (Input.GetTouch(i).phase == TouchPhase.Stationary || Input.GetTouch(i).phase == TouchPhase.Moved)
                    {
                        hit.collider.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1); // Mathf.Lerp());
                    }
                    else if (Input.GetTouch(i).phase == TouchPhase.Ended)
                    {
                        hit.collider.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.40f);
                    }*/

                    if (!toRight)
                    {
                        transform.position = new Vector2(transform.position.x + walkSpeed * Time.deltaTime, transform.position.y);
                    }
                    

                    if (onGround)
                    {
                        setCurrentStateTo(run_anim_str);
                    }
                }
                /*else if (!hit.transform.CompareTag("forwardButton"))
                {
                    // hit.collider.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.81f);
                }*/

                // MOVING BACKWARDS (-FORWARD)
                else if (hit.transform.CompareTag("backwardButton")) 
                {
                    sprite.flipX = true;

                    if (!toLeft)
                    {
                        transform.position = new Vector2(transform.position.x - walkSpeed * Time.deltaTime, transform.position.y);
                    }

                    if (onGround)
                    {
                        setCurrentStateTo(run_anim_str);
                    }

                }

                // JUMP 
                else if (hit.transform.CompareTag("jumpButton") && Input.GetTouch(i).phase == TouchPhase.Began && onGround ||
                    hit.transform.CompareTag("jumpButton") && Input.GetTouch(i).phase == TouchPhase.Began && !toGround && coyoteTime >= 0)
                {
                    jumpSound.Play();
                    jumpPressed = true;
                    GetComponent<Rigidbody2D>().velocity = new Vector3(0f, jumpSpeed, 0f);
                }

                // THIS PREVENTS THE JUMP ANIMATION TO KEEP LOOPING IF YOU KEEP THE JUMP BUTTON PRESSED
                if (hit.transform.CompareTag("jumpButton") && Input.GetTouch(i).phase == TouchPhase.Moved && onGround && _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == jump_anim_str ||
                    hit.transform.CompareTag("jumpButton") && Input.GetTouch(i).phase == TouchPhase.Stationary && onGround && _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == jump_anim_str)
                {
                    setCurrentStateTo(idle_anim_str);
                }

                // IF YOU SLIDE THE TOUCH FROM ANY MOVING BUTTON TO ANY PLACE OUTSIDE THE COLLIDER THE ANIMATION WILL STILL RUN, THIS IS TO AVOID IT
                if (!hit.transform.CompareTag("forwardButton") && !hit.transform.CompareTag("backwardButton") && !hit.transform.CompareTag("jumpButton") && onGround)
                {
                    Debug.Log("this");
                    setCurrentStateTo(idle_anim_str);
                    //hit.collider.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.40f);

                }

            }
            
        }
        // set x axix to make if x > 1 && !onground >> not idle

        if (onGround)
        {
            coyoteTime = coyoteTimeSet;
        }
    

        if (!onGround)
        {
            coyoteTime -= Time.deltaTime;
            setCurrentStateTo(jump_anim_str);
        }

        // IDLE

        // SECON PARAMETER = WHEN YOU RUN, JUMP, AND KEEP THE JUMP BUTTON PRESSED, AND THE SUDDENLY RELEASE THE MOVE BUTTON THE RUN ANIMATION WILL KEEP LOOPIN WITHOUT POSITION CHANGE
        // THIS MAKES SURE THAT IF THERE'S 1 TOUCH AND IT IS ON THE JUMP BUTTON THE ANIMATION WILL CHANGE TO IDLE 
        if (Input.touchCount <= 0 && onGround || Input.touchCount == 1 && onGround && jumpPressed)
        {
            jumpPressed = false;

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
