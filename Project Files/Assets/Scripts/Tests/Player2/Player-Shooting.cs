using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    private void Charge()
    {
        if (armCount == 0) return;
        if (!isGrounded)
        {
            power = 0;
            isMovable = true;
            TurnGauagesOff();
            return;
        }

        if (Input.GetKey(KeyCode.L) ||
            Input.GetKey(KeyCode.F))
        {
            state = State.charge;
            isMovable = false;
            if (power < maxPower) power += 0.2f;
            TurnGaugesOn();
        }
    }

    private void TurnGaugesOn()
    {
        for (int i = 0; i < 5; i++)
        {
            if (power / maxPower >= i * 0.2f)
            {
                if (lastDir == 1)
                {
                    gauges[i].transform.localPosition = (new Vector2(-1.2f, 0.5f + 0.25f * i));
                }
                if (lastDir == -1)
                {
                    gauges[i].transform.localPosition = (new Vector2(1.2f, 0.5f + 0.25f * i));
                }
                gauges[i].SetActive(true);
            }
        }
    }

    private void TurnGauagesOff()
    {
        foreach (GameObject gauge in gauges) gauge.SetActive(false);
    }

    private void Fire()
    {
        if (armCount == 0) return;
        if (!isGrounded) return;

        if (Input.GetKeyUp(KeyCode.L) ||
            Input.GetKeyUp(KeyCode.F))
        {
            state = State.fire;
            isStateFixed = true;
            TurnGauagesOff();
            Invoke(nameof(OnFireDone), 0.3f);
            if (!firstArm.isFired)
            {
                firstArm.Fire(power);
                return;
            }
            if (!secondArm.isFired)
            {
                secondArm.Fire(power);
                return;
            }
            
        }
    }

    private void OnFireDone()
    {
        state           = State.idle;
        isMovable       = true;
        isStateFixed    = false;
        power           = 0.0f;
        armCount--;
    }

    private void Retrieve()
    {
        if (!isMovable)             return;
        if (firstArm.isReturning)   return;
        if (secondArm.isReturning)  return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (firstArm.isFired)
            {
                firstArm.Return();
            }
            if (secondArm.isFired)
            {
                secondArm.Return();
            }
        }
    }

    public void FinishRetrieval(Arm arm)
    {
        armCount++;
    }
}