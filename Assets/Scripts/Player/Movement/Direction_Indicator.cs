using UnityEngine;

public class Direction_Indicator : MonoBehaviour
{

    public float radius = 1.5f;

    private Player_dash Player_dash;
    private Player_state playerState;
    private Transform arrowTransform;


    void Start()
    {
        arrowTransform = GetComponent<Transform>();
        Player_dash = GetComponentInParent<Player_dash>();
        playerState = GetComponentInParent<Player_state>();
    }
    void Update()
    {
        UpdateIndicator();
    }

    private void UpdateIndicator()
    {
        if(playerState.Locomotion != LocomotionState.Dashing)
        {
            Vector2 moveDirection = Vector2.right;
            if(Player_dash.lastMoveDirection != Vector2.zero)
            {
                moveDirection = Player_dash.lastMoveDirection;
            }

            Vector3 offset = (Vector3)(moveDirection * radius);
            arrowTransform.position = Player_dash.transform.position + offset;


            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            arrowTransform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    } 
}
