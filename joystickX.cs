using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joystickX : MonoBehaviour
{
    Vector2 referenceInitialPos, joystickInitialPos;
    public GameObject reference;
    void Start()
    {
        referenceInitialPos = reference.transform.position;
        joystickInitialPos = transform.position;
    }

    // -2.789999 Y
    void Update()
    {
        //if (Input.touchCount >= 1)
        for (int i = 0; i < Input.touchCount; i++)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.CompareTag("joystickX"))
                {
                    // THIS OFFSET = IS JUST THE POSITION OF THE REFERENCE POINT FOR THE BACK AND FORTH POINT OF THE JOYSTICK
                    Vector3 offset = new Vector3(reference.transform.position.x, reference.transform.position.y, reference.transform.position.z);

                    // THIS VECTOR IS THE VECTOR OF THE TOUCH WITHIN THE CAMERA CONTAINER (ScreenToWorldPoint) MINUS THE OFFSET ABOVE
                    Vector2 touchV = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position) - offset;

                    // THIS IS FOR THE JOYSTICK NOT TO GO OUTSITE ITS CONTAINER
                    touchV.x = Mathf.Clamp(touchV.x, -2, 2);

                    // THIS WILL SET THE JOYSTICK POSITION TO THE CALCULATED POITION
                    transform.localPosition = new Vector2(touchV.x, 0f);

                    if (touchV.x >= 0)
                    {
                        GetComponent<SpriteRenderer>().flipX = false;
                    }
                    else if (touchV.x < 0)
                    {
                        GetComponent<SpriteRenderer>().flipX = true;
                    }
                }
            }

            

            //Debug.Log(touchV.x);
        }

        if (Input.touchCount <= 0)
        {
            // THIS WILL SET THE REFERENCE AND THE JOYSTICK BACK TO THEIR ORIGINAL POSITION IF NO TOUCH DETECTED
            reference.transform.position = referenceInitialPos;
            transform.position = joystickInitialPos;
            GetComponent<SpriteRenderer>().flipX = false;
        }


        //ScreenToViewportPoint, ViewportToScreenPoint 
    }
}
