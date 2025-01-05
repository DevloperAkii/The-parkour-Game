using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    [SerializeField]
    private float walkingSpeed;
    [SerializeField]
    private float runningSpeed;
    [SerializeField]
    private float smoothTime;
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private float sphereRadius;
    [SerializeField]
    private float sphereOffset;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float gravityForce;


    public bool Grounded;

    private CharacterController _playerMover = null;

    private Vector3 _horizontalForce = new Vector3();
    private Vector3 _verticalForce = new Vector3();

    private Vector2 _inputDirection = Vector2.zero;
    private bool _run;

    private Camera _playerFollowCam = null;

    private bool _inControl;
    private void Awake()
    {
        _playerMover = GetComponent<CharacterController>();
        _playerFollowCam = Camera.main;
        instance = this;
    }

    private void Start()
    {
        SetControl(true);
    }
    private void MoveCharacter()
    {

        if (_inControl)
        {
            _horizontalForce = CalculatedMovementDiraction();
            _verticalForce = CalculatedGravityForce();

            Vector3 movementDiraction =_horizontalForce + _verticalForce;
            _playerMover.Move(movementDiraction);
        }
    }
    private Vector3 CalculatedGravityForce()
    {
        float gravity = 0;
        gravity += gravityForce * Time.deltaTime;
        if (Grounded)
        {
            gravity = -0.2f;
        }
        return new Vector3(0,gravity,0);
    }
    private Vector3 CalculatedMovementDiraction()
    {
        float speed = _run ? runningSpeed : walkingSpeed;

        Vector3 diraction = _playerFollowCam.transform.forward * _inputDirection.y +
                            _playerFollowCam.transform.right * _inputDirection.x;
        diraction.y = 0;
        diraction = diraction.normalized;

        transform.forward = Vector3.Slerp(transform.forward, diraction, smoothTime * Time.deltaTime);
        
        return diraction * speed * Time.deltaTime;
    }
    private void Inputs()
    {
        _inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        _run = Input.GetKey(KeyCode.LeftShift);
    }
    private void GroundCheck()
    {
        Grounded = Physics.CheckSphere(transform.position + (Vector3.up * sphereOffset), sphereRadius, groundMask);
    }
    private void Update()
    {
        GroundCheck();
        Inputs();
        MoveCharacter();
    }

    public void SetControl(bool control)
    {
        _inControl = control;
        _playerMover.enabled = control;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Grounded ? new Color(0, 1, 0, 0.2f) : Gizmos.color = new Color(1,0,0,0.2f);
        Gizmos.DrawSphere(transform.position + (Vector3.up * sphereOffset), sphereRadius);
    }
}
