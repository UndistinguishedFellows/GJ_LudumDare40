using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour {
    public float speed;
    public Image[] skills;
    public int skillFocus = 0;
    public Slider energyBar;
    public int energy;
    public int energyCost;

    public GameObject rockGameobject;
    public GameObject skillspawnSkillsTrasnform;
    public GameObject rotationAxis;
    public GameObject walkie;
    bool bInDash = false;
    //--
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
        ChangeSkillFocus(-1);
        
    }
	
	// Update is called once per frame
	void Update () {
        InputMovement();
        InputMouseAction();
        SpawnerRotateMouse();

	}
    void SpawnerRotateMouse()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - rotationAxis.transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        rotationAxis.transform.rotation = Quaternion.Euler(0f, 0f, rotation_z-90);
    }

    void SpawnRock()
    {
        GameObject rock = (GameObject)Instantiate(rockGameobject, skillspawnSkillsTrasnform.transform.position, Quaternion.identity);
        //Debug.Log(rock.transform.position);
        rock.GetComponent<Rock>().GoToPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), 20f); // TODO: Set the max throw distance as attribute and check the destination before call
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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Debug.Log("mouse R click at: " + Input.mousePosition);
            
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //Debug.Log("mouse L click at" + Input.mousePosition);
            ChangeEnergy(-1);
            if (skillFocus == 0)
            { 
                SpawnRock();
            }
            if (skillFocus == 1){
                SpawnWalkie();
            }
            if (skillFocus == 2)
            {
                Dash();
            }
                
        }
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            //Debug.Log("Mouse scroll UP");
            ChangeSkillFocus(1);
        }else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            //Debug.Log("Mouse scroll DOWN");
            ChangeSkillFocus(-1);
        }
    }
    public void SetEnergyCost(int value)
    {
        energyCost = value;
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
    public void ChangeEnergy(int amount)
    {
        energy += amount*energyCost;
        energy = Mathf.Clamp(energy, 0, (int)energyBar.maxValue);
        energyBar.value = energy;
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





