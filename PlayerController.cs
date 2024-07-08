using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 500f;
    [SerializeField] float GroundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;

    bool IsGrounded;

    float ySpeed;

    Quaternion targetRotation;

    CameraController cameraController;
    Animator animator;
    CharacterController characterController;
    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        

        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));
        var moveInput = (new Vector3(h, 0, v)).normalized;

        var moveDir = cameraController.transform.rotation * moveInput;

        GroundCheck();
        Debug.Log("IsGrounded = " + IsGrounded);
        if (IsGrounded)
        {
            ySpeed = -0.5f;
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }
        var velocity = moveDir * moveSpeed;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);

        if (moveAmount > 0)
{
        transform.rotation = Quaternion.LookRotation(moveDir);
}
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 
        rotationSpeed * Time.deltaTime);

        animator.SetFloat("moveAmount", moveAmount);
        
    }

    void GroundCheck()
    {
        IsGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), GroundCheckRadius, groundLayer);
    }
    private void OnDrawGizmosSelected() 
    {
    Gizmos.color = new Color (0, 1, 0, 0.5f);
    Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), GroundCheckRadius); 
    }
}

