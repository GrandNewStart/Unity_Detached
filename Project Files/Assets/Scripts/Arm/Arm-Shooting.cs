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
        rigidbody.gravityScale = normalGScale;
        rigidbody.mass = normalMass;
        gameObject.transform.position = playerPosition;
        gameObject.SetActive(true);
        isOut = true;
        isFireComplete = false;
        dir = 0;
    }

    private Vector2 CalculateVector(float power)
    {
        Vector2 fireVector = Vector2.zero;
        playerPosition = player.transform.position;
        switch (player.lastDir)
        {
            case 1:
                lastDir = 1;
                playerPosition.x += .5f;
                fireVector = new Vector2(5 + power, 15 + power);
                break;
            case -1:
                lastDir = -1;
                playerPosition.x -= .5f;
                fireVector = new Vector2(-5 - power, 15 + power);
                break;
        }
        return fireVector;
    }

}