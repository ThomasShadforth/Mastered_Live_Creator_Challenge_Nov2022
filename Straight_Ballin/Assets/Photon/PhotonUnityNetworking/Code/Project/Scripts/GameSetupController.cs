using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviour
{
    //Store an array of the player start positions. (Ensures players don't spawn in a stack on top of each other)
    [SerializeField] Transform[] _playerStartPositions;
    //Records the number of players currently in a game. Set to 1 by default
    [SerializeField] int _playerNum = 1;

    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer(); //Method responsible for creating a networked object for each player that joins the room/scene
    }

    void CreatePlayer()
    {
        for(int i = 0; i < _playerStartPositions.Length; i++)
        {
            if ((_playerNum - 1) == i) {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerChar"), _playerStartPositions[i].position, _playerStartPositions[i].rotation);
                _playerNum++;
                break;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
