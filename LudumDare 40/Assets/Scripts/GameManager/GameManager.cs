using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	private GameObject playerGO;

	private float levelTimeElapse = 0f;

	private bool gameStarted = false;
	private bool gameEnded = false;

	public Text timeCounter;
	public Text infoText;

	public enum LevelState
	{
		LS_Start,
		LS_Game,
		LS_End
	}

	public LevelState levelState = LevelState.LS_Start;

    //-------------------------------------------
    void Awake ()
    {
	    playerController = FindObjectOfType<CharacterMovement>();
	    playerGO = playerController.gameObject;
	    playerTransform = playerGO.transform;
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
			float secs = levelTimeElapse % 60;
			float min = levelTimeElapse / 60;

			timeCounter.text = string.Format("{0:0}:{1:00}", (int)min, (int)secs);
		}

		if (levelState == LevelState.LS_End)
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				gameEnded = true;
				StartCoroutine(OnLevelEnded());
			}
		}
	}

    //-------------------------------------------

	IEnumerator InitialAnimation()
	{
		BoxCollider c = playerGO.GetComponent<BoxCollider>();
		c.enabled = false;

		playerTransform.position = ropeStart.position;

		yield return new WaitForSeconds(0.5f);
		
		while (Vector3.Distance(playerTransform.position, endRope.position) >= 0.2f) // Player is appearing
		{
			Vector3 dir = endRope.position - ropeStart.position;
			dir.Normalize();
			dir.z = 0f;
			dir *= (ropeMoveSpeed * Time.deltaTime);

			playerTransform.position += dir;
		
			yield return null;
		}

		playerTransform.position = endRope.position;
		c.enabled = true;

		infoText.gameObject.SetActive(true);
		infoText.text = "Start";

		yield return new WaitForSeconds(1.5f);

		infoText.gameObject.SetActive(false);

		// Enable character control
		playerController.enabled = true;

		// Reproduce audio TODO: 
		
		// Start time counter
		gameStarted = true;
	}

	IEnumerator OnGameOver()
	{
		playerController.enabled = false;
		infoText.text = "Game over";

		yield return new WaitForSeconds(1.5f);

		// TODO: Fade??

		// TODO: Serialize points data and change scene

	}

	IEnumerator OnLevelEnded()
	{
		// Disable button image
		//TODO

		BoxCollider c = playerGO.GetComponent<BoxCollider>();
		c.enabled = false;

		// First go to end rope

		while (Vector3.Distance(playerTransform.position, endRope.position) >= 0.2f)
		{
			Vector3 dir = endRope.position - playerTransform.position;
			dir.Normalize();
			dir.z = 0f;
			dir *= (ropeMoveSpeed * Time.deltaTime);

			playerTransform.position += dir;

			yield return null;
		}

		playerTransform.position = endRope.position;

		yield return new WaitForSeconds(0.75f);

		// Now move throw the rope

		while (Vector3.Distance(playerTransform.position, ropeStart.position) >= 0.2f)
		{
			Vector3 dir = ropeStart.position - playerTransform.position;
			dir.Normalize();
			dir.z = 0f;
			dir *= (ropeMoveSpeed * Time.deltaTime);

			playerTransform.position += dir;

			yield return null;
		}

		playerTransform.position = ropeStart.position;

		infoText.gameObject.SetActive(true);
		infoText.text = "Level ended!";

		// TODO: Fade??

		// TODO: Serialize points data and change scene
	}

	// -------------------------------------------

	public void EnterEndArea()
	{
		levelState = LevelState.LS_End;

		//Disable button image //TODO
	}

	public void ExitedEndArea()
	{
		levelState = LevelState.LS_Game;

		//Enable button image //TODO

	}

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
