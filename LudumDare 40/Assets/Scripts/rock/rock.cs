using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
	private Vector3 destination;
	private Vector2 velocity;

	public float rockSpeed = 3f;
	private float speed;

	public float noiseDefaultRadius = 2f;
	private float noiseRadius;

	public LayerMask noiseDetectorsLayer;

	public GameObject area;

	private bool mustEndMovement = false;

	private Rigidbody2D rb;
    
	//-------------------------------------------------

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		mustEndMovement = true;
	}

	//-------------------------------------------------

	public void GoToPoint(Vector3 _destination, float maxPlayerThrowDistance)
	{
		destination = _destination;
		destination.z = 0;

		// Will set speed according to the distance.
		speed = rockSpeed; //(Vector3.Distance(transform.position, _destination) / maxPlayerThrowDistance) * rockSpeed;

		Vector2 dir = (destination - transform.position);
		dir.Normalize();
		velocity = dir * speed;

		StartCoroutine(MoveRock());
	}

	IEnumerator MoveRock()
	{
		while (Vector3.Distance(transform.position, destination) >= 0.1 && !mustEndMovement) //Let a bit of threshold
		{
			rb.position += velocity * Time.deltaTime;

			// TODO: Animatios

			yield return null;
		}

		if (mustEndMovement)
		{
			// Bounce??
		}

		ProduceNoise();
	}

	void ProduceNoise()
	{
		area.SetActive(true);

		// Collect all listeners could listen to the rock.

		Collider2D[] targets = Physics2D.OverlapCircleAll(rb.position, noiseDefaultRadius, noiseDetectorsLayer);
		foreach (Collider2D target in targets)
		{
			target.BroadcastMessage("OnNoise",  target.transform.position); //TODO: 
		}


		//Debug.Log("Rock arrived");
		// TODO: Can play an animation to decrease the alpha and destroy it after that
		Destroy(gameObject, 1f);
	}
}
