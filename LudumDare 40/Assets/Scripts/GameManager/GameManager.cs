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

    //-------------------------------------------
    void Awake ()
    {
	    playerController = FindObjectOfType<CharacterMovement>();
    }
	

	void Update ()
    {
	    if (gameOver)
	    {
		    Debug.Log("GameOver");
	    }
	}

    //-------------------------------------------

    

    //-------------------------------------------

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
		
		// TODO: Modify UI to show stars??
		itemsPicked++;
		
    }
}
