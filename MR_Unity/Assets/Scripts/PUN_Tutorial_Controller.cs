using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PUN_Tutorial_Controller : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 10f;
    PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
        {

            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Debug.Log("Horizontal: " + moveHorizontal + " Vertical: " + moveVertical);


            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

            if (Input.GetAxis("Fire1") > 0)
            {
                transform.RotateAround(transform.position, Vector3.up, 10 * speed * Time.deltaTime);
            }
            if (Input.GetAxis("Fire2") > 0)
            {
                transform.RotateAround(transform.position, Vector3.down, 10 * speed * Time.deltaTime);
            }
            rb.AddForce(movement * speed);
        }
    }
}
