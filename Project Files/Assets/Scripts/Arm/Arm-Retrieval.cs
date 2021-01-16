﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ArmController
{
    private void InitRetrievalAttributes()
    {
        sprite = normal.GetComponent<SpriteRenderer>();
        playerPosition = player.transform.position;
        normalScale = rigidbody.gravityScale;
        normalMass = rigidbody.mass;
    }

    public void StartRetrieve()
    {
        sprite.enabled = true;
        capsuleCollider.isTrigger = true;
        circleCollider_1.isTrigger = true;
        circleCollider_2.isTrigger = true;
        rigidbody.velocity = Vector3.zero;
        rigidbody.gravityScale = 0f;
        rigidbody.mass = 0f;
        isMovable = false;
        isRetrieving = true;
        OnPlugOut();

        StartCoroutine(Retrieve());
    }

    private IEnumerator Retrieve()
    {
        while (isRetrieving)
        {
            // Player's position is the target position.
            playerPosition = player.transform.position;

            Vector3 temp = new Vector3(transform.position.x, transform.position.y, 0);
            Vector3 diff = playerPosition - temp;
            Vector3 direction = diff.normalized;
            Vector3 movement = direction * retrieveSpeed * Time.deltaTime;

            // Move towards the player
            transform.Translate(movement, Space.World);

            // Retrieve complete
            if (diff.magnitude < retreiveRadius)
            {
                gameObject.SetActive(false);
                transform.position = origin;
                rigidbody.gravityScale = normalScale;
                rigidbody.mass = normalMass;
                capsuleCollider.isTrigger = false;
                circleCollider_1.isTrigger = false;
                circleCollider_2.isTrigger = false;
                isFireComplete = false;
                isRetrieving = false;
                isRetrieveComplete = true;
            }
            yield return null;
        }
    }

}