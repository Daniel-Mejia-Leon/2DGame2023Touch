using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorChecker : MonoBehaviour
{
    public characterScript character;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            character.onGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            character.onGround = false;
        }

        if (collision.CompareTag("enemyHead"))
        {
            character.onEnemy = true;
        }
    }
}
