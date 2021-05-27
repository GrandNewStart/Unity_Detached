using System;
using UnityEngine;

public class PhysicalObject : MonoBehaviour
{
    public bool             isDestroyed = false;
    public GameObject       normalSprite;
    public GameObject       destroyedSprite;
    public new Rigidbody2D  rigidbody;
    protected float         gravityScale;
    protected float         mass;
    protected float         treadmillVelocity = 0;
    protected bool          isOnTreadmill = false;
    protected Vector2       velocity;

    protected virtual void Awake() {}

    protected virtual void Start() {
        rigidbody       = GetComponent<Rigidbody2D>();
        gravityScale    = rigidbody.gravityScale;
        mass            = rigidbody.mass;
        normalSprite.SetActive(true);
        destroyedSprite.SetActive(false);
    }

    protected virtual void FixedUpdate()
    {
        MoveOnTreadmill();
    }

    public void DestroyObject()
    {
        isDestroyed = true;
        normalSprite.SetActive(false);
        destroyedSprite.SetActive(true);
        OnDestruction();
    }

    public void RecoverObject()
    {
        isDestroyed = false;
        normalSprite.SetActive(true);
        destroyedSprite.SetActive(false);
        OnRestoration();
    }

    protected virtual void OnDestruction() {}
    protected virtual void OnRestoration() {}
    public virtual void OnPause() {}
    public virtual void OnResume() {}

    protected virtual void MoveOnTreadmill()
    {
        if (!isOnTreadmill) return;
        velocity = new Vector2(treadmillVelocity, rigidbody.velocity.y);
        rigidbody.velocity = velocity;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform") ||
            collision.collider.CompareTag("Metal"))
        {
            transform.parent = collision.transform;
        }
        if (collision.collider.CompareTag("Crusher"))
        {
            DestroyObject();
        }
        if (collision.collider.CompareTag("Treadmill"))
        {
            SurfaceEffector2D effector = collision.collider.GetComponent<SurfaceEffector2D>();
            if (effector != null)
            {
                isOnTreadmill = true;
                treadmillVelocity = effector.speed;
            }
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform") ||
            collision.collider.CompareTag("Metal"))
        {
            transform.parent = null;
        }
        if (collision.collider.CompareTag("Treadmill"))
        {
            isOnTreadmill = false;
            rigidbody.AddForce(new Vector2(treadmillVelocity, 0));
            treadmillVelocity = 0;
        }
    }
}
