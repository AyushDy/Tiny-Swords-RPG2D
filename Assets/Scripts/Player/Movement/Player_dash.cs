using System.Collections;
using UnityEngine;

public class Player_dash : MonoBehaviour
{
    public float dashSpeed = 20f;
    public float dashDuration = 0.25f;
    public float dashCooldown = 1f;
    private float dashTimer;
    public Rigidbody2D rb;
    public Player_health player_Health;
    public Player_state playerState;


    public Vector2 lastMoveDirection = Vector2.right;


    public float smoothStopTime = 0.1f;


    public GameObject afterImagePrefab;
    public float afterImageSpawnInterval = 0.05f;

    public float afterImageTimer;
    SpriteRenderer playerSR;


    public AudioSource audioSource;
    public AudioClip dashSound;


    public bool IsDashing => playerState != null &&
                             playerState.Locomotion == LocomotionState.Dashing;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDashing)
        {
            afterImageTimer -= Time.deltaTime;
            if (afterImageTimer <= 0)
            {
                SpawnAfterImage();
                afterImageTimer = afterImageSpawnInterval;
            }
        }
        dashTimer -= Time.deltaTime;
    }

    public void TryDash()
    {
        if (IsDashing || dashTimer > 0) return;

        if (!playerState.TrySetLocomotion(LocomotionState.Dashing)) return;
        StartDash();
    }

    private void StartDash()
    {
        audioSource.PlayOneShot(dashSound);
        afterImageTimer = 0f;
        rb.linearVelocity = lastMoveDirection * dashSpeed;
        StartCoroutine(DashCoroutine(dashDuration, dashCooldown));
    }


    IEnumerator DashCoroutine(float dashDuration, float dashCooldown)
    {
        player_Health.StartInvincibility(dashDuration);
        yield return new WaitForSeconds(dashDuration);
        float t = 0f;

        Vector2 startVelocity = rb.linearVelocity;

        while (t < smoothStopTime)
        {
            t += Time.deltaTime;
            Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

            float currentSpeed = StatsManager.Instance.speed;
            Vector2 TargetVelocity = moveInput * currentSpeed;

            float progress = t / smoothStopTime;
            progress = 1 - Mathf.Pow(1 - progress, 3);

            rb.linearVelocity = Vector2.Lerp(startVelocity, TargetVelocity, progress);

            yield return null;
        }
        bool result = playerState.TrySetLocomotion(LocomotionState.Normal, true);
        dashTimer = dashCooldown;
        Player_combat.instance.canReceiveInput = true;
    }


    void SpawnAfterImage()
    {
        GameObject afterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
        afterImage.transform.localScale = transform.localScale;

        SpriteRenderer sr = afterImage.GetComponent<SpriteRenderer>();
        sr.sprite = playerSR.sprite;
        sr.color = new Color(playerSR.color.r, playerSR.color.g, playerSR.color.b, 0.7f);
    }

    public void AttackLunge()
    {
        rb.linearVelocity = Vector2.zero;
        float force = 10f;
        float duration = 0.07f;
        StopCoroutine(nameof(AttackLungeCoroutine));
        StartCoroutine(AttackLungeCoroutine(force, duration));
    }

    IEnumerator AttackLungeCoroutine(float force, float duration)
    {
        Vector2 dir = lastMoveDirection.normalized;
        float t = 0f;
        Vector2 startVelocity = rb.linearVelocity;
        Vector2 targetVelocity = dir * force;

        while (t < duration)
        {
            t += Time.deltaTime;

            float progress = t / duration;
            progress = 1 - Mathf.Pow(1 - progress, 3);

            rb.linearVelocity = Vector2.Lerp(targetVelocity, startVelocity, progress);

            yield return null;
        }
    }
}
