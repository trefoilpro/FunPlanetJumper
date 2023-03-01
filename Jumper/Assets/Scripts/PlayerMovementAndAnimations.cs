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

    private void Start()
    {
        _typeOfPlayerSprite = TypeOfPlayerSprite.MovingMid;
    }

    private void FixedUpdate()
    {
        _isStayingPlayer = true;
        GetInput();
        
    }
    
    private void GetInput()
    {

        if (_networkTransform.hasAuthority && isClient && isLocalPlayer)
        {
            if (Input.GetKey(KeyCode.A))
            {
           
                PressedA();

            }
            else if (Input.GetKey(KeyCode.D))
            {
            
                PressedD();
            
            }
        
            if (Input.GetKey(KeyCode.Space) && _groundChecker.IsGrounded && _playerRigidbody.velocity.y == 0f)
            {
                Jump();
            }
            
            CheckForStaticStaying();
            
            
        }
        
        CheckEnum();
    }
    
    public enum TypeOfPlayerSprite
    {
        MovingLeft,
        MovingMid,
        MovingRight,
    }

    [SyncVar] private TypeOfPlayerSprite _typeOfPlayerSprite;

    [Command]
    private void CmdChangeSprite(TypeOfPlayerSprite type)
    {
        _typeOfPlayerSprite = type;
    }


    private void CheckEnum()
    {
        if (_typeOfPlayerSprite == TypeOfPlayerSprite.MovingLeft)
        {
            _playerRenderer.sprite = _spritesMoveVariants[0];
        }
        else if (_typeOfPlayerSprite == TypeOfPlayerSprite.MovingMid)
        {
            _playerRenderer.sprite = _spritesMoveVariants[1];
        }
        else if (_typeOfPlayerSprite == TypeOfPlayerSprite.MovingRight)
        {
            _playerRenderer.sprite = _spritesMoveVariants[2];
        }
    }
    
    private void Jump()
    {
        if (_networkTransform.hasAuthority)
        {
            Debug.Log("Jump");
            _isStayingPlayer = false;
            _playerRigidbody.velocity += new Vector2(0f, _jumpPower);

            
            CheckCoroutine();
            
            //_playerRenderer.sprite = _jumpSprite;
            _typeOfPlayerSprite = TypeOfPlayerSprite.MovingLeft;
            CmdChangeSprite(_typeOfPlayerSprite);
            

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

    private void PressedA()
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

            _typeOfPlayerSprite = TypeOfPlayerSprite.MovingLeft;
            CmdChangeSprite(_typeOfPlayerSprite);
            //_playerRenderer.sprite = _jumpSprite;

        }

        _isStayingPlayer = false;
        transform.Translate(new Vector2(-1 * _speed * Time.deltaTime, 0));
    }


    private void PressedD()
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
                
            //_playerRenderer.sprite = _jumpSprite;
            _typeOfPlayerSprite = TypeOfPlayerSprite.MovingLeft;
            CmdChangeSprite(_typeOfPlayerSprite);
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
            
            
            //_playerRenderer.sprite = _staticSprite;
            _typeOfPlayerSprite = TypeOfPlayerSprite.MovingMid;
            CmdChangeSprite(_typeOfPlayerSprite);
        }
    }
    
    private IEnumerator AnimationCoroutine()
    {
        while (true)
        {
            if (_sideStepIndex >= _spritesMoveVariants.Count)
                _sideStepIndex = 0;

            switch (_sideStepIndex)
            {
                case 0:
                {
                    _typeOfPlayerSprite = TypeOfPlayerSprite.MovingLeft;
                    break;
                }
                case 1 or 3:
                {
                    _typeOfPlayerSprite = TypeOfPlayerSprite.MovingMid;
                    break;
                }
                case 2:
                {
                    _typeOfPlayerSprite = TypeOfPlayerSprite.MovingRight;
                    break;
                }
            }
            
            
            CmdChangeSprite(_typeOfPlayerSprite);
            yield return new WaitForSeconds(_stepSpeed);
            _sideStepIndex++;
        }
    }
    
}
