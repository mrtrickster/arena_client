using UnityEngine;

public class Stepper : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent onValueChanged;

    [SerializeField] private UnityEngine.UI.Button decreaseButton;
    [SerializeField] private TMPro.TextMeshProUGUI valueLabel;
    [SerializeField] private UnityEngine.UI.Button increaseButton;
    [SerializeField] private int minValue;
    [SerializeField] private int initValue;
    [SerializeField] private int maxValue;
    [SerializeField] private int curValue;

    private void Start()
    {
        curValue = initValue;
        UpdateElements();
    }

    public int GetValue()
    {
        return curValue;
    }

    public void OnDecreaseButtonClick()
    {
        if (curValue > minValue) curValue--;
        onValueChanged.Invoke();
        UpdateElements();
    }

    public void OIncreaseButtonClick()
    {
        if (curValue < maxValue) curValue++;
        onValueChanged.Invoke();
        UpdateElements();
    }

    private void UpdateElements()
    {
        decreaseButton.interactable = curValue > minValue;
        increaseButton.interactable = curValue < maxValue;
        valueLabel.text = curValue.ToString();
    }
}
