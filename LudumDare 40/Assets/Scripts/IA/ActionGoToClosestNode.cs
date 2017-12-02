using BBUnity.Actions;
using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using UnityEngine;

[Action("Scripts/GoToClosestNode")]
[Help("Gets the next node near the closest node of the graph")]
public class ActionGoToClosestNode : GOAction
{
	// Define the input parameter "graph".
	[InParam("graph")]
	public NodeManager graph;
	
	[OutParam("RandomNodePosition")]
	
	public Vector3 position { get; set; }

	private UnityEngine.AI.NavMeshAgent navAgent;

	// Initialization method. If not established, we look for the shooting point.
	public override void OnStart()
	{
		navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
		//Find the next node
		NavNode node = graph.GetClosestNode(navAgent.gameObject.transform);
		Debug.LogWarning("Closest Node " + node.name);
		Vector3 pos = node.transform.position;
		position = new Vector3(pos.x, pos.y, pos.z);

		base.OnStart();
	} // OnStart

	public override TaskStatus OnUpdate()
	{
		Debug.Log("Closest node Completed task");
		return TaskStatus.COMPLETED;
	}

}
