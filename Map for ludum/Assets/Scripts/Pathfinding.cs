using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private Map map;
    private List<Tile> _path;

    public Transform seeker, target;

    void Awake()
    {
        map = FindObjectOfType<Map>();
    }

    void Update()
    {
        FindPath(seeker.position, target.position);
    }

    public void FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Tile start = map.TileFromWorldPoint(startPosition);
        Tile end = map.TileFromWorldPoint(targetPosition);

        if (start == null || end == null || start.tileType == Tile.TileType.TileObstacle || end.tileType == Tile.TileType.TileObstacle)
            return;

        List<Tile> openSet = new List<Tile>();
        HashSet<Tile> cloedSet = new HashSet<Tile>();

        openSet.Add(start);

        while (openSet.Count > 0)
        {
            Tile currenTile = openSet[0];
            for (int i = 1; i < openSet.Count; ++i)
            {
                if (openSet[i].FCost < currenTile.FCost || openSet[i].FCost == currenTile.FCost && openSet[i].hCost < currenTile.hCost)
                {
                    currenTile = openSet[i];
                }
            }

            openSet.Remove(currenTile);
            cloedSet.Add(currenTile);

            if (currenTile == end)
            {
                RetracePath(start, end);
                return;
            }

            foreach (Tile neighbour in currenTile.neighbours)
            {
                if (neighbour.tileType == Tile.TileType.TileObstacle || cloedSet.Contains(neighbour))
                    continue;
                int newMoveCostToNeighbour = currenTile.gCost + GetDistace(currenTile, neighbour);
                if (newMoveCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMoveCostToNeighbour;
                    neighbour.hCost = GetDistace(neighbour, end);
                    neighbour.parent = currenTile;

                    if(!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }

    void RetracePath(Tile start, Tile end)
    {
        List<Tile> path = new List<Tile>();
        Tile currentNde = end;
        while (currentNde != start)
        {
            path.Add(currentNde);
            currentNde = currentNde.parent;
        }

        path.Reverse();

        _path = path;
    }

    int GetDistace(Tile a, Tile b)
    {
        int dstX = Mathf.Abs(a.gridPosX - b.gridPosX);
        int dstY = Mathf.Abs(a.gridPosY - b.gridPosY);

        //In order to dont use diagonals will just add the absolute values of both distances. TODO: Must check the rsult calculating distacnes with diagonals
        return Mathf.Abs(dstX) + Mathf.Abs(dstY);

        //For diagonals
        //if (dstX > dstY)
        //    return 14 * dstY + 10 * (dstX - dstY);
        //else
        //    return 14 * dstX + 10 * (dstY - dstX);
    }

    void OnDrawGizmos()
    {
        if (_path != null)
        {
            foreach (Tile tile in _path)
            {
                Debug.Log("Hi");
                Gizmos.color = Color.white;
                Gizmos.DrawCube(tile.transform.position, Vector3.one);
            }
        }
    }
}
