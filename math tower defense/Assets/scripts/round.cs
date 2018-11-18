using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class round : MonoBehaviour {
    public float delay = 5f;
    public float spawnRate = 1f;
    public float enemySpeed = 0.01f;
    public float spriteSpeed = 0.3f;
    public int numEnemies = 1;
    public int health = 1;
    public string prefabName = "plus_enemy";
    public enemy.TYPE type = enemy.TYPE.Addition;
    public Sprite upcomingDisplay;

    private float curTime;
    private int numSpawned = 0;
    private bool isSpawning = false;
    private upcomingPanel upcomingPanel = null;
    private upcomingPanelControl panelControl = null;
    
	// Use this for initialization
	void Start () {
        curTime = Time.time;
        numSpawned = 0;
        isSpawning = false;
        panelControl = GameObject.FindGameObjectWithTag("upcoming").GetComponent<upcomingPanelControl>();
    }
	
	// Update is called once per frame
	void Update () {
        if(isSpawning == false)
        {
            if(upcomingPanel == null && delay - (Time.time - curTime) < 10f)
            {
                upcomingPanel = panelControl.addPanel();
                upcomingPanel.image.sprite = upcomingDisplay;
                if (type == enemy.TYPE.Addition)
                    upcomingPanel.description.text = "Type: Addition";
                else
                    upcomingPanel.description.text = "Type: Subtraction";

                upcomingPanel.timeRemain.text = (delay - (Time.time - curTime)).ToString("0.##");
                upcomingPanel.bar.transform.localScale = new Vector3((delay - (Time.time - curTime)) / 10f, 1f, 1f);
            }
            else if(upcomingPanel != null)
            {
                upcomingPanel.timeRemain.text = (delay - (Time.time - curTime)).ToString("0.##");
                upcomingPanel.bar.transform.localScale = new Vector3((delay - (Time.time - curTime)) / 10f, 1f, 1f);
            }

            if(Time.time - curTime > delay)
            {
                isSpawning = true;
                curTime = Time.time;
                panelControl.removePanel(upcomingPanel);
                spawnEnemy();
            }
        }
        else
        {
            if(Time.time - curTime > spawnRate)
            {
                curTime = Time.time;
                spawnEnemy();
            }
        }
	}

    private void spawnEnemy()
    {
        GameObject enemy = Instantiate(Resources.Load("prefabs/" + prefabName), transform) as GameObject;
        enemy.GetComponent<enemy>().value = health;
        enemy.GetComponent<enemy>().speed = enemySpeed;
        enemy.GetComponent<enemy>().spriteSpeed = spriteSpeed;
        enemy.GetComponent<enemy>().type = type;

        numSpawned++;
        if (numSpawned >= numEnemies)
        {
            Destroy(this);
        }
    }
}
