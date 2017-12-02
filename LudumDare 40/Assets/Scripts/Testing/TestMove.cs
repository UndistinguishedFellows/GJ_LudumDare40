using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    private Rigidbody2D rb;

	// Use this for initialization
	void Start ()
	{
	    rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	    p.z = 0;
	    rb.position = p;
	}
    
}
