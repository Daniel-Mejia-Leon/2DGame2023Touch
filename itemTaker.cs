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

        if (collision.CompareTag("instaKill"))
        {
            character.damageTaken(3);
        }
    }
}
