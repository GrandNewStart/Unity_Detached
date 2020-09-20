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
    public GameObject       background;
    public GameObject       cube;
    public ArmController    leftArm;
    public ArmController    rightArm;
    public new Camera       camera;
    

    protected void Start()
    {
        cube.SetActive(false);
        OnStageStarted();
    }

    protected void Update()
    {
        RotateCube();
    }

    private void OnStageStarted()
    {
        if (shouldLoadSaveFile)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            player.transform.position = position;
            playerController.EnableArms(enabledArms);
        }
    }

    protected IEnumerator TransitionIn()
    {
        background.SetActive(true);
        background.transform.localScale = new Vector3(.1f, .1f, .1f);
        float currentScale = background.transform.localScale.x;
        float targetScale = 5;

        while (currentScale < targetScale)
        {
            currentScale *= 1.1f;
            background.transform.localScale = new Vector3(currentScale, currentScale, 1);
            yield return null;
        }

        background.SetActive(false);
    }

    protected IEnumerator TransitionOut()
    {
        background.SetActive(true);
        background.transform.localScale = new Vector3(20, 20, 20);
        float currentScale = background.transform.localScale.x;
        float targetScale = 0;

        while (currentScale >= targetScale)
        {
            currentScale *= 0.9f;
            background.transform.localScale = new Vector3(currentScale, currentScale, 1);
            yield return null;
        }

        background.SetActive(false);
    }

    private void RotateCube()
    {
        cube.transform.Rotate(new Vector3(1, 1, 1));
    }

    public void ShowCube()
    {
        cube.SetActive(true);
        Invoke("HideCube", 2f);
    }

    private void HideCube()
    {
        cube.SetActive(false);
    }

    public void RetrieveHands()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        switch (playerController.GetArms())
        {
            case 1:
                switch(playerController.GetEnabledArms())
                {
                    case 1:
                        break;
                    case 2:
                        if (!playerController.GetLeftRetrieving())
                        {
                            playerController.SetLeftRetrieving(true);
                            leftArm.StartRetrieve();
                        }
                        break;
                }
                break;
            case 0:
                switch (playerController.GetEnabledArms())
                {
                    case 1:
                        if (!playerController.GetLeftRetrieving())
                        {
                            playerController.SetLeftRetrieving(true);
                            leftArm.StartRetrieve();
                        }
                        break;
                    case 2:
                        if (!playerController.GetLeftRetrieving())
                        {
                            playerController.SetLeftRetrieving(true);
                            leftArm.StartRetrieve();
                        }
                        if (!playerController.GetRightRetrieving())
                        {
                            playerController.SetRightRetrieving(true);
                            rightArm.StartRetrieve();
                        }
                        break;
                }
                break;
        }
    }

}
