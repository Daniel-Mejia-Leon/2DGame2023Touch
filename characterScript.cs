﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterScript : MonoBehaviour
{
    Vector2 doNothingVector;
    Animator _animator;
    public controlsV2 joystickInput;
    
    SpriteRenderer sprite;
    [SerializeField] private float walkSpeed, jumpSpeed, minimumValueToStartMovingOnX, coyoteTime, coyoteTimeSet;
    public float life;
    public bool onGround, toRight, toLeft, toGround, onEnemy, dead, lifeCounterRunning, ableToTakeDamage;

    private string
        idle_anim_str = "idle",
        jump_anim_str = "jump",
        run_anim_str = "run",
        dead_anim_str = "dead",
        current_anim;

    [Header("All Sounds")]
    [SerializeField] private AudioSource itemTakenSound;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource pedoSound;

    // BUGS
    // IF YOU MOVE AND YOU TAP THE TILEMAP TOO, THE CHARACTER WILL ACT WEIRD

    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        _animator = gameObject.GetComponent<Animator>();
        current_anim = idle_anim_str;
        dead = false;
        pedoSound.GetComponent<AudioSource>().enabled = false;
        lifeCounterRunning = true;
        ableToTakeDamage = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (life <= 0 && lifeCounterRunning)
        {
            Debug.Log("dead");
            
            dead = true;
            pedoSound.GetComponent<AudioSource>().enabled = true;
            gameObject.GetComponent<characterScript>().enabled = false;
            setCurrentStateTo(dead_anim_str);
            
            lifeCounterRunning = false;
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
        if (joystickInput.touchedJumpButton || onEnemy && ableToTakeDamage)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector3(0f, jumpSpeed, 0f);
            joystickInput.touchedJumpButton = false;
            jumpSound.Play();
            onEnemy = false;
        }

        // ON AIR
        if (!onGround)
        {
            coyoteTime -= Time.deltaTime;
            setCurrentStateTo(jump_anim_str);

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

        float loops = 3;

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

    public void damageTaken()
    {
        if (ableToTakeDamage)
        {
            life -= 1;
            StartCoroutine(damageTiltColor());
        }

        
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

    void destroyOnDead()
    {
        Destroy(gameObject, 0f);
    }
}
