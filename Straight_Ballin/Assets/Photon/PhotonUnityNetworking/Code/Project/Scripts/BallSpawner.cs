using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
using Photon.Realtime;

public class BallSpawner : MonoBehaviourPunCallbacks
{

    PhotonView _photonView;

    public float ballSpawnRate;
    float spawnRateTime;


    // Start is called before the first frame update
    void Start()
    {
        _photonView = GetComponent<PhotonView>();
        spawnRateTime = ballSpawnRate;
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnRateTime > 0)
        {
            spawnRateTime -= 1 * Time.deltaTime;
        }

        if(spawnRateTime <= 0)
        {
            SpawnBall();
            spawnRateTime = ballSpawnRate;
        }
    }

    public void SpawnBall()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ball"), transform.position, Quaternion.identity);
        }
    }
}
