using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{  
    public static GameController Instance { get; private set; } // Singleton instance of the GameController
    public static event Action<float> OnTimerUpdated; // Event to notify when the timer is updated

    [Header("Game Settings")]
    [SerializeField,Tooltip("Tempo Restante em segundos")]
    private float curretTimer = 60f; // Current timer value in seconds

    [SerializeField, Tooltip("Cena de Game Over")]
    private string gameOverSceneName = "GameOver"; // Name of the Game Over scene
    public float CurretTimer => curretTimer; // Public property to access the current timer value
    private bool isTimerRunning = false; // Flag to check if the timer is running   


    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isTimerRunning)
        {
            if (curretTimer > 0f)
            {
                curretTimer -= Time.deltaTime;
                OnTimerUpdated?.Invoke(curretTimer);

                if (curretTimer <= 0f)
                {
                    curretTimer = 0f;
                    isTimerRunning = false;
                    OnTimerUpdated?.Invoke(curretTimer);
                    TriggerGameOver();
                }
            }
        }
    }

    public void StartLevelTimer(float duration)
    {
        curretTimer = duration;
        isTimerRunning = true;
        OnTimerUpdated?.Invoke(curretTimer);
    }

    public void TriggerGameOver()
    {
        // 1. Verifica se a cena configurada existe e pode ser carregada
        if (!string.IsNullOrEmpty(gameOverSceneName) && Application.CanStreamedLevelBeLoaded(gameOverSceneName))
        {
            SceneManager.LoadScene(gameOverSceneName);
        }
        else
        {
            int totalScenes = SceneManager.sceneCountInBuildSettings;
            Debug.LogWarning("Carregando a última cena da lista");
            SceneManager.LoadScene(totalScenes-1);
        }
    }

    public void LoadNextLevel() {
        isTimerRunning = false; // Stop the timer when loading the next level

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex+1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels to load.");
        }
    }
    
}
