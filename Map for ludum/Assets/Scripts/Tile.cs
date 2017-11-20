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


    //For pathfinding 
    [HideInInspector]
    public int gridPosX, gridPosY;

    [HideInInspector]
    public int gCost;
    [HideInInspector]
    public int hCost;

    [HideInInspector]
    public Tile parent;


    [HideInInspector]
    public Tile[] neighbours = new Tile[4]; // 0:Top, 1:Right, 2:Bottom, 3:Left
    

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

    void OnDrawGizmosSelected()
    {
       // Gizmos.color = tileType == TileType.TileObstacle ? Color.red : Color.green;
       // Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

	#endregion

    public void SetTile(int gridX, int gridY)
    {
        gridPosX = gridX;
        gridPosY = gridY;
    }
}
