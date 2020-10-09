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
    private bool canPush;

    public Animator Anim;

    public GameObject destroyedBlockParticle;

    void Start()
    {
        currentDirection = up;
        nextPos = Vector3.forward;
        destination = transform.position;
    }

    void Update()
    {
        Move();
        Push();
        Anim.SetBool("isWalking", isWalking);
        Anim.SetBool("isPushing", isPushing);
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        isWalking = false;

        //use (Input.GetKeyDown(KeyCode.W)) for individual tile to tile movement - the one that set right now kinda moves across an extra block
        if (Input.GetKeyDown(KeyCode.W))
        {
            nextPos = Vector3.forward;
            currentDirection = up;
            canMove = true;
            isWalking = true;
         
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

    void Push()
    {
        if(Input.GetKey(KeyCode.W))
        {
            nextPos = Vector3.forward;
            currentDirection = up;
            canPush = true;
            if (!Valid())
            {
                canMove = false;
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
            /*if (hit.collider.tag == "Obstacle" || hit.collider.tag == "StaticBlock" || hit.collider.tag == "DestroyableBlock")
            {
                isWalking = false;
                isPushing = false;
                return false;
            }*/
            if(hit.collider.tag == "Obstacle" && Input.GetKeyDown(KeyCode.LeftShift) && canPush)
            {
                Debug.Log("Pushed Block");
                //hit.collider.gameObject.transform.SetParent(this.transform);
                hit.collider.gameObject.transform.position = Vector3.MoveTowards(hit.collider.gameObject.transform.position, hit.collider.gameObject.transform.position + nextPos, speed * Time.deltaTime);
                isWalking = false;
                isPushing = true;
                return false;
            }
            if (hit.collider.tag == "DestroyableBlock" && Input.GetKeyDown(KeyCode.Return) && canPush)
            {
                Debug.Log("Destroyed Block");
                Instantiate(destroyedBlockParticle, hit.collider.gameObject.transform.position, hit.collider.gameObject.transform.rotation);
                Destroy(hit.collider.gameObject);
                isWalking = false;
                //isPushing = true;
                return false;
            }
            else
            {
                //hit.collider.gameObject.transform.SetParent(null);
                isWalking = false;
                isPushing = false;

                return false;
            }

        }
        return true;
    }


}
