using System.Collections.Generic;
using UnityEngine;

public class LocationHistoryTracker : MonoBehaviour
{
    private readonly HashSet<LocationSO> visitedLocations = new HashSet<LocationSO>();


    public void RecordLocation(LocationSO locationSO)
    {
        visitedLocations.Add(locationSO);
    }

    public bool HasVisitedLocation(LocationSO locationSO)
    {
        return visitedLocations.Contains(locationSO);
    }
}
