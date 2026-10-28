using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    [SerializeField,Tooltip("Nome do jogo")]
    private string gameName = "ROLL TO FREEDOM";

    [SerializeField, Tooltip("Nome da primeira Fase/Level")]
    private string fistLevelSceneName = "Level1";

    [SerializeField, Tooltip("Imagem do jogo")]
    private Texture2D gameImageTexture;

    private PanelRenderer panelRenderer;
    private Button btnStartGame;
    private Button btnQuitGame;

    private void Awake()
    {
        panelRenderer = GetComponent<PanelRenderer>();
    }

    private void OnEnable()
    {
        panelRenderer.RegisterUIReloadCallback(ConfigureUI); 
    }

    private void OnDisable()
    {
        panelRenderer.UnregisterUIReloadCallback(ConfigureUI);
        btnStartGame.clicked -= StartGame;
        btnQuitGame.clicked -= QuitGame;
    }

    void ConfigureUI(PanelRenderer renderer, VisualElement root, int version)
    {
        root.Q<Label>("lblTitle").text = gameName;
        root.Q<VisualElement>("gameArt").style.backgroundImage = gameImageTexture;
        //botões
        btnStartGame = root.Q<Button>("btnPlay");
        btnQuitGame = root.Q<Button>("btnQuit");

        btnStartGame.clicked += StartGame;
        btnQuitGame.clicked += QuitGame;
    }

    void StartGame()
    {
        SceneManager.LoadScene(fistLevelSceneName);
    }
    void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
