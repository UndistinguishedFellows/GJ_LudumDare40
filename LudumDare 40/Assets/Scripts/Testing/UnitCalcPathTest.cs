using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitCalcPathTest : MonoBehaviour
{
    public GameObject reachablePrefab;
    public Transform reachableNodesParent;
    public GameObject target;
    [Range(0, 15)]
    public int unitRange = 4;

    public float unitSpeed = 1.0f;

    Pathfinding pathfinding;
    private Map map;
    List<Node> path = null;
    List<Node> reachableNodes = null;
    private List<GameObject> reachableNodesGameObjects = new List<GameObject>();

    private Node clickedNode = null;
    private Node nextNode = null;
    private int nextNodeIndex = 0;

	void Awake ()
    {
        pathfinding = FindObjectOfType<Pathfinding>();
        map = FindObjectOfType<Map>();
    }

    void Start()
    {
        CalcReachableNodes();
    }

	void Update ()
    {
        if (reachableNodes == null)
        {
            CalcReachableNodes();
        }

        if (Input.GetMouseButtonDown(0) && path == null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickedNode = map.NodeFromWorldPoint(mousePos);

            if (clickedNode != null)
            {
                if (clickedNode.tileType == Node.TileType.TileObstacle)
                {
                    Debug.Log("Clicked on an obstacle node!");
                }
                else
                {
                    if (reachableNodes.Contains(clickedNode))
                    {
                        if (pathfinding.FindPath(transform.position, mousePos))
                        {
                            path = pathfinding.GetLastPath();
                            nextNode = path[0];
                        }
                    }
                    else
                    {
                        Debug.Log("Clicked on a node out of range.");
                    }
                }
            }
            else
            {
                Debug.Log("Clicked out of map.");
            }
        }

        if (path != null)
        {
            // Follow path

            if (Vector3.Distance(nextNode.transform.position, transform.position) < 0.2)
            {
                // If unit is really close to the target node move to next

                // Check first if the last node unit was following was the end.
                if (nextNodeIndex == path.Count - 1)
                {
                    //We are there
                    // Ajust the position, set path to null and recalc reachable nodes.
                    transform.position = nextNode.transform.position;
                    path = null;
                    CalcReachableNodes();
                    nextNodeIndex = 0;
                }
                else
                {
                    // Still some nodes to reach target
                    nextNodeIndex++;
                    if (nextNodeIndex < path.Count)
                    {
                        // Assertion not really needed but for now just in case.

                        nextNode = path[nextNodeIndex];
                    }
                }
            }

            if (nextNode != null)
            {
                Vector3 velocity = nextNode.transform.position - transform.position;
                velocity.Normalize();
                velocity *= (unitSpeed * Time.deltaTime);

                transform.position += velocity;
                //TODO: Need to orientate
            }
        }
	}

    private void OnDrawGizmos()
    {
        if(reachableNodes != null)
        {
            Gizmos.color = Color.grey;
            foreach(Node n in reachableNodes)
            {
                Gizmos.DrawCube(n.transform.position, Vector3.one);
            }
        }

        if (path != null)
        {
            Gizmos.color = Color.cyan;
            foreach (Node n in path)
            {
                Gizmos.DrawCube(n.transform.position, Vector3.one * 0.75f);
            }
        }
    }

    //-----------------------------------------------------------------------------

    void CalcReachableNodes()
    {
        var childs = new List<Transform>();
        foreach (Transform t in reachableNodesParent)
        {
            childs.Add(t);
        }
        childs.ForEach(child => DestroyImmediate(child.gameObject));

        reachableNodesGameObjects.Clear();

        reachableNodes = pathfinding.GetReachableNodes(transform.position, unitRange);

        if(reachableNodes != null)
        {
            foreach (Node node in reachableNodes)
            {
                Vector3 pos = node.transform.position + Vector3.forward * -0.01f;
                reachableNodesGameObjects.Add(Instantiate(reachablePrefab, pos, Quaternion.identity, reachableNodesParent));
            }
        }
    }
}
