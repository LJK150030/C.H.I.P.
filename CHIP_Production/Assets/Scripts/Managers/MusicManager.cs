using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public GameObject[] musicPlayers;
    public GameObject startMusic;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        musicPlayers = GameObject.FindGameObjectsWithTag("Music");
        if (musicPlayers.Length > 1)
        {
            Destroy(startMusic);
        }
		
	}
}
