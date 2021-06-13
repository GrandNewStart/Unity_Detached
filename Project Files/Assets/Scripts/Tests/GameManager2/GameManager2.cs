using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager2 : MonoBehaviour
{
    public ControlIndex controlIndex = ControlIndex.player;

    public CinemachineVirtualCamera vCam;
    public Player player;
    public Arm firstArm;
    public Arm secondArm;
    private Transform cameraTarget;
    public bool isPaused = false;

    private void Start()
    {
        OnGameStart();
        player.EnableControl(true);
    }

    private void Update()
    {
        OnGameUpdate();
        Control();
    }

    private void FixedUpdate()
    {
        OnGameFixedUpdated();
    }

    private void LateUpdate()
    {
        UpdateCamera();
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
    }

    private void ForcePauseGame()
    {
        
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
    }

    protected virtual void OnGameStart() { }
    protected virtual void OnGameUpdate() { }
    protected virtual void OnGameFixedUpdated() { }
    protected virtual void OnGamePause() { }
    protected virtual void OnGameResume() { }


}
