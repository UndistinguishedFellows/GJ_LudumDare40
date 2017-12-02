using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{

    public float speed;
	public float stoneThrowRange = 15f;
	public float throwStoneCd = 2f;

	private float timeElapsedSinceLastThrow;


    public Image[] skills;
    public Slider energyBar;

    public int energy;
    public int energyCost;

    public GameObject rockGameobject;
	public GameObject walkie;

	public GameObject rotationAxis;
	public GameObject skillspawnSkillsTrasnform;

    private bool bInDash = false;
	private int skillFocus = 0;

	private GameManager gm;
	

	//------------------------------------

	void Awake()
	{
		gm = FindObjectOfType<GameManager>();
	}

	void Start()
	{
		ChangeSkillFocus(-1);

		timeElapsedSinceLastThrow = throwStoneCd; // Set to cd in order to be able to throw one on start.

	}
	
	void Update()
	{
		InputMovement();
		InputMouseAction();
		SpawnerRotateMouse();

	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, stoneThrowRange);
	}

	//------------------------------------
	
	public float Speed
	{
		get { return speed; }
		set { speed = value; }
	}

	//------------------------------------

	public void InputMovement()
	{
		float multiplyY = 0.0f;
		float multiplyX = 0.0f;
		if (Input.GetKey(KeyCode.W))
		{
			multiplyY = Time.deltaTime * speed;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			multiplyY = -Time.deltaTime * speed;
		}
		if (Input.GetKey(KeyCode.D))
		{
			multiplyX = Time.deltaTime * speed;
		}
		else if (Input.GetKey(KeyCode.A))
		{
			multiplyX = -Time.deltaTime * speed;
		}

		transform.position = new Vector3(transform.position.x + multiplyX, transform.position.y + multiplyY, transform.position.z);
	}

	public void InputMouseAction()
	{
		// Update cd's
		timeElapsedSinceLastThrow += Time.deltaTime;

		//--------

		if (Input.GetKeyDown(KeyCode.Mouse0)) // Left mouse button
		{
			//Debug.Log("mouse L click at: " + Input.mousePosition);

		}

		if (Input.GetKeyDown(KeyCode.Mouse1)) // Right mouse button
		{
			//Debug.Log("mouse R click at" + Input.mousePosition);
			ChangeEnergy(-1);
			switch (skillFocus)
			{
				case 0:
					{
						if (timeElapsedSinceLastThrow >= throwStoneCd)
						{
							SpawnRock();
							timeElapsedSinceLastThrow = 0f;
						}
					}
					break;

				case 1:
					//TODO: Set / Active
				{
					SpawnWalkie();
				}
					break;

				case 2:
				{
					Dash();
				}
					break;
			}

		}

		if (Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			//Debug.Log("Mouse scroll UP");
			ChangeSkillFocus(1);
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			//Debug.Log("Mouse scroll DOWN");
			ChangeSkillFocus(-1);
		}
	}
	
	void SpawnerRotateMouse()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - rotationAxis.transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        rotationAxis.transform.rotation = Quaternion.Euler(0f, 0f, rotation_z-90);
    }

	//------------------------------------

	void SpawnRock()
    {
	    Vector3 worldMousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	    worldMousePoint.z = 0;

		float distance = Vector3.Distance(worldMousePoint, transform.position);
	    if (distance > stoneThrowRange)
	    {
			// If  mouseisout of range, set the spawn position to the limit range on the direction.
		    Vector3 dir = worldMousePoint - transform.position;
		    dir.z = 0;
			dir.Normalize();
		    dir *= stoneThrowRange;
		    worldMousePoint = transform.position + dir;
	    }

		GameObject rock = (GameObject)Instantiate(rockGameobject, skillspawnSkillsTrasnform.transform.position, Quaternion.identity);
        rock.GetComponent<Rock>().GoToPoint(worldMousePoint, stoneThrowRange);
    }

	public void SpawnWalkie()
	{
		GameObject walkieobj = (GameObject)Instantiate(walkie, skillspawnSkillsTrasnform.transform.position, Quaternion.identity);
		Debug.Log(walkieobj.transform.position);

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
        skillFocus=Mathf.Clamp(skillFocus,0,skills.Length-1);
        //Debug.Log("skill selected: "+skillFocus);
        foreach(Image skill in skills)
        {
            skill.color = Color.white;
        }
        skills[skillFocus].color = Color.blue;
    }

	public void SetEnergyCost(int value)
	{
		energyCost = value;
	}

	public void ChangeEnergy(int amount)
    {
        energy += amount*energyCost;
        energy = Mathf.Clamp(energy, 0, (int)energyBar.maxValue);
        energyBar.value = energy;
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





