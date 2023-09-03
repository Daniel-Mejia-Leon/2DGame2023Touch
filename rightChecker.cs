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

        else if (collision.CompareTag("enemy"))
        {
            character.damageTaken(1);
        }

        else if (collision.CompareTag("bullet"))
        {
            character.damageTaken(1);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            character.damageTaken(1);
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
