using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour

{
    private CharacterController controller;
    private bool IsGround;
    public Transform GroundCheck;
    public LayerMask layerMask;
    public float CheckRadius = 0.2f;
    public float Speed = 10f;
    public float Gravity = -9.8f;
    public float JumpHeight = 3f;
    private Vector3 Velocity = Vector3.zero;
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] bool lockCursor = true;

    float cameraPitch = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        controller = transform.GetComponent<CharacterController>();
        
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        UpdateMouseLook();

    }

    private void PlayerMovement()
    {
        IsGround = Physics.CheckSphere(GroundCheck.position, CheckRadius, layerMask);
        if(IsGround && Velocity.y < 0)
        {
            Velocity.y = 0;
        }

        if (IsGround && Input.GetButtonDown("Jump")) 
        {

            Velocity.y += Mathf.Sqrt(JumpHeight * -2 * Gravity);
        }
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var move = transform.forward * Speed * vertical * Time.deltaTime;
        controller.Move(move);

        Velocity.y += Gravity * Time.deltaTime;
        controller.Move(Velocity * Time.deltaTime);

    }

    void UpdateMouseLook()
    {

        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        cameraPitch -= mouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity);

    }
}
