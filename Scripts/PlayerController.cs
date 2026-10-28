using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controla a movimentação física do jogador (esfera) aplicando forças ao Rigidbody,
/// orientando a direção do movimento de acordo com o ângulo atual da câmera.
/// </summary>
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;            // Referência ao componente de física do jogador
    private PlayerInput playerInput; // Componente que gerencia o Input Action Asset na cena
    private InputAction moveAction;  // Referência direta para a ação "Move" (WASD / Setas / Analógico)
    private Vector2 moveInput;       // Armazena os valores X (horizontal) e Y (vertical) lidos a cada frame

    [Header("Movement Settings")]
    [SerializeField, Tooltip("Velocidade do player")]
    private float moveSpeed = 5f; // Multiplicador de intensidade da força física aplicada

    [Header("Orientation Reference")]
    [SerializeField, Tooltip("Opcional: Se vazio, usa a Main Camera automaticamente")]
    private Transform cameraTransform; // Transform de referência para calcular "frente" e "direita" relativas à visão

    void Start()
    {
        // Obtém os componentes anexados ao mesmo GameObject do jogador
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        // Localiza e armazena a ação "Move" configurada no mapa de ações do PlayerInput
        moveAction = playerInput.actions["Move"];

        // Fallback automático: se nenhuma câmera ou pivô foi arrastado no Inspector,
        // busca a câmera principal da cena (que possua a tag "MainCamera")
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    // O Update roda a cada frame renderizado — ideal para capturar entradas (inputs) com precisão
    void Update()
    {
        // Lê o vetor 2D do controle/teclado (X varia de -1 a 1 para lados; Y varia de -1 a 1 para frente/trás)
        moveInput = moveAction.ReadValue<Vector2>();
    }

    // O FixedUpdate roda em intervalos de tempo fixos sincronizados com o motor de física da Unity
    private void FixedUpdate()
    {
        Vector3 move; // Vetor final 3D que indicará para qual direção a bola será empurrada

        if (cameraTransform != null)
        {
            // 1. Captura os vetores direcionais para onde a câmera está apontando no espaço 3D
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            // 2. Zera a inclinação vertical (eixo Y) para projetar os vetores no plano do chão (XZ).
            // Isso evita que olhar para baixo empurre a bola contra o piso e reduza sua velocidade.
            forward.y = 0f;
            right.y = 0f;

            // 3. Reajusta o comprimento dos vetores para 1 (vetor unitário) após zerar o Y,
            // garantindo que a força máxima seja constante independentemente da inclinação da câmera
            forward.Normalize();
            right.Normalize();

            // 4. Combina o input vertical (W/S) com a frente da câmera e o horizontal (A/D) com a direita da câmera
            move = (forward * moveInput.y) + (right * moveInput.x);
        }
        else
        {
            // Comportamento de segurança (eixos globais do mundo) caso não exista câmera na cena
            move = new Vector3(moveInput.x, 0f, moveInput.y);
        }

        // Aplica força contínua (ForceMode.Force por padrão) ao Rigidbody para rolar a esfera fisicamente
        rb.AddForce(move * moveSpeed);
    }
}