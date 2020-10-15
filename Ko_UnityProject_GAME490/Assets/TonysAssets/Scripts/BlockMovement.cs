using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    Vector3 up = Vector3.zero,                                  //to make the object look up (north)
    right = new Vector3(0, 90, 0),                              //to make the object look right (east)
    down = new Vector3(0, 180, 0),                              //to make the object look down (south)
    left = new Vector3(0, 270, 0),                              //to make the player look left (west)
    currentDirection = Vector3.zero;                            //this will be its default state - the direction it'll face when you start the game

    Vector3 nextBlockPos, destination, direction;

    bool canMoveBlock;                                          //the bool is used to determine when the object can move

    float speed = 5f;                                           //the speed at which the object will move from its current position to the destination               
    float rayLength = 1f;

    void Start()
    {
        currentDirection = up;                                   //the direction the object faces when you start the game
        nextBlockPos = Vector3.forward;                          //the next block postion is equal to the object's forward axis (it will move along the direction it is facing)
        destination = transform.position;                        //the point where the object is currenlty at 
    }

    void Update()
    {
        //MoveBlock();                                           //calls the MoveBlock function stated below
    }

    public void MoveBlock()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed /* * Time.deltaTime*/); //when the object starts moving, the object moves from its current position to the destination

        if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.LeftShift))                                     //when the player is moving and a certain key is pressed...
        {
            Debug.Log("Pushed Block Up");
            nextBlockPos = Vector3.forward;                                                                     //...the object is set to move along the stated axis
            currentDirection = up;                                                                              //sets the object to rotate towards the specified direction
            canMoveBlock = true;                                                                                //the object can move while the statement above is true
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.LeftShift))                    
        {
            Debug.Log("Pushed Block Down");
            nextBlockPos = Vector3.back;
            currentDirection = down;
            canMoveBlock = true;
        }

        if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("Pushed Block Right");
            nextBlockPos = Vector3.right;
            currentDirection = right;
            canMoveBlock = true;
        }

        if (Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("Pushed Block Left");
            nextBlockPos = Vector3.left;
            currentDirection = left;
            canMoveBlock = true;
        }

        if (Vector3.Distance(destination, transform.position) <= 0.00001f)                      //checks to see how big the distance is between the object's current position and the destination
        {
            transform.localEulerAngles = currentDirection;                                      //rotates the actual object to the current direction
            if (canMoveBlock)
            {
                if (Valid())                                                                    //if the bool function below is returned as true
                {       
                    destination = transform.position + nextBlockPos;                            //updates the destination by adding the next position to the object's current position
                    direction = nextBlockPos;
                    canMoveBlock = false;                                                       //prevents the object from constantly moving towards the object's current direction
                }
            }
        }
    }

    bool Valid()                                                                                //the bool function that checks to see if the next position is valid or not
    {
        Ray myRay = new Ray(transform.position + new Vector3(0, 0.25f, 0), transform.forward);  //shoots a ray into the direction that the object is looking towards
        RaycastHit hit;

        Debug.DrawRay(myRay.origin, myRay.direction, Color.red);                                //shows a debug line of the raycast that was called previously (just to see if its working in Unity editor)

        if (Physics.Raycast(myRay, out hit, rayLength))                                         //checks to see what the ray hit depending on its range - raylength
        {
            if (hit.collider.tag == "Obstacle" || hit.collider.tag == "StaticBlock" || hit.collider.tag == "DestroyableBlock")  //if the ray hits an object tagged with the specified tag
            {
                return false;
            }
        }
        return true;

    }
}
