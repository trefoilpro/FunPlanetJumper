using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CameraFollower : NetworkBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            CameraMovement();
        }
    }

    private void CameraMovement()
    {
        _camera.transform.localPosition = new Vector3(transform.position.x, transform.position.y, -3f);
        //transform.position = Vector2.MoveTowards()
    }
}
