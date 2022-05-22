using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CubeController : MonoBehaviour
{
    PhotonView pv;
    Rigidbody rb;
    static bool gravityEnabled;
    private GameObject floor;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();

    }
    [PunRPC]
    public static void ToggleGravity()
    {
        gravityEnabled = !gravityEnabled;
    }

    [PunRPC]
    public void UpdateGravity()
    {
        if (pv.IsMine)
        {
            rb.useGravity = gravityEnabled;
            rb.isKinematic = !gravityEnabled;
        }
    }

    [PunRPC]
    public void DestroyThis()
    {
        if (pv.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            Debug.Log("Couldn't delete since it's not mine");
        }
    }
    //resets the cubes to theirs originalPosition and size
    public void ResetTransform(int position, int totalCount, Transform basePosition)
    {
        //ownership should be already checked earlier, but as a security this is done again
        if (pv.IsMine)
        {
            transform.position = basePosition.position + basePosition.forward * 5 - basePosition.right * 1.3f * (totalCount / 2 - position);
            transform.localScale = new Vector3(1, 1, 1);
            transform.rotation = basePosition.rotation;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

}
