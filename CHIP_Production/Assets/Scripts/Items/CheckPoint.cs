using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Components;
using UnityEngine;
using Assets.Scripts.Controllers;
using Assets.Scripts.Utilities;

[RequireComponent(typeof(Collider2D))]
public class CheckPoint : MonoBehaviour
{
    public int checkpointID;
    [HideInInspector] public bool _activated = false;

    public AudioClip SFX_passed;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !_activated)
        {
            this.gameObject.GetComponentInChildren<Animator>().SetBool("Active", true);
            PlayerPrefs.SetFloat("PlayerRespawnPosX", transform.position.x);
            PlayerPrefs.SetFloat("PlayerRespawnPosY", transform.position.y);
            PlayerPrefs.SetFloat("PlayerRespawnPosZ", transform.position.z);
            PlayerPrefs.SetInt("CheckPointID", checkpointID);
            AudioUtil.PlayOneOff(SFX_passed);
            _activated = true;
        }
    }

    public static void InitialPlayerPosition()
    {
        if (PlayerPrefs.HasKey("PlayerRespawnPosX") && PlayerPrefs.HasKey("PlayerRespawnPosY") && PlayerPrefs.HasKey("PlayerRespawnPosZ"))
        {
            Transform player = ((RobotController) FindObjectOfType(typeof(RobotController))).transform;
            player.position = new Vector3(PlayerPrefs.GetFloat("PlayerRespawnPosX"), PlayerPrefs.GetFloat("PlayerRespawnPosY"), PlayerPrefs.GetFloat("PlayerRespawnPosZ"));

            Transform camera = Camera.main.transform;
            camera.position = player.position;
        }
        if (PlayerPrefs.HasKey("CheckPointID"))
        {
            float latestID = PlayerPrefs.GetInt("CheckPointID");
            CheckPoint[] allCheckPoint = FindObjectsOfType<CheckPoint>();
            for (int i = 0; i < allCheckPoint.Length; i++)
            {
                if (allCheckPoint[i].checkpointID <= latestID)
                {
                    allCheckPoint[i].GetComponentInChildren<Animator>().SetBool("Active", true);
                    allCheckPoint[i]._activated = true;
                }
            }
        }
    }

    public static void CleanCheckPoint()
    {
        PlayerPrefs.DeleteKey("PlayerRespawnPosX");
        PlayerPrefs.DeleteKey("PlayerRespawnPosY");
        PlayerPrefs.DeleteKey("PlayerRespawnPosZ");
        PlayerPrefs.DeleteKey("CheckPointID");
    }
}
