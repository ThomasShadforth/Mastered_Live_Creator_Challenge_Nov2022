using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public Material material;

    Rigidbody _rb;
    PlayerControllerAction _playerInput;

    public float moveSpeed;
    public float rotateSpeed;

    public LayerMask ballCheckLayer;

    Vector2 _moveInput;

    PhotonView _view;

    [SerializeField] Transform _handPos;

    // Start is called before the first frame update
    void Start()
    {
        
        _rb = GetComponentInChildren<Rigidbody>();
        material = GetComponentInChildren<MeshRenderer>().material;
        material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        _view = GetComponent<PhotonView>();

        _playerInput = new PlayerControllerAction();
        _playerInput.Player.Enable();
        _playerInput.Player.GrabThrow.performed += GrabThrow;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_view.IsMine)
        {
            return;
        }

        _moveInput = _playerInput.Player.Movement.ReadValue<Vector2>();
        _moveInput = _moveInput.normalized;

        if (_moveInput != Vector2.zero)
        {
            Vector3 direction = new Vector3(_moveInput.x, 0, _moveInput.y);
            Quaternion toRotation = Quaternion.LookRotation(direction, transform.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
        }

    }

    private void FixedUpdate()
    {
        if (!_view.IsMine)
        {
            return;
        }

        _rb.velocity = new Vector3(_moveInput.x * moveSpeed, _rb.velocity.y, _moveInput.y * moveSpeed);
    }

    public void OnHit()
    {
        //Called when the player is hit by an opponents ball after being thrown
    }

    public void GrabThrow(InputAction.CallbackContext context)
    {
        if (!_view.IsMine)
        {
            return;
        }

        Collider[] colliderCheck = Physics.OverlapBox(_handPos.position, new Vector3(80, 80, 80), Quaternion.identity, ballCheckLayer);

        if(colliderCheck != null)
        {
            Debug.Log("NOT EMPTY");
            for(int i = 0; i < colliderCheck.Length; i++)
            {
                if (colliderCheck[i].gameObject.GetComponent<Ball>())
                {
                    Ball ball = colliderCheck[i].gameObject.GetComponent<Ball>();
                    ball.OnPickup(transform);

                    break;
                }
            }
        }
    }

    //Add Player character input events
}
