using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controla um objeto pivô (CameraPivot) que segue a posição do alvo sem herdar sua rotação física,
/// permitindo orbitar a câmera em terceira pessoa usando o novo Input System.
/// </summary>
public class CameraPivotController : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField, Tooltip("Alvo que a câmera vai seguir (ex: Player)")]
    private Transform target; // Referência ao Transform do jogador para copiar sua posição

    [SerializeField, Tooltip("PlayerInput de onde virá a ação Look")]
    private PlayerInput playerInput; // Componente que gerencia os inputs do jogador

    [Header("Orbit Settings")]
    [SerializeField, Tooltip("Sensibilidade do giro do mouse")]
    private float mouseSensitivity = 0.15f; // Multiplicador de velocidade da rotação

    [SerializeField, Tooltip("Limite mínimo de inclinação vertical")]
    private float bottomClamp = -15f; // Impede que a câmera atravesse o chão ao olhar de baixo para cima

    [SerializeField, Tooltip("Limite máximo de inclinação vertical")]
    private float topClamp = 70f; // Impede que a câmera dê uma cambalhota (gimbal flip) ao olhar por cima

    private InputAction lookAction; // Referência direta para a ação "Look" (movimento do mouse/analógico)
    private float yaw;   // Acumula a rotação horizontal ao redor do eixo Y (esquerda/direita)
    private float pitch; // Acumula a inclinação vertical ao redor do eixo X (cima/baixo)

    void Start()
    {
        // Fallback de segurança: se o PlayerInput não foi atribuído no Inspector,
        // tenta localizá-lo automaticamente no próprio objeto do alvo (Player)
        if (playerInput == null && target != null)
        {
            playerInput = target.GetComponent<PlayerInput>();
        }

        // Busca e armazena a ação "Look" configurada dentro do Input Action Asset
        if (playerInput != null)
        {
            lookAction = playerInput.actions["Look"];
        }

        // Captura a rotação inicial do pivô na cena para evitar um "salto" brusco da câmera no primeiro frame
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;

        // Oculta e prende o cursor no centro da tela ao iniciar o jogo
        LockCursor();
    }

    void Update()
    {
        // Verifica a cada frame se o jogador pediu para soltar (ESC) ou prender (Clique) o mouse
        HandleCursorLock();

        // Lê o deslocamento do mouse apenas se a ação existir (e opcionalmente se o cursor estiver travado)
        if (lookAction != null && Cursor.lockState == CursorLockMode.Locked)
        {
            // Lê o vetor 2D de deslocamento (Delta X e Delta Y do mouse)
            Vector2 lookInput = lookAction.ReadValue<Vector2>();

            // Soma o movimento horizontal do mouse ao giro no eixo Y (Yaw)
            yaw += lookInput.x * mouseSensitivity;

            // Subtrai o movimento vertical no eixo X (Pitch) para que mover o mouse para cima incline a visão para cima
            pitch -= lookInput.y * mouseSensitivity;

            // Restringe o ângulo vertical entre os limites mínimo e máximo definidos no Inspector
            pitch = Mathf.Clamp(pitch, bottomClamp, topClamp);
        }
    }

    // O LateUpdate roda após todos os cálculos de física e Update, evitando tremores (jitter) na câmera
    void LateUpdate()
    {
        // Evita erro de referência nula caso o alvo seja destruído ou não tenha sido atribuído
        if (target == null) return;

        // Copia APENAS a posição espacial da esfera, ignorando completamente o rolamento físico dela
        transform.position = target.position;

        // Aplica a rotação calculada pelo mouse ao pivô (Z fica em 0 para não entortar o horizonte)
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    /// <summary>
    /// Gerencia o destravamento e retravamento do ponteiro do mouse durante os testes e gameplay.
    /// </summary>
    private void HandleCursorLock()
    {
        // Se existir um teclado ativo e a tecla ESC foi pressionada neste frame, libera o cursor
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None; // Solta o ponteiro para mover livremente
            Cursor.visible = true;                  // Torna a seta do mouse visível novamente
        }

        // Se existir um mouse ativo e o botão esquerdo foi clicado na tela, volta a prender o cursor
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            LockCursor();
        }
    }

    /// <summary>
    /// Centraliza, trava e esconde o ponteiro do mouse para permitir rotação contínua em 360 graus.
    /// </summary>
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; // Trava o cursor no centro da janela Game
        Cursor.visible = false;                   // Esconde o ícone da seta na tela
    }
}