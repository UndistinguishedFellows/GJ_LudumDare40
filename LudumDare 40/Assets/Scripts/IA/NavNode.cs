using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavNode : MonoBehaviour
{
    public List<NavNode> neighbours;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnDrawGizmos()
    {
	    foreach (var neighbour in neighbours)
	    {
			Gizmos.color = Color.red;
		    Gizmos.DrawLine(transform.position, neighbour.transform.position);
	    }
    }

	public NavNode GetRandomNode()
	{
		int pos = Random.Range(0, neighbours.Count);
		return neighbours[pos];
	}
}
