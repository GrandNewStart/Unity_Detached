using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager
{
    private void InitTransition()
    {
        transition = new Transition(this, background);
    }

    private void RotateCube()
    {
        cube.transform.Rotate(new Vector3(1, 1, 1));
    }

    public void ShowCube(float seconds)
    {
        cube.SetActive(true);
        if (seconds != INFINITE)
        {
            Invoke("HideCube", seconds);
        }
    }

    public void HideCube()
    {
        cube.SetActive(false);
    }

}