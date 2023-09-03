using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headColliderBunny : MonoBehaviour
{
    public bool hit;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("groundChecker"))
        {
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;

            hit = true;
        }    
    }

}
