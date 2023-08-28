using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headCollider : MonoBehaviour
{
    public GameObject parent, feet;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*if (collision.gameObject.CompareTag("ground"))
        {
            parent.GetComponent<mushRoom>().speed *= -1;
        }*/

        /*else if (collision.gameObject.CompareTag("enemyHead"))
        {
            Debug.Log("test");
            parent.GetComponent<mushRoom>().speed *= -1;
        }*/


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("groundChecker"))
        {
            Debug.Log("test");
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            feet.GetComponent<BoxCollider2D>().enabled = false;
            parent.GetComponent<mushRoom>().enabled = false;
            parent.GetComponent<Rigidbody2D>().gravityScale = 4f;
            parent.GetComponent<Rigidbody2D>().velocity = new Vector3(3, 10, 0f);
            Destroy(parent, 1f);
            //parent.GetComponent<Animator>().Play("hitMushroom");
        }
    }
}
