using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgParallax : MonoBehaviour
{
    public GameObject cameraMain, hills18, hills19, sky, mountainBack, mountainMiddle, mountainFront, cloudsBack, cloudsMiddle, cloudsFront;
    public float speedHills18, speedHills19, speedCloudsBack;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hills18.transform.position = new Vector2(cameraMain.transform.position.x * speedHills18, hills18.transform.position.y);
        mountainFront.transform.position = new Vector2(cameraMain.transform.position.x * speedHills18, mountainFront.transform.position.y);
        cloudsFront.transform.position = new Vector2(cameraMain.transform.position.x * speedHills18, cloudsFront.transform.position.y);

        hills19.transform.position = new Vector2(cameraMain.transform.position.x * speedHills19, hills19.transform.position.y);
        mountainMiddle.transform.position = new Vector2(cameraMain.transform.position.x * speedHills19, mountainMiddle.transform.position.y);
        cloudsMiddle.transform.position = new Vector2(cameraMain.transform.position.x * speedHills19, cloudsMiddle.transform.position.y);

        mountainBack.transform.position = new Vector2(cameraMain.transform.position.x * speedCloudsBack, mountainBack.transform.position.y);
        cloudsBack.transform.position = new Vector2(cameraMain.transform.position.x * speedCloudsBack, cloudsBack.transform.position.y);

        sky.transform.position = new Vector2(cameraMain.transform.position.x, sky.transform.position.y);
    }
}
