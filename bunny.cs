using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bunny : MonoBehaviour
{
    Animator _animator;
    public headColliderBunny head;
    public floorCheckerPig floor;
    public float speed, jumpForce, rayDistanceLeft, rayDistanceRight;
    public bool running, heartTaken, raycastActive;
    public GameObject upLeft, upRight, heart, heartItem, parent, headColliderObject;
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        heart.SetActive(false);
        heartItem.SetActive(false);
        headColliderObject.GetComponent<CapsuleCollider2D>().enabled = false;
        headColliderObject.GetComponent<headColliderBunny>().enabled = false;
        //raycastActive = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (head.hit)
        {
            _animator.Play("hit");
        }

        else if (floor.onGround)
        {
            if (running)
            {
                _animator.Play("run");
            }
            else
            {
                _animator.Play("idle");
            }
        }

        else
        {
            _animator.Play("jump");
        }

        if (floor.jumpOnPlayer)
        {
            speed *= -1;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, jumpForce);
        }

        if (running)
        {
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
        }

        if (speed < 0) // <--
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            rayDistanceLeft = 3f;
            rayDistanceRight = 0f;
        }

        else if (speed > 0) // -->
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            rayDistanceLeft = 0;
            rayDistanceRight = 3f;
        }

        RaycastHit2D[] rayToRight = Physics2D.RaycastAll(gameObject.transform.position, Vector2.right, rayDistanceRight);

        foreach (RaycastHit2D hit in rayToRight)
        {
            if (hit.transform.tag == "ground"  && raycastActive || hit.transform.tag == "Player" && heartTaken && raycastActive )
            {
                //speed *= -1;
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(2f, jumpForce);
                Debug.DrawRay(transform.position, Vector2.right * hit.distance, Color.blue);
            }

        }

        RaycastHit2D[] rayToRightUp = Physics2D.RaycastAll(upRight.transform.position, Vector2.right, rayDistanceRight);

        foreach (RaycastHit2D hit in rayToRightUp)
        {
            if (hit.transform.tag == "ground" && raycastActive )
            {
                speed *= -1;
                Debug.DrawRay(upRight.transform.position, Vector2.right * hit.distance, Color.blue);
            }

        }

        RaycastHit2D[] rayToLeft = Physics2D.RaycastAll(gameObject.transform.position, Vector2.left, rayDistanceLeft);

        foreach (RaycastHit2D hit in rayToLeft)
        {
            if (hit.transform.tag == "ground"  && raycastActive || hit.transform.tag == "Player" && heartTaken && raycastActive )
            {
                //speed *= -1;
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-2f, jumpForce);
                Debug.DrawRay(transform.position, Vector2.left * hit.distance, Color.blue);
            }

        }

        RaycastHit2D[] rayToLeftUp = Physics2D.RaycastAll(upLeft.transform.position, Vector2.left, rayDistanceLeft);

        foreach (RaycastHit2D hit in rayToLeftUp)
        {
            if (hit.transform.tag == "ground" && raycastActive )
            {
                speed *= -1;
                Debug.DrawRay(upLeft.transform.position, Vector2.left * hit.distance, Color.blue);
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //raycastActive = true;
            heartTaken = true;
            heart.SetActive(true);
            running = true;
            headColliderObject.GetComponent<CapsuleCollider2D>().enabled = true;
            headColliderObject.GetComponent<headColliderBunny>().enabled = true;
        }
    }

    void killedByPlayer()
    {
        _animator.enabled = false;
        
        Destroy(heart, 0f);
        Destroy(gameObject, 55f);
    }

    void jumpKilled()
    {
        heartTaken = false;
        speed = 0f;
        raycastActive = false;
        heart.SetActive(false);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(4f, 10, 0);
        heartItem.transform.SetParent(parent.transform);
        heartItem.SetActive(true);
        heartItem.GetComponent<Rigidbody2D>().velocity = new Vector3(4f, 10, 0);
    }
}
