using UnityEngine;

public partial class PlayerController
{
    private void InitAnimationAttributes()
    {
        animator = normal.GetComponent<Animator>();
        dir = 0;
        lastDir = 1;
        state = State.idle;
        isStateFixed = false;
    }

    private void AnimationControl()
    {
        if (isDestroyed) return;
        if (!isGrounded) state = State.jump;
        switch (state)
        {
            case State.idle:
                if (lastDir == 1)
                {
                    if (arms == 2)
                    {
                        switch(resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Idle_Right_1(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Idle_Right_1(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Idle_Right_1(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Idle_Right_1(128)");
                                break;
                        }
                    }
                    if (arms == 1)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Idle_Right_2(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Idle_Right_2(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Idle_Right_2(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Idle_Right_2(128)");
                                break;
                        }
                    }
                    if (arms == 0)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Idle_Right_3(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Idle_Right_3(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Idle_Right_3(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Idle_Right_3(128)");
                                break;
                        }
                    }
                }
                if (lastDir == -1)
                {
                    if (arms == 2)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Idle_Left_1(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Idle_Left_1(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Idle_Left_1(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Idle_Left_1(128)");
                                break;
                        }
                    }
                    if (arms == 1)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Idle_Left_2(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Idle_Left_2(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Idle_Left_2(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Idle_Left_2(128)");
                                break;
                        }
                    }
                    if (arms == 0)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Idle_Left_3(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Idle_Left_3(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Idle_Left_3(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Idle_Left_3(128)");
                                break;
                        }
                    }
                }
                break;
            case State.walk:
                if (dir == 1)
                {
                    if (arms == 2)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Walk_Right_1(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Walk_Right_1(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Walk_Right_1(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Walk_Right_1(128)");
                                break;
                        }
                    }
                    if (arms == 1)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Walk_Right_2(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Walk_Right_2(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Walk_Right_2(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Walk_Right_2(128)");
                                break;
                        }
                    }
                    if (arms == 0)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Walk_Right_3(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Walk_Right_3(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Walk_Right_3(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Walk_Right_3(128)");
                                break;
                        }
                    }
                }
                if (dir == -1)
                {
                    if (arms == 2)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Walk_Left_1(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Walk_Left_1(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Walk_Left_1(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Walk_Left_1(128)");
                                break;
                        }
                    }
                    if (arms == 1)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Walk_Left_2(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Walk_Left_2(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Walk_Left_2(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Walk_Left_2(128)");
                                break;
                        }
                    }
                    if (arms == 0)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Walk_Left_3(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Walk_Left_3(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Walk_Left_3(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Walk_Left_3(128)");
                                break;
                        }
                    }
                }
                if (!hasControl) state = State.idle;
                break;
            case State.jump:
                if (lastDir == 1)
                {
                    if (arms == 2)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Jump_Right_Air_1(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Jump_Right_Air_1(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Jump_Right_Air_1(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Jump_Right_Air_1(128)");
                                break;
                        }
                    }
                    if (arms == 1)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Jump_Right_Air_2(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Jump_Right_Air_2(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Jump_Right_Air_2(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Jump_Right_Air_2(128)");
                                break;
                        }
                    }
                    if (arms == 0)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Jump_Right_Air_3(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Jump_Right_Air_3(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Jump_Right_Air_3(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Jump_Right_Air_3(128)");
                                break;
                        }
                    }
                }
                if (lastDir == -1)
                {
                    if (arms == 2)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Jump_Left_Air_1(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Jump_Left_Air_1(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Jump_Left_Air_1(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Jump_Left_Air_1(128)");
                                break;
                        }
                    }
                    if (arms == 1)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Jump_Left_Air_2(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Jump_Left_Air_2(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Jump_Left_Air_2(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Jump_Left_Air_2(128)");
                                break;
                        }
                    }
                    if (arms == 0)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Jump_Left_Air_3(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Jump_Left_Air_3(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Jump_Left_Air_3(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Jump_Left_Air_3(128)");
                                break;
                        }
                    }
                }
                if (!hasControl && groundCheck) state = State.idle;
                break;
            case State.charge:
                if (lastDir == 1)
                {
                    if (arms == 2)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Charge_Right_1(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Charge_Right_1(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Charge_Right_1(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Charge_Right_1(128)");
                                break;
                        }
                    }
                    if (arms == 1)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Charge_Right_2(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Charge_Right_2(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Charge_Right_2(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Charge_Right_2(128)");
                                break;
                        }
                    }
                }
                if (lastDir == -1)
                {
                    if (arms == 2)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Charge_Left_1(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Charge_Left_1(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Charge_Left_1(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Charge_Left_1(128)");
                                break;
                        }
                    }
                    if (arms == 1)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Charge_Left_2(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Charge_Left_2(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Charge_Left_2(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Charge_Left_2(128)");
                                break;
                        }
                    }
                }
                break;
            case State.fire:
                if (lastDir == 1)
                {
                    if (arms == 2)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Fire_Right_1(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Fire_Right_1(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Fire_Right_1(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Fire_Right_1(128)");
                                break;
                        }
                    }
                    if (arms == 1)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Fire_Right_2(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Fire_Right_2(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Fire_Right_2(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Fire_Right_2(128)");
                                break;
                        }
                    }
                }
                if (lastDir == -1)
                {
                    if (arms == 2)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Fire_Left_1(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Fire_Left_1(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Fire_Left_1(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Fire_Left_1(128)");
                                break;
                        }
                    }
                    if (arms == 1)
                    {
                        switch (resolution)
                        {
                            case Resolution._1024:
                                animator.Play("Fire_Left_2(1024)");
                                break;
                            case Resolution._512:
                                animator.Play("Fire_Left_2(512)");
                                break;
                            case Resolution._256:
                                animator.Play("Fire_Left_2(256)");
                                break;
                            case Resolution._128:
                                animator.Play("Fire_Left_2(128)");
                                break;
                        }
                    }
                }
                break;
        }
    }

}