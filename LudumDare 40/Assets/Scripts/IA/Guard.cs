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
	private bool wasMoving = false;

	public enum Directions
	{
		DIR_UP,
		DIR_RIGHT,
		DIR_DOWN,
		DIR_LEFT
	}
	private Directions currentDirection = Directions.DIR_UP;
	//------------------------------------

	private BehaviorExecutor executor = null;
	private Rigidbody rb;
	private Animator animator;

	void Start()
		{
			executor = GetComponent<BehaviorExecutor>();
			rb = GetComponentInParent<Rigidbody>();
			animator = GetComponentInChildren<Animator>();
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

			Vector3 velocity = transform.InverseTransformDirection(rb.velocity);
			
			Directions direction = Directions.DIR_UP;

			if (Mathf.Abs(velocity.x) >= Mathf.Abs(velocity.y))
			{
				if (velocity.x > 0)
				{
					direction = Directions.DIR_RIGHT;
				}
				else
				{
					direction = Directions.DIR_LEFT;
				}
			}
			else
			{
				if (velocity.y > 0)
				{
					direction = Directions.DIR_UP;
				}
				else
				{
					direction = Directions.DIR_DOWN;
				}
			}

			if (direction != currentDirection || !wasMoving)
			{
				// TODO: Change animation.
				currentDirection = direction;

				switch (currentDirection)
				{
					case Directions.DIR_LEFT:
						animator.SetInteger("direction", 3);
						break;
					case Directions.DIR_RIGHT:
						animator.SetInteger("direction", 2);
						break;
					case Directions.DIR_UP:
						animator.SetInteger("direction", 1);
						break;
					case Directions.DIR_DOWN:
						animator.SetInteger("direction", 0);
						break;
				}
			}

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