using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class NetworkController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); //Connect to photon master servers
    }

    public override void OnConnectedToMaster()
    {
        //Called when the client connects to the master server
        Debug.Log("We have connected to the " + PhotonNetwork.CloudRegion + " server!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
