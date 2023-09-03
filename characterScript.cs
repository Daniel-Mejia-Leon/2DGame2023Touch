using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterScript : MonoBehaviour
{
    Vector2 doNothingVector;
    Animator _animator;
    new Rigidbody2D rigidbody;
    public controlsV2 joystickInput;
    //public bunny buni;
    
    SpriteRenderer sprite;
    [SerializeField] private float walkSpeed, jumpSpeed, minimumValueToStartMovingOnX, coyoteTime, coyoteTimeSet;
    public float life, heartPiecesCounter;
    public bool onGround, toRight, toLeft, toGround, onEnemy, onBox, lifeCounterRunning, ableToTakeDamage, trampolinePush, heartTaken, dead;

    private string
        idle_anim_str = "idle",
        jumpRoll_anim_str = "jumpRoll",
        jump_anim_str = "jump",
        fall_anim_str = "fall",
        run_anim_str = "run",
        dead_anim_str = "dead",
        current_anim;

    [Header("All Sounds")]
    public AudioSource itemTakenSound, heartTakenSound, boxBreakSound;
    [SerializeField] private AudioSource jumpSound, trampolineSound, pedoSound, mushKillSound;

    public GameObject heart1, heart2, heart3, heart11, heart12, heart21, heart22, heart31, heart32;
    // BUGS
    // IF YOU MOVE AND YOU TAP THE TILEMAP TOO, THE CHARACTER WILL ACT WEIRD

    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        _animator = gameObject.GetComponent<Animator>();
        current_anim = idle_anim_str;
        pedoSound.GetComponent<AudioSource>().enabled = false;
        lifeCounterRunning = true;
        ableToTakeDamage = true;
        setCurrentStateTo(idle_anim_str);
        dead = false;
    }


    // Update is called once per frame
    void Update()
    {
        switch (life)
        {
            case 9:
                heart1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                heart11.SetActive(false);
                heart12.SetActive(false);
                // heart1.SetActive(true);
                heart2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                heart21.SetActive(false);
                heart22.SetActive(false);
                // heart2.SetActive(true);
                heart3.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                heart31.SetActive(false);
                heart32.SetActive(false);
                break;

            case 8:
                heart1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                heart11.SetActive(false);
                heart12.SetActive(false);
                // heart1.SetActive(true);
                heart2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                heart21.SetActive(false);
                heart22.SetActive(false);
                // heart2.SetActive(true);
                heart3.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
                heart31.SetActive(true);
                heart32.SetActive(true);
                break;

            case 7:
                heart1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                heart11.SetActive(false);
                heart12.SetActive(false);
                // heart1.SetActive(true);
                heart2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                heart21.SetActive(false);
                heart22.SetActive(false);
                // heart2.SetActive(true);
                heart3.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
                heart31.SetActive(true);
                heart32.SetActive(false);
                break;

            case 6:
                heart1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                heart11.SetActive(false);
                heart12.SetActive(false);
                // heart1.SetActive(true);
                heart2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                heart21.SetActive(false);
                heart22.SetActive(false);
                // heart2.SetActive(true);
                heart3.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
                heart31.SetActive(false);
                heart32.SetActive(false);
                break;

            case 5:
                heart1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                heart11.SetActive(false);
                heart12.SetActive(false);
                // heart1.SetActive(true);
                heart2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
                heart21.SetActive(true);
                heart22.SetActive(true);
                // heart2.SetActive(true);
                heart3.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
                heart31.SetActive(false);
                heart32.SetActive(false);
                break;

            case 4:
                heart1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                heart11.SetActive(false);
                heart12.SetActive(false);
                // heart1.SetActive(true);
                heart2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
                heart21.SetActive(true);
                heart22.SetActive(false);
                // heart2.SetActive(true);
                heart3.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
                heart31.SetActive(false);
                heart32.SetActive(false);
                break;

            case 3:
                heart1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                heart11.SetActive(false);
                heart12.SetActive(false);
                // heart1.SetActive(true);
                heart2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
                heart21.SetActive(false);
                heart22.SetActive(false);
                // heart2.SetActive(true);
                heart3.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
                heart31.SetActive(false);
                heart32.SetActive(false);
                break;

            case 2:
                heart1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
                heart11.SetActive(true);
                heart12.SetActive(true);
                // heart1.SetActive(true);
                heart2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
                heart21.SetActive(false);
                heart22.SetActive(false);
                // heart2.SetActive(true);
                heart3.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
                heart31.SetActive(false);
                heart32.SetActive(false);
                break;

            case 1:
                heart1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
                heart11.SetActive(true);
                heart12.SetActive(false);
                // heart1.SetActive(true);
                heart2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
                heart21.SetActive(false);
                heart22.SetActive(false);
                // heart2.SetActive(true);
                heart3.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
                heart31.SetActive(false);
                heart32.SetActive(false);
                break;

        }

        if (life <= 0 && lifeCounterRunning)
        {
            setCurrentStateTo(dead_anim_str);
            heart1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
            heart11.SetActive(false);
            heart12.SetActive(false);
            // heart1.SetActive(true);
            heart2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
            heart21.SetActive(false);
            heart22.SetActive(false);
            // heart2.SetActive(true);
            heart3.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
            heart31.SetActive(false);
            heart32.SetActive(false);


            pedoSound.GetComponent<AudioSource>().enabled = true;
            
            
            lifeCounterRunning = false;
            
            dead = true;
            gameObject.GetComponent<characterScript>().enabled = false;
        }

        if (heartTaken)
        {

            if (life >= 6)
            {
                life = 6;
            }

            heart3.SetActive(false);
            
        }

        else
        {
            heart3.SetActive(true);
        }

        if (life > 9)
        {
            life = 9;
        }



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
        if (joystickInput.touchedJumpButton) // && ableToTakeDamage) // remove this because it looks like the character doesnt jump when killing an enemy
        {
            rigidbody.velocity = new Vector3(0f, jumpSpeed, 0f);
            joystickInput.touchedJumpButton = false;
            jumpSound.Play();
            onEnemy = false;
        }

        if (onEnemy)
        {
            rigidbody.velocity = new Vector3(0f, jumpSpeed, 0f);
            joystickInput.touchedJumpButton = false;
            mushKillSound.Play();
            onEnemy = false;
        }

        if (onBox)
        {
            rigidbody.velocity = new Vector3(0f, jumpSpeed + 2, 0f);
            joystickInput.touchedJumpButton = false;
            onBox = false;
            boxBreakSound.Play();
            //onEnemy = false;
        }

        if (trampolinePush)
        {
            rigidbody.velocity = new Vector3(0f, jumpSpeed + 12, 0f);
            trampolineSound.Play();
            trampolinePush = false;
        }


        // ON AIR
        if (!onGround)
        {
            coyoteTime -= Time.deltaTime;

            if (rigidbody.velocity.y > 0)
            {
                setCurrentStateTo(jump_anim_str); 
            }

            if (rigidbody.velocity.y < 0)
            {
                setCurrentStateTo(fall_anim_str);
            }

            // PUT THIS HERE BECAUSE IF THE CHARACTER JUMPS RIGHT NEXT TO A MUSHROOM IT WILL DO A DOUBLE JUMP BECAUSE THE onEnemy BOOL WILL BE TRUE (NOT SURE WHY
            // BUT THIS WORKS)
            onEnemy = false;
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

        

    }

    void setCurrentStateTo(string newState)
    {
        if (current_anim == newState){ return; }

        _animator.Play(newState);

        current_anim = newState;

    }

    IEnumerator damageTiltColor()
    {
        ableToTakeDamage = false;

        float loops = 7;

        for (float i = 0; i < loops; i++)
        {
            Color transparent = new Color(gameObject.GetComponent<SpriteRenderer>().color.r, 1, 1, 0.1f);

            gameObject.GetComponent<SpriteRenderer>().color = transparent;

            yield return new WaitForSeconds(.13f);

            Color greenColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            gameObject.GetComponent<SpriteRenderer>().color = greenColor;

            yield return new WaitForSeconds(.13f);

        }

        ableToTakeDamage = true;

    }

    IEnumerator addLifeNoPlus()
    {
        
        yield return new WaitForSeconds(0.2f);
        life+=1;
        Debug.Log("yieldReturn");
    }

    public void damageTaken(int damage)
    {

        if (ableToTakeDamage)
        {
            life -= damage;
            StartCoroutine(damageTiltColor());
        }

        
    }

    void destroyOnDead()
    {
        // I added the item script to get the destroy method for the animation of dead
        //Destroy(gameObject, 0f);
    }
}
