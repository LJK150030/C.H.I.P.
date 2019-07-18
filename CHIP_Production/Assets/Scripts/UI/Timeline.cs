using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Timeline : MonoBehaviour {
    public List<RectTransform> timelineList;
    public float hideTime = 0.2f;
    public UnityEvent OnStart = new UnityEvent();
    public UnityEvent OnEnd = new UnityEvent();

    void Start () {
        OnStart.Invoke();
        for (int i = 0; i < timelineList.Count; i++)
        {
            Vector2 newSize = timelineList[i].sizeDelta;
            newSize.x = 0;
            timelineList[i].sizeDelta = newSize;
        }
	}

	public void StartTimeline (float unitTime) {
        StartCoroutine(RunTimeline(unitTime));
	}

    IEnumerator RunTimeline(float unitTime)
    {
        for (int i = 0; i < timelineList.Count; i++)
        {
            timelineList[i].GetComponent<Image>().color = Color.white;
        }
        float timer = 0;
        for(int i = 0; i < timelineList.Count; i++)
        {
            timer = 0;
            Vector2 newSize = timelineList[i].sizeDelta;
            while (timer < unitTime)
            {
                newSize.x = Mathf.Lerp(0, 110, timer / unitTime);
                timelineList[i].sizeDelta = newSize;
                timer += Time.deltaTime;
                yield return 0;
            }
        }
        for (int i = 0; i < timelineList.Count; i++)
        {
            Vector2 newSize = timelineList[i].sizeDelta;
            newSize.x = 0;
            timelineList[i].sizeDelta = newSize;
        }

        OnEnd.Invoke();
    }
}
