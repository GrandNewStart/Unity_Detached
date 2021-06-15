using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : BaseSwitch
{
    [SerializeField] private GameObject door;

    public override void PlugIn(Arm arm)
    {
        base.PlugIn(arm);
        door.SetActive(false);
        sprite.sprite = sprite_plugged_green;
    }

    public override void PlugOut()
    {
        base.PlugOut();
        door.SetActive(true);
        sprite.sprite = sprite_unplugged;
    }

}
