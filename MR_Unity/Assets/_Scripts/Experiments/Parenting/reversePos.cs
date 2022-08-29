using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reversePos : MonoBehaviour
{
    public GameObject miniOther;
    public GameObject miniBoard;
    public GameObject board;
    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = miniOther.transform.position - miniBoard.transform.position;
        newPos = Quaternion.Inverse(miniBoard.transform.rotation) * newPos;
        newPos = board.transform.rotation * newPos;
        newPos += board.transform.position;
        transform.position = newPos;
        Quaternion newRot = miniOther.transform.rotation;
        newRot = Quaternion.Inverse(miniBoard.transform.rotation) * newRot;
        newRot = board.transform.rotation * newRot;
        transform.rotation = newRot;
    }
}
