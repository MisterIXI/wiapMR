using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerController : MonoBehaviourPun
{
    private Rigidbody _rb;
    public float speed = 10f;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            // horizontal movement
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            _rb.AddForce(movement * speed);
            //rotate around y axis
            if (Input.GetAxis("Fire1") > 0)
            {
                transform.RotateAround(transform.position, Vector3.up, 10 * speed * Time.deltaTime);
            }
            if (Input.GetAxis("Fire2") > 0)
            {
                transform.RotateAround(transform.position, Vector3.down, 10 * speed * Time.deltaTime);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (photonView.IsMine)
        {
            if (other.gameObject.tag == "Player")
            {
                if(!other.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    Debug.Log("Trying to request ownership from: " + other.gameObject.name);
                    other.gameObject.GetComponent<PhotonView>().RequestOwnership();
                }
            }
        }
    }
}
