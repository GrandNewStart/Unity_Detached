using UnityEngine;

public class CrusherController : MonoBehaviour
{
    private enum State { up, wait, down }

    public float speed;
    public float accel;
    public float bound;
    public float waitTime;

    public GameManager      gameManager;
    public GameObject       column;
    public GameObject       maxHeight;
    public BoxCollider2D    deathCollider;
    public AudioSource      crushSound;

    private Vector3 crusherPosition;
    private Vector3 minHeight;
    private bool    isActivated;
    private State   state = State.down;

    private void Awake()
    {
        gameManager.crushers.Add(this);
    }

    private void Start()
    {
        crusherPosition = column.transform.position;
        minHeight       = crusherPosition;
        isActivated     = true;

        crushSound.loop         = false;
        crushSound.playOnAwake  = false;
    }

    private void Update()
    {
        OperateCrusher();
    }

    private void OperateCrusher()
    {
        isActivated = !gameManager.isPaused;
        crusherPosition = column.transform.position;

        if (isActivated)
        {
            switch (state)
            {
                case State.up:
                    GoUp();
                    break;
                case State.down:
                    GoDown();
                    break;
            }
        }
    }

    private void GoUp()
    {
        if (crusherPosition.y < maxHeight.transform.position.y)
        {
            Vector3 upVector = new Vector3(0, speed, 0) * Time.deltaTime;
            column.transform.Translate(upVector);
        }
        else
        {
            state = State.wait;
            Invoke("StopWait", waitTime);
        }
    }

    private void StopWait()
    {
        state = State.down;
    }

    private void GoDown()
    {
        if (crusherPosition.y > minHeight.y)
        {
            float downSpeed = -1 * Mathf.Pow(accel, speed * Time.deltaTime);
            Vector3 downVector = new Vector3(0, downSpeed, 0);
            column.transform.Translate(downVector);

            if (crusherPosition.y > minHeight.y)
            {
                column.transform.position = minHeight;
                crushSound.Play();
                state = State.up;
            }
        }
        else
        {
            crushSound.Play();
            state = State.up;
        }
    }

    public void AdjustAudio(float volume)
    {
        crushSound.volume = volume;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(maxHeight.transform.position, 0.5f);
    }
}