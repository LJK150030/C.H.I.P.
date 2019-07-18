using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endLevel : MonoBehaviour {
    public GameObject gameCompleteMenu;

	// Use this for initialization
	void Start () {
        gameCompleteMenu.SetActive(false);
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        foreach(Transform child in col.transform)
        {
            if (child.tag == "Player")
            {
                gameCompleteMenu.SetActive(true);
                break;
            }
        }
    }

}
