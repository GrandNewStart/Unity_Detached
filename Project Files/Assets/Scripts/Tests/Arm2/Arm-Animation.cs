using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Arm
{
    private void SetDirctionalState(float movement)
    {
        if (movement < 0)
        {
            state = State.walk;
            dir = -1;
            lastDir = -1;
        }

        if (movement > 0)
        {
            state = State.walk;
            dir = 1;
            lastDir = 1;
        }

        if (movement == 0)
        {
            state = State.idle;
            dir = 0;
        }
    }


    private void ManageAnimation()
    {
        if (!isGrounded) state = State.air;

        switch (state)
        {
            case State.idle:
                switch (lastDir)
                {
                    case 1:
                        switch (armIndex)
                        {
                            case ArmIndex.first:
                                animator.Play("Left_Idle_Right(128)");
                                break;
                            case ArmIndex.second:
                                animator.Play("Right_Idle_Right(128)");
                                break;
                        }
                        break;
                    case -1:
                        switch (armIndex)
                        {
                            case ArmIndex.first:
                                animator.Play("Left_Idle_Left(128)");
                                break;
                            case ArmIndex.second:
                                animator.Play("Right_Idle_Left(128)");
                                break;
                        }
                        break;
                }
                break;
            case State.walk:
                switch (dir)
                {
                    case 1:
                        switch (armIndex)
                        {
                            case ArmIndex.first:
                                animator.Play("Left_Move_Right(128)");
                                break;
                            case ArmIndex.second:
                                animator.Play("Right_Move_Right(128)");
                                break;
                        }
                        break;
                    case -1:
                        switch (armIndex)
                        {
                            case ArmIndex.first:
                                animator.Play("Left_Move_Left(128)");
                                break;
                            case ArmIndex.second:
                                animator.Play("Right_Move_Left(128)");
                                break;
                        }
                        break;
                }
                break;
            case State.air:
                switch (lastDir)
                {
                    case 1:
                        switch (armIndex)
                        {
                            case ArmIndex.first:
                                animator.Play("Left_Idle_Right(128)");
                                break;
                            case ArmIndex.second:
                                animator.Play("Right_Idle_Right(128)");
                                break;
                        }
                        break;
                    case -1:
                        switch (armIndex)
                        {
                            case ArmIndex.first:
                                animator.Play("Left_Idle_Left(128)");
                                break;
                            case ArmIndex.second:
                                animator.Play("Right_Idle_Left(128)");
                                break;
                        }
                        break;
                }
                break;
            }
        }
}