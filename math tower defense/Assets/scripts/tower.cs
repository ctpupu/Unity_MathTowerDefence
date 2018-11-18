using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tower : MonoBehaviour {
    public enum BUTTON_TYPE { Plus, Minus, Multiply, Division};

    private CircleCollider2D range;
    private float currentTime = 0f;
    private float animationStartTime = 0f;
    private float animationDuration = 0.2f;
    private bool isActivate = false;
    private bool isAnimating = false;
    private Vector3 plusButtonPos;
    private Vector3 minusButtonPos;
    private Vector3 multiplyButtonPos;
    private Vector3 divisionButtonPos;

    public int value = 1;
    public float rate = 1.0f;
    public enemy.TYPE type;
    public GameObject plusButton;
    public GameObject minusButton;
    public GameObject multiplyButton;
    public GameObject divisionButton;
    public Sprite[] towerSprites;
    
	// Use this for initialization
	void Start () {
        range = gameObject.GetComponentInChildren<CircleCollider2D>();

        plusButtonPos = plusButton.transform.localPosition;
        minusButtonPos = minusButton.transform.localPosition;
        multiplyButtonPos = multiplyButton.transform.localPosition;
        divisionButtonPos = divisionButton.transform.localPosition;

        plusButton.transform.localPosition = new Vector3();
        minusButton.transform.localPosition = new Vector3();
        multiplyButton.transform.localPosition = new Vector3();
        divisionButton.transform.localPosition = new Vector3();
        toggleButtons(false);
    }
	
	// Update is called once per frame
	void Update () {
		if(isAnimating)
        {
            float timeElapsed = Time.time - animationStartTime;
            float progress = Mathf.Min(timeElapsed / animationDuration, 1f);

            animateButton(progress, plusButton, plusButtonPos);
            animateButton(progress, minusButton, minusButtonPos);
            animateButton(progress, multiplyButton, multiplyButtonPos);
            animateButton(progress, divisionButton, divisionButtonPos);

            if (progress >= 1)
            {
                isAnimating = false;
                if (isActivate == false)
                {
                    toggleButtons(false);
                }
            }
        }
	}

    //shoot enemy within the range.
    void OnTriggerStay2D(Collider2D hit)
    {
        if (type != enemy.TYPE.None)
        {
            if (Time.time - currentTime > rate)
            {
                currentTime = Time.time;
                GameObject projectile = Instantiate(Resources.Load("prefabs/projectile"), transform) as GameObject;
                projectile.GetComponent<projectile>().shoot(gameObject, hit.gameObject, value, type);
            }
        }
    }

    public void applyButton(BUTTON_TYPE t)
    {
        switch(t)
        {
            case BUTTON_TYPE.Plus: type = enemy.TYPE.Addition; break;
            case BUTTON_TYPE.Minus: type = enemy.TYPE.Subtraction; break;
            case BUTTON_TYPE.Multiply: type = enemy.TYPE.Multiplication; break;
            case BUTTON_TYPE.Division: type = enemy.TYPE.Division; break;
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = towerSprites[(int)type];
        towerClicked();
    }

    public void towerClicked()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("tower");
        for(int i = 0; i < towers.Length; i++)
        {
            if(towers[i] != gameObject)
            {
                towers[i].GetComponent<tower>().turnOff();
            }
        }
        if (isActivate)
        {
            isActivate = false;
        }
        else
        {
            toggleButtons(true);
            isActivate = true;
        }
        animationStartTime = Time.time;
        isAnimating = true;
    }

    public void turnOff()
    {
        if(isActivate)
        {
            towerClicked();
        }
    }

    private void toggleButtons(bool on)
    {
        plusButton.SetActive(on);
        minusButton.SetActive(on);
        multiplyButton.SetActive(on);
        divisionButton.SetActive(on);
    }

    private void animateButton(float progress, GameObject button, Vector3 pos)
    {
        Vector3 orig = new Vector3(0, 0, 0);
        Vector3 diff = pos - orig;
        diff.x *= progress;
        diff.y *= progress;

        if (isActivate)
        {
            button.transform.localPosition = diff;
        }
        else
        {
            button.transform.localPosition = pos - diff;
        }
    }
}
