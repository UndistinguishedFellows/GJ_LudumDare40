﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickItem : MonoBehaviour
{
    public Image icon;
    public GameObject alertMark;
    private GameManager gm;
    private CharacterMovement player;

    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
		if(gm == null)  Debug.LogAssertion("Could not get game manager.");
        player = FindObjectOfType<CharacterMovement>();
    }

    void OnTriggerEnter(Collider other)
    {
        // NOTE: Tag the player.
        if (other.tag.Equals("Player"))
        {
            // Active a mark to feed player this item is reachable.
            alertMark.SetActive(true);

			// Notify player this item is reachable
			player.BroadcastMessage("ItemReached", this);
		}
        
    }

    void OnTriggerExit(Collider other)
    {
        // NOTE: Tag the player.
        if (other.tag.Equals("Player"))
        {
            // Disable a mark to feed player this item is reachable.
            alertMark.SetActive(false);

			// Notify player this item is no longer reachable
	        player.BroadcastMessage("ItemLost", this);
		}
    }

    //-------------------------------------------------

    public void Pick()
    {
        // 1. Disable the item and alert mark.
        gameObject.SetActive(false); // TODO: Destroy
        alertMark.SetActive(false);

        // 2. Enable the UI icon.
        icon.gameObject.SetActive(true);

        // 3. Notify the manager.
        gm.ItemCollected();

		// 4. Notify the player.
	    player.BroadcastMessage("ItemLost", this);
	}

    //-------------------------------------------------
}
