using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joystickX : MonoBehaviour
{
    Vector2 referenceInitialPos, joystickInitialPos;
    public GameObject characterToMove, reference, joystick;
    public bool touchedJoyWhenOnCenter;
    public int touchIndexTouchedJoystick, touchCount, walkSpeed, jumpSpeed;
    private float movementX;
    void Start()
    {
        referenceInitialPos = reference.transform.position;
        joystickInitialPos = transform.position;
        touchedJoyWhenOnCenter = false;
    }

    // BUGS TO FIX
    // 1: THERE'S A BUT WHERE IF YOU PUT SOME FINGERS ON THE JOYSTICK SIDE OF THE SCREEN, THEN, WHILE KEEP THOSE FINGERS PRESSING YOU PRESS THE JOYSTICK
    // THE JOYSTICK WILL ACT WEIRD, DECIDE NOT TO FIX THIS FOR NOW SINCE NO ONE WILL PUT 5 FINGERS WHERE JUST THE THUMB IS SUPPOSED TO BE

    void Update()
    {
        touchCount = Input.touchCount;
        //if (Input.touchCount >= 1)
        for (int i = 0; i < Input.touchCount; i++)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.CompareTag("joystickX"))
                {
                    touchedJoyWhenOnCenter = true;
                    joystick.GetComponent<BoxCollider>().enabled = false;
                    touchIndexTouchedJoystick = i;
                }

                if (hit.transform.CompareTag("jumpButton"))
                {
                    characterToMove.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, jumpSpeed, 0f);
                }
            }

            
        }

        // MOVING THE JOYSTICK
        if (Input.touchCount > 0 && touchedJoyWhenOnCenter && touchIndexTouchedJoystick != Input.touchCount)
        {
            // THIS OFFSET = IS JUST THE POSITION OF THE REFERENCE POINT FOR THE BACK AND FORTH POINT OF THE JOYSTICK
            Vector3 offset = new Vector3(reference.transform.position.x, reference.transform.position.y, reference.transform.position.z);

            // THIS VECTOR IS THE VECTOR OF THE TOUCH WITHIN THE CAMERA CONTAINER (ScreenToWorldPoint) MINUS THE OFFSET ABOVE
            Vector2 touchV = Camera.main.ScreenToWorldPoint(Input.GetTouch(touchIndexTouchedJoystick).position) - offset;

            // IF THE TOUCH GOES TO FAR AWAY ON X FROM THE JOYSTICK IT WILL NOT BE TAKEN AS INPUT ANYMORE,
            // THIS WAS SET TO AVOID THE JUMP BUTTON TO INTERRUPT WITH THE JOYSTICK MOVEMENT SINCE IF U REMOVE THIS AND LIFT UR BUTTON FROM THE JOYSTICK WHILE PRESSING THE
            // JUMP BUTTON, THE JUMP BUTTON TOUCH WILL BE TAKEN AS THE INPUT FOR THIS VECTOR
            if (touchV.x > 10f)
            {
                touchedJoyWhenOnCenter = false;
            }/* I DELETED THIS ELSE BECAUSE DIDN'T SEEM TO BE NEEDED ANYMORE
            else if (touchV.x < -4f)
            {
                touchedJoyWhenOnCenter = false;
            }*/

            // THIS IS FOR THE JOYSTICK NOT TO GO OUTSITE ITS CONTAINER // THIS IS BEING APPLIED TO THE VECTOR GOTTEN FORM THE INPUT.TOUCH -
            // THIS CAN/SHOULD ALSO BE APPLIED TO THE JOYSTICK POSITION INSTEAD OF THE VECTOR GOTTEM FROM THE INPUT
            touchV.x = Mathf.Clamp(touchV.x, -1.2f, 1.2f);

            

            // THIS WILL SET THE JOYSTICK POSITION TO THE CALCULATED POITION
            transform.localPosition = new Vector2(touchV.x, 0f);

            // THIS WILL SIMPLY FLIP THE SPRITE ON ITS X AXIS
            if (touchV.x >= 0 || touchV.x > 0)
            {
                movementX = 1;
                GetComponent<SpriteRenderer>().flipX = false;
                characterToMove.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (touchV.x < 0)
            {
                movementX = -1;
                GetComponent<SpriteRenderer>().flipX = true;
                characterToMove.GetComponent<SpriteRenderer>().flipX = true;
            }

            // CHARACTER MOVES
            // characterToMove.transform.position = new Vector2(characterToMove.transform.position.x + transform.localPosition.x * walkSpeed * Time.deltaTime, characterToMove.transform.position.y);
            characterToMove.transform.position = new Vector2(characterToMove.transform.position.x + movementX * walkSpeed * Time.deltaTime, characterToMove.transform.position.y);


        }

        // NO TOUH AT ALL
        if (Input.touchCount <= 0 || !touchedJoyWhenOnCenter || touchIndexTouchedJoystick == Input.touchCount)
        {
            // THIS WILL SET THE REFERENCE AND THE JOYSTICK BACK TO THEIR ORIGINAL POSITION IF NO TOUCH DETECTED
            reference.transform.position = referenceInitialPos;
            transform.position = joystickInitialPos;
            GetComponent<SpriteRenderer>().flipX = false;
            touchedJoyWhenOnCenter = false;
            joystick.GetComponent<BoxCollider>().enabled = true;
            touchIndexTouchedJoystick = -1;
        }

        if (touchIndexTouchedJoystick > Input.touchCount)
        {

        }


        //ScreenToViewportPoint, ViewportToScreenPoint 
    }
}
