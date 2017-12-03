using BBUnity.Conditions;
using Pada1.BBCore;
using UnityEngine;

[Condition("Scripts/IsCloseToNode")]
[Help("Checks if the guard is in route")]

public class ConditionIsCloseToANode : GOCondition
{
	[InParam("closeDistance")]
	[Help("The maximun distance to consider that the target is close")]
	public float closeDistance;

	[InParam("nodeManager")]
	[Help("Node manager containing the graph")]
	public NodeManager nodeManager;

	private UnityEngine.AI.NavMeshAgent navAgent;

	public override bool Check()
	{
		navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();

		NavNode node = nodeManager.GetClosestNode(navAgent.transform);

		float distance = Vector3.Distance(node.transform.position, 
											navAgent.transform.position);
		if (distance < closeDistance)
		{
			return false;
		}

		return true;
	}
}
