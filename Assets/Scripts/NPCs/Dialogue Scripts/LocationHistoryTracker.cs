using System.Collections.Generic;
using UnityEngine;

public class LocationHistoryTracker : MonoBehaviour
{
    public static LocationHistoryTracker Instance;
    private readonly HashSet<LocationSO> visitedLocations = new HashSet<LocationSO>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
            Instance = this;
    }

    public void RecordLocation(LocationSO locationSO)
    {
        visitedLocations.Add(locationSO);
    }

    public bool HasVisitedLocation(LocationSO locationSO)
    {
        return visitedLocations.Contains(locationSO);
    }
}
