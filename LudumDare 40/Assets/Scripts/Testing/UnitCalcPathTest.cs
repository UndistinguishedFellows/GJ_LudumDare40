using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCalcPathTest : MonoBehaviour {

    public GameObject target;
    [Range(0, 15)]
    public int unitRange = 4;

    Pathfinding pathfinding;
    List<Node> path = null;
    List<Node> reachableNodes = null;



	void Awake ()
    {
        pathfinding = FindObjectOfType<Pathfinding>();
	}
	

	void Update ()
    {
        if(pathfinding.FindPath(transform.position, target.transform.position))
        {
            path = pathfinding.GetLastPath();
        }

        reachableNodes = pathfinding.GetReachableNodes(transform.position, unitRange);
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
}
