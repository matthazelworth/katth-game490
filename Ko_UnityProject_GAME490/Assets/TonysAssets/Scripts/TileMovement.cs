using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TileMovement : MonoBehaviour
{
    [SerializeField]
    public AudioClip[] clips;                                   //creates an array of audio clips 

    private AudioSource audioSource;                            //establishes a variable for an audio source component

    Vector3 up = Vector3.zero,                                  //to make the object look up (north)
    right = new Vector3(0, 90, 0),                              //to make the object look right (east)
    down = new Vector3(0, 180, 0),                              //to make the object look down (south)
    left = new Vector3(0,270,0),                                //to make the player look left (west)
    currentDirection = Vector3.zero;                            //this will be its default state - the direction it'll face when you start the game

    Vector3 nextPos, destination, direction;

    private bool canMove;                                       //the bool is used to determine when the object can move

    float speed = 5f;                                           
    float rayLength = 1f;

    private bool isWalking;                                     //the bool is used to determine when to play an object's animation
    private bool isPushing;                                     //the bool is used to determine when the object can move 
    private bool canPush;                                       //the bool is used to determine when the object can be pushed 

    public Animator Anim;                                       //establishes a variable for an animator component

    public GameObject destroyedBlockParticle;                   //the paarticle effect that spawn when you break a block;

    void Start()
    {
        currentDirection = up;                                  //the direction the object faces when you start the game
        nextPos = Vector3.forward;                              //the next block postion is equal to the object's forward axis (it will move along the direction it is facing)
        destination = transform.position;                       //the point where the object is currenlty at 
        audioSource = GetComponent<AudioSource>();              //sets the audio source variable to the object's audio source component (sets instance)
    }

    void Update()
    {
        Move();                                                 //calls the Move function stated below
        Push();                                                 //calls the Move function stated below
        Anim.SetBool("isWalking", isWalking);                   //sets the bool stated in this script to the corresponding bool stated within the object's animator
        Anim.SetBool("isPushing", isPushing);                   //sets the bool stated in this script to the corresponding bool stated within the object's animator
    }

    void Move()                                                                                           //the main moving function for the object
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);//when the object starts moving, the object moves from its current position to the destination
        isWalking = false;

        if (Input.GetKeyDown(KeyCode.W))                                                                  //when a ceratin key is pressed...
        {
            nextPos = Vector3.forward;                                                                    //...the object is set to move along the stated axis
            currentDirection = up;                                                                        //sets the object to rotate towards the specified direction
            canMove = true;                                                                               //the object can move while the statement above is true
            isWalking = true;                                                                             //the object can perform the walking animation while the statement above is true

        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            nextPos = Vector3.back;
            currentDirection = down;
            canMove = true;
            isWalking = true;
          
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            nextPos = Vector3.right;
            currentDirection = right;
            canMove = true;
            isWalking = true;
         
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            nextPos = Vector3.left;
            currentDirection = left;
            canMove = true;
            isWalking = true;
            
        }

        if(Vector3.Distance(destination, transform.position) <= 0.00001f)                                //checks to see how big the distance is between the object's current position and the destination
        {
            transform.localEulerAngles = currentDirection;                                               //rotates the actual object to the current direction - the raycast will roatate with it, along the axis its facing
            if (canMove)
            {
                if(Valid())                                                                              //if the bool function below is returned as true
                {
                    Footstep();
                    destination = transform.position + nextPos;                                          //updates the destination by adding the next position to the object's current position
                    direction = nextPos;
                    canMove = false;                                                                     //prevents the object from constantly moving towards the object's current direction                                                       
                }
            }    
        }
    }

    void Push()                                                                                          //the function that determines when the object can push another
    {
        if(Input.GetKey(KeyCode.W))                                                                      //when a ceratin key is pressed...
        {
            nextPos = Vector3.forward;                                                                   //...the object is set to move along the stated axis
            currentDirection = up;                                                                       //sets the object to rotate towards the specified direction
            canPush = true;                                                                              //the object can push another object while the statement above is true
            if (!Valid())                                                                                //if the bool function below is returned as false, then the object cannot move
            {
                canMove = false;
                if (Input.GetKeyDown(KeyCode.LeftShift))                                                 //when the bool function is returned as false, and you press a certain key...
                {
                    isPushing = true;                                                                    //the object can play its pushing animation
                }
                else
                {
                    isPushing = false;                                                                   //the object cannot play its pushing animation for any other possible statements
                }
            }
            
        }

        if (Input.GetKey(KeyCode.A))
        {
            nextPos = Vector3.left;
            currentDirection = left;
            canPush = true;
            if (!Valid())
            {
                canMove = false;
                if (Input.GetKeyDown(KeyCode.LeftShift))                                                                    
                {
                    isPushing = true;
                }
                else
                {
                    isPushing = false;
                }
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            nextPos = Vector3.back;
            currentDirection = down;
            canPush = true;
            if (!Valid())
            {
                canMove = false;
                if (Input.GetKeyDown(KeyCode.LeftShift))                                                                 
                {
                    isPushing = true;
                }
                else
                {
                    isPushing = false;
                }
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            nextPos = Vector3.right;
            currentDirection = right;
            canPush = true;
            if (!Valid())
            {
                canMove = false;
                if (Input.GetKeyDown(KeyCode.LeftShift))                                                                    
                {
                    isPushing = true;
                }
                else
                {
                    isPushing = false;
                }
            }
        }
    }

    bool Valid()                                                                                                                                 //the bool function that checks to see if the next position is valid or not
    {
        Ray myRay = new Ray(transform.position + new Vector3(0, 0.25f, 0), transform.forward);                                                   //shoots a ray into the direction that the object is looking towards
        RaycastHit hit;

        Debug.DrawRay(myRay.origin, myRay.direction, Color.red);                                                                                 //shows a debug line of the raycast that was called previously (just to see if its working in Unity editor)

        if (Physics.Raycast(myRay, out hit, rayLength))                                                                                          //checks to see what the ray hit depending on its range - raylength
        {
            //if (hit.collider.tag == "Obstacle" || hit.collider.tag == "StaticBlock" || hit.collider.tag == "DestroyableBlock")                 //this is just for future reference - ignore this line
            

            if (hit.collider.tag == "Obstacle" && canPush)
            {
                hit.collider.gameObject.GetComponent<BlockMovement>().MoveBlock();
                isWalking = false;
                return false;
            }

            if (hit.collider.tag == "DestroyableBlock" && Input.GetKeyDown(KeyCode.Return) && canPush)                                          //if the ray hits an object tagged "DestroyableBlock" and etc...
            {
                Debug.Log("Destroyed Block");                                                                                                   //send a debug message saying that the block was destroyed (just to see if its working in Unity editor)
                Instantiate(destroyedBlockParticle, hit.collider.gameObject.transform.position, hit.collider.gameObject.transform.rotation);    //spawns the block destruction particle effect on the tagged object's location
                Destroy(hit.collider.gameObject);                                                                                               //destroys the tagged object
                isWalking = false;                                                                                                              //the object cannot play its walking animation while the statement above is true
                return false;                                                                                                                   //the bool function will return as false if the statement above is true
            }
            else
            {
                isWalking = false;                                                                                                               //the object cannot play its walking animation for any other possible statements
                return false;                                                                                                                    //the bool function will return as false for any other possible statements
            }
        }
        return true;                                                                                                                             //the bool function will return as true - this is needed for it to work)

    }

    private void Footstep()                                                                                                                      //the function that plays the audio clip
    {
        AudioClip clips = GetRandomClip();                                                                                                       //calls the random audio clip function below
        audioSource.PlayOneShot(clips);                                                                                                          //plays the audio clip (from start to end - without intturuption) through the object's audio source component
    }
    private AudioClip GetRandomClip()                                                                                                            //the function for getting a random clip for the footstep sfx
    {
        return clips[UnityEngine.Random.Range(0, clips.Length)];                                                                                 //selects a random audio clip based on the size of the array - its length

    }

}
