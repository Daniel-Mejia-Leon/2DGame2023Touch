using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bee : MonoBehaviour
{
    public GameObject character, distanceObject;
    public float speed, distance, distanceToStop;
    Animator _animator;
    Vector3 initialPos;

    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        initialPos = gameObject.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        float distanceToKeepFollowing = Vector3.Distance(character.transform.position, distanceObject.transform.position);
        float distanceToCharacter = Vector3.Distance(character.transform.position, gameObject.transform.position);

        if (distanceToKeepFollowing < distance)
        {
            gameObject.transform.up = gameObject.transform.position - character.transform.position;

            if (distanceToCharacter > distanceToStop)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, character.transform.position, speed * Time.deltaTime);  
            }
        }
        else
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, initialPos, speed * 3 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _animator.Play("attack");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _animator.Play("idle");
        }
    }
}
