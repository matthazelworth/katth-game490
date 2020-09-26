using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTile : MonoBehaviour
{
    public int tileX;
    public int tileY;
    public TileMap map;

    void OnMouseDown()
    {
        Debug.Log("Click!");

        map.MoveSelectedUnitTo(tileX, tileY);
    }
}
