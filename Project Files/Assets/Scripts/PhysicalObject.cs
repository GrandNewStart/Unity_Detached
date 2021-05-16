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

    protected virtual void Awake() {}

    protected virtual void Start() {
        rigidbody       = GetComponent<Rigidbody2D>();
        gravityScale    = rigidbody.gravityScale;
        mass            = rigidbody.mass;
        normalSprite.SetActive(true);
        destroyedSprite.SetActive(false);
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
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform") ||
            collision.collider.CompareTag("Metal"))
        {
            transform.parent = null;
        }
    }
}
