using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour {

    // Use this for initialization
    public GameObject esquerre, central, dreta;
    public int currentLvl = 0;
    public int amountOfLevles = 1;
    public float interpollator = 0.0f;
	void Start () {
        //Debug.Log(central.transform.localPosition);
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(central.transform.localPosition);
    }


    public void ButtonAnimationLeft()
    {
        currentLvl--;
        currentLvl = currentLvl < 0 ? amountOfLevles : currentLvl;
        StartCoroutine(interpolator('d', central));
        StartCoroutine(interpolator('d', dreta));
        StartCoroutine(interpolator('d', esquerre));
    }
    public void ButtonAnimationRight()
    {
        currentLvl++;
        currentLvl = currentLvl % amountOfLevles;
        StartCoroutine(interpolator('e', central));
        StartCoroutine(interpolator('e', esquerre));
        StartCoroutine(interpolator('e', dreta));
    }
    public void ButtonPlayLevel()
    {

    }
    public void ButtonGoBack()
    {

    }
    IEnumerator interpolator(char direction, GameObject a) {
        interpollator = 0.0f;
        float aPos = a.transform.localPosition.x;
        Debug.Log("apos"+aPos);
        Transform trasnform = a.transform;
        float b;
        float newPosx;
        if(direction == 'e')
        {
            b = aPos - 1920;
            Debug.Log(b);
        }
        else
        {
            b= aPos + 1920;
            Debug.Log(b);
        }
        while (interpollator < 1.0f)
        {

            interpollator += Time.deltaTime;
            newPosx = Mathf.Lerp(aPos, b, interpollator);
            Debug.Log(newPosx);
            trasnform.localPosition = new Vector3(newPosx,0, 0);
            yield return null;
        }
        interpollator = 1;
        trasnform.localPosition = new Vector3(b, 0, 0);
    }
    public void AnimationR()
    {

       
    }

}
