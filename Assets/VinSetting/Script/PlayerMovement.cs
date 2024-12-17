using System.Collections;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController Controller;
    [SerializeField] private Animator animator;

    private const float BASE_WALKSPEED = 4f;
    private const float BASE_SPRINTSPEED = 5f;
    private const float BASE_CROUCHSPEED = 1f;
    private float walkSpeed;
    private float sprintSpeed;
    private float crouchSpeed;
    public float pushForce = 15f;
    private float gravity = -9.81f;
    public float jumpHeight = 3f;
    public Transform playerTransform;
    public Transform cameraTransform;
    float cameraStartPosY;
    public float cameraCrouchPosY;
    float currentSpeed;

    public Transform groundcheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    // Crouch
    public float crouchYScale = 0.5f;
    private float startYScale;
    private bool isCrouching = false;

    // Sliding
    bool isSliding = false;
    public float slideSpeed = 20f;
    public float slideTime = 0.5f;


    void Start()
    {
        Controller = GetComponent<CharacterController>();
        startYScale = playerTransform.localScale.y;  
        cameraStartPosY = Camera.main.transform.localPosition.y;
        walkSpeed = BASE_WALKSPEED;
        crouchSpeed = BASE_CROUCHSPEED;
        sprintSpeed = BASE_SPRINTSPEED;
    }

    void Update()
    {
        Move();
        if(Input.GetButtonDown("Jump") && IsGrounded()){
            Jump();
        }
    }

    // Mekanik Bergerak
    void Move(){
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = (transform.right * x + transform.forward * z).normalized;

        currentSpeed = walkSpeed;
        Vector3 horizontalSpeed = new Vector3(Controller.velocity.x, 0, Controller.velocity.z);

        // Speed Ngesprint
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            currentSpeed = sprintSpeed;
            animator.speed = 2f;
        } else if(Input.GetKeyUp(KeyCode.LeftShift)){
            animator.speed = 1f;
        }

        // Mekanik crouch
        if(Input.GetKeyDown(KeyCode.LeftControl) && horizontalSpeed.magnitude <= 0.1f){
            isCrouching = !isCrouching;
            if(isCrouching){
                playerTransform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
                cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, cameraCrouchPosY, cameraTransform.localPosition.z);
            } else{
                playerTransform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
                cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, cameraStartPosY, cameraTransform.localPosition.z);
            }  
        }

        // Mekanik slide
        if(Input.GetKeyDown(KeyCode.LeftControl) && horizontalSpeed.magnitude > 0.1f && !isCrouching && !isSliding && IsGrounded()){
            StartCoroutine(Sliding(slideSpeed));
        }

        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }

        velocity.y += gravity * Time.deltaTime;
        Controller.Move(velocity * Time.deltaTime);
        Controller.Move(move * currentSpeed * Time.deltaTime);
        animator.SetFloat("x", x);
        animator.SetFloat("z", z);
    }

    void Jump(){
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    bool IsGrounded(){
        return Physics.CheckSphere(groundcheck.position, groundDistance, groundMask);
    }

    IEnumerator Sliding(float slideSpeed){
        isSliding = true;
        float timer = slideTime;
        currentSpeed = slideSpeed;
        playerTransform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, cameraCrouchPosY, cameraTransform.localPosition.z);
        while(timer > 0){
            if(Input.GetKeyUp(KeyCode.LeftControl)){
                break;
            }
            timer -= Time.deltaTime;
            Controller.Move(Vector3.down * currentSpeed * Time.deltaTime);
            yield return null;
        }
        playerTransform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, cameraStartPosY, cameraTransform.localPosition.z);
        isSliding = false;
        yield return null;
    }

    public void ChangeMovementSpeed(float buffValue){
        walkSpeed += BASE_WALKSPEED * buffValue;
        sprintSpeed += BASE_SPRINTSPEED * buffValue;
        crouchSpeed += BASE_CROUCHSPEED * buffValue;
    }
}
