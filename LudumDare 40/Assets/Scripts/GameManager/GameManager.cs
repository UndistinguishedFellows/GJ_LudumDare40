﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int maxItemsToPick = 3;
    private int itemsPicked = 0;

	public int[] energyCostIncrements;
	public float[] speedIncrements;
	public float[] noiseRadiusIncrements;

	public Transform ropeStart;
	public Transform endRope;

	public float ropeMoveSpeed = 3f;

	public bool gameOver = false;

	private CharacterMovement playerController;
	private Transform playerTransform;

	private float levelTimeElapse = 0f;

	private bool gameStarted = false;
	private bool gameEnded = false;

    //-------------------------------------------
    void Awake ()
    {
	    playerController = FindObjectOfType<CharacterMovement>();
	    playerTransform = playerController.transform;
    }

	void Start()
	{
		StartCoroutine(InitialAnimation());
	}

	void Update ()
	{
		if (gameStarted && !gameOver && !gameEnded)
		{
			levelTimeElapse += Time.deltaTime;
			// Modify UI

		}
	}

    //-------------------------------------------

	IEnumerator InitialAnimation()
	{
		//playerTransform.position = ropeStart.position;
		
		//while (Vector3.Distance(playerController.transform.position, endRope.position) >= 0.2f) // Player is appearing
		//{
		//	Vector3 dir = endRope.position - ropeStart.position;
		//	dir.Normalize();
		//	dir.z = 0f;
		//	dir *= ropeMoveSpeed;
		//
		//
		//
		//	yield return null;
		//	break;
		//}

		yield return new WaitForSeconds(1.5f);


		// Enable character control
		playerController.enabled = true;
		// Reproduce audio

		// Start time counter
		gameStarted = true;
	}

	IEnumerator OnGameOver()
	{
		playerController.enabled = false;
		yield return null;
	}

	IEnumerator OnLevelEnded()
	{
		yield return null;
	}

	// -------------------------------------------

	public void ItemCollected()
    {
	    if (itemsPicked >= maxItemsToPick)
	    {
			// Should never get here but just in case.
		    return;
	    }

	    playerController.Speed = playerController.Speed - speedIncrements[itemsPicked];
	    playerController.HabEnergyCost = playerController.HabEnergyCost + energyCostIncrements[itemsPicked];
	    playerController.NoiseRadius = playerController.NoiseRadius + noiseRadiusIncrements[itemsPicked];
		
		itemsPicked++;
		
    }

	public void GameOver()
	{
		gameOver = true;
		StartCoroutine(OnGameOver());
	}

	public void EndLevel()
	{
		gameEnded = true;
		StartCoroutine(OnLevelEnded());
	}
}
