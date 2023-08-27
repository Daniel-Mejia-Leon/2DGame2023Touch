using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joystickX : MonoBehaviour
{
    // "This object will be used as the offset for our touch x, -x and it must perfectly align with the joystick x axis
    Vector2 referenceInitialPos;
    // This must be the Joystick initial position so that it can be set back to its original position if no input detected
    Vector2 joystickInitialPos;
    // This will be the scale of the jump button when pressed
    private float toScaleButtonOnPressed = 0.7f;
    // Jump button will be scaled to
    Vector2 toScaleButtonVector2;
    // Original size of jump button
    Vector2 originalButtonSize;

    [Tooltip("To get the on ground bool")]
    public characterScript character;
    [Tooltip("Reference point for the camera offset")] 
    public GameObject reference;
    [Tooltip("Joystick")]
    public GameObject joystick;
    [Tooltip("Jump button")]
    public GameObject jumpButton;
    private bool touchedJoyWhenOnCenter;
    // This bool will be accessed from the movement.cs for the jump
    [Tooltip("This bool will be accessed from the movement.cs for the jump")]
    public bool touchedJumpButton;
    private int touchIndexTouchedJoystick;
    [Tooltip("movement output on the X axis")]
    public float getAxisRaw, getAxisX;
    public Vector2 touchV;

    void Start()
    {
        originalButtonSize = jumpButton.transform.localScale;
        toScaleButtonVector2 = new Vector2(toScaleButtonOnPressed, toScaleButtonOnPressed);
        referenceInitialPos = new Vector2(0, 0);
        joystickInitialPos = new Vector2(0, 0);
        //Debug.Log(joystickInitialPos);
        touchedJoyWhenOnCenter = false;
        touchedJumpButton = true;
    }

    // BUGS TO FIX
    // 1: THERE'S A BUT WHERE IF YOU PUT SOME FINGERS ON THE JOYSTICK SIDE OF THE SCREEN, THEN, WHILE KEEP THOSE FINGERS PRESSING YOU PRESS THE JOYSTICK
    // THE JOYSTICK WILL ACT WEIRD, DECIDE NOT TO FIX THIS FOR NOW SINCE NO ONE WILL PUT 5 FINGERS WHERE JUST THE THUMB IS SUPPOSED TO BE
    // 2: WHEN HOLDING THE JUMP BUTTON PRESSED AND SLIDE IT OUT OF THE BUTTON, THE BUTTON WILL GET STUCK IN ITS PRESSED ANIMATION (SCALE)

    void Update()
    {

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

                // WE MUST REQUIRE AN EXTERNAL INPUT FORM THE CHARACTER (public bool onGround) TO SET IT IN THIS IF, OTHERWISE IT WILL KEEP JUMPING FOREVER WHEN .Began
                if (hit.transform.CompareTag("jumpButton"))
                {
                    if (Input.GetTouch(i).phase == TouchPhase.Began)
                    {
                        if (character.onGround)
                        {
                            // THIS VALUE MUST BE SET BACK TO FALSE EXTERNALLY (WHEN CHARACTER TOUCHES THE GROUND AGAIN)
                            touchedJumpButton = true;

                            //
                            hit.collider.gameObject.transform.localScale = toScaleButtonVector2;
                            //StartCoroutine(setBackButtonToNormalSize(hit.collider.gameObject, originalButtonSize, toScaleButtonVector2));  
                        }
                        else
                        {
                            hit.collider.gameObject.transform.localScale = toScaleButtonVector2;
                            //StartCoroutine(setBackButtonToNormalSize(hit.collider.gameObject, originalButtonSize, toScaleButtonVector2));
                        }

                    }

                    if (Input.GetTouch(i).phase == TouchPhase.Ended)
                    {
                        hit.collider.gameObject.transform.localScale = originalButtonSize;
                    }
                }

                if (hit.transform.CompareTag("backgroundCol"))
                {
                    jumpButton.transform.localScale = originalButtonSize;
                }

            }
  
        }

        // MOVING THE JOYSTICK
        if (Input.touchCount > 0 && touchedJoyWhenOnCenter && touchIndexTouchedJoystick != Input.touchCount)
        {
            // THIS OFFSET = IS JUST THE POSITION OF THE REFERENCE POINT FOR THE BACK AND FORTH POINT OF THE JOYSTICK
            Vector3 offset = new Vector3(reference.transform.position.x, reference.transform.position.y, reference.transform.position.z);

            // THIS VECTOR IS THE VECTOR OF THE TOUCH WITHIN THE CAMERA CONTAINER (ScreenToWorldPoint) MINUS THE OFFSET ABOVE
            touchV = Camera.main.ScreenToWorldPoint(Input.GetTouch(touchIndexTouchedJoystick).position) - offset;

            // IF THE TOUCH GOES TO FAR AWAY ON X FROM THE JOYSTICK IT WILL NOT BE TAKEN AS INPUT ANYMORE,
            // THIS WAS SET TO AVOID THE JUMP BUTTON TO INTERRUPT WITH THE JOYSTICK MOVEMENT SINCE IF U REMOVE THIS AND LIFT UR BUTTON FROM THE JOYSTICK WHILE PRESSING THE
            // JUMP BUTTON, THE JUMP BUTTON TOUCH WILL BE TAKEN AS THE INPUT FOR THIS VECTOR
            if (touchV.x > 10f)
            {
                touchedJoyWhenOnCenter = false;
            }

            // THIS IS FOR THE JOYSTICK NOT TO GO OUTSITE ITS CONTAINER // THIS IS BEING APPLIED TO THE VECTOR GOTTEN FORM THE INPUT.TOUCH -
            // THIS CAN/SHOULD ALSO BE APPLIED TO THE JOYSTICK POSITION INSTEAD OF THE VECTOR GOTTEM FROM THE INPUT
            touchV.x = Mathf.Clamp(touchV.x, -1.2f, 1.2f);

            // THIS WILL SET THE JOYSTICK POSITION TO THE CALCULATED POITION
            transform.localPosition = new Vector2(touchV.x, 0f);

            // THIS WILL SIMPLY FLIP THE SPRITE ON ITS X AXIS
            if (touchV.x >= 0 || touchV.x > 0)
            {
                getAxisRaw = 1;  // TO MODIFY
                GetComponent<SpriteRenderer>().flipX = false;
                // characterToMove.GetComponent<SpriteRenderer>().flipX = false; // TO MODIFY
            }
            else if (touchV.x < 0)
            {
                getAxisRaw = -1;
                GetComponent<SpriteRenderer>().flipX = true;
                // characterToMove.GetComponent<SpriteRenderer>().flipX = true; // TO MODIFY
            }

            getAxisX = touchV.x;

        }

        // NO TOUH AT ALL
        if (Input.touchCount <= 0 || !touchedJoyWhenOnCenter || touchIndexTouchedJoystick == Input.touchCount)
        {
            // THIS WILL SET THE REFERENCE AND THE JOYSTICK BACK TO THEIR ORIGINAL POSITION IF NO TOUCH DETECTED
            reference.transform.localPosition = referenceInitialPos;
            transform.localPosition = joystickInitialPos;
            //Debug.Log(joystickInitialPos);
            GetComponent<SpriteRenderer>().flipX = false;
            touchedJoyWhenOnCenter = false;
            joystick.GetComponent<BoxCollider>().enabled = true;
            touchIndexTouchedJoystick = -1;
            getAxisRaw = 0;
        }

        if (transform.localPosition.x == 0)
        {
            touchV = new Vector2(0, 0);
            getAxisX = 0f;
        }

        //Debug.Log(touchV.x);

    }

    // THIS WASN'T USED
    IEnumerator setBackButtonToNormalSize(GameObject buttonToSetBackToNormal, Vector2 originalSize, Vector2 sizeOnPressed)
    {
        buttonToSetBackToNormal.transform.localScale = sizeOnPressed;
        yield return new WaitForSeconds(0.1f);
        buttonToSetBackToNormal.transform.localScale = originalSize;
    }
}
