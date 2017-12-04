using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnterTrigger : MonoBehaviour
{
	private bool hasExited = false;

	private GameManager gm;

	//----------------------------------------

	void Awake()
	{
		gm = FindObjectOfType<GameManager>();
	}

	void OnTriggerEnter(Collider col)
	{
		if (hasExited)
		{
			gm.EnterEndArea();
		}
		else
		{
			gm.levelState = GameManager.LevelState.LS_Start;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (!hasExited)
		{
			hasExited = true;
		}

		gm.ExitedEndArea();
	}
}
