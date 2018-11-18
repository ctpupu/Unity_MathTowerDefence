using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour {
    private float currentTime = 0f;
    public Vector3[] path;
	
    // Update is called once per frame
    void Update() {
        //print(Input.mousePosition + ", " + Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (Input.GetMouseButtonDown(0)) {
            Vector3 trueMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hit = Physics2D.RaycastAll(trueMousePos, -Vector2.up,0);
            bool hitFound = false;
            GameObject tower = null;

            //check if hit button
            for(int i = 0; i < hit.Length; i++)
            {
                if(hit[i].collider.gameObject.GetComponent<button>())
                {
                    hitFound = true;
                    hit[i].collider.gameObject.GetComponent<button>().buttonClick();
                    break;
                }

                else if(hit[i].collider.gameObject.name == "tower")
                {
                    float dist = Vector3.Distance(hit[i].point, hit[i].collider.gameObject.transform.localPosition);
                    if(dist < 0.1) tower = hit[i].collider.gameObject;
                }
            }

            //check if hit tower
            if(hitFound == false && tower != null)
            {
                tower.GetComponent<tower>().towerClicked();
            }
        }
	}
}
