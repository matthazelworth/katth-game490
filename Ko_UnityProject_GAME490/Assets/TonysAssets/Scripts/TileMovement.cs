using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TileMovement : MonoBehaviour
{
    Vector3 up = Vector3.zero, 
    right = new Vector3(0, 90, 0),
    down = new Vector3(0, 180, 0),
    left = new Vector3(0,270,0),
    currentDirection = Vector3.zero;

    Vector3 nextPos, destination, direction;

    bool canMove;

    float speed = 5;
    float rayLength = 1f;

    private bool isWalking;
    private bool isPushing;

    public Animator Anim;

    void Start()
    {
        currentDirection = up;
        nextPos = Vector3.forward;
        destination = transform.position;
    }

    void Update()
    {
        Move();
        Anim.SetBool("isWalking", isWalking);
        Anim.SetBool("isPushing", isPushing);
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        isWalking = false;

        //use (Input.GetKeyDown(KeyCode.W)) for individual tile to tile movement - the one that set right now kinda moves across an extra block
        if (Input.GetKey(KeyCode.W))
        {
            nextPos = Vector3.forward;
            currentDirection = up;
            canMove = true;
            isWalking = true;
        }

        //use (Input.GetKeyDown(KeyCode.S)) for individual tile to tile movement
        if (Input.GetKey(KeyCode.S))
        {
            nextPos = Vector3.back;
            currentDirection = down;
            canMove = true;
            isWalking = true;
        }

        //use (Input.GetKeyDown(KeyCode.D)) for individual tile to tile movement
        if (Input.GetKey(KeyCode.D))
        {
            nextPos = Vector3.right;
            currentDirection = right;
            canMove = true;
            isWalking = true;
        }

        //use (Input.GetKeyDown(KeyCode.A)) for individual tile to tile movement
        if (Input.GetKey(KeyCode.A))
        {
            nextPos = Vector3.left;
            currentDirection = left;
            canMove = true;
            isWalking = true;
        }

        if(Vector3.Distance(destination, transform.position) <= 0.00001f)
        {
            transform.localEulerAngles = currentDirection;
            if(canMove)
            {
                if(Valid())
                {
                    destination = transform.position + nextPos;
                    direction = nextPos;
                    canMove = false;
                } 
            }    
        }
    }

    bool Valid()
    {
        Ray myRay = new Ray(transform.position + new Vector3(0, 0.25f, 0), transform.forward);
        RaycastHit hit;

        Debug.DrawRay(myRay.origin, myRay.direction, Color.red);

        if (Physics.Raycast(myRay, out hit, rayLength))
        {
            if (hit.collider.tag == "Obstacle" && Input.GetKeyDown(KeyCode.LeftShift) && isWalking)
            {
                Debug.Log("Pushing/Pulling Object");                            //Destroy(hit.collider.gameObject); //Use this for when the baby mammoth breaks a rock/block
                hit.collider.gameObject.transform.SetParent(this.transform);    //Parents the tagged object to the player, mimicing its movement
                isWalking = false;
                isPushing = true;
                return true;
            }
            else
            {
                Debug.Log("NOT Pushing/Pulling Object");
                hit.collider.gameObject.transform.SetParent(null);                //Un-Parents the tagged object;
                isWalking = false;
                isPushing = false;
                return false;
            }

        }
        return true;
    }


}
