using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ArmController
{
    private void AnimationControl()
    {
        switch (dir)
        {
            case 1:
                anim.Play("move_right");
                break;
            case -1:
                anim.Play("move_left");
                break;
            case 0:
                if (lastDir == 1)
                {
                    anim.Play("idle_right");
                }
                if (lastDir == -1)
                {
                    anim.Play("idle_left");
                }
                break;
        }
    }
}