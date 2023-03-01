using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    public bool IsGrounded { get; private set; }

    private void Update()
    {
        IsGrounded = Physics2D.OverlapCapsule(_groundCheck.position, new Vector2(1.8f, 1f), CapsuleDirection2D.Horizontal, 0, _groundLayer);
        Debug.Log("IsGrounded = " + IsGrounded);
    }
}
