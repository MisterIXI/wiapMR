using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTest : MonoBehaviour
{
    BoxCollider boxCollider;
    public BoxCollider other;
    // Start is called before the first frame update
    void Start()
    {
        // boxCollider = GetComponent<BoxCollider>();
        // if(boxCollider == null){
        //     boxCollider = gameObject.AddComponent<BoxCollider>();
        // }
        BoxCollider bc = gameObject.AddComponent<BoxCollider>();
        bc.size = other.bounds.size;
        bc.center = other.gameObject.transform.position;
        other.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // if (boxCollider != null)
        // {
        //     Debug.Log("BoxCollider.bounds.size = " + boxCollider.bounds.size);
        //     Debug.Log("BoxCollider.bounds.center = " + boxCollider.bounds.center);
        //     Debug.Log("BoxCollider.bounds.extents = " + boxCollider.bounds.extents);
        //     Debug.Log("Boxcollider.size = " + boxCollider.size);
        //     Debug.Log("Boxcollider.center = " + boxCollider.center);
        // }
        // if(boxCollider2 != null){
        //     boxCollider2.size = boxCollider.size;
        //     boxCollider2.center = boxCollider.center;
        // }
    }
}
