using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : SwitchController
{
    public override void OnActivation()
    {
        target.SetActive(false);
    }

    public override void OnDeactivation()
    {
        target.SetActive(true);
    }
}