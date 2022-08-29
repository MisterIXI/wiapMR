using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fakePun : MonoBehaviour
{
    public static readonly Vector3 OFFSET = new Vector3(5, 0, 0);
    public GameObject other;
    // Update is called once per frame
    void Update()
    {
        transform.localPosition = other.transform.localPosition;
        transform.localRotation = other.transform.localRotation;
        transform.localScale = other.transform.localScale;
    }
}
