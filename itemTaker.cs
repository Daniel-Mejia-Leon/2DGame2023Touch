using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemTaker : MonoBehaviour
{
    public characterScript character;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("item"))
        {
            collision.GetComponent<CircleCollider2D>().enabled = false;
            collision.GetComponent<Animator>().SetBool("taken", true);
            character.itemTakenSound.Play();
            character.life++;
        }

        if (collision.CompareTag("itemFullLife"))
        {
            collision.GetComponent<CircleCollider2D>().enabled = false;
            collision.GetComponent<Animator>().SetBool("taken", true);
            character.heartTakenSound.Play();
            character.life=9;
        }

        if (collision.CompareTag("heartFromBunny"))
        {
            character.heartTaken = false;
            collision.GetComponent<CircleCollider2D>().enabled = false;
            collision.GetComponent<Rigidbody2D>().gravityScale = 0f;
            collision.GetComponent<Animator>().SetBool("taken", true);
            character.heartTakenSound.Play();
            //character.life = 9;
        }

        if (collision.CompareTag("instaKill"))
        {
            character.damageTaken(3);
        }
    }
}
