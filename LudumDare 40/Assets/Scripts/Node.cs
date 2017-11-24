using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public enum TileType
    {
        TileObstacle,
        TileWalkable
    }

    public TileType tileType;

    //For pathfinding 
    [HideInInspector]
    public int gridPosX, gridPosY;

    [HideInInspector]
    public int gCost;
    [HideInInspector]
    public int hCost;

    [HideInInspector]
    public int distacneFromStart = 0;

    [HideInInspector]
    public Node parent;


    [HideInInspector]
    public Node[] neighbours = new Node[4]; // 0:Top, 1:Right, 2:Bottom, 3:Left

    //-------------------------------------

    void Start ()
    {
		
	}
	

	void Update ()
    {
		
	}

    //-------------------------------------

    public int FCost
    {
        get { return gCost + hCost; }
    }

    //-------------------------------------

    public void SetNode(int gridX, int gridY)
    {
        gridPosX = gridX;
        gridPosY = gridY;
    }

}
