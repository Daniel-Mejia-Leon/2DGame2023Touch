using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rightChecker : MonoBehaviour
{
    public characterScript character;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            character.toRight = true;
        }

        else if (collision.CompareTag("mushroom"))
        {
            character.damageTaken();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("mushroom"))
        {
            character.damageTaken();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            character.toRight = false;
        }
    }
}
