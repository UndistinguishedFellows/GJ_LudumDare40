using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

	public Transform target;
	public float smoothStep = 0.125f;

	public Vector3 offset;
	public bool autoCalcOffset = true;

	// --------------------------------

	void Start()
	{
		if (autoCalcOffset)
		{
			offset = target.position - transform.position;
		}
	}

	void Update ()
	{
		Vector3 dest = target.position + offset;
		transform.position = Vector3.Lerp(transform.position, dest, smoothStep);
		transform.LookAt(target);
	}

	// --------------------------------
}
