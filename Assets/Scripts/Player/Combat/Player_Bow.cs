using System;
using UnityEngine;

public class Player_Bow : MonoBehaviour
{

    public Player_AnimationHandler animationHandler;
    public Transform launchPoint;
    public GameObject arrowPrefab;
    public Player_state playerState;
    public float shootCooldown = 0.5f;
    private float shootTimer;
    private Vector2 aimDirection = Vector2.right;

    public Animator anim;



    private void Awake()
    {
        if (playerState != null)
            playerState.OnStateChanged += HandleStateChanged;
    }

    private void OnDestroy()
    {
        if (playerState != null)
            playerState.OnStateChanged -= HandleStateChanged;
    }

    private void Start()
    {
        ApplyBowVisualState();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerState.Equipment != EquipmentState.Bow) return;
        shootTimer -= Time.deltaTime;
        HandleAiming();
    }

    public void StartShooting()
    {
        if (shootTimer <= 0)
        {
            if (playerState.TryStartAction(ActionState.Shooting))
            {
                anim.SetBool("isShooting", true);
                animationHandler.UpdateStateAnimations();
            }
        }
    }

    void OnDisable()
    {
        anim.SetLayerWeight(0, 1);
        anim.SetLayerWeight(1, 0);
    }

    private void HandleStateChanged()
    {
        ApplyBowVisualState();
    }

    private void ApplyBowVisualState()
    {
        if (anim == null || playerState == null) return;

        bool isBowEquipped = playerState.Equipment == EquipmentState.Bow;
        anim.SetLayerWeight(0, isBowEquipped ? 0 : 1);
        anim.SetLayerWeight(1, isBowEquipped ? 1 : 0);

        if (!isBowEquipped)
            animationHandler.UpdateStateAnimations();
    }


    // private void HandleAiming()
    // {
    //     Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //     float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
    //     launchPoint.rotation = Quaternion.Euler(0, 0, angle);
    //     aimDirection = (mousePosition - (Vector2)launchPoint.position).normalized;
    //     anim.SetFloat("aimX", aimDirection.x);
    //     anim.SetFloat("aimY", aimDirection.y);
    //     Debug.DrawLine(launchPoint.position, mousePosition, Color.red);
    // }

    private void HandleAiming()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 1. Get direction from PLAYER (not launchPoint)
        Vector2 dir = (mousePosition - (Vector2)transform.position).normalized;

        aimDirection = dir;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float snappedAngle = Mathf.Round(angle / 45f) * 45f;
        float radius = 0.6f; 

        float rad = snappedAngle * Mathf.Deg2Rad;
        Vector2 snappedDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        launchPoint.rotation = Quaternion.Euler(0, 0, snappedAngle);
        launchPoint.position = (Vector2)transform.position + snappedDir * radius;

        // Animator
        anim.SetFloat("aimX", dir.x);
        anim.SetFloat("aimY", dir.y);

        Debug.DrawRay(transform.position, dir * 2f, Color.green);
    }

    public void Shoot()
    {
        if (shootTimer <= 0)
        {
            Arrow arrow = Instantiate(arrowPrefab, launchPoint.position, launchPoint.rotation).GetComponent<Arrow>();
            arrow.direction = aimDirection;
            shootTimer = shootCooldown;
        }
        animationHandler.UpdateStateAnimations();
        playerState.EndAction();
    }
}
