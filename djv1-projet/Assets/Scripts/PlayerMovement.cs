using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float rotationSpeed = 3f;
    [SerializeField] private Vector3 startingPosition;
    
    private Vector3 _velocity;
    private Vector3 _playerInput = Vector3.zero;
    private Vector3 _direction;
    private CharacterController _characterController;
    private Animator _animator;
    private int _runHashCode;
    private int _diesHashCode;
    
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _runHashCode = Animator.StringToHash("IsRunning");
        _diesHashCode = Animator.StringToHash("Dies");
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        // we get the player's instructions and update the animator accordingly
        if (Input.GetKeyDown(KeyCode.W))
        {
            _playerInput += Vector3.forward;
            _animator.SetBool(_runHashCode, true);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            _playerInput -= Vector3.forward;
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            _playerInput += Vector3.left;
            _animator.SetBool(_runHashCode, true);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            _playerInput -= Vector3.left;
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            _playerInput += Vector3.back;
            _animator.SetBool(_runHashCode, true);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            _playerInput -= Vector3.back;
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            _playerInput += Vector3.right;
            _animator.SetBool(_runHashCode, true);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            _playerInput -= Vector3.right;
        }
        
        // when no instructions are given, we launch the idle animation
        bool _isRunning = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        if (!_isRunning) _animator.SetBool(_runHashCode, false);

        // if an instruction is given, we make the character rotate and move
        // towards the given direction
        if (_playerInput.sqrMagnitude >= 0.5)
        {
            _direction = Vector3.RotateTowards(
                transform.forward, 
                _playerInput, 
                rotationSpeed * Time.deltaTime, 
                0.0f
                );
            // we make the character move and turn
            transform.rotation = Quaternion.LookRotation(_direction);
            _characterController.Move(movementSpeed * Time.deltaTime * Vector3.Normalize(_playerInput));
        }
        
        // testing
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _animator.SetTrigger(_diesHashCode);
        }
    }

    public void ResetPosition()
    {
        transform.position = startingPosition;
        transform.rotation = Quaternion.LookRotation(-startingPosition);
    }
}
