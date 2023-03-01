using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerMovementAndAnimations : NetworkBehaviour
{
    [SerializeField] private NetworkTransform _networkTransform;
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private Rigidbody2D _playerRigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpPower;
    [Header("Animations")]
    [SerializeField] private SpriteRenderer _playerRenderer;
    [SerializeField] private float _stepSpeed;
    [Space(4f)] 
    [SerializeField] private Sprite _staticSprite;
    [SerializeField] private Sprite _jumpSprite;
    [SerializeField] private List<Sprite> _spritesMoveVariants;
    

    private float _stepTime;
    private int _sideStepIndex;
    private bool _faceRightDirection;
    private bool _isStayingPlayer;
    private Coroutine _movingCoroutine;
    
    private void FixedUpdate()
    {
        /*Debug.Log("_faceRightDirection = " + _faceRightDirection);
        Debug.Log(" isOwned Before if = " + _networkTransform.isOwned + " isClient = " + isClient + " isLocalPlayer = " + isLocalPlayer);
        Debug.Log(" hasAuthority Before if = " + _networkTransform.hasAuthority + " isClient = " + isClient + " isLocalPlayer = " + isLocalPlayer);
        
        Debug.Log(" isOwned Before if = " + _networkTransform.isOwned + " isServer = " + isServer + " isLocalPlayer = " + isLocalPlayer);
        Debug.Log(" hasAuthority Before if = " + _networkTransform.hasAuthority + " isServer = " + isServer + " isLocalPlayer = " + isLocalPlayer);*/
        
        
        
            
            _isStayingPlayer = true;
            GetInput();
        
        
        /*Debug.Log(" hasAuthority Before after if = " + _networkTransform.hasAuthority);
        Debug.Log(" isOwned Before after if = " + _networkTransform.isOwned);*/
    }

    private void GetInput()
    {

        if (_networkTransform.hasAuthority)
        {
            if (Input.GetKey(KeyCode.A))
            {
           
                CmdPressedA();
            
            
            }
            else if (Input.GetKey(KeyCode.D))
            {
            
                CmdPressedD();
            
            }
        
            if (Input.GetKey(KeyCode.Space) && _groundChecker.IsGrounded && _playerRigidbody.velocity.y == 0f)
            {
                CmdJump();
            }



            CheckForStaticStaying();
        }
        
        

    }

    private void CmdJump()
    {
        if (_networkTransform.hasAuthority)
        {
            Debug.Log("Jump");
            _isStayingPlayer = false;
            _playerRigidbody.velocity += new Vector2(0f, _jumpPower);

            
            CheckCoroutine();
            
            _playerRenderer.sprite = _jumpSprite;
        }
    }

    private void Flip(bool variable)
    {
        if(_networkTransform.hasAuthority)
        {
            _faceRightDirection = variable;
            Vector3 Scale = transform.localScale;
            Scale.x *= -1;
            transform.localScale = Scale;
        }
    }

    private void CheckCoroutine()
    {
        if (_movingCoroutine != null)
        {
            StopCoroutine(_movingCoroutine);
            _movingCoroutine = null;
        }
    }

    private void CmdPressedA()
    {
        if (_faceRightDirection)
        {
            Flip(false);
        }
            
        if (_groundChecker.IsGrounded)
        {
            if (_movingCoroutine == null && _playerRigidbody.velocity.y == 0f)
            {
                _sideStepIndex = 0;
                    
                CheckCoroutine();

                _movingCoroutine = StartCoroutine(AnimationCoroutine());
            }
        }
        else
        {
            CheckCoroutine();
                
            _playerRenderer.sprite = _jumpSprite;
        }

        _isStayingPlayer = false;
        transform.Translate(new Vector2(-1 * _speed * Time.deltaTime, 0));
    }


    private void CmdPressedD()
    {
        if (!_faceRightDirection)
        {
            Flip(true);
        }
            
        if (_groundChecker.IsGrounded)
        {
            if (_movingCoroutine == null && _playerRigidbody.velocity.y == 0f)
            {
                _sideStepIndex = 0;

                CheckCoroutine();

                _movingCoroutine = StartCoroutine(AnimationCoroutine());
            }
        }
        else
        {
            CheckCoroutine();
                
            _playerRenderer.sprite = _jumpSprite;
        }
            
            
        _isStayingPlayer = false;
        transform.Translate(new Vector2(1 * _speed * Time.deltaTime, 0));
    }
    
    
    private void CheckForStaticStaying()
    {
        if (!_groundChecker.IsGrounded)
        {
            _isStayingPlayer = false;
        }

        if (_isStayingPlayer)
        {
            CheckCoroutine();
            
            
            _playerRenderer.sprite = _staticSprite;
        }
    }
    
    
    private IEnumerator AnimationCoroutine()
    {
        while (true)
        {
            if (_sideStepIndex >= _spritesMoveVariants.Count)
                _sideStepIndex = 0;

            _playerRenderer.sprite = _spritesMoveVariants[_sideStepIndex];
            
            yield return new WaitForSeconds(_stepSpeed);
            _sideStepIndex++;
        }
    }
    
}
