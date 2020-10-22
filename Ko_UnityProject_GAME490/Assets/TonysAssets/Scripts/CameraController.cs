using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //creates a tranform array
    public Transform[] levelViews;

    //speed at which the camera will transition, value set in Unity inspector
    public float transitonSpeed;

    //the variable that is used to determine which view the camera is currenlty at
    Transform currentView;

    int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        //the initial camera view when the scene loads
        currentView = levelViews[currentIndex];     
    }

    void Update()
    {
       //when a certain key is pressed: "f"
       /*if(Input.GetKeyDown(KeyCode.F))
        {
            //set the camera view a specific view within the array
            currentView = levelViews[3]; 
        }

       //when a certain key is pressed "1"
       if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentView = levelViews[0];
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentView = levelViews[1];
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentView = levelViews[2];
        }*/

       if(Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Lol this worked?");

            if (currentIndex >= 4)
            {
                Debug.Log("Reset dis shiiiiii");
                currentIndex = 0;
            }

            currentView = levelViews[currentIndex++];
        }

    }


    // Update is called once per frame
    void LateUpdate()
    {
        //move the camera's current position to the new position via linear interpolation
        transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime * transitonSpeed);

    }
}
