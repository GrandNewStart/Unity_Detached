using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : SwitchController
{
    public override void OnActivation(ArmController arm)
    {
        base.OnActivation(arm);
        target.SetActive(false);
    }

    public override void OnDeactivation()
    {
        base.OnDeactivation();
        target.SetActive(true);
    }
}