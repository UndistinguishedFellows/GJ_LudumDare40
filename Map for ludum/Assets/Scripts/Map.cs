using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

	#region Variables
    
    [HideInInspector]
    public MapVisualInfo visualInfo;
    public Tile[,] tiles;

    private int width, height;

	#endregion

	#region UnityMethods

	void Start () 
	{
		
	}
	

	void Update () 
	{

    }

	#endregion

    public int Width
    {
        get { return width; }
    }

    public int Height
    {
        get { return height; }
    }

    public void OnMapLoaded()
    {
        width = visualInfo.width;
        height = visualInfo.height;
        
        //Debug.Log(string.Format("Map loaded: Width {0}, Height {1}", width, height));

        if (tiles == null)
        {
            Debug.LogError("Tiles array was not created.");
        }
        else
        {
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    Tile tile = tiles[x, y];
                    if (tile != null)
                    {
                        int checkX = tile.gridPosX + 1;
                        int checkY = tile.gridPosY - 1;
                        //Debug.Log("----------");
                        //Debug.Log("Gpos: " + tile.gridPosX + " " + tile.gridPosY);
                        //Debug.Log("Top: x,y:" + checkX + " " + checkY);
                        if (checkY >= 0) tile.neighbours[0] = tiles[tile.gridPosX, checkY]; //Top
                        else tile.neighbours[0] = null;
                        checkY += 2; //Set to bottom
                        //Debug.Log("Right: x,y:" + checkX + " " + checkY);
                        if (checkX < width) tile.neighbours[1] = tiles[checkX, tile.gridPosY]; //Right
                        else tile.neighbours[1] = null;
                        checkX -= 2; //Set to left
                        //Debug.Log("Bottom: x,y:"  + checkX + " " + checkY);
                        if (checkY < height) tile.neighbours[2] = tiles[tile.gridPosX, checkY]; //Bottom
                        else tile.neighbours[2] = null;
                        //Debug.Log("Left: x,y:" + checkX + " " + checkY);
                        if (checkX >= 0) tile.neighbours[3] = tiles[checkX, tile.gridPosY]; //Left
                        else tile.neighbours[3] = null;
                    }
                }
            }
        }
    }

    public List<Tile> GetNeightbours(Tile tile)
    {
        List<Tile> ret = new List<Tile>();

        int checkX = tile.gridPosX - 1;
        int checkY = tile.gridPosY - 1;

        if(checkX >= 0) ret.Add(tiles[checkX, tile.gridPosY]);
        checkX += 2;
        if (checkX < width) ret.Add(tiles[checkX, tile.gridPosY]);

        if(checkY >= 0) ret.Add(tiles[tile.gridPosX, checkY]);
        checkY += 2;
        if (checkY < height) ret.Add(tiles[tile.gridPosX, checkY]);

        return ret;
    }

    public Tile TileFromWorldPoint(Vector3 worldPosition)
    {
        float percentageX = (worldPosition.x + width / 2) / width; //Use width assuming each tile has a size of one so numtiles * 1 = width
        float percentageY = 1 - (worldPosition.y + height/ 2) / height;
        percentageX = Mathf.Clamp01(percentageX);
        percentageY = Mathf.Clamp01(percentageY);
        
        int x = (int)(width * percentageX);
        int y = (int)(height * percentageY);
        

        if (x >= 0 && y >= 0 && x < width && y < height)
            return tiles[x, y];
        else
            return null;
    }
    
}
