using UnityEngine;

public partial class PlayerController
{
    private void InitShootingAttributes()
    {
        power       = 0;
        tempPower   = 0;
        arms        = enabledArms;
        foreach (GameObject gauge in gauges) gauge.SetActive(false);
    }

    public void CancelFire()
    {
        if (state == State.charge||
            state == State.fire)
        {
            power = 0.0f;
            tempPower = power;
            foreach (GameObject gauge in gauges) gauge.SetActive(false);
            state = State.idle;
            isMovable = true;
        }
    }

    private void Shoot()
    {
        if (isDestroyed) return;
        if (!hasControl) return;
        if (firstArm.isRetrieving) return;
        if (secondArm.isRetrieving) return;
        if (!isGrounded) return;
        if (isStateFixed) return;

        Charge();
        Fire();        
    }

    private void Charge()
    {
        if (arms == 0) return;

        if (Input.GetKey(KeyCode.L) ||
            Input.GetKey(KeyCode.F))
        {
            state = State.charge;
            PlayChargeSound();
            isMovable = false;
            if (power < powerLimit) power += powerIncrement/3;

            for (int i = 0; i < 5; i++)
            {
                if (power / powerLimit >= 0.2f * (i + 1))
                {
                    if (lastDir == 1)
                    {
                        gauges[i].transform.localPosition = (new Vector2(-2, 0.8f + 0.4f * i));
                    }
                    if (lastDir == -1)
                    {
                        gauges[i].transform.localPosition = (new Vector2(2, 0.8f + 0.4f * i));
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

        if (Input.GetKeyUp(KeyCode.L) ||
            Input.GetKeyUp(KeyCode.F))
        {
            state = State.fire;
            PlayFireSound();
            isStateFixed = true;
            Invoke(nameof(FinishFire), 0.3f);
            if (arms == 2)
            {
                firstArm.Fire(power);
            }
            if (arms == 1)
            {
                if (firstArm.isOut)
                {
                    secondArm.Fire(power);
                }
                else
                {
                    firstArm.Fire(power);
                }
            }
            power = 0.0f;
        }
    }

    private void FinishFire()
    {
        isMovable       = true;
        isStateFixed    = false;
        state           = State.idle;
        arms--;
    }


    private void Retrieve()
    {
        if (isDestroyed)            return;
        if (!hasControl)            return;
        if (!isMovable)             return;
        if (firstArm.isRetrieving)  return;
        if (secondArm.isRetrieving) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (firstArm.isOut) RetrieveFirstArm();
            if (secondArm.isOut) RetrieveSecondArm();
        }

    }

    public void OnArmRetrieved()
    {
        arms++;
        PlayRetrieveCompleteSound();
    }

    public void RetrieveFirstArm()
    {
        firstArm.StartRetrieve();
        PlayRetrieveSound();
    }

    public void RetrieveSecondArm()
    {
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