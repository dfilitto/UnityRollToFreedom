using UnityEngine;

public class LevelSettings : MonoBehaviour
{
    [Header("Configuração da Fase")]
    [SerializeField, Tooltip("Duração do nível em segundos")]
    private float levelDuration = 60f; // Duration of the level in seconds
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameController.Instance.StartLevelTimer(levelDuration); // Start the level timer with a duration of 60 seconds
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
