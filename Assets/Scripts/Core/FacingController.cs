using UnityEngine;

public class FacingController : MonoBehaviour
{
    [SerializeField] private int facingDirection;

    public int FacingDirection => facingDirection;

    public void FaceDirection(float directionX)
    {
        if (directionX > 0 && facingDirection < 0)
        {
            Flip();
        }
        else if (directionX < 0 && facingDirection > 0)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingDirection *= -1;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void SetFacing(int direction)
    {
        if (direction != facingDirection)
        {
            Flip();
        }
    }
}
