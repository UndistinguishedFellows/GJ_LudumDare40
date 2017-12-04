using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int maxItemsToPick = 3;
    private int itemsPicked = 0;

	public int[] energyCostIncrements;
	public float[] speedIncrements;
	public float[] noiseRadiusIncrements;

	public bool gameOver = false;

	private CharacterMovement playerController;

	private float levelTimeElapse = 0f;

	private bool gameStarted = false;
	private bool gameEnded = false;

    //-------------------------------------------
    void Awake ()
    {
	    playerController = FindObjectOfType<CharacterMovement>();
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
		while (true) // Player is appearing
		{
			yield return null;
			break;
		}

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
