using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plant : MonoBehaviour
{
    public GameObject character, body, cannon, cannonBallShot;
    public AudioSource plantShootSound;
    public float speedBullet;
    Animator _animator;
    GameObject cannonBall;
    List<GameObject> cannonBallList = new List<GameObject>();
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();

        if (gameObject.GetComponent<SpriteRenderer>().flipX == true)
        {
            cannon.transform.position = new Vector3(cannon.transform.position.x + 3.5f, cannon.transform.position.y, cannon.transform.position.z);
        }

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerRelativePosition = gameObject.transform.InverseTransformPoint(character.transform.position);

        //Debug.Log(playerRelativePosition);

        if (gameObject.GetComponent<SpriteRenderer>().flipX == false)
        {
            if (playerRelativePosition.y < 1 && playerRelativePosition.y > -1f && playerRelativePosition.x > -3f && playerRelativePosition.x < 0.5f)
            {
                _animator.Play("attack");
            }

            else
            {
                _animator.Play("idle");
            }

        }

        else if (gameObject.GetComponent<SpriteRenderer>().flipX == true)
        {
            if (playerRelativePosition.y < 1 && playerRelativePosition.y > -1f && playerRelativePosition.x < 3f && playerRelativePosition.x > -0.5f)
            {
                _animator.Play("attack");
            }

            else
            {
                _animator.Play("idle");
            }
        }
        

        foreach (GameObject cannonBall in cannonBallList)
        {
            if (cannonBall != null)
            {
                if (gameObject.GetComponent<SpriteRenderer>().flipX == false)
                {
                    cannonBall.transform.position = new Vector2(cannonBall.transform.position.x - speedBullet * Time.deltaTime, cannonBall.transform.position.y);
                    Destroy(cannonBall, 2f);

                }

                else if (gameObject.GetComponent<SpriteRenderer>().flipX == true)
                {
                    cannonBall.GetComponent<SpriteRenderer>().flipX = true;
                    cannonBall.transform.position = new Vector2(cannonBall.transform.position.x + speedBullet * Time.deltaTime, cannonBall.transform.position.y);
                    Destroy(cannonBall, 2f);
                }
            }
        }


        /*laser.transform.up = character.transform.position - laser.transform.position;

        RaycastHit2D[] raysToCharacter = Physics2D.RaycastAll(laser.transform.position, character.transform.position - laser.transform.position, laserDistance);

        foreach (RaycastHit2D hit in raysToCharacter)
        {
            if (hit.transform.tag == "Player")
            {
                Debug.DrawRay(laser.transform.position, (character.transform.position - laser.transform.position) * hit.distance, Color.blue);
            }
            
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("groundChecker"))
        {
            //_animator.enabled = false;
            //Debug.Log("test");
            gameObject.GetComponent<Animator>().Play("hit");
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<plant>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 4f;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(3, 10, 0f);
            body.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject, 1f);
            //parent.GetComponent<Animator>().Play("hitMushroom");
        }
    }

    void cannonBallInstant()
    {
        plantShootSound.Play();
        cannonBall = Instantiate(cannonBallShot, cannon.transform.position, Quaternion.identity);
        cannonBallList.Add(cannonBall);

    }

    private void OnDisable()
    {
        if (cannonBall != null)
        {
            Destroy(cannonBall);
        }

        Destroy(GameObject.Find("Bullet(Clone)"));
    }
}
