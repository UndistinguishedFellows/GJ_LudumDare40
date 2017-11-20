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

        Debug.Log(string.Format("Map loaded: Width {0}, Height {1}", width, height));


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
}
