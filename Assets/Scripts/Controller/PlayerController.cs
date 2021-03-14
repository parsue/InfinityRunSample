using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] [Range(1,20)] private float startMoveSpeed = 4;
    [SerializeField] [Range(0.01f, 1.5f)] private float smoothTime = 0.75f;
    [SerializeField] private AnimationCurve curve;

    private enum Track { left, center, right, }
    private Track track;
    private Animator anim;
    private float startX, targetX;
    private float timer;
    private float lastZ;

    private float TrackWidth => GameManager.Instance.trackWidth;
    private float MoveSpeed => startMoveSpeed + GameManager.playerSpeed;

    private void Start()
    {
        anim = GetComponent<Animator>();
        track = Track.center;
        timer = 0;
        startX = 0;
        targetX = 0;
        lastZ = transform.position.z;
        StartCoroutine(Procedure());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
            GetCoin(other.gameObject);

        if (other.CompareTag("Obstacle"))
            Dead();
    }

    private IEnumerator Procedure()
    {
        Idle();
        yield return new WaitUntil(() => GameManager.State == GameState.Run);

        while (GameManager.State == GameState.Run)
        {
            UserInput();
            TrackMove();
            Run();
            CalculateDistance();
            yield return null;
        }
    }

    private void CalculateDistance()
    {
        DataCenter.LiveData.distance += transform.position.z - lastZ;
        lastZ = transform.position.z;
    }

    private void TrackMove()
    {
        if (timer < smoothTime + 0.15f)
        {
            transform.position = transform.position.x(Mathf.Lerp(startX, targetX, curve.Evaluate(timer / smoothTime)));
            timer += Time.deltaTime;
        }
    }

    private void UserInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            MoveLeft();
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            MoveRight();
    }

    private void MoveLeft()
    {
        switch (track)
        {
            case Track.center:
                timer = 0;
                track = Track.left;
                startX = transform.position.x;
                targetX = -TrackWidth;
                break;
            case Track.right:
                timer = 0;
                track = Track.center;
                startX = transform.position.x;
                targetX = 0;
                break;
        }
    }

    private void MoveRight()
    {
        switch (track)
        {
            case Track.left:
                timer = 0;
                track = Track.center;
                startX = transform.position.x;
                targetX = 0;
                break;
            case Track.center:
                timer = 0;
                track = Track.right;
                startX = transform.position.x;
                targetX = TrackWidth;
                break;
        }
    }

    private void Idle()
    {
        anim.SetBool("run", false);
    }

    private void Run()
    {
        anim.SetBool("run", true);
        transform.Translate(transform.forward * MoveSpeed * Time.deltaTime);
    }

    private void GetCoin(GameObject coin)
    {
        GameManager.GetCoin();
        Destroy(coin);
    }

    private void Dead()
    {
        anim.SetTrigger("dead");
        GameManager.PlayerDead();
    }
}
