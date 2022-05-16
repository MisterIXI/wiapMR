using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.UI;

public class ButtonHelper : MonoBehaviour
{
    public GameObject spawnableCube;
    public GameObject spawnableFloor;
    public float spawnDistance;
    public TMPro.TextMeshPro gravitySliderLabel;

    private List<GameObject> cubeList;
    private GameObject floor;
    [HideInInspector] public GameObject FarCubeInstance;
    [HideInInspector] public GameObject NearCubeInstance;

    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
        Physics.gravity = new Vector3(0, -1f, 0);
        cubeList = new List<GameObject>();
    }

    public void gravityUpdate(SliderEventData eventData)
    {
        float gravity = eventData.NewValue * 5;
        Physics.gravity = new Vector3(0, -gravity, 0);
        gravitySliderLabel.text = "Gravity: " + gravity;
    }

    public void toggleGravity()
    {
        if (cubeList.Count > 0)
        {
            foreach (GameObject cube in cubeList)
            {
                cube.GetComponent<Rigidbody>().isKinematic = !cube.GetComponent<Rigidbody>().isKinematic;
                cube.GetComponent<Rigidbody>().useGravity = !cube.GetComponent<Rigidbody>().useGravity;
            }
        }
    }

    public void resetPosition()
    {
        if (cubeList.Count > 0)
        {
            foreach (GameObject cube in cubeList)
            {
                cube.transform.position = startPos;
                cube.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }

    public void cycleColor()
    {
        if (cubeList.Count > 0)
        {
            foreach (GameObject cube in cubeList)
            {
                cube.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);
            }
        }
    }

    public void toggleFloor()
    {
        if(floor == null)
        {
           floor = PhotonNetwork.Instantiate(spawnableFloor.name, new Vector3(0, -1, 0), Quaternion.identity);
        }
        else{
            PhotonNetwork.Destroy(floor);
        }
    }
    public void spawnCube()
    {
        GameObject parent = this.transform.parent.gameObject;
        var cube = PhotonNetwork.Instantiate(spawnableCube.name, parent.transform.position + parent.transform.forward * spawnDistance, Quaternion.identity, 0);
        cubeList.Add(cube);
    }

}
