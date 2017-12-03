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
        player = FindObjectOfType<CharacterMovement>(); // TODO: Adapt to player controller class
    }

    void OnTriggerEnter2D(Collider2D other)
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

    void OnTriggerExit2D(Collider2D other)
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
        gameObject.SetActive(false);
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
