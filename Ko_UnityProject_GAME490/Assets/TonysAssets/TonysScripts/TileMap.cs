using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public GameObject selectedUnit;

    public TileType[] tileTypes;

    int[,] tiles;

    int mapSizeX = 10;
    int mapSizeY = 10;

    void Start()
    {
        GernerateMapData();
        GenerateMapVisuals();

        //Now that we have all of our map data, spawn the visual prefabs
    }

    void GernerateMapData()
    {
        // Allcoate our map tiles
        tiles = new int[mapSizeX, mapSizeY];

        int x, y;

        // Initialize our map tiles for the floor (walkable)
        for (x = 0; x < mapSizeX; x++)
        {
            for (y = 0; y < mapSizeY; y++)
            {
                tiles[x, y] = 0;
            }
        }

        //Makes an area for heavy obstacle tiles
        for(x = 3; x <= 5; x++)
        {
            for(y = 0; y < 4; y++)
            {
                tiles[x, y] = 1;
            }
        }

        //Makes a u-shape out of light obstacle tiles
        tiles[4, 4] = 2;
        tiles[5, 4] = 2;
        tiles[6, 4] = 2;
        tiles[7, 4] = 2;
        tiles[8, 4] = 2;

        tiles[4, 5] = 2;
        tiles[4, 6] = 2;
        tiles[8, 5] = 2;
        tiles[8, 6] = 2;
    }

    void GenerateMapVisuals()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                TileType tt = tileTypes[tiles[x, y]];

                GameObject go = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(x, y, 0), Quaternion.identity);

                ClickableTile ct = go.GetComponent<ClickableTile>();

                ct.tileX = x;
                ct.tileY = y;
                ct.map = this;
            }
        }
    }

    public Vector3 TileCoordToWorldCoord(int x, int y)
    {
        return new Vector3(x, y, 0);
    }

    public void MoveSelectedUnitTo(int x, int y)
    {
        selectedUnit.GetComponent<Unit>().tileX = x;
        selectedUnit.GetComponent<Unit>().tileY = y;

        selectedUnit.transform.position = TileCoordToWorldCoord(x, y);
    }

}
