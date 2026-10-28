using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPivotController : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField, Tooltip("Alvo que a câmera vai seguir (ex: Player)")]
    private Transform target;

    [SerializeField, Tooltip("PlayerInput de onde virá a ação Look")]
    private PlayerInput playerInput;

    [Header("Orbit Settings")]
    [SerializeField, Tooltip("Sensibilidade do giro do mouse")]
    private float mouseSensitivity = 0.15f;

    [SerializeField, Tooltip("Limite mínimo de inclinação vertical")]
    private float bottomClamp = -15f;

    [SerializeField, Tooltip("Limite máximo de inclinação vertical")]
    private float topClamp = 70f;

    private InputAction lookAction;
    private float yaw;   // Giro horizontal (Eixo Y)
    private float pitch; // Inclinação vertical (Eixo X)

    void Start()
    {
        // Se não arrastou no Inspector, tenta buscar automaticamente no próprio Target
        if (playerInput == null && target != null)
        {
            playerInput = target.GetComponent<PlayerInput>();
        }

        if (playerInput != null)
        {
            lookAction = playerInput.actions["Look"];
        }

        // Inicializa os ângulos com a rotação inicial do pivô na cena
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;

        LockCursor();
    }

    void Update()
    {
        HandleCursorLock();

        if (lookAction != null)
        {
            Vector2 lookInput = lookAction.ReadValue<Vector2>();
            yaw += lookInput.x * mouseSensitivity;
            pitch -= lookInput.y * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, bottomClamp, topClamp);
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Segue apenas a POSIÇÃO da bola, ignorando completamente o giro físico dela
        transform.position = target.position;
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private void HandleCursorLock()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            LockCursor();
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}