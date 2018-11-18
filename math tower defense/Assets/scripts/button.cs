using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour {
    private tower targetTower;
	// Use this for initialization
	void Start () {
        targetTower = gameObject.GetComponentInParent<tower>();
	}
	
	public void buttonClick()
    {
        switch(gameObject.name)
        {
            case "plusButton":
                print("plus button clicked");
                targetTower.applyButton(tower.BUTTON_TYPE.Plus);
                break;
            case "minusButton":
                print("minus button clicked");
                targetTower.applyButton(tower.BUTTON_TYPE.Minus);
                break;
            case "multiplyButton":
                print("multiply button clicked");
                targetTower.applyButton(tower.BUTTON_TYPE.Multiply);
                break;
            case "divisionButton":
                print("division button clicked");
                targetTower.applyButton(tower.BUTTON_TYPE.Division);
                break;
        }
    }
}
