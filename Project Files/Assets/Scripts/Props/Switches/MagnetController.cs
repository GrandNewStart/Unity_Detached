using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// HORIZONTAL -> startPoint must come left to the endPoint
// VERTICAL -> startPoint must come under the endPoint
public class MagnetController : SwitchController
{
    [SerializeField] private GameObject startPoint;
    [SerializeField] private GameObject endPoint;
    [SerializeField] private GameObject pullPoint;
    [SerializeField] private GameObject cameraPivot;
    [SerializeField] private GameObject ray1Point;
    [SerializeField] private GameObject ray2Point;
    [SerializeField] private GameObject ray3Point;
    [SerializeField] private bool isHorizontal;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float pullSpeed;
    [SerializeField] private float pullRange;
    private Camera mainCamera;
    private Transform pullTarget;
    private new Rigidbody2D rigidbody;
    private Vector3 pullPointOrigin;
    private Rigidbody2D pullTargetRigidbody;
    private float pullTargetGravityScale = 0;
    private bool isActivated = false;
    private bool isPulling = false;
    private bool isPullingPlayer = false;
    private bool isPullingArm = false;

    protected override void Start()
    {
        base.Start();
        mainCamera = gameManager.camera;
        cameraTarget = cameraPivot.transform;
        rigidbody = target.GetComponent<Rigidbody2D>();
        firstArm = gameManager.firstArm;
        secondArm = gameManager.secondArm;
        pullPointOrigin = pullPoint.transform.position;
    }

    public override void OnDeactivation()
    {
        isActivated = false;
        isPulling = false;
        isPullingArm = false;
        isPullingPlayer = false;
        if (pullTargetRigidbody != null)
        {
            pullTargetRigidbody.gravityScale = pullTargetGravityScale;
            pullTargetRigidbody.velocity = rigidbody.velocity;
            pullTargetRigidbody = null;
        }
    }

    public override void Control()
    {
        base.Control();
        Move();
        Pull();
    }

