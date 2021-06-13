using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    private void SetDirctionalState(float movement)
    {
        if (movement < 0)
        {
            dir = -1;
            lastDir = -1;
            if (isStateFixed) return;
            state = State.walk;
        }

        if (movement > 0)
        {   
            dir = 1;
            lastDir = 1;
            if (isStateFixed) return;
            state = State.walk;
        }

        if (movement == 0)
        {
            dir = 0;
            if (isStateFixed) return;
            state = State.idle;
        }
    }

    private void UpdateAnimation()
    {
        if (!isGrounded)
        {
            state = State.air;
        }
        else
        {
            if (state == State.air)
            {
                state = State.idle;
            }
        }
        
        switch (state)
        {
            case State.idle:
                switch (lastDir)
                {
                    case 1:
                        switch (armCount)
                        {
                            case 0:
                                animator.Play("Idle_Right_3(256)");
                                break;
                            case 1:
                                animator.Play("Idle_Right_2(256)");
                                break;
                            case 2:
                                animator.Play("Idle_Right_1(256)");
                                break;
                        }
                        break;
                    case -1:
                        switch (armCount)
                        {
                            case 0:
                                animator.Play("Idle_Left_3(256)");
                                break;
                            case 1:
                                animator.Play("Idle_Left_2(256)");
                                break;
                            case 2:
                                animator.Play("Idle_Left_1(256)");
                                break;
                        }
                        break;
                }
                break;
            case State.walk:
                switch (dir)
                {
                    case 1:
                        switch (armCount)
                        {
                            case 0:
                                animator.Play("Walk_Right_3(256)");
                                break;
                            case 1:
                                animator.Play("Walk_Right_2(256)");
                                break;
                            case 2:
                                animator.Play("Walk_Right_1(256)");
                                break;
                        }
                        break;
                    case -1:
                        switch (armCount)
                        {
                            case 0:
                                animator.Play("Walk_Left_3(256)");
                                break;
                            case 1:
                                animator.Play("Walk_Left_2(256)");
                                break;
                            case 2:
                                animator.Play("Walk_Left_1(256)");
                                break;
                        }
                        break;
                }
                break;
            case State.air:
                switch (lastDir)
                {
                    case 1:
                        switch (armCount)
                        {
                            case 0:
                                animator.Play("Jump_Right_Air_3(256)");
                                break;
                            case 1:
                                animator.Play("Jump_Right_Air_2(256)");
                                break;
                            case 2:
                                animator.Play("Jump_Right_Air_1(256)");
                                break;
                        }
                        break;
                    case -1:
                        switch (armCount)
                        {
                            case 0:
                                animator.Play("Jump_Left_Air_3(256)");
                                break;
                            case 1:
                                animator.Play("Jump_Left_Air_2(256)");
                                break;
                            case 2:
                                animator.Play("Jump_Left_Air_1(256)");
                                break;
                        }
                        break;
                }
                break;
            case State.charge:
                switch (lastDir)
                {
                    case 1:
                        switch (armCount)
                        {
                            case 0:
                                animator.Play("Charge_Right_3(256)");
                                break;
                            case 1:
                                animator.Play("Charge_Right_2(256)");
                                break;
                            case 2:
                                animator.Play("Charge_Right_1(256)");
                                break;
                        }
                        break;
                    case -1:
                        switch (armCount)
                        {
                            case 0:
                                animator.Play("Charge_Left_3(256)");
                                break;
                            case 1:
                                animator.Play("Charge_Left_2(256)");
                                break;
                            case 2:
                                animator.Play("Charge_Left_1(256)");
                                break;
                        }
                        break;
                }
                break;
            case State.fire:
                switch (lastDir)
                {
                    case 1:
                        switch (armCount)
                        {
                            case 0:
                                animator.Play("Fire_Right_3(256)");
                                break;
                            case 1:
                                animator.Play("Fire_Right_2(256)");
                                break;
                            case 2:
                                animator.Play("Fire_Right_1(256)");
                                break;
                        }
                        break;
                    case -1:
                        switch (armCount)
                        {
                            case 0:
                                animator.Play("Fire_Left_3(256)");
                                break;
                            case 1:
                                animator.Play("Fire_Left_2(256)");
                                break;
                            case 2:
                                animator.Play("Fire_Left_1(256)");
                                break;
                        }
                        break;
                }
                break;
        }
    }
}