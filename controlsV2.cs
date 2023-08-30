using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlsV2 : MonoBehaviour
{
    // THE OBJECTS TO CREATE AND THEIR HIERARCHY ARE THESE
    // ControlSystem (this script)
        // Background collider for joystick
        // Background collider for jump button
        // JoystickParent // Also Dummy but without scripts and children with fadeOut animations
            //JoystickSprite (Animator with fadeIn animation)
            //JoystickHoleSprite (Animator with fadeIn animation)
            //JoysticCenterSprite (Animator with fadeIn animation)
            //JoystickReference (object which determines the value on x of the joystick)
        // JumpButtonParent
            //JumpButtonSprite (With a box collider)
            //JumpButtonHolle


    // "This object will be used as the offset for our touch x, -x and it must perfectly align with the joystick x axis
    Vector2 referenceInitialPos;
    // This must be the Joystick initial position so that it can be set back to its original position if no input detected
    Vector2 joystickInitialPos;
    // This will be the scale of the jump button when pressed
    private float toScaleButtonOnPressed = 1.8f;
    // Jump button will be scaled to
    Vector2 toScaleButtonVector2;
    // Original size of jump button
    Vector2 originalButtonSize;

    [Tooltip("To get the on ground bool")]
    public characterScript character;

    [Tooltip("Reference point for the camera offset")]
    public GameObject reference;

    [Tooltip("Joystick")]
    public GameObject joystickParent;
    public GameObject joystick;
    public GameObject joystickHole;

    [Tooltip("Jump button")]
    public GameObject jumpButton;

    [Tooltip("")]
    public GameObject dummyJoystick;
    private GameObject dummyInstantiated;
    public GameObject colliderForJoystick;

    [Tooltip("Dummy joystick parent")]
    public GameObject virtualCameraObject;

    private bool pressingJoystick, ableToActivateDummyJoystick;

    // This bool will be accessed from the movement.cs for the jump
    [Tooltip("This bool will be accessed from the movement.cs for the jump")]
    public bool touchedJumpButton;

    // THIS WAS CREATED TO STORE THE INPUT.GETTOUCH(i) AND PASS IT TO THE JOYSTICK MOVEMENT
    private int touchIndexTouchedJoystick;

    // THESE VALUES ARE THE X MOVEMENT WHICH CAN BE ACCESSED EXTERNALLY BY THE CHARACTER SCRIPT
    [HideInInspector] public float getAxisRaw, getAxisX;

    // THIS VECTOR IS USED TO GET THE AXIS ABOVE, AND IT IS THE JOYSTICK POSITION VALUE ON X
    private Vector2 touchV;

    // THIS VECTOR IS USED TO DETERMINE THE DUMMY JOYSTICK POSITION WHEN THE REMOVES THEIR FINGER FROM THE SCREEN
    private Vector3 dummyJoystickPos;


    void Start()
    {
        // TO DOCUMENT 

        dummyJoystick.SetActive(true);
        joystickParent.SetActive(false);
        originalButtonSize = jumpButton.transform.localScale;
        toScaleButtonVector2 = new Vector2(toScaleButtonOnPressed, toScaleButtonOnPressed);
        referenceInitialPos = new Vector2(0, 0);
        joystickInitialPos = new Vector2(0, 0);
        pressingJoystick = false;
        touchedJumpButton = false;
    }

    // BUGS TO FIX
    // 1: THERE'S A BUT WHERE IF YOU PUT SOME FINGERS ON THE JOYSTICK SIDE OF THE SCREEN, THEN, WHILE KEEP THOSE FINGERS PRESSING YOU PRESS THE JOYSTICK
    // THE JOYSTICK WILL ACT WEIRD, DECIDE NOT TO FIX THIS FOR NOW SINCE NO ONE WILL PUT 5 FINGERS WHERE JUST THE THUMB IS SUPPOSED TO BE
    // 2: WHEN HOLDING THE JUMP BUTTON PRESSED AND SLIDE IT OUT OF THE BUTTON, THE BUTTON WILL GET STUCK IN ITS PRESSED ANIMATION (SCALE) FIXED!! BY ADDING A 
    // BACKGROUND COLLIDER TO THE JUMP BUTTON

    void Update()
    {
        // THIS DESTROYS THE CLONES CREATED WHEN RELEASING THE JOYSTICK, CLONES ARE THE ONES WHO FADE OUT
        Destroy(GameObject.Find("joystickPlateDummy (1)(Clone)"), 0.5f);

        for (int i = 0; i < Input.touchCount; i++)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.CompareTag("joystickX"))
                {
                    // WITH THIS THE JOYSTICK WILL FADE IN
                    joystickParent.SetActive(true);
                    joystickParent.transform.position = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position) + new Vector3(0, 2.5f, 9);
                    
                    //THIS WILL SET THE REFERENCE POINT WHERE THE USER TAPS
                    reference.transform.position = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);

                    // TOUCHED LEFT COLLIDER TAGGED AS joystickX
                    pressingJoystick = true;

                    // DISABLING THE COLLIDER, DONT REMEMBER WHY I ADDED THIS (DIDNT TRY WITHOUT IT THO)
                    colliderForJoystick.GetComponent<BoxCollider>().enabled = false;

                    // FOR JOYSTICK MOVEMENT (DONT REMEMBER WHY)
                    touchIndexTouchedJoystick = i;

                    ableToActivateDummyJoystick = true;
                }

                // WE MUST REQUIRE AN EXTERNAL INPUT FORM THE CHARACTER (public bool onGround) TO SET IT IN THIS IF, OTHERWISE IT WILL KEEP JUMPING FOREVER WHEN .Began
                if (hit.transform.CompareTag("jumpButton"))
                {
                    if (Input.GetTouch(i).phase == TouchPhase.Began)
                    {
                        if (character.onGround)
                        {
                            // THIS VALUE WILL BE THE ONE MAKING THE CHARACTER JUMP BY MAKING IT PUBLIC
                            // AND IT MUST BE SET BACK TO FALSE EXTERNALLY (WHEN CHARACTER TOUCHES THE GROUND AGAIN)
                            touchedJumpButton = true;

                            // SCALING JUMP BUTTON FOR PRESSING SENSE
                            hit.collider.gameObject.transform.localScale = toScaleButtonVector2;
                        }
                        else
                        {
                            // SCALING JUMP BUTTON FOR PRESSING SENSE ON AIR (NO JUMP ON AIR)
                            hit.collider.gameObject.transform.localScale = toScaleButtonVector2;
                        }

                    }

                    // SCALING BUTTON JUMP BACK TO ITS ORIGINAL SIZE WHEN TOUCH.ENDED
                    if (Input.GetTouch(i).phase == TouchPhase.Ended)
                    {
                        
                        hit.collider.gameObject.transform.localScale = originalButtonSize;
                    }
                }

                // THIS STATEMENT AND ITS BACKGROUND COLLIDER WILL PREVENT THE JUMP BUTTON FROM GETTING STUCK IF THE USER 
                // SLIDES THE FINGER OUTSIDE THE JUMP COLLIDER
                if (hit.transform.CompareTag("backgroundCol"))
                {
                    jumpButton.transform.localScale = originalButtonSize;
                }

            }

        }

        // MOVING THE JOYSTICK
        if (Input.touchCount > 0 && pressingJoystick && touchIndexTouchedJoystick != Input.touchCount)
        {
            

            // THIS OFFSET = IS JUST THE POSITION OF THE REFERENCE POINT FOR THE BACK AND FORTH POINT OF THE JOYSTICK
            Vector3 offset = new Vector3(reference.transform.position.x, reference.transform.position.y, reference.transform.position.z);

            // THIS VECTOR IS THE VECTOR OF THE TOUCH WITHIN THE CAMERA CONTAINER (ScreenToWorldPoint) MINUS THE OFFSET ABOVE
            touchV = Camera.main.ScreenToWorldPoint(Input.GetTouch(touchIndexTouchedJoystick).position) - offset;

            // IF THE TOUCH GOES TO FAR AWAY ON X FROM THE JOYSTICK IT WILL NOT BE TAKEN AS INPUT ANYMORE,
            // THIS WAS SET TO AVOID THE JUMP BUTTON TO INTERRUPT WITH THE JOYSTICK MOVEMENT SINCE IF U REMOVE THIS AND LIFT UR BUTTON FROM THE JOYSTICK WHILE PRESSING THE
            // JUMP BUTTON, THE JUMP BUTTON TOUCH WILL BE TAKEN AS THE INPUT FOR THIS VECTOR

            // IN THIS V2 CONTROLS THE USER CAN GO CLOSER TO THE JUMP BUTTON WHILE MOVING THE JOYSTICK SINCE THE JOYSTICK MOVES WITH THE USER FIRST TAP
            // HAVEN'T SEEN THAT THIS VALUE LETS THE USER REACH THE JUMP BUTTON, NO ISSUE DETECTED SO FAR, IF ISSUE IS DETECTED DECREASE THIS VALUE
            if (touchV.x > 10f)
            {
                pressingJoystick = false;
            }

            // THIS IS FOR THE JOYSTICK NOT TO GO OUTSITE ITS CONTAINER // THIS IS BEING APPLIED TO THE VECTOR GOTTEN FORM THE INPUT.TOUCH -
            // THIS CAN/SHOULD ALSO BE APPLIED TO THE JOYSTICK POSITION INSTEAD OF THE VECTOR GOTTEM FROM THE INPUT
            touchV.x = Mathf.Clamp(touchV.x, -1.4f, 1.4f);

            // THIS WILL SET THE JOYSTICK POSITION TO THE CALCULATED POSITION BASED ON THE REFERENCE POINT, WHICH WILL BE ALWAYS SET WHERE THE USER TAPS
            joystick.transform.localPosition = new Vector2(touchV.x, 0f);

            // THIS IS TO EXTRACT A GETAXISRAW FORM THIS FILE
            if (touchV.x > 0)
            {
                getAxisRaw = 1;  
            }
            else if (touchV.x < 0)
            {
                getAxisRaw = -1;
            }

            // THIS VALUE WILL BE EXTRACTED FROM THE CHARACTERS SCRIPT, AS WELL AS THE getAxisRaw ABOVE
            getAxisX = touchV.x;

        }

        // NO TOUH AT ALL
        if (Input.touchCount <= 0 || !pressingJoystick || touchIndexTouchedJoystick == Input.touchCount)
        {
            // THIS WILL SET THE REFERENCE AND THE JOYSTICK BACK TO THEIR ORIGINAL POSITION IF NO TOUCH DETECTED
            reference.transform.localPosition = referenceInitialPos;
            joystick.transform.localPosition = joystickInitialPos;
            pressingJoystick = false;
            colliderForJoystick.GetComponent<BoxCollider>().enabled = true;
            touchIndexTouchedJoystick = -1;
            
            // NO NEED TO SET getAxis TO 0 BECAUSE IT IS BEING DECLARED AS touchV.x ON UPDATE
            getAxisRaw = 0;

            // THIS WILL MAKE THE ACTUALL JOYSTICK DISAPPEAR
            joystickParent.SetActive(false);

            // THIS WILL MAKE THE DUMMY JOYSTICK TO APPEAR INSTEAD OF THE ACTUALL ONE FOR THE FADE OUT ANIMATION
            dummyJoystickPos = joystickParent.transform.position;

            if (ableToActivateDummyJoystick)
            {
                dummyInstantiated = Instantiate(dummyJoystick, dummyJoystickPos, Quaternion.identity);
                dummyInstantiated.transform.SetParent(virtualCameraObject.transform);
                ableToActivateDummyJoystick = false;
            }



        }

        // I DONT REMEMBER WHY I PUT THIS HERE (DIDNT TRY WITHOUT IT)
        if (joystick.transform.localPosition.x == 0)
        {
            touchV = new Vector2(0, 0);
            getAxisX = 0f;
        }


    }

    // THIS WASN'T USED
    IEnumerator setBackButtonToNormalSize(GameObject buttonToSetBackToNormal, Vector2 originalSize, Vector2 sizeOnPressed)
    {
        buttonToSetBackToNormal.transform.localScale = sizeOnPressed;
        yield return new WaitForSeconds(0.1f);
        buttonToSetBackToNormal.transform.localScale = originalSize;
    }

}
