using System.Collections;
using UnityEngine;

public partial class ArmController
{
    private void InitRetrievalAttributes()
    {
        playerPosition = player.transform.position;
        normalGScale = rigidbody.gravityScale;
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
        isOnTreadmill = false;
        isMovable = false;
        if (isPlugged)
        {
            PlugOut();
        }
        StartCoroutine(Retrieve());
    }

    private void ForceRetrieve()
    {
        if (isPlugged) PlugOut();
        if (isLeft) player.RetrieveFirstArm();
        else player.RetrieveSecondArm();
        trapped = false;
    }

    private IEnumerator Retrieve()
    {
        isRetrieving = true;

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
                OnRetrieveComplete();
            }
            yield return null;
        }
    }

    private void OnRetrieveComplete()
    {
        gameObject.SetActive(false);
        transform.SetParent(null);
        transform.position = origin;
        rigidbody.gravityScale = normalGScale;
        rigidbody.mass = normalMass;
        capsuleCollider.isTrigger = false;
        circleCollider_1.isTrigger = false;
        circleCollider_2.isTrigger = false;
        isOut = false;
        isFireComplete = false;
        isRetrieving = false;
        player.OnArmRetrieved();
        gameManager.controlIndex = GameManager.PLAYER;
        gameManager.SetControl();
    }

    public void RetrieveOnTrapped()
    {
        if (isRetrieving) return;
        if (trapped) return;
        if (currentSwitch != null) currentSwitch.Deactivate();
        trapped = true;
        rigidbody.velocity = Vector2.zero;
        rigidbody.AddForce(new Vector2(0, 500));
        capsuleCollider.isTrigger = true;
        circleCollider_1.isTrigger = true;
        circleCollider_2.isTrigger = true;
        Invoke(nameof(ForceRetrieve), 0.3f);
    }

}