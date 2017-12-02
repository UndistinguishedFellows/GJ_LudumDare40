using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class NodeManager : MonoBehaviour
{
    public List<NavNode> navigationNodeList;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

	public NavNode GetClosestNode(Transform transform)
	{
		NavNode closestNode = null;
		float closestDistance = float.MaxValue;
		foreach (var navNode in navigationNodeList)
		{
			float tmpDistance = Vector3.Distance(navNode.transform.position, transform.position);
			if (tmpDistance < closestDistance)
			{
				closestDistance = tmpDistance;
				closestNode = navNode;
			}

		}

		return closestNode;
	}
}
