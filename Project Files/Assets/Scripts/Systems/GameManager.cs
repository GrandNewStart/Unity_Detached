using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool      shouldLoadSaveFile = false;
    public static int       stage;
    public static int       enabledArms;
    public static Vector3   position;
    public GameObject       player;

    private void Start()
    {
        OnStageStarted();
    }

    public void OnStageStarted()
    {
        if (shouldLoadSaveFile)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            player.transform.position = position;
            playerController.EnableArms(enabledArms);
        }
    }
}
