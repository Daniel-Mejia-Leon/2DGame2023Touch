using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disItem : MonoBehaviour
{
    private bool start = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            StartCoroutine(TiltColor());
            start = false;
        }
    }

    IEnumerator TiltColor()
    {
        yield return new WaitForSeconds(5);
        
        float loops = 7;

        for (float i = 0; i < loops; i++)
        {
            Color transparent = new Color(gameObject.GetComponent<SpriteRenderer>().color.r, 1, 1, 0.1f);

            gameObject.GetComponent<SpriteRenderer>().color = transparent;

            yield return new WaitForSeconds(.13f);

            Color originalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            gameObject.GetComponent<SpriteRenderer>().color = originalColor;

            yield return new WaitForSeconds(.13f);

        }

        if (gameObject.GetComponent<Animator>().GetBool("taken") != true)
        {
            Destroy(gameObject, 0f);
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("ground"))
        {

            gameObject.GetComponent<CircleCollider2D>().isTrigger = true;

            Destroy(gameObject.GetComponent<Rigidbody2D>(), 0f);
        }
    }

}
