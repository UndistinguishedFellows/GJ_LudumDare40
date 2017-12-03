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

		private BehaviorExecutor executor = null;

		void Start()
		{
			executor = GetComponent<BehaviorExecutor>();
		}
		void Update()
		{
			//This is a fucking shit but idk how to acces to the blackboard...
			List<string> names = executor.blackboard.intParamsNames;
			List<int> flags = executor.blackboard.intParams;
			int GameOver = 0;
			for (int i = 0; i < names.Count; ++i)
			{
				if (names[i].Equals("GameOver"))
				{
					GameOver = flags[i];
				}
			}
			if(GameOver > 0)
				Debug.Log("GameOver");
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
	}

