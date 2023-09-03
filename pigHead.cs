using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pigHead : MonoBehaviour
{
    public GameObject parent;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("groundChecker"))
        {
            parent.GetComponent<pigBoss>().hit = true;
        }
    }
}
