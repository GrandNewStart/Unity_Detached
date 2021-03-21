using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ArmController
{

    // This function is called by PlayerController
    public void Fire(float power)
    {
        Vector3 fireVector = CalculateVector(power);
        PrepareFire();
        rigidbody.AddForce(fireVector, ForceMode2D.Impulse);
    }

    private void PrepareFire()
    {
        rigidbody.gravityScale = normalScale;
        rigidbody.mass = normalMass;
        isRetrieveComplete = false;
        gameObject.transform.position = playerPosition;
        gameObject.SetActive(true);
        dir = 0;
    }

    private Vector3 CalculateVector(float power)
    {
        Vector3 fireVector = Vector3.zero;
        playerPosition = player.transform.position;
        switch (player.GetDir())
        {
            case 1:
                lastDir = 1;
                playerPosition.x += 1;
                fireVector = new Vector3(5 + power, 15 + power, 0);
                break;
            case -1:
                lastDir = -1;
                playerPosition.x -= 1;
                fireVector = new Vector3(-5 - power, 15 + power, 0);
                break;
        }
        return fireVector;
    }

}