using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private Vector2 moveInput;

    [Header("Movement Settings")]
    [SerializeField, Tooltip("Velocidade do player")]
    private float moveSpeed = 5f;

    [Header("Orientation Reference")]
    [SerializeField, Tooltip("Opcional: Se vazio, usa a Main Camera automaticamente")]
    private Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector3 move;

        if (cameraTransform != null)
        {
            // Projeta a frente e a direita da câmera no chão (plano XZ)
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            move = (forward * moveInput.y) + (right * moveInput.x);
        }
        else
        {
            // Comportamento original caso não haja câmera
            move = new Vector3(moveInput.x, 0f, moveInput.y);
        }

        rb.AddForce(move * moveSpeed);
    }
}