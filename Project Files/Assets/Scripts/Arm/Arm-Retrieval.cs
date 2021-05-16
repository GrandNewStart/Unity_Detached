using System.Collections;
using UnityEngine;

public partial class ArmController
{
    private void InitRetrievalAttributes()
    {
        normalGScale    = rigidbody.gravityScale;
        normalMass      = rigidbody.mass;
    }

    public void StartRetrieve()
    {
        if (isRetrieving) return;

        if (isPlugged) PlugOut();

        EnableCollider(false);
        rigidbody.velocity      = Vector3.zero;
        rigidbody.gravityScale  = 0f;
        rigidbody.mass          = 0f;

        isOnTreadmill   = false;
        isMovable       = false;
        trapped         = false;
        
        StartCoroutine(Retrieve());
    }

    public void RetrieveOnTrapped()
    {
        if (isRetrieving)   return;
        if (trapped)        return;

        if (isPlugged) PlugOut();

        EnableCollider(false);
        rigidbody.velocity = Vector2.zero;
        rigidbody.AddForce(new Vector2(0, 500));
        trapped = true;

        Invoke(nameof(StartRetrieve), 0.3f);
    }

    private IEnumerator Retrieve()
    {
        isRetrieving = true;

        while (isRetrieving)
        {
            Vector2 distance    = player.transform.position - transform.position;
            Vector2 direction   = distance.normalized;
            Vector2 movement    = direction * retrieveSpeed * Time.deltaTime;

            transform.Translate(movement, Space.World);

            if (distance.magnitude < retreiveRadius) OnRetrieveComplete();

            yield return null;
        }
    }

    private void OnRetrieveComplete()
    {
        gameObject.SetActive(false);
        EnableCollider(true);
        transform.position      = origin;
        rigidbody.gravityScale  = normalGScale;
        rigidbody.mass          = normalMass;
        isOut           = false;
        isFireComplete  = false;
        isRetrieving    = false;
        player.OnArmRetrieved();

        if (hasControl)
        {
            gameManager.SetControlTo(GameManager.PLAYER);
        }
    }

}