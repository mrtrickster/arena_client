using UnityEngine;

public class SpeedControls : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Slider speedSlider;
    [SerializeField] private TMPro.TextMeshProUGUI speedLabel;
    [SerializeField] private UnityEngine.UI.Button pauseButton;
    [SerializeField] private UnityEngine.UI.Button speedUpButton;

    private void Awake()
    {
        speedSlider.onValueChanged.AddListener(ChangeSpeed);
        pauseButton.onClick.AddListener(ResetSpeed);
        speedUpButton.onClick.AddListener(IncreaseSpeed);
    }

    public void SetSpeed(int value)
    {
        if (speedSlider.value != value) speedSlider.SetValueWithoutNotify(value);
        speedLabel.text = "×" + value;
        AppData.currentSpeed = value;
        Time.timeScale = AppData.currentSpeed;
    }

    private void ResetSpeed()
    {
        SetSpeed(0);
    }

    private void IncreaseSpeed()
    {
        if (speedSlider.value < 5) SetSpeed((int)speedSlider.value + 1);
    }

    private void ChangeSpeed(float value)
    {
        if (value <= 5 && value >= 0) SetSpeed((int)speedSlider.value);
    }
}
