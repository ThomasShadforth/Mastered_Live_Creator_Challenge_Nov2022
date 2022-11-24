using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    public Transform[] SpawnPositions;
    public int playerIndex = 0;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void RPC_SpawnMessage()
    {
        Debug.Log("SPAWN MESSAGE");
        playerIndex++;
    }
}
