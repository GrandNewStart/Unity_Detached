using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : BaseSwitch
{
    [SerializeField] private GameObject door;

    protected override void OnPlugIn()
    {
        base.OnPlugIn();
        door.SetActive(false);
        sprite.sprite = sprite_plugged_green;
    }

    protected override void OnPlugOut()
    {
        base.OnPlugOut();
        door.SetActive(true);
        sprite.sprite = sprite_unplugged;
    }

}
