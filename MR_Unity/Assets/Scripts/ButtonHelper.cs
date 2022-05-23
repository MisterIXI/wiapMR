using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.UI;

namespace WiapMR.PUN
{
    public class ButtonHelper : MonoBehaviour
    {
        public GameObject spawnableCube;
        public GameObject spawnableFloor;
        public float spawnDistance;
        public TMPro.TextMeshPro gravitySliderLabel;

        private List<GameObject> cubeList;

        private GameObject floor;
        PhotonView photonView;
        [HideInInspector] public GameObject FarCubeInstance;
        [HideInInspector] public GameObject NearCubeInstance;

        private Vector3 startPos;
        // Start is called before the first frame update
        void Start()
        {
            startPos = this.transform.position;
            Physics.gravity = new Vector3(0, -1f, 0);
            cubeList = new List<GameObject>();
            photonView = GetComponent<PhotonView>();
        }

        public void GravityUpdate(SliderEventData eventData)
        {
            photonView.RPC("UpdateSlider", RpcTarget.Others, eventData.NewValue);
            float gravity = eventData.NewValue * 5;
            Physics.gravity = new Vector3(0, -gravity, 0);
            gravitySliderLabel.text = "Gravity: " + gravity;
        }

        [PunRPC]
        public void UpdateSlider(float value)
        {
            GameObject.Find("Gravity_Slider").GetComponent<PinchSlider>().SliderValue = value;
        }

        public void ToggleGravity()
        {
            //call static toggleGravity function on a random cube (Can't call it in a "real" static way due to RPC)
            if (cubeList.Count > 0)
            {
                cubeList[0].GetComponent<PhotonView>().RPC("ToggleGravity", RpcTarget.All);
            }
            //update gravity in all cubes
            foreach (GameObject cube in cubeList)
            {
                cube.GetComponent<PhotonView>().RPC("UpdateGravity", RpcTarget.All);
            }
        }

        public void ResetTransform()
        {
            //Build temporary list of owned cubes
            List<GameObject> myCubes = new List<GameObject>();
            foreach (GameObject cube in cubeList)
            {
                if (cube.GetComponent<PhotonView>().IsMine)
                {
                    myCubes.Add(cube);
                }
            }
            //Reset all owned cubes
            Transform baseTransform = Camera.main.transform;
            for (int i = 0; i < myCubes.Count; i++)
            {
                myCubes[i].GetComponent<CubeController>().ResetTransform(i + 1, myCubes.Count, baseTransform);
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
            if (floor == null)
            {
                floor = PhotonNetwork.Instantiate(spawnableFloor.name, new Vector3(0, -1, 0), Quaternion.identity);
            }
            else
            {
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
}