using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    //Testing
    public bool diagonalDistance = false;

    private Map map;

    private List<Node> lastPath;
    private List<Node> lastReachableNodes;

	void Awake ()
    {
        map = FindObjectOfType<Map>();

        lastPath = new List<Node>();
        lastReachableNodes = new List<Node>();
	}

    //----------------------------------------------------------

    public bool FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node start = map.NodeFromWorldPoint(startPos);
        Node end = map.NodeFromWorldPoint(targetPos);

        return FindPath(start, end);
    }

    public bool FindPath(Node start, Node end)
    {
        if (start == null || end == null
            || start.tileType == Node.TileType.TileObstacle
            || end.tileType == Node.TileType.TileObstacle)
            return false;

        lastPath.Clear();

        Heap<Node> openSet = new Heap<Node>(map.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(start);

        while (openSet.Count > 0)
        {
            // - Get the node with lowest cost ----
            Node current = openSet.RemoveFirst();
            closedSet.Add(current);

            // -------------------------------------

            // - If is the end, we got the path. ------

            if (current == end)
            {
                RetracePath(start, end);
                return true;
            }

            // -------------------------------------

            // - Otherwise keep looking on neighbours --------------

            foreach (Node neighbour in current.neighbours)
            {
                if (neighbour.tileType == Node.TileType.TileObstacle || closedSet.Contains(neighbour))
                    continue;

                int newMoveCostToNeighbour = current.gCost + GetDistance(current, neighbour);
                if (newMoveCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMoveCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, end);
                    neighbour.parent = current;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                    else
                        openSet.UpdateItem(neighbour);
                }
            }

            // ------------------------------------------
        }

        return false;
    }

    public List<Node> GetLastPath()
    {
        return lastPath;
    }

    public List<Node> GetReachableNodes(Vector3 position, int range)
    {
        Node start = map.NodeFromWorldPoint(position);

        if (start == null || start.tileType == Node.TileType.TileObstacle)
            return null;

        lastReachableNodes.Clear();

        start.distacneFromStart = 0;

        List<Node> openSet = new List<Node>();

        openSet.Add(start);
        lastReachableNodes.Add(start);

        while(openSet.Count > 0)
        {
            Node current = openSet[0];

            openSet.Remove(current);

            if(current.distacneFromStart < range)
            {
                foreach(Node n in current.neighbours)
                {
                    if(n.tileType != Node.TileType.TileObstacle && !lastReachableNodes.Contains(n))
                    {
                        n.distacneFromStart = current.distacneFromStart + 1;
                        lastReachableNodes.Add(n);
                        openSet.Add(n);
                    }
                }
            }
        }

        if (lastReachableNodes.Count > 0)
            return lastReachableNodes;
        else
            return null;
    }

    //----------------------------------------------------------

    int GetDistance(Node a, Node b)
    {
        int dstX = Mathf.Abs(a.gridPosX - b.gridPosX);
        int dstY = Mathf.Abs(a.gridPosY - b.gridPosY);

        if(diagonalDistance)
        {
            return dstX + dstY;
        }
        else
        {
            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            else
                return 14 * dstX + 10 * (dstY - dstX);
        }
    }

    void RetracePath(Node start, Node end)
    {
        Node current = end;
        while(current != start)
        {
            lastPath.Add(current);
            current = current.parent;
        }

        lastPath.Reverse();
    }

    //----------------------------------------------------------

}
