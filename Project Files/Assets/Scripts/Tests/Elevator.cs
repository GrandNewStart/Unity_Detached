using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    private Vector3 startPos;
    private Vector3 endPos;
    private new Rigidbody2D rigidbody;

    void Start()
    {
        rigidbody   = GetComponent<Rigidbody2D>();
        startPos    = startPoint.position;
        endPos      = endPoint.position;
    }

    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Q))
        {
            if (transform.position == startPos)
            {
                StartCoroutine(LerpRoutine(gameObject, endPos));
            }
            if (transform.position == endPos)
            {
                StartCoroutine(LerpRoutine(gameObject, startPos));
            }
        }
    }


    private IEnumerator LerpRoutine(GameObject obj, Vector2 targetPosition)
    {
        Vector2 startPosition = obj.transform.position;
        float time = 0f;

        while (rigidbody.position != targetPosition)
        {
            float t = (time / Vector2.Distance(startPosition, targetPosition)) * speed;
            obj.transform.position = Vector2.Lerp(startPosition, targetPosition, t);
            time += Time.deltaTime;
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(startPoint.position, .3f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(endPoint.position, .3f);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(startPoint.position, endPoint.position);
    }
}
