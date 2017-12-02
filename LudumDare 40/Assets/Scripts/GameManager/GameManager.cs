using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int maxItemsToPick = 3;
    private int itemsPicked = 0;

    public float playerDefaultSpeed = 2f;
    public float speedToReduce = 1f;
    private float playerSpeed;

    public float playerDefaultNoise = 1f;
    public float noiseToIncrease = 1f;
    private float playerNoise;

    //-------------------------------------------
    void Awake ()
    {
        playerSpeed = playerDefaultSpeed; //Can change this to get the speed from the player.
        playerNoise = playerDefaultNoise;
    }
	

	void Update ()
    {
		
	}

    //-------------------------------------------

    public float PlayerNoise
    {
        get { return playerNoise; }
    }

    public float PlayerSpeed
    {
        get { return playerSpeed; }
    }

    //-------------------------------------------

    public void ItemCollected()
    {
        // TODO: Modify UI to show stars??
        itemsPicked++;

		

        if (itemsPicked > maxItemsToPick)
        {
            Debug.LogError("Picked more items than max items to pick.");
        }
        
        playerNoise += noiseToIncrease;
        playerSpeed -= speedToReduce;

        // TODO: Set player's speed and noisiness

    }
}
