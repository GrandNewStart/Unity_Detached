﻿using UnityEngine;

public partial class GameManager
{
    private void RotateCube()
    {
        cube.transform.Rotate(new Vector3(1, 1, 1));
    }

    public void ShowCube(float seconds)
    {
        cube.SetActive(true);
        if (seconds != INFINITE)
        {
            Invoke(nameof(HideCube), seconds);
        }
    }

    public void HideCube()
    {
        cube.SetActive(false);
    }

}