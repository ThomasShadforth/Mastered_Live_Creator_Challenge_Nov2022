using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Ball : MonoBehaviourPunCallbacks
{
    Rigidbody _rb;
    Collider _collider;
    PhotonView _photonView;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _photonView = new PhotonView();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

 
    public void OnPickup(Transform parent)
    {
        _photonView.RPC("RPC_SetBallParent", RpcTarget.All, parent);
    }

    [PunRPC]
    void RPC_SetBallParent(Transform parent)
    {
        transform.parent = parent;
        transform.position = parent.position;
        _rb.useGravity = false;
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        _collider.enabled = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            
            if(other.transform != transform.parent)
            {
                PlayerController playerHit = other.gameObject.GetComponent<PlayerController>();
                //Hit the other player
                playerHit.OnHit();
            }
        }
    }
}
