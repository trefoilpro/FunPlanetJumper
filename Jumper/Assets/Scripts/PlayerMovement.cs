using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private Rigidbody2D _playerRigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpPower;
    
    
    private void Update()
    {
        
        if (hasAuthority)
        {
            GetInput();
        }
    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector2(-1 * _speed * Time.deltaTime, 0));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector2(1 * _speed * Time.deltaTime, 0));
        }
        
        if (Input.GetKey(KeyCode.Space) && _groundChecker.IsGrounded && _playerRigidbody.velocity.y == 0f)
        {
            Jump();
        }
    }

    private void Jump()
    {
        Debug.Log("Jump");
        _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, _jumpPower);
    }
}
