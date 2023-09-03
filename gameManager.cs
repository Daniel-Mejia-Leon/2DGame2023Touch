using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public characterScript character;
    public GameObject text;
    void Start()
    {
        text.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (character.dead)
        {
            text.SetActive(true);
        }
    }
}
