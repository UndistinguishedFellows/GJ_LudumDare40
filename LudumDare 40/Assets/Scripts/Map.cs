using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    [HideInInspector]
    public MapVisualInfo visualInfo;
    public Node[,] nodes;

    private int width, height;

    //-------------------------------------

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    //-------------------------------------

    public int Width
    {
        get { return width; }
    }

    public int Height
    {
        get { return height; }
    }

    //-------------------------------------

    public void OnMapVisualInfoLoaded()
    {
        width = visualInfo.width;
        height = visualInfo.height;

        nodes = new Node[width, height];
    }

    public void OnMapLoaded()
    {
        if(nodes == null)
        {
            Debug.LogError("Node array not initialized.");
        }
        else
        {
            for(int x = 0; x < width; ++x)
            {
                for(int y = 0; y < height; ++y)
                {
                    Node node = nodes[x, y];
                    if (node != null)
                    {
                        int checkX = node.gridPosX + 1;
                        int checkY = node.gridPosY - 1;
                        //Debug.Log("----------");
                        //Debug.Log("Gpos: " + node.gridPosX + " " + tile.gridPosY);
                        //Debug.Log("Top: x,y:" + checkX + " " + checkY);
                        if (checkY >= 0) node.neighbours[0] = nodes[node.gridPosX, checkY]; //Top
                        else node.neighbours[0] = null;
                        checkY += 2; //Set to bottom
                        //Debug.Log("Right: x,y:" + checkX + " " + checkY);
                        if (checkX < width) node.neighbours[1] = nodes[checkX, node.gridPosY]; //Right
                        else node.neighbours[1] = null;
                        checkX -= 2; //Set to left
                        //Debug.Log("Bottom: x,y:"  + checkX + " " + checkY);
                        if (checkY < height) node.neighbours[2] = nodes[node.gridPosX, checkY]; //Bottom
                        else node.neighbours[2] = null;
                        //Debug.Log("Left: x,y:" + checkX + " " + checkY);
                        if (checkX >= 0) node.neighbours[3] = nodes[checkX, node.gridPosY]; //Left
                        else node.neighbours[3] = null;
                    }
                }
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentageX = Mathf.Clamp01((worldPosition.x + width / 2) / width);
        float percentageY = Mathf.Clamp01(1 - (worldPosition.y + height / 2) / height);

        int x = (int)(width * percentageX);
        int y = (int)(height * percentageY);

        if (x >= 0 && x < width && y >= 0 && y < height)
            return nodes[x, y];
        else
            return null;
    }

}
