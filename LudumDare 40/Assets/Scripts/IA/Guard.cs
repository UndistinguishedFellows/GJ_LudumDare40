using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class Guard : MonoBehaviour
	{
		//[HideInInspector]
		public bool inRoute;

		public float visionAngle = 60;
		public float visionDistance = 10;



		void OnDrawGizmos()
		{
			Gizmos.color = Color.black;
			Gizmos.DrawWireSphere(transform.position, visionDistance);
			Vector3 position1 = transform.position;
			position1.z += visionDistance;
			Quaternion rot = Quaternion.AngleAxis(visionAngle/2, Vector3.forward);
			Vector3 dir = rot * transform.forward;
			dir.Normalize();
			position1 = rot * position1;
			Gizmos.color = Color.black;
			Gizmos.DrawLine(transform.position, transform.position + dir * visionDistance);
			rot = Quaternion.AngleAxis(-visionAngle / 2, Vector3.forward);
			dir = rot * transform.forward;
			dir.Normalize();
			Gizmos.color = Color.black;
			Gizmos.DrawLine(transform.position, transform.position + dir * visionDistance);
			
		}
	}

