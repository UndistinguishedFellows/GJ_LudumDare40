﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{

    public float initialSpeed = 5f;
	private float speed;
	public float crouchSpeedDevider = 2f;
	public float crouchNoiseDevider = 2f;

	private bool isCrouch = false;

	public int initialEnergy = 10;
	private int energyAvailable;

	public int initialHabilityEnergyCost = 1;
	private int habilityEnergyCost;

	public float interactionRange = 5f;
	public float initialNoiseRadius = 10f;
	private float noiseRadius;
	public float stoneThrowRange = 15f;

	public float interactionDuration = 2f;
	private float interactionCounter = 0f;
	private bool isInteracting = false;
	
	public float habilitiesCD = 2f;

	private float timeElapsedSinceLastHabiliy;
	
	private Walkie locatedWalkie = null;

    public Image[] skillsImages;
    public Slider energyBarSlider;
	

    public GameObject rockPrefab;
	public GameObject walkiePrefab;

	public GameObject rotationAxis;
	public GameObject skillspawnSkillsTrasnform;
	
	public LayerMask noiseDetectorsLayer;

	private bool bInDash = false;
	private int skillFocus = 0;

	public float interactionNoiseMultiplier = 0.5f;

	private GameManager gm;

	public enum Directions
	{
		DIR_UP,
		DIR_RIGHT,
		DIR_DOWN,
		DIR_LEFT
	}

	private Directions currentDirection = Directions.DIR_UP;

	private List<PickItem> reachableItems;

	private Rigidbody rb;

	public AudioSource stepsAsAudioSource;
	public AudioSource stepsCrouchAsAudioSource;
	public AudioSource interactAudioSource;

	public AudioSource cdErrorAudioSource;
	public AudioSource cdRestoredAudioSource;

	private Animator animator;

	private bool lasFrameWasCDUp = true;
	private bool wasMoving = false;

	//------------------------------------

	void Awake()
	{
		gm = FindObjectOfType<GameManager>();
		rb = GetComponent<Rigidbody>();
	}

	void Start()
	{
		ChangeSkillFocus(-1);

		timeElapsedSinceLastHabiliy = habilitiesCD; // Set to cd in order to be able to throw one on start.

		speed = initialSpeed;
		energyAvailable = initialEnergy;
		habilityEnergyCost = initialHabilityEnergyCost;
		noiseRadius = initialNoiseRadius;

		reachableItems = new List<PickItem>();

		SphereCollider col = GetComponent<SphereCollider>();
		if (col != null) col.radius = interactionRange;

		animator = GetComponentInChildren<Animator>();
	}
	
	void Update()
	{
		InputMovement();
		InputMouseAction();
		Rotation();
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, stoneThrowRange);

		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, interactionRange);

		Gizmos.color = Color.black;
		Gizmos.DrawWireSphere(transform.position, initialNoiseRadius);
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, isCrouch ? noiseRadius / crouchNoiseDevider : noiseRadius);
	}

	//------------------------------------
	
	public float Speed
	{
		get { return speed; }
		set { speed = value; }
	}

	public float NoiseRadius
	{
		get { return noiseRadius; }
		set { noiseRadius = value; }
	}

	public int HabEnergyCost
	{
		get { return habilityEnergyCost; }
		set { habilityEnergyCost = value; }
	}

	//------------------------------------

	public void InputMovement()
	{
		if (!isInteracting)
		{
			Vector3 vel = Vector3.zero;

			vel.x = Input.GetAxisRaw("Horizontal");
			vel.y = Input.GetAxisRaw("Vertical");

			vel.Normalize();
			vel.z = 0;

			float sp = speed;
			float nRad = noiseRadius;

			if (Input.GetKey(KeyCode.LeftControl))
				isCrouch = true;
			else if (isCrouch)
				isCrouch = false;

			if (isCrouch)
			{
				sp /= crouchSpeedDevider;
				nRad /= crouchNoiseDevider;
			}

			vel *= (sp * Time.deltaTime);

			if (vel != Vector3.zero)
			{
				GetDirectionFromAngle(vel);
				if (!wasMoving)
				{
					wasMoving = true;
				}
				
				rb.position += vel;

				// Reproduce steps sound
				if (isCrouch)
				{
					if(stepsAsAudioSource.isPlaying)
						stepsAsAudioSource.Stop();

					if (!stepsCrouchAsAudioSource.isPlaying)
						stepsCrouchAsAudioSource.Play();
				}
				else
				{
					if (stepsCrouchAsAudioSource.isPlaying)
						stepsCrouchAsAudioSource.Stop();

					if (!stepsAsAudioSource.isPlaying)
						stepsAsAudioSource.Play();
				}

					// Generate noise
					Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, nRad, noiseDetectorsLayer);
				foreach (Collider2D col in cols)
				{
					col.BroadcastMessage("OnNoise", transform.position); //TODO: Graphical noise feed
				}
			}
			else
			{
				// Stop steps sound
				if (stepsAsAudioSource.isPlaying)
				{
					stepsAsAudioSource.Stop();
				}
				if (stepsCrouchAsAudioSource.isPlaying)
				{
					stepsCrouchAsAudioSource.Stop();
				}

				if (wasMoving)
				{
					animator.SetInteger("direction", 0);
				}
				wasMoving = false;

			}
		}
		else
		{
			// if player is interacting, generate noise the half radius of the run radius.
			Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, noiseRadius * interactionNoiseMultiplier, noiseDetectorsLayer);
			foreach (Collider2D col in cols)
			{
				col.BroadcastMessage("OnNoise", transform.position); //TODO:  Grphical noise feed
			}

			if (stepsAsAudioSource.isPlaying)
			{
				stepsAsAudioSource.Stop();
			}
			if (stepsCrouchAsAudioSource.isPlaying)
			{
				stepsCrouchAsAudioSource.Stop();
			}
		}
	}

	public void InputMouseAction()
	{
		timeElapsedSinceLastHabiliy += Time.deltaTime;
		if (timeElapsedSinceLastHabiliy >= habilitiesCD && !lasFrameWasCDUp)
		{
			cdRestoredAudioSource.Play();
			lasFrameWasCDUp = true;
		}

		if (reachableItems.Count > 0)
		{
			if (Input.GetKeyDown(KeyCode.F)) // Left mouse button
			{
				// Begin the interaction
				isInteracting = true;
				interactAudioSource.Play();
			}
			else if (Input.GetKey(KeyCode.F))
			{
				// Keep interaction
				if (interactionCounter >= interactionDuration)
				{
					interactAudioSource.Stop();
					isInteracting = false;
					interactionCounter = 0f;

					foreach (PickItem item in reachableItems)
					{
						item.Pick();
					}
					reachableItems.Clear();
				}
				else
				{
					interactionCounter += Time.deltaTime;
				}
			}
			else if (Input.GetKeyUp(KeyCode.F))
			{
				// Stop interaction
				isInteracting = false;
				interactionCounter = 0f;
				interactAudioSource.Stop();
			}

		}

		if (Input.GetKeyDown(KeyCode.Mouse1)) // Right mouse button
		{
			if (timeElapsedSinceLastHabiliy >= habilitiesCD)
			{
				if (energyAvailable >= habilityEnergyCost)
				{
					lasFrameWasCDUp = false;
					ChangeEnergy(-1);

					switch (skillFocus)
					{
						case 0:
							SpawnRock();
							break;

						case 1:
						{
							if (locatedWalkie == null)
								SpawnWalkie();
							else if (!locatedWalkie.IsReproducing) locatedWalkie.Activate();

							break;
						}

						case 2:
						{
							//Dash();
							break;
						}
					}
				}
				else
				{
					cdErrorAudioSource.Play();
				}

				timeElapsedSinceLastHabiliy = 0f;
			}
			else
			{
				cdErrorAudioSource.Play();
			}
		}

		if (Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			//Debug.Log("Mouse scroll UP");
			ChangeSkillFocus(-1);
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			//Debug.Log("Mouse scroll DOWN");
			ChangeSkillFocus(1);
		}
	}
	
	void Rotation()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - rotationAxis.transform.position;
        difference.Normalize();
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
		//GetDirectionFromAngle(rotationZ);
        rotationAxis.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ - 90);
    }

	//------------------------------------

	void GetDirectionFromAngle(Vector3 velocity)
	{
		Directions direction = Directions.DIR_UP;

		if (Mathf.Abs(velocity.x) >= Mathf.Abs(velocity.y))
		{
			if (velocity.x > 0)
			{
				direction = Directions.DIR_RIGHT;
			}
			else
			{
				direction = Directions.DIR_LEFT;
			}
		}
		else
		{
			if (velocity.y > 0)
			{
				direction = Directions.DIR_UP;
			}
			else
			{
				direction = Directions.DIR_DOWN;
			}
		}

		if (direction != currentDirection || !wasMoving)
		{
			// TODO: Change animation.
			currentDirection = direction;

			switch (currentDirection)
			{
				case Directions.DIR_LEFT:
					animator.SetInteger("direction", 4);
					break;
				case Directions.DIR_RIGHT:
					animator.SetInteger("direction", 3);
					break;
				case Directions.DIR_UP:
					animator.SetInteger("direction", 2);
					break;
				case Directions.DIR_DOWN:
					animator.SetInteger("direction", 1);
					break;
			}
		}
	}

	void SpawnRock()
    {
	    Vector3 worldMousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	    worldMousePoint.z = 0;

		float distance = Vector3.Distance(worldMousePoint, transform.position);
	    if (distance > stoneThrowRange)
	    {
			// If  mouse is out of range, set the spawn position to the limit range on the direction.
		    Vector3 dir = worldMousePoint - transform.position;
		    dir.z = 0;
			dir.Normalize();
		    dir *= stoneThrowRange;
		    worldMousePoint = transform.position + dir;
	    }

		GameObject rock = (GameObject)Instantiate(rockPrefab, skillspawnSkillsTrasnform.transform.position, Quaternion.identity);
        rock.GetComponent<Rock>().GoToPoint(worldMousePoint, stoneThrowRange);
    }

	public void SpawnWalkie()
	{
		GameObject walkieobj = (GameObject)Instantiate(walkiePrefab, skillspawnSkillsTrasnform.transform.position, Quaternion.identity);
		locatedWalkie = walkieobj.GetComponent<Walkie>();
	}

	public void Dash()
	{
		if (!bInDash)
		{
			StartCoroutine(DashTime());
		}

	}






	public void ChangeSkillFocus(int amount)
    {
        skillFocus += amount;
        skillFocus = Mathf.Clamp(skillFocus, 0, skillsImages.Length - 1);
        
        foreach(Image skill in skillsImages)
        {
            skill.color = Color.grey;
        }
        skillsImages[skillFocus].color = Color.white;
    }

	public void SetEnergyCost(int value)
	{
		habilityEnergyCost = value;
	}

	public void ChangeEnergy(int amount)
    {
        energyAvailable += amount * habilityEnergyCost;
        energyAvailable = Mathf.Clamp(energyAvailable, 0, (int)energyBarSlider.maxValue);
        energyBarSlider.value = energyAvailable;
    }


	//------------------------------------

	public void ItemReached(PickItem item)
	{
		reachableItems.Add(item);
	}

	public void ItemLost(PickItem item)
	{
		reachableItems.Remove(item);
	}

	//------------------------------------

	IEnumerator DashTime()
    {
        bInDash = true;
        float initialSpeed = speed;
        float time = 0.2f;
        speed = speed * 3;
        
        while (time>0)
        {
            Debug.Log(time);
            time -= Time.deltaTime;
            yield return null;
        }
        speed = initialSpeed;
        bInDash = false;
    }

	
}





