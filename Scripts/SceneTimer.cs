using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTimer : MonoBehaviour
{
    [SerializeField,Tooltip("Time in seconds before loading the next scene")] 
    private float timeBeforeLoad = 5f;
    [SerializeField,Tooltip("Name of the scene to load after the timer")]
    private string sceneToLoad = "MainMenu";

    void Start()
    {
        Invoke("LoadLevel", timeBeforeLoad);
    }

    void LoadLevel()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

}
