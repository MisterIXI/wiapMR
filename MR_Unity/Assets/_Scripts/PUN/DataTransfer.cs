using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;

namespace WiapMR.PUN
{
    public class DataTransfer : MonoBehaviour
    {
        private const int CHUNKSIZE = 450000;
        private Dictionary<string, int> dataProgress = new Dictionary<string, int>();
        private Dictionary<string, byte[]> dataDictionary = new Dictionary<string, byte[]>();
        public void SendData(byte[] data, string tag)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            if (data.Length > CHUNKSIZE)
            {
                int chunks = (int)Math.Ceiling((double)data.Length / CHUNKSIZE);
                for (int i = 0; i < chunks; i++)
                {
                    byte[] chunk = new byte[CHUNKSIZE];
                    Array.Copy(data, i * CHUNKSIZE, chunk, 0, Math.Min(CHUNKSIZE, data.Length - i * CHUNKSIZE));
                    photonView.RPC("ReceiveData", RpcTarget.All, i + 1, chunks, data.Length, tag, chunk);
                }
            }
            else
            {
                photonView.RPC("ReceiveData", RpcTarget.All, 1, 1, data.Length, tag, data);
            }
        }

        public event Action<string, byte[]> OnDataReceived = delegate { };

        [PunRPC]
        public void ReceiveData(int step, int totalSteps, int arrSize, string tag, byte[] data)
        {
            // check for tag in Dictionary
            if (!dataProgress.ContainsKey(tag))
            {
                dataProgress.Add(tag, 1);
                dataDictionary.Add(tag, new byte[arrSize]);
            }
            else
            {
                dataProgress[tag]++;
            }
            int currentSteps = dataProgress[tag];
            // add chunk to data in Dictionary
            int startIndex = (step - 1) * CHUNKSIZE;
            // int endIndex = Math.Min(step * CHUNKSIZE, arrSize);
            Array.Copy(data, startIndex, dataDictionary[tag], 0, data.Length);
            // check if all chunks are received
            if (currentSteps == totalSteps)
            {
                // Debug.Log("Event: " + OnDataReceived + "| Tag: " + tag + "| Data: " + dataDictionary[tag].Length);
                OnDataReceived?.Invoke(tag, dataDictionary[tag]);
                dataProgress.Remove(tag);
                dataDictionary.Remove(tag);
            }
        }


    }
}