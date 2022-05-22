using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PUN_Tutorial_Controller : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 10f;
    bool collisionLock;
    private Vector3 currentVelocity;
    PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
        collisionLock = false;
    }

    [PunRPC]
    public void enableCollision()
    {
        collisionLock = false;
        Debug.Log("Collision Unlocked");
    }

    [PunRPC]
    public void disableCollision()
    {
        collisionLock = true;
        Debug.Log("Collision Locked");
    }

    [PunRPC]
    public bool getCollisionLock()
    {
        return collisionLock;
    }
    void FixedUpdate()
    {
        if (photonView.IsMine)
        {

            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            // Debug.Log("Horizontal: " + moveHorizontal + " Vertical: " + moveVertical);


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
        currentVelocity = rb.velocity;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && this.GetComponent<PhotonView>().IsMine)
        {
            //Check if the collided object is owned by another player
            if (collision.gameObject.GetComponent<PhotonView>().Owner != PhotonNetwork.LocalPlayer)
            {

                Debug.Log(this.currentVelocity.magnitude + " vs " + collision.gameObject.GetComponent<PUN_Tutorial_Controller>().currentVelocity.magnitude);
                //The GameObject with higher velocity wins

                if (this.currentVelocity.magnitude > collision.gameObject.GetComponent<PUN_Tutorial_Controller>().currentVelocity.magnitude)
                {
                    collision.gameObject.GetComponent<PhotonView>().RPC("disableCollision", RpcTarget.All);
                }
                else
                {
                    this.GetComponent<PhotonView>().RPC("disableCollision", RpcTarget.All);
                }
            }
            Debug.Log(this.GetComponent<PhotonView>().Owner + ": CollisionLock: " + collisionLock + " isMine: " + collision.gameObject.GetComponent<PhotonView>().IsMine);
            if (!collisionLock && !collision.gameObject.GetComponent<PhotonView>().IsMine)
            {
                Debug.Log(this.GetComponent<PhotonView>().Owner + " collided with " + collision.gameObject.GetComponent<PhotonView>().Owner);
                collisionLock = true;
                collision.gameObject.GetComponent<PhotonView>().RequestOwnership();
                //Destroy(GameObject.Find("DespawnFence"));
            }
        }
    }

}
