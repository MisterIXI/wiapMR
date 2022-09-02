using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;

namespace WiapMR.PUN
{
    public class DataTransfer : MonoBehaviour
    {
        private const int Chunksize = 450000;
        private Dictionary<string, int> _dataProgress = default;
        private Dictionary<string, byte[]> _dataDictionary = default;

        /// <summary>
        /// Event to subscribe to to be notified of received data.
        /// </summary>
        public event Action<string, byte[]> OnDataReceived = delegate { };

        /// <summary>
        /// Sends data to all players.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="tag"></param>
        public void SendData(byte[] data, string tag)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            if (data.Length > Chunksize)
            {
                int chunks = (int)Math.Ceiling((double)data.Length / Chunksize);
                for (int i = 0; i < chunks; i++)
                {
                    byte[] chunk = new byte[Chunksize];
                    Array.Copy(data, i * Chunksize, chunk, 0, Math.Min(Chunksize, data.Length - i * Chunksize));
                    photonView.RPC("ReceiveData", RpcTarget.All, i + 1, chunks, data.Length, tag, chunk);
                }
            }
            else
            {
                photonView.RPC("ReceiveData", RpcTarget.All, 1, 1, data.Length, tag, data);
            }
        }

        /// <summary>
        /// Receives Data and triggers the event on completion.
        /// </summary>
        /// <param name="step">The index of the sent chunk</param>
        /// <param name="totalSteps">The total packets of the given tag</param>
        /// <param name="arrSize">The size of the original array</param>
        /// <param name="tag">The name of data being sent (including ID suffix)</param>
        /// <param name="data">The actual byte data</param>
        [PunRPC]
        public void ReceiveData(int step, int totalSteps, int arrSize, string tag, byte[] data)
        {
            // check for tag in Dictionary
            if (!_dataProgress.ContainsKey(tag))
            {
                _dataProgress.Add(tag, 1);
                _dataDictionary.Add(tag, new byte[arrSize]);
            }
            else
            {
                _dataProgress[tag]++;
            }
            int currentSteps = _dataProgress[tag];
            // add chunk to data in Dictionary
            int startIndex = (step - 1) * Chunksize;
            // int endIndex = Math.Min(step * CHUNKSIZE, arrSize);
            Array.Copy(data, startIndex, _dataDictionary[tag], 0, data.Length);
            // check if all chunks are received
            if (currentSteps == totalSteps)
            {
                // Debug.Log("Event: " + OnDataReceived + "| Tag: " + tag + "| Data: " + dataDictionary[tag].Length);
                OnDataReceived?.Invoke(tag, _dataDictionary[tag]);
                _dataProgress.Remove(tag);
                _dataDictionary.Remove(tag);
            }
        }

    }
}