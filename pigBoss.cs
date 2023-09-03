using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pigBoss : MonoBehaviour
{
    Animator _animator;
    Rigidbody2D rb;
    public characterScript character;
    //public floorCheckerPig floorPig;
    public rbDestroyer ray;
    public bool walking, running, hit, littlePig, lastPigs, ableToJump, onGround;
    public float speed, life, distanceToStartJump, forceToJump;
    [SerializeField] GameObject bodyCollider, head, pig1, pig2, megaParent, jumpPlace, jumpPlace1, jumpPlace2;
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        //rb = gameObject.GetComponent<Rigidbody2D>();
        if (pig1 != null && pig2 != null)
        {
            pig1.SetActive(false);
            pig2.SetActive(false);
        }
        ableToJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        float randomVal =  Random.Range(0, 150);

        if (randomVal == 10 && onGround)
        {
            speed *= -1;
        }

        jump(jumpPlace);
        jump(jumpPlace1);
        jump(jumpPlace2);

        /*if (!floorPig.onGround)
        {
            if (gameObject.GetComponent<Rigidbody2D>() == null)
            {
                gameObject.AddComponent<Rigidbody2D>();
                gameObject.GetComponent<Rigidbody2D>().freezeRotation = true;
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 2;
            } 
        }*/ // else { The ray is in charge of removing the rigid body if the user is on ground }

        // I decided to use a coroutine to true ableToJump since the onground triggerEnter flips the speed;
        /*if (floorPig.onGround)
        {
            ableToJump = true;
        }*/

        if (littlePig)
        {
            running = true;
        }

        if (life <= 0)
        {
            if (!lastPigs && pig1 != null && pig2 != null)
            {
                pig1.SetActive(true);
                pig2.SetActive(true);
                pig1.GetComponent<pigBoss>().speed *= -1;
                pig1.GetComponent<Rigidbody2D>().velocity = new Vector2(-2, 7);
                pig2.GetComponent<Rigidbody2D>().velocity = new Vector2(2, 9);
                pig1.transform.SetParent(megaParent.transform);
                pig2.transform.SetParent(megaParent.transform);

                Destroy(gameObject, 0f); 
            }

            if (lastPigs)
            {
                
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 10f;
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(2, 15);
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                bodyCollider.GetComponent<BoxCollider2D>().enabled = false;
                head.GetComponent<CapsuleCollider2D>().isTrigger = true;
                Destroy(gameObject, 1f);
                lastPigs = false;
            }
        }

        if (!character.ableToTakeDamage)
        {
            //bodyCollider.GetComponent<BoxCollider2D>().isTrigger = true;
            head.GetComponent<CapsuleCollider2D>().enabled = false;
            //gameObject.GetComponent<BoxCollider2D>().enabled = false;

        }

        else
        {
            // bodyCollider.GetComponent<BoxCollider2D>().isTrigger = false;
            head.GetComponent<CapsuleCollider2D>().enabled = true;

            // gameObject.GetComponent<BoxCollider2D>().enabled = true;

        }

        if (hit)
        {
            _animator.Play("hitOpenEyes");
        }

        else if (walking)
        {
            _animator.Play("walk");

            gameObject.transform.position = new Vector3(gameObject.transform.position.x + speed * Time.deltaTime, gameObject.transform.position.y, gameObject.transform.position.z);

            if (speed > 0)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }

            else if (speed < 0)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
        }

        else if (running)
        {
            _animator.Play("run");

            if (life >= 0)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x + speed * 1.5f * Time.deltaTime, gameObject.transform.position.y, gameObject.transform.position.z);

            }

            if (speed > 0)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }

            else if (speed < 0)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ground") && life > 0) //  || collision.CompareTag("enemy"))
        {
            speed *= -1;
        }


    }

    void hitBoolOff()
    {
        life -= 1f;
        hit = false;

        if (life > 0)
        {
            running = true;
        }

        else
        {
            _animator.enabled = false;
            running = false;
            speed = 0;
        }
    }

    IEnumerator setTrueAbleToJump()
    {
        yield return new WaitForSeconds(1f);
        ableToJump = true;
    }

    void jump(GameObject jumpPlace)
    {
        float distanceToJumpObject = Vector2.Distance(gameObject.transform.position, jumpPlace.transform.position);

        if (distanceToJumpObject < distanceToStartJump && ableToJump)
        {
            //Debug.Log("this");
            //rbdestro.rayActive = false;
            //gameObject.AddComponent<Rigidbody2D>();
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, forceToJump);
            //gameObject.GetComponent<Rigidbody2D>().freezeRotation = true;
            //
            ableToJump = false;
            StartCoroutine(setTrueAbleToJump());

        }

    }
}
