using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class delayExit : MonoBehaviour {

	// Use this for initialization
	void Awake ()
	{
		StartCoroutine(Delay());
	}

	IEnumerator Delay()
	{
		yield return new WaitForSeconds(5);
		Application.Quit();
	}


}
