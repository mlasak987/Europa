using UnityEngine;

namespace Europa.Player
{
    public class MovementController : MonoBehaviour
    {
        public bool isSwimming {  get; private set; }
        [HideInInspector] public bool IsCompletelyUnderwater = false;

        [SerializeField] private float walkingSpeed = 6f;
        [SerializeField] private float runningSpeed = 12f;
        [SerializeField] private float swimmingSpeed = 5f;
        [SerializeField] private float fasterSwimmingSpeed = 8f;
        [SerializeField] private float jumpHeight = 1.5f;
        [Header("Gravity")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundMask;

        private CharacterController controller;

        private float movementSpeed = 0f, gravity = -9.81f;
        private Vector3 velocity;

        private readonly float groundDistance = 0.4f;
        private bool isGrounded;


        private void Start()
        {
            controller = GetComponent<CharacterController>();

            isSwimming = Player.Singleton.DefaultWater;
        }

        private void Update()
        {
            if (Player.Singleton.GamePaused) return;

            if (isSwimming)
            {
                movementSpeed = Input.GetKey(KeyCode.LeftShift) ? fasterSwimmingSpeed : swimmingSpeed;

                Vector3 movement = transform.right * Input.GetAxis("Horizontal") + transform.up * Input.GetAxis("Vertical") + Player.Singleton.CamController.transform.forward * Input.GetAxis("Forward");
                controller.Move(movementSpeed * Time.deltaTime * movement);
                if (!IsCompletelyUnderwater) controller.Move(-transform.up * Time.deltaTime);
            }
            else
            {
                isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
                if (isGrounded && velocity.y < 0) velocity.y = -2f;

                movementSpeed = Input.GetKey(KeyCode.LeftShift) ? runningSpeed : walkingSpeed;

                Vector3 movement = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Forward");
                controller.Move(movementSpeed * Time.deltaTime * movement);

                if (Input.GetButtonDown("Jump") && isGrounded) velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

                velocity.y += gravity * Time.deltaTime;
                controller.Move(velocity * Time.deltaTime);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Water")) isSwimming = true;
            else if (other.CompareTag("Air")) isSwimming = false;
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Water")) isSwimming = false;
            else if (other.CompareTag("Air")) isSwimming = true;
        }
    }
}
