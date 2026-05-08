using UnityEngine;

public class LocationVisitedTrigger : MonoBehaviour
{
    [SerializeField] private LocationSO locationVisited;
    public bool destroyOnTouch = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.locationHistoryTracker.RecordLocation(locationVisited);
            if (destroyOnTouch  )
                Destroy(gameObject);
        }
    }
}
