using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walkie : MonoBehaviour
{

	public float noiseDuration = 5f;
	private float noiseCounter = 0f;

	public float noiseRadius;
	public LayerMask noiseListenersLayer;

	public GameObject noiseMark;

	public bool useAudioClipDurationAsNoiseDuration = true;

	private bool isOn = false;

	private AudioSource aPlayer;

	// --------------------------------------

	public bool IsReproducing
	{
		get { return isOn; }
	}

	// --------------------------------------

	void Awake()
	{
		aPlayer = GetComponent<AudioSource>();

		if (useAudioClipDurationAsNoiseDuration)
		{
			noiseDuration = aPlayer.clip.length;
		}
	}

	void Update()
	{
		if (isOn)
		{
			if (noiseCounter >= noiseDuration)
			{
				isOn = false;
				noiseMark.SetActive(false);
				aPlayer.Stop();
			}
			else
			{
				// Generte noise
				Collider[] cols = Physics.OverlapSphere(transform.position, noiseRadius, noiseListenersLayer);
				foreach (Collider col in cols)
				{
					col.BroadcastMessage("OnNoise", transform.position);
				}

				noiseCounter += Time.deltaTime;
			}
		}
	}

	// --------------------------------------

	public void Activate()
	{
		isOn = true;
		noiseCounter = 0f;
		noiseMark.SetActive(true);
		aPlayer.Play();
	}

	// --------------------------------------
}
