using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class SC_CharacterController : MonoBehaviour
{
    public float speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 1.0f; //2
    public float lookXLimit = 0.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    [HideInInspector]
    public bool canMove = true;

    bool isMoving = false;

    AudioSource audioSource;
    [SerializeField] private AudioClip[] walks;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;

            moveDirection = (forward * curSpeedX) + (right * curSpeedY);
             
            isMoving = true;

            if (Input.GetButton("Jump") && canMove)
            {
                moveDirection.y = jumpSpeed;
                isMoving = false;
            }
        }

        else
            isMoving = false;

        if (isMoving && characterController.velocity.x != 0)
        {
            WalkSounds();
        }
        else
        {
            audioSource.Stop();
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
 

        // Player and Camera rotation
        if (canMove)
        {
            float lookUp = Input.GetAxis("Mouse X");
            float lookSide = Input.GetAxis("Mouse Y");

            rotation.y += lookUp * lookSpeed;
            rotation.x += -lookSide * lookSpeed;
            rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotation.x, 0, 0);
            transform.eulerAngles = new Vector2(0, rotation.y);
        }

    }

    void WalkSounds()
    {
        Invoke("RandomWalks", 0);
    }

    void RandomWalks()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.spatialBlend = 0.8f;
            audioSource.volume = 0.3f;
            audioSource.pitch = Random.Range(1.2f, 1.3f);
            audioSource.clip = walks[Random.Range(0, walks.Length)];
            audioSource.Play();
        }
    }
}