using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upcomingPanelControl : MonoBehaviour {
    private List<GameObject> upcomings = new List<GameObject>();
	
    public upcomingPanel addPanel()
    {
        GameObject upcoming = Instantiate(Resources.Load("prefabs/upcoming"), transform) as GameObject;
        upcomings.Add(upcoming);
        organizePanels();
        return upcoming.GetComponent<upcomingPanel>();
    }

    public void removePanel(upcomingPanel up)
    {
        upcomings.Remove(up.gameObject);
        Destroy(up.gameObject);
        organizePanels();
    }

    private void organizePanels()
    {
        float top = 0f;
        float bottom = 550f;
        float size = 58f;
        for(int i = 0; i < upcomings.Count; i++)
        {
            RectTransform rect = upcomings[i].GetComponent<RectTransform>();
            rect.offsetMin = new Vector2(rect.offsetMin.x, bottom);
            rect.offsetMax = new Vector2(rect.offsetMax.x, top);
            bottom -= size;
            top -= size;
        }
    }
}
