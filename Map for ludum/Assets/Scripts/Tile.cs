using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public enum TileType
    {
        TileObstacle,
        TileWalkable
    }

	#region Variables

    public TileType tileType;
    [HideInInspector]
    public int gridPosX, gridPosY;

    private int gCost;
    private int hCost;

    public Tile parent;
    

	#endregion

    public int FCost
    {
        get { return gCost + hCost; }
    }

	#region UnityMethods

	void Start () 
	{
		
	}
	

	void Update () 
	{
		
	}

    void OnDrawGuizmos()
    {
        
    }

	#endregion

    public void SetTile(int gridX, int gridY)
    {
        gridPosX = gridX;
        gridPosY = gridY;
    }
}
