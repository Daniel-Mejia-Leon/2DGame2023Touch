using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mushRoom : MonoBehaviour
{
    public float speed;
    public bool colDetected;
    public AudioSource mushKillSound;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x - speed * Time.deltaTime, gameObject.transform.position.y, gameObject.transform.position.z);

        if (speed < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    void killedByCharacter()
    {
              
        Destroy(gameObject, 0f); 

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ground") || collision.CompareTag("enemy") || collision.CompareTag("characterWallChecker"))
        {
            speed *= -1;
        }


    }

}
