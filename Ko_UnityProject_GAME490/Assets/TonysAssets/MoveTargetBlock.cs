using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTargetBlock : MonoBehaviour
{
    public LayerMask hitLayers;
   

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 mouse = Input.mousePosition; //Get the mouse position
            Ray castPoint = Camera.main.ScreenPointToRay(mouse); //Cast a ray to where the mouse is pointing at
            RaycastHit hit; //Stores the position of where the raycast hit.
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers)) //If the raycast doesnt hit a wall
            {
                this.transform.position = hit.point; //Move the target to the mouse position
            }
        }
    }
   
}
