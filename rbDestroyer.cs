using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rbDestroyer : MonoBehaviour
{
    public pigBoss pig;
    public float distanceToFloor;
    public bool rayActive;
    void Start()
    {
        rayActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D[] raysDown = Physics2D.RaycastAll(gameObject.transform.position, Vector2.down, distanceToFloor);

        foreach (RaycastHit2D hit in raysDown)
        {
            if (hit.transform.tag == "ground" && rayActive)
            {
                //Removed this because it wont be useful anmyore since I added the pigs to the pigs layermask where they dont have collision.
                //So having the rb in the objects wont affect them by colliding with each other
                //Destroy(gameObject.GetComponent<Rigidbody2D>());
                Debug.DrawRay(transform.position, Vector2.down * hit.distance, Color.blue);
                pig.onGround = true;
                //pig.ableToJump = true;
            }
            else { pig.onGround = false; }
        }

        /*if (false)
        {
            // IF RAY IS NOT TOUCHING THE GROND THEN THE RB MUST BE ADDED BACK SO TAHT THE PIG CAN FALL
        }*/
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            pig.onGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            pig.onGround = false;

        }
    }*/
}
