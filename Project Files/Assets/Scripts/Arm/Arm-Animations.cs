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
                switch (resolution)
                {
                    case Resolution._1024:
                        if (isLeft) anim.Play("Left_Move_Right(1024)");
                        else anim.Play("Right_Move_Right(1024)");
                        break;
                    case Resolution._512:
                        if (isLeft) anim.Play("Left_Move_Right(512)");
                        else anim.Play("Right_Move_Right(512)");
                        break;
                    case Resolution._256:
                        if (isLeft) anim.Play("Left_Move_Right(256)");
                        else anim.Play("Right_Move_Right(256)");
                        break;
                    case Resolution._128:
                        if (isLeft) anim.Play("Left_Move_Right(128)");
                        else anim.Play("Right_Move_Right(128)");
                        break;
                }
                break;
            case -1:
                switch (resolution)
                {
                    case Resolution._1024:
                        if (isLeft) anim.Play("Left_Move_Left(1024)");
                        else anim.Play("Right_Move_Left(1024)");
                        break;
                    case Resolution._512:
                        if (isLeft) anim.Play("Left_Move_Left(512)");
                        else anim.Play("Right_Move_Left(512)");
                        break;
                    case Resolution._256:
                        if (isLeft) anim.Play("Left_Move_Left(256)");
                        else anim.Play("Right_Move_Left(256)");
                        break;
                    case Resolution._128:
                        if (isLeft) anim.Play("Left_Move_Left(128)");
                        else anim.Play("Right_Move_Left(128)");
                        break;
                }
                break;
            case 0:
                if (lastDir == 1)
                {
                    switch (resolution)
                    {
                        case Resolution._1024:
                            if (isLeft) anim.Play("Left_Idle_Right(1024)");
                            else anim.Play("Right_Idle_Right(1024)");
                            break;
                        case Resolution._512:
                            if (isLeft) anim.Play("Left_Idle_Right(512)");
                            else anim.Play("Right_Idle_Right(512)");
                            break;
                        case Resolution._256:
                            if (isLeft) anim.Play("Left_Idle_Right(256)");
                            else anim.Play("Right_Idle_Right(256)");
                            break;
                        case Resolution._128:
                            if (isLeft) anim.Play("Left_Idle_Right(128)");
                            else anim.Play("Right_Idle_Right(128)");
                            break;
                    }
                }
                if (lastDir == -1)
                {
                    switch (resolution)
                    {
                        case Resolution._1024:
                            if (isLeft) anim.Play("Left_Idle_Left(1024)");
                            else anim.Play("Right_Idle_Left(1024)");
                            break;
                        case Resolution._512:
                            if (isLeft) anim.Play("Left_Idle_Left(512)");
                            else anim.Play("Right_Idle_Left(512)");
                            break;
                        case Resolution._256:
                            if (isLeft) anim.Play("Left_Idle_Left(256)");
                            else anim.Play("Right_Idle_Left(256)");
                            break;
                        case Resolution._128:
                            if (isLeft) anim.Play("Left_Idle_Left(128)");
                            else anim.Play("Right_Idle_Left(128)");
                            break;
                    }
                }
                break;
        }
    }
}