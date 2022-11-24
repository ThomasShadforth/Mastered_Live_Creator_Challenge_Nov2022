using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class DelayWaitingRoomController : MonoBehaviourPunCallbacks
{
    //This must be attached to an object in the waiting room scene for the game:

    //Photon view for sending remote procedure callback to update the waiting timer (For when enough players join)
    PhotonView _photonView;

    [SerializeField]
    int _multiplayerSceneIndex;
    [SerializeField]
    int _menuSceneIndex;
    int _playerCount;
    int _roomSize;
    [SerializeField]
    int _minPlayerCount;

    [SerializeField]
    Text _playerCountText;
    [SerializeField]
    Text _timerCountDisplay;

    bool _readyToCount;
    bool _readyToStart;
    bool _starting;

    float _timerToStart;
    float _notFullTimer;
    float _fullGameTimer;

    [SerializeField] float _maxWaitTime;
    [SerializeField] float _maxFullWaitTime;

    // Start is called before the first frame update
    void Start()
    {
        _photonView = GetComponent<PhotonView>();
        _fullGameTimer = _maxFullWaitTime;
        _notFullTimer = _maxWaitTime;
        _timerToStart = _maxWaitTime;

        PlayerCountUpdate();
    }

    void PlayerCountUpdate()
    {
        //Update display when a player joins / leaves

        _playerCount = PhotonNetwork.PlayerList.Length;
        _roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;
        _playerCountText.text = _playerCount + " / " + _roomSize;

        if(_playerCount == _roomSize)
        {
            _readyToStart = true;
        } else if(_playerCount >= _minPlayerCount)
        {
            _readyToCount = true;
        }
        else
        {
            _readyToCount = false;
            _readyToStart = false;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerCountUpdate();
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_SendTimer", RpcTarget.Others, _timerToStart);
        }
    }

    [PunRPC]
    void RPC_SendTimer(float timeIn)
    {
        //Synchronises time to other players that join after the countdown starts
        _timerToStart = timeIn;
        _notFullTimer = timeIn;

        if(timeIn < _fullGameTimer)
        {
            _fullGameTimer = timeIn;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PlayerCountUpdate();
    }

    private void Update()
    {
        WaitingForMorePlayers();
    }

    void WaitingForMorePlayers()
    {
        if(_playerCount <= 1)
        {
            ResetTimer();
        }

        if (_readyToStart)
        {
            _fullGameTimer -= Time.deltaTime;
            _timerToStart = _fullGameTimer;
        } else if (_readyToCount)
        {
            _notFullTimer -= Time.deltaTime;
            _timerToStart = _notFullTimer;
        }

        string tempTimer = string.Format("{0:00}", _timerToStart);
        _timerCountDisplay.text = tempTimer;

        if(_timerToStart <= 0f)
        {
            if (_starting)
            {
                return;
            }

            StartGame();
        }
    }

    void ResetTimer()
    {
        _timerToStart = _maxWaitTime;
        _notFullTimer = _maxWaitTime;
        _fullGameTimer = _maxFullWaitTime;
    }

    public void StartGame()
    {
        _starting = true;
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(_multiplayerSceneIndex);
    }

    public void DelayCancel()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(_menuSceneIndex);
    }
}
