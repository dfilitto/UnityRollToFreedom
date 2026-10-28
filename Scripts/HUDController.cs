using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PanelRenderer))]
public class HUDController : MonoBehaviour
{
    private PanelRenderer panelRenderer;
    private Label timerValueLabel;

    private void Awake()
    {
        panelRenderer = GetComponent<PanelRenderer>();
    }

    private void OnEnable()
    {
        // 1. Registra o callback para quando a árvore visual estiver pronta
        panelRenderer.RegisterUIReloadCallback(ConfigureUI);

        // 2. Inscreve o método no evento de tempo do GameController
        GameController.OnTimerUpdated += UpdateTimerDisplay;
    }

    private void OnDisable()
    {
        // Remove os ouvintes para evitar vazamentos de memória
        panelRenderer.UnregisterUIReloadCallback(ConfigureUI);
        GameController.OnTimerUpdated -= UpdateTimerDisplay;
    }

    private void ConfigureUI(PanelRenderer renderer, VisualElement root, int version)
    {
        // Captura o Label do cronômetro definido no UXML
        timerValueLabel = root.Q<Label>("TimerValue");
    }

    /// <summary>
    /// Método executado automaticamente toda vez que o GameController dispara o evento OnTimerUpdated
    /// </summary>
    private void UpdateTimerDisplay(float timeInSeconds)
    {
        if (timerValueLabel == null) return;

        // Evita exibir valores negativos caso o tempo passe ligeiramente de zero
        float clampedTime = Mathf.Max(0f, timeInSeconds);

        int minutes = Mathf.FloorToInt(clampedTime / 60f);
        int seconds = Mathf.FloorToInt(clampedTime % 60f);

        // Formatação digital MM:SS (ex: 01:05)
        timerValueLabel.text = $"{minutes:00}:{seconds:00}";

        // Feedback de alerta visual nos últimos 10 segundos
        if (clampedTime <= 10f)
        {
            timerValueLabel.style.color = new StyleColor(new Color(1f, 0.25f, 0.25f));
        }
        else
        {
            timerValueLabel.style.color = new StyleColor(new Color(1f, 0.84f, 0f));
        }
    }
}