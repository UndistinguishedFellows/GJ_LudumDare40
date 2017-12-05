using System.Collections;
using System.Collections.Generic;
using BBUnity;
using UnityEngine;

public class Guard : MonoBehaviour
{
	//[HideInInspector]
	public bool inRoute;

	public float visionAngle = 60;
	public float visionDistance = 10;
	public float pursueDistance = 15;

	public Vector3 suspiciousSpot;
	public bool search = false;

	//------------------------------------

	private BehaviorExecutor executor = null;

	void Start()
		{
			executor = GetComponent<BehaviorExecutor>();
		}
	void Update()
		{
			
		}

	void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, visionDistance);
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(transform.position, pursueDistance);
			Vector3 position1 = transform.position;
			position1.z += visionDistance;
			Quaternion rot = Quaternion.AngleAxis(visionAngle/2, Vector3.forward);
			Vector3 dir = rot * transform.forward;
			dir.Normalize();
			position1 = rot * position1;
			Gizmos.color = Color.magenta;
			Gizmos.DrawLine(transform.position, transform.position + dir * pursueDistance);
			rot = Quaternion.AngleAxis(-visionAngle / 2, Vector3.forward);
			dir = rot * transform.forward;
			dir.Normalize();
			Gizmos.color = Color.magenta;
			Gizmos.DrawLine(transform.position, transform.position + dir * pursueDistance);
			
		}

	//------------------------------------

	public void OnNoise(Vector3 pos)
	{
		search = true;
		suspiciousSpot = pos;
	}

	//------------------------------------
}