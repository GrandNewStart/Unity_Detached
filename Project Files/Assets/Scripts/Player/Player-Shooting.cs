using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    private void InitShootingAttributes()
    {
        power = 0.0f;
        tempPower = power;
        arms = enabledArms;
        isFirstArmRetrieving = false;
        isSecondArmRetrieving = false;
        isFirstArmOut = false;
        isSecondArmOut = false;

        foreach (GameObject gauge in gauges) gauge.SetActive(false);
    }

    public void CancelFire()
    {
        power = 0.0f;
        tempPower = power;
        foreach (GameObject gauge in gauges) gauge.SetActive(false);
        state = State.idle;
        isMovable = true;
    }

    private void Shoot()
    {
        if (isFirstArmRetrieving) return;
        if (isSecondArmRetrieving) return;
        if (!isGrounded) return;
        if (isStateFixed) return;

        Charge();
        Fire();        
    }

    private void Charge()
    {
        if (arms == 0) return;
        if (Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.F))
        {
            // Charging start.
            state = State.charge;
            // Play charge sound
            if (!isChargeSoundPlaying)
            {
                isChargeSoundPlaying = true;
                StartCoroutine(PlayChargeSound());
            }
            // Player can't move while charging.
            isMovable = false;
            // Increase power until limit;
            if (power < powerLimit) power += powerIncrement;
            // Raise pitch according to power;
            if (chargeSound.pitch < 1.5f) chargeSound.pitch *= 1.01f;
            else chargeSound.pitch = 1.5f;

            for (int i = 0; i < 5; i++)
            {
                if (power / powerLimit >= 0.2f * (i + 1))
                {
                    if (lastDir == 1)
                    {
                        //gauges[i].transform.localPosition = (new Vector2(-18.0f, 0.8f + 2.4f * i));
                        gauges[i].transform.localPosition = (new Vector2(-2.0f, 0.8f + 0.4f * i));
                    }
                    else if (lastDir == -1)
                    {
                        gauges[i].transform.localPosition = (new Vector2(2.0f, 0.8f + 0.4f * i));
                        //gauges[i].transform.localPosition = (new Vector2(18.0f, 0.8f + 2.4f * i));
                    }
                    gauges[i].SetActive(true);
                }
            }
        }
        else
        {
            gauges[0].SetActive(false);
            gauges[1].SetActive(false);
            gauges[2].SetActive(false);
            gauges[3].SetActive(false);
            gauges[4].SetActive(false);
        }

    }

    private void Fire()
    {
        if (arms == 0) return;
        if (Input.GetKeyUp(KeyCode.L) || Input.GetKeyUp(KeyCode.F))
        {
            // Firing start.
            state = State.fire;
            // Wait for the fire animation to finish.
            Invoke(nameof(FinishFire), 0.3f);
            // Player's state is fixed while the animation is playing
            isStateFixed = true;
            // Call HandController class's function to actually fire
            if (arms == 2)
            {
                firstArm.Fire(power);
                isFirstArmOut = true;
            }
            if (arms == 1)
            {
                if (isFirstArmOut)
                {
                    secondArm.Fire(power);
                    isSecondArmOut = true;
                }
                else
                {
                    firstArm.Fire(power);
                    isFirstArmOut = true;
                }
            }
            // Play Fire sound 
            fireSound.Play();
            power = 0.0f;
        }
    }

    private void FinishFire()
    {
        // Once firing is done, player is able to move, change state and an arm is reduced.
        isMovable = true;
        isStateFixed = false;
        state = State.idle;
        arms--;
    }


    private void Retrieve()
    {
        // Retrieve
        if (Input.GetKeyDown(KeyCode.R)
            && isMovable
            && !isFirstArmRetrieving
            && !isSecondArmRetrieving)
        {
            if (isFirstArmOut)
            {
                isFirstArmRetrieving = true;
                firstArm.StartRetrieve();
                PlayRetrieveSound();
            }
            if (isSecondArmOut)
            {
                isSecondArmRetrieving = true;
                secondArm.StartRetrieve();
                PlayRetrieveSound();
            }
        }

        // Check if retreiving is all done
        if (isFirstArmRetrieving)
        {
            isFirstArmRetrieving = !firstArm.GetRetrieveComplete();
            if (!isFirstArmRetrieving)
            {
                arms++;
                isFirstArmOut = false;
                PlayRetrieveCompleteSound();
            }
        }
        if (isSecondArmRetrieving)
        {
            isSecondArmRetrieving = !secondArm.GetRetrieveComplete();
            if (!isSecondArmRetrieving)
            {
                arms++;
                isSecondArmOut = false;
                PlayRetrieveCompleteSound();
            }
        }
    }

    public void RetrieveFirstArm()
    {
        isFirstArmRetrieving = true;
        firstArm.StartRetrieve();
        PlayRetrieveSound();
    }

    public void RetrieveSecondArm()
    {
        isSecondArmRetrieving = true;
        secondArm.StartRetrieve();
        PlayRetrieveSound();
    }

    public void ResetPower()
    {
        tempPower = power;
        power = 0.0f;
    }

    public void RecoverPower()
    {
        if (!Input.GetKey(KeyCode.L) && !Input.GetKey(KeyCode.F))
        {
            isMovable = true;
            power = 0;
            tempPower = 0;
        }
        else
        {
            power = tempPower;
            tempPower = 0;
        }
    }
}