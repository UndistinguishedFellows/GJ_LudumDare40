using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
	private Vector3 destination;
	private Vector3 velocity;

	public float rockSpeed = 3f;
	private float speed;

	public float noiseDefaultRadius = 2f;
	private float noiseRadius;

	public LayerMask noiseDetectorsLayer;

	public GameObject area;

	private bool mustEndMovement = false;

	private Rigidbody rb;

	public AudioSource aSArrived;
	public AudioSource aSBounce;

	//-------------------------------------------------

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	void OnTriggerEnter(Collider coll)
	{
		if(coll.tag != "Player" && coll.tag != "Floor")
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
			aSBounce.Play();
		}
		else
		{
			aSArrived.Play();
		}

		ProduceNoise();
	}

	void ProduceNoise()
	{
		area.SetActive(true);

		// Collect all listeners could listen to the rock.

		Collider[] targets = Physics.OverlapSphere(rb.position, noiseDefaultRadius, noiseDetectorsLayer);
		foreach (Collider target in targets)
		{
			target.BroadcastMessage("OnNoise", transform.position); //TODO:  Grphical noise feed
		}


		//Debug.Log("Rock arrived");
		// TODO: Can play an animation to decrease the alpha and destroy it after that
		Destroy(gameObject, 1f);
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, noiseRadius);
	}
}