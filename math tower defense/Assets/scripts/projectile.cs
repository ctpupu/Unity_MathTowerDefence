using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour {
    public GameObject tower;
    public GameObject target;
    public int value;
    public enemy.TYPE type;
    public float speed = 0.01f;
    
    public void shoot(GameObject from, GameObject to, int v, enemy.TYPE t)
    {
        tower = from;
        target = to;
        value = v;
        type = t;

        gameObject.transform.position = from.transform.position;
    }

	// Update is called once per frame
	void Update () {
        if (target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            Vector3 dest = target.transform.position;
            Vector3 from = gameObject.transform.position;
            Vector3 diff = dest - from;
            float speedX, speedY;
            if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
            {
                speedX = speed;
                speedY = speed * (Mathf.Abs(diff.y) / Mathf.Abs(diff.x));
            }
            else
            {
                speedX = speed * (Mathf.Abs(diff.x) / Mathf.Abs(diff.y));
                speedY = speed;
            }

            if (Mathf.Abs(dest.x - from.x) < speedX)
            {
                gameObject.transform.Translate(dest.x - from.x, 0, 0);
            }
            else
            {
                gameObject.transform.Translate(((Mathf.Abs(dest.x - from.x) / (dest.x - from.x)) * speedX), 0, 0);
            }

            if (Mathf.Abs(dest.y - from.y) < speedY)
            {
                gameObject.transform.Translate(0, dest.y - from.y, 0);
            }
            else
            {
                gameObject.transform.Translate(0, ((Mathf.Abs(dest.y - from.y) / (dest.y - from.y)) * speedY), 0);
            }

            float dist = Vector3.Distance(gameObject.transform.position, dest);
            if (dist <= speed)
            {
                if (target)
                {
                    target.GetComponent<enemy>().damage(value, type);
                }
                Destroy(gameObject);
            }
        }
    }
}
