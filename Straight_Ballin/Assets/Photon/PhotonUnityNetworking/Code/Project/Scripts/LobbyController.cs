using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class LobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject _startButton;
    [SerializeField] GameObject _cancelButton;
    [SerializeField] int _roomSize;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        _startButton.SetActive(true);
    }

    public void QuickStart()
    {
        _startButton.SetActive(false);
        _cancelButton.SetActive(true);

        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a room!");
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("Creating room!");

        int randomRoomID = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = (byte)_roomSize
        };

        PhotonNetwork.CreateRoom("Room" + randomRoomID, roomOps);

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room!");
        CreateRoom();
    }

    public void QuickCancel()
    {
        _cancelButton.SetActive(false);
        _startButton.SetActive(true);

        PhotonNetwork.LeaveRoom();
    }
}
