using BBUnity.Actions;
using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using UnityEngine;

[Action("Scripts/GoToNextNode")]
[Help("Gets the next node near the closest node of the graph")]
public class ActionGoToNode : GOAction
{
	// Define the input parameter "graph".
	[InParam("graph")]
	public NodeManager graph;

	[OutParam("RandomNodePosition")]
	
	public Vector3 randomPosition { get; set; }


	private UnityEngine.AI.NavMeshAgent navAgent;

	// Initialization method. If not established, we look for the shooting point.
	public override void OnStart()
	{
		navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
		//Find the next node
		NavNode node = graph.GetClosestNode(navAgent.gameObject.transform);
		Debug.LogWarning("Next Node " + node.name);

		Vector3 pos = node.GetRandomNode().transform.position;
		randomPosition = new Vector3(pos.x, pos.y, pos.z);

		base.OnStart();
	} // OnStart

	public override TaskStatus OnUpdate()
	{
		return TaskStatus.COMPLETED;
	}

}
