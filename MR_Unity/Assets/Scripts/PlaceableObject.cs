using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    bool isSnapping;
    SnapPoint snappedTo;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void snapTo(SnapPoint snapPoint)
    {
        if (snapPoint != null)
        {
            snappedTo = snapPoint;
            isSnapping = true;
        }
    }
}
