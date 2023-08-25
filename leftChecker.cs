using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leftChecker : MonoBehaviour
{
    public characterScript character;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            character.toLeft = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            character.toLeft = false;
        }
    }
}
