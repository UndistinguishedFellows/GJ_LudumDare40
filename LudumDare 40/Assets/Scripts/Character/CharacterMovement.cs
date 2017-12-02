using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {
    public float speed;
    public void SetSpeed(float value)
    {
        speed = value;
    }
    public float GetSpeed()
    {
        return speed;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        InputMovement();
        InputMouseAction();
	}

    public void InputMovement()
    {
        float multiplyY = 0.0f;
        float multiplyX = 0.0f;
        if (Input.GetKey(KeyCode.W))
        {
            multiplyY = Time.deltaTime * speed;
        }else if (Input.GetKey(KeyCode.S))
        {
            multiplyY = -Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            multiplyX = Time.deltaTime * speed;
        }else if (Input.GetKey(KeyCode.A))
        {
            multiplyX = -Time.deltaTime * speed;
        }

        transform.position = new Vector3(transform.position.x + multiplyX, transform.position.y + multiplyY, transform.position.z);
    }
    public void InputMouseAction()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Debug.Log("mouse R click at: " + Input.mousePosition);
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Debug.Log("mouse L click at" + Input.mousePosition);
        }
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            Debug.Log("Mouse scroll UP");
        }else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            Debug.Log("Mouse scroll DOWN");
        }
    }
}
