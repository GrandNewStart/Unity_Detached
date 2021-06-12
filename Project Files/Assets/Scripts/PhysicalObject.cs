using System;
using UnityEngine;

public class PhysicalObject : MonoBehaviour
{
    public GameManager      gameManager;
    public bool             isDestroyed = false;
    public GameObject       normalSprite = null;
    public GameObject       destroyedSprite = null;
    public new Rigidbody2D  rigidbody;
    protected Vector2       origin;
    protected Vector2       velocity;
    protected float         gravityScale;
    protected float         mass;

    protected virtual void Start() {
        origin          = transform.position;
        rigidbody       = GetComponent<Rigidbody2D>();
        gravityScale    = rigidbody.gravityScale;
        mass            = rigidbody.mass;

        if (normalSprite != null) { normalSprite.SetActive(true); }
        if (destroyedSprite != null) { destroyedSprite.SetActive(false); }
        if (gameManager != null) { gameManager.objects.Add(this); }
    }

    public void DestroyObject()
    {
        isDestroyed = true;
        if (normalSprite != null) { normalSprite.SetActive(false); }
        if (destroyedSprite != null) { destroyedSprite.SetActive(true); }
        OnDestruction();
    }

    public void RecoverObject()
    {
        transform.position = origin;
        isDestroyed = false;
        if (normalSprite != null) { normalSprite.SetActive(true); }
        if (destroyedSprite != null) { destroyedSprite.SetActive(false); }
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
            transform.SetParent(collision.transform);
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
            transform.SetParent(null);
        }
    }
}
