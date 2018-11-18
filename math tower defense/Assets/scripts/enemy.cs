using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour {
    public enum TYPE {Addition, Subtraction, Multiplication, Division, None};
    private Vector3[] path;
    private int pathIndex = 0;
    private Renderer[] renderers;
    private TextMesh text;
    private float curTime = 0f;
    private int curSpriteIndex = 0;
    private SpriteRenderer sprite;
    private int minValue = 1;
    private int maxValue = 100;
    private float minScale = 0.5f;
    private float maxScale = 3f;
    private bool isDead = false;
    
    public float speed = 0.01f;
    public float spriteSpeed = 0.3f;
    public int value = 1;
    public int idleAnimationStart = 0;
    public int idleAnimationEnd = 0;
    public int deadAnimationStart = 0;
    public int deadAnimationEnd = 0;

    
    public TYPE type = TYPE.Addition;
    public Sprite[] sprites;

	// Use this for initialization
	void Start () {
        path = GameObject.FindGameObjectWithTag("map").GetComponent<main>().path;
        transform.position = path[pathIndex];
        sprite = gameObject.GetComponent<SpriteRenderer>();
        renderers = gameObject.GetComponentsInChildren<Renderer>();
        for(int i = 1; i < renderers.Length; i++)
        {
            renderers[i].sortingLayerName = "default";
            renderers[i].sortingOrder = i+1;
        }
        text = gameObject.GetComponentInChildren<TextMesh>();
        text.text = value.ToString();
	}
	
	// Update is called once per frame
	void Update () {
        resizeObject();
        controlSprite();

        //move alone the path
        if (isDead == false)
        {
            if (Mathf.Abs(path[pathIndex].x - transform.position.x) < speed)
                transform.Translate(new Vector3(path[pathIndex].x - transform.position.x, 0, 0));
            else
                transform.Translate(new Vector3(((path[pathIndex].x - transform.position.x) / Mathf.Abs(path[pathIndex].x - transform.position.x)) * speed, 0, 0));

            if (Mathf.Abs(path[pathIndex].y - transform.position.y) < speed)
                transform.Translate(new Vector3(0, path[pathIndex].y - transform.position.y, 0));
            else
                transform.Translate(new Vector3(0, ((path[pathIndex].y - transform.position.y) / Mathf.Abs(path[pathIndex].y - transform.position.y)) * speed, 0));

            //check if reach next path.
            if (transform.position.x == path[pathIndex].x && transform.position.y == path[pathIndex].y)
            {

                if (pathIndex < path.Length - 1)
                {
                    pathIndex++;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
	}

    private void resizeObject()
    {
        float percentage = Mathf.Min((float)value / (float)maxValue, 1f);
        float targetScale = minScale + (maxScale - minScale) * percentage;
        
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(targetScale, targetScale, 1f);
    }

    private void controlSprite()
    {
        if(Time.time - curTime > spriteSpeed)
        {
            curTime = Time.time;
            if (isDead == false)
            {
                if (curSpriteIndex < idleAnimationEnd) curSpriteIndex++;
                else curSpriteIndex = idleAnimationStart;
            }
            else
            {
                if (curSpriteIndex < deadAnimationEnd) curSpriteIndex++;
                else Destroy(gameObject);
            }
            sprite.sprite = sprites[curSpriteIndex];
        }
    }

    public void damage(int v, TYPE t)
    {
        if (isDead == false)
        {
            switch (t)
            {
                case TYPE.Addition:
                    if (type == TYPE.Addition) value += v;
                    else value -= v;
                    break;
                case TYPE.Subtraction:
                    if (type == TYPE.Subtraction) value += v;
                    else value -= v;
                    break;
                case TYPE.Division: //you can't kill enemy with division tower
                    if (value > 1)
                    {
                        value = Mathf.RoundToInt(value / v);
                        value = Mathf.Max(value, 1);
                        GameObject copiedOne = Instantiate(gameObject);
                        copiedOne.transform.SetParent(gameObject.transform.parent.transform);
                        copiedOne.transform.position = gameObject.transform.position;
                    }
                    break;
            }

            text.text = value.ToString();

            if (value <= 0)
            {
                Destroy(gameObject.GetComponent<Rigidbody2D>());
                isDead = true;
                curTime = Time.time;
                curSpriteIndex = deadAnimationStart;
                sprite.sprite = sprites[curSpriteIndex];
            }
        }
    }

    private GameObject getClone()
    {
        GameObject go = new GameObject();
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprites[0];
        sr.sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder;

        enemy em = go.AddComponent<enemy>();
        em.speed = speed;
        em.spriteSpeed = spriteSpeed;
        em.value = value;
        em.idleAnimationStart = idleAnimationStart;
        em.idleAnimationEnd = idleAnimationEnd;
        em.deadAnimationStart = deadAnimationStart;
        em.deadAnimationEnd = deadAnimationEnd;
        em.sprites = sprites;
        em.type = type;

        return go;
    }
}
