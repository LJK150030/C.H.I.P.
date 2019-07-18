using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionController : MonoBehaviour {
    public List<GameObject> LevelList;
    public int latestLevel
    {
        get
        {
            int index;
            if (PlayerPrefs.HasKey("LatestLevel"))
            {
                index = PlayerPrefs.GetInt("LatestLevel");
                index = Mathf.Min(index, LevelList.Count - 1);
            }
            else
            {
                index = 0;
            }
            PlayerPrefs.SetInt("LatestLevel", index);
            return index;
        }
    }

	void Start () {
        if (LevelList.Count > 1)
        {
            for(int i = 0; i <= latestLevel; i++)
            {
                LevelList[i].SetActive(true);
            }
            for (int i = latestLevel + 1; i < LevelList.Count; i++)
            {
                LevelList[i].SetActive(false);
            }
        }
	}

	void Update () {
		
	}

    public static void NextLevel(int i)
    {
        int index;
        if (PlayerPrefs.HasKey("LatestLevel"))
        {
            index = PlayerPrefs.GetInt("LatestLevel");
        }
        else
        {
            index = 1;
        }
        if (index < i)
        {
            PlayerPrefs.SetInt("LatestLevel", i);
        }
    }

    public static void Reset()
    {
        PlayerPrefs.SetInt("LatestLevel", 0);
    }
}
