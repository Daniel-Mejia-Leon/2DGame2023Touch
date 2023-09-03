using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorCheckerPig : MonoBehaviour
{
    public bool onGround, jumpOnPlayer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            onGround = true;
        }
        
        if (collision.CompareTag("Player"))
        {
            jumpOnPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            onGround = false;
        }
        
        if (collision.CompareTag("Player"))
        {
            jumpOnPlayer = false;
        }
    }
}
