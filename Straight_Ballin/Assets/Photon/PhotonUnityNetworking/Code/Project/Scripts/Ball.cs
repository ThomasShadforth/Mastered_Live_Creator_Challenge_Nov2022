using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Ball : MonoBehaviourPunCallbacks
{
    Rigidbody _rb;
    Collider _collider;
    PhotonView _photonView;

    Transform _parent;

    bool _isBeingThrown;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

 
    public void OnPickup(Transform parent)
    {
        _parent = parent;

        int testParam = 1;
        _photonView.RPC("RPC_SetBallParent", RpcTarget.All, testParam);
    }

    public void OnThrow(float throwSpeed)
    {
        _rb.constraints = RigidbodyConstraints.None;
        _rb.velocity = _parent.forward * throwSpeed;
        _collider.enabled = true;
        _rb.useGravity = true;
        _isBeingThrown = true;
    }

    [PunRPC]
    void RPC_SetBallParent(int testParam)
    {
        transform.parent = _parent;
        transform.position = _parent.position;
        _rb.useGravity = false;
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        _collider.enabled = false;
    }

    void RPC_ThrowBall()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_isBeingThrown)
        {
            if (other.gameObject.GetComponent<PlayerController>())
            {
                if (transform.parent != null)
                {
                    if (other.transform != transform.parent)
                    {
                        PlayerController playerHit = other.gameObject.GetComponent<PlayerController>();
                        //Hit the other player
                        Debug.Log("HIT ENEMY PLAYER: " + other.gameObject.name);
                        playerHit.OnHit();
                        transform.parent = null;
                        _parent = null;
                    }
                }
            }
            else if (!other.gameObject.GetComponent<PlayerController>())
            {
                Debug.Log("Hit Ground/Surface");
                transform.parent = null;
                _parent = null;
            }

            _isBeingThrown = false;
        }
    }
}
