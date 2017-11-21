//#define DEBUG_REACHABLE_TILES

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Diagnostics;

#if DEBUG_REACHABLE_TILES
using VisualDebugging;
#endif


public class Pathfinding : MonoBehaviour
{
    private Map map;

    //Testing vars
    public bool showPath = true;
    public bool waitForKeyToCalc = false;
    public bool useDiagonalsOnGettingDistace = false;
    public int unitRange;
    private List<Tile> _path;
    private List<Tile> _reachableTiles;

    public Transform seeker, target;

    void Awake()
    {
        map = FindObjectOfType<Map>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || !waitForKeyToCalc)
        {
            Stopwatch watch;
            watch = new Stopwatch();
            watch.Start();

            if (showPath)
            {
                FindPath(seeker.position, target.position);
                watch.Stop();
                print("Path found in: " + watch.ElapsedMilliseconds + "ms.");
            }
            else
            {
                GetReachableTiles(seeker.position, unitRange);
                watch.Stop();
                print("Reachables tiles get in: " + watch.ElapsedMilliseconds + "ms.");
            }
        }
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

    public void GetReachableTiles(Vector3 position, int range)
    {
        Tile start = map.TileFromWorldPoint(position);

        if (start == null || start.tileType == Tile.TileType.TileObstacle)
            return;

        start.distacneFromStart = 0;

#if DEBUG_REACHABLE_TILES

        VisualDebug.Initialize();
        VisualDebug.BeginFrame("Start position", true);
        VisualDebug.SetColour(Colours.lightGreen);
        VisualDebug.DrawCube(start.transform.position, .5f);
#endif

        List<Tile> openSet = new List<Tile>();
        List<Tile> ret = new List<Tile>();

        openSet.Add(start);
        ret.Add(start);

        while(openSet.Count > 0)
        {
            Tile currentTile = openSet[0];

            openSet.Remove(currentTile);

            if (currentTile.distacneFromStart < range)
            {
                foreach (Tile n in currentTile.neighbours)
                {
                    if (n.tileType != Tile.TileType.TileObstacle && 
                        !ret.Contains(n))
                    {

                        n.distacneFromStart = currentTile.distacneFromStart + 1;
                        ret.Add(n);
                        openSet.Add(n);
#if DEBUG_REACHABLE_TILES

                        VisualDebug.BeginFrame("Getting neighbours", false);
                        VisualDebug.SetColour(Colours.lightGrey);
                        string text = string.Format("Distance from start: {0}.", n.distacneFromStart);
                        VisualDebug.DrawPointWithLabel(n.transform.position, .1f, text);

#endif
                    }
                }
            }
        }

        if (ret.Count > 0)
            _reachableTiles = ret;

#if DEBUG_REACHABLE_TILES

        VisualDebug.Save();

#endif
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
        if (!useDiagonalsOnGettingDistace)
        {
            return dstX + dstY;
        }
        else
        {
            //For diagonals
            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            else
                return 14 * dstX + 10 * (dstY - dstX);
        }
    }

    void OnDrawGizmos()
    {
        if (showPath)
        {
            if (_path != null)
            {
                foreach (Tile tile in _path)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawCube(tile.transform.position, Vector3.one);
                }
            }
        }
        else
        {
            if(_reachableTiles != null)
            {
                foreach(Tile tile in _reachableTiles)
                {
                    Gizmos.color = Color.grey;
                    Gizmos.DrawCube(tile.transform.position, Vector3.one);
                }
            }
        }
    }
}
