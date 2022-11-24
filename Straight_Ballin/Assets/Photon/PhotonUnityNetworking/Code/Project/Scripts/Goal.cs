using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Goal : MonoBehaviourPunCallbacks
{
    [SerializeField] Text teamUIDisplayLeft, teamUIDisplayRight;
    public string team;
    PhotonView _photonView;

    int goalIncrease = 1;

    // Start is called before the first frame update
    void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Ball>())
        {
            //Set ball back to middle
            other.gameObject.transform.position = new Vector3(0, other.gameObject.transform.position.y, 0);
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

            photonView.RPC("RPC_GoalScored", RpcTarget.All, goalIncrease);
        }
    }

    [PunRPC]
    void RPC_GoalScored(int pointToAdd)
    {
        if (team == "Left")
        {
            GameManager.rightScore += goalIncrease;
            teamUIDisplayRight.text = "Right: " + GameManager.rightScore.ToString();
        }
        else {
            GameManager.leftScore += goalIncrease;
            teamUIDisplayLeft.text = "Left: " + GameManager.leftScore.ToString();
        }
    }
}