    public override void ChangeControl()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            gameManager.controlIndex = gameManager.GetControlIndex();
            switch (gameManager.controlIndex)
            {
                case GameManager.PLAYER:
                    Debug.Log("PLAYER");
                    player.hasControl = true;
                    firstArm.hasControl = false;
                    secondArm.hasControl = false;
                    if (isPullingPlayer) player.isMovable = false;
                    rigidbody.velocity = Vector2.zero;
                    gameManager.cameraTarget = player.transform;
                    StartCoroutine(gameManager.MoveCamera());
                    break;
                case GameManager.FIRST_ARM:
                    Debug.Log("FIRST ARM");
                    if (isFirstArmPlugged)
                    {
                        gameManager.cameraTarget = cameraPivot.transform;
                    }
                    else
                    {
                        gameManager.cameraTarget = firstArm.transform;
                        if (isPullingArm) firstArm.isMovable = false;
                        rigidbody.velocity = Vector2.zero;
                    }
                    player.hasControl = false;
                    firstArm.hasControl = true;
                    secondArm.hasControl = false;
                    StartCoroutine(gameManager.MoveCamera());
                    break;
                case GameManager.SECOND_ARM:
                    Debug.Log("SECOND ARM");
                    if (isSecondArmPlugged)
                    {
                        gameManager.cameraTarget = cameraPivot.transform;
                    }
                    else
                    {
                        gameManager.cameraTarget = secondArm.transform;
                        if (isPullingArm) secondArm.isMovable = false;
                        rigidbody.velocity = Vector2.zero;
                    }
                    player.hasControl = false;
                    firstArm.hasControl = false;
                    secondArm.hasControl = true;
                    StartCoroutine(gameManager.MoveCamera());
                    break;
            }
        }
    }

    public override void AdjustAudio(float volume)
    {
        
    }

    public override void MoveCamera()
    {
        if (gameManager.cameraMoving) return;
        Vector3 cameraPos = cameraPivot.transform.position;
        cameraPos.z = -1;
        cameraPos.y += 2;
        mainCamera.transform.position = cameraPos;
    }

    private void Move()
    {
        Vector2 movement = new Vector2(0, 0);
        if (isHorizontal)
        {
            float horizontal = Input.GetAxis("Horizontal");
            if (horizontal != 0)
            {
                PlayMotorSound();
            }
            horizontal = horizontal * moveSpeed * Time.deltaTime;
            movement.x += horizontal;

            Vector3 targetPos = target.transform.position;
            if (targetPos.x < startPoint.transform.position.x)
            {
                Vector3 position = startPoint.transform.position;
                position.z = 0;
                position.y = target.transform.position.y;
                target.transform.position = position;
                return;
            }
            if (targetPos.x > endPoint.transform.position.x)
            {
                Vector3 position = endPoint.transform.position;
                position.z = 0;
                position.y = target.transform.position.y;
                target.transform.position = position;
                return;
            }
        }
        else
        {
            float vertical = Input.GetAxis("Vertical");
            if (vertical != 0)
            {
                PlayMotorSound();
            }
            vertical = vertical * moveSpeed * Time.deltaTime;
            movement.y += vertical;

            Vector3 targetPos = target.transform.position;
            if (targetPos.y < startPoint.transform.position.y)
            {
                Vector3 position = startPoint.transform.position;
                position.z = 0;
                position.y = target.transform.position.y;
                target.transform.position = position;
                return;
            }
            if (targetPos.y > endPoint.transform.position.y)
            {
                Vector3 position = endPoint.transform.position;
                position.z = 0;
                position.y = target.transform.position.y;
                target.transform.position = position;
                return;
            }
        }

        rigidbody.velocity = movement;
    }

    private void Pull()
    {
        pullPointOrigin.x = pullPoint.transform.position.x;
        pullPointOrigin.z = pullPoint.transform.position.z;

        if (isActivated)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isActivated = false;
                isPullingPlayer = false;
                isPullingArm = false;
                pullTarget = null;
                player.isMovable = true;
                firstArm.isMovable = true;
                secondArm.isMovable = true;
                pullTargetRigidbody.gravityScale = pullTargetGravityScale;
                pullTargetRigidbody.velocity = rigidbody.velocity;
            }
            if (pullTarget != null && !isPulling)
            {
                pullTarget.transform.position = pullPoint.transform.position;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DetectTarget();
                if (pullTarget != null)
                {
                    StartCoroutine(PullTarget(pullTarget));
                    isActivated = true;
                }
            }
        }
    }

    private void DetectTarget()
    {
        RaycastHit2D hit1 = Physics2D.Raycast(
            ray1Point.transform.position, 
            new Vector2(0, -pullRange));
        float hit1Distance = pullRange + 1;
        RaycastHit2D hit2 = Physics2D.Raycast(
            ray2Point.transform.position,
            new Vector2(0, -pullRange));
        float hit2Distance = pullRange + 1;
        RaycastHit2D hit3 = Physics2D.Raycast(
            ray3Point.transform.position,
            new Vector2(0, -pullRange));
        float hit3Distance = pullRange + 1;
        if (hit1)
        {
            if (hit1.collider.CompareTag("Player") ||
                hit1.collider.CompareTag("Arm"))
            {
                hit1Distance = MeasureDistance(
                    pullPoint.transform.position,
                    hit1.transform.position);
            }
        }
        if (hit2)
        {
            if (hit2.collider.CompareTag("Player") ||
                hit2.collider.CompareTag("Arm"))
            {
                hit2Distance = MeasureDistance(
                    pullPoint.transform.position,
                    hit2.transform.position);
            }
        }
        if (hit3)
        {
            if (hit3.collider.CompareTag("Player") ||
                hit3.collider.CompareTag("Arm"))
            {
                hit3Distance = MeasureDistance(
                    pullPoint.transform.position,
                    hit3.transform.position);
            }
        }

        float minDistance = Mathf.Min(hit1Distance, hit2Distance, hit3Distance);
        if (minDistance > pullRange)
        {
            pullTarget = null;
            isPullingPlayer = false;
            isPullingArm = false;
            Debug.Log("NOT PULLING");
            return;
        }
        if (minDistance == hit2Distance)
        {
            pullTarget = hit2.transform;
            pullTargetRigidbody = hit2.rigidbody;
            isPullingPlayer = (hit2.collider.CompareTag("Player"));
            isPullingArm = (hit2.collider.CompareTag("Arm"));
            Debug.Log("RAY 2 PULLING (" + hit2.collider.name + ")");
            return;
        }
        if (minDistance == hit1Distance)
        {
            pullTarget = hit1.transform;
            pullTargetRigidbody = hit1.rigidbody;
            isPullingPlayer = (hit1.collider.CompareTag("Player"));
            isPullingArm = (hit1.collider.CompareTag("Arm"));
            Debug.Log("RAY 1 PULLING (" + hit1.collider.name + ")");
            return;
        }
        if (minDistance == hit3Distance)
        {
            pullTarget = hit3.transform;
            pullTargetRigidbody = hit3.rigidbody;
            isPullingPlayer = (hit3.collider.CompareTag("Player"));
            isPullingArm = (hit3.collider.CompareTag("Arm"));
            Debug.Log("RAY 3 PULLING (" + hit3.collider.name + ")");
            return;
        }

    }

    private IEnumerator PullTarget(Transform target)
    {
        isPulling = true;
        if (isPullingPlayer) pullPoint.transform.position = pullPointOrigin + new Vector3(0, -1.2f, 0);
        if (isPullingArm) pullPoint.transform.position = pullPointOrigin + new Vector3(0, -0.5f, 0);

        pullTargetGravityScale = pullTargetRigidbody.gravityScale;
        pullTargetRigidbody.gravityScale = 0;
        pullTargetRigidbody.velocity = Vector2.zero;

        while (isPulling)
        {
            Vector3 temp = new Vector3(target.position.x, target.position.y, 0);
            Vector3 diff = pullPoint.transform.position - temp;
            Vector3 direction = diff.normalized;
            Vector3 movement = direction * pullSpeed * Time.deltaTime;

            target.transform.Translate(movement, Space.World);

            if (diff.magnitude < 0.5f)
            {
                target.transform.position = pullPoint.transform.position;
                isPulling = false;
            }
            yield return null;
        }
    }

    private float MeasureDistance(Vector3 v1, Vector3 v2)
    {
        Vector3 diff = v1 - v2;
        return diff.magnitude;
    }

    private void PlayMotorSound()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(startPoint.transform.position, 0.5f);
        Gizmos.DrawWireSphere(endPoint.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pullPoint.transform.position, 0.5f);
        Gizmos.DrawLine(startPoint.transform.position, endPoint.transform.position);
        Gizmos.DrawRay(ray1Point.transform.position, new Vector2(0, -pullRange));
        Gizmos.DrawRay(ray2Point.transform.position, new Vector2(0, -pullRange));
        Gizmos.DrawRay(ray3Point.transform.position, new Vector2(0, -pullRange));
    }
}
