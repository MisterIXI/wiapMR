using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHelper : MonoBehaviour
{
    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
        Physics.gravity = new Vector3(0, -1f, 0);
    }


    public void toggleGravity()
    {
        this.GetComponent<Rigidbody>().isKinematic = !this.GetComponent<Rigidbody>().isKinematic;
        this.GetComponent<Rigidbody>().useGravity = !this.GetComponent<Rigidbody>().useGravity;
    }

    public void resetPosition(){
        this.transform.position = startPos;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void cycleColor(){
        this.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);
    }
}
