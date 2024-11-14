using Dojo.Starknet;
using System;
using UnityEngine;

public class CharacterCreator : SceneController
{
    [SerializeField] private TMPro.TMP_InputField characterNameInput;
    [SerializeField] private TMPro.TMP_InputField strategyHashInput;
    [SerializeField] private Stepper strengthStepper;
    [SerializeField] private Stepper agilityStepper;
    [SerializeField] private Stepper vitalityStepper;
    [SerializeField] private Stepper staminaStepper;
    [SerializeField] private UnityEngine.UI.Button mintButton;
    [SerializeField] private TopMenu topMenu;

    private CharacterData characterData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        topMenu.DisableButtons();
        characterNameInput.text = "";
        strategyHashInput.text = "0x53673358efd0bed498c39f6726ccbac76281093a653c2522500d7d0481d7085";
        characterData = new();
        UpdateElements();
    }

    private bool IsCharacterNameValid()
    {
        return characterNameInput.text.Length > 3;
    }

    private bool IsStrategyHashValid()
    {
        //Debug.Log(strategyHashInput.text.Length);
        return strategyHashInput.text.Length == 65 && strategyHashInput.text.IndexOf("0x") == 0;
    }

    private bool IsAttributesValid()
    {
        return strengthStepper.GetValue() + agilityStepper.GetValue() + vitalityStepper.GetValue() + staminaStepper.GetValue() == 5;
    }

    public void UpdateElements()
    {
        mintButton.interactable = IsCharacterNameValid() && IsStrategyHashValid() && IsAttributesValid();
    }

    public async void OnMintButtonClick()
    {
        Debug.Log("OnMintButtonClick");

        if (IsCharacterNameValid() && IsStrategyHashValid() && IsAttributesValid())
        {
            characterData.name = characterNameInput.text;
            characterData.strategyHash = strategyHashInput.text;
            characterData.strength = strengthStepper.GetValue();
            characterData.agility = agilityStepper.GetValue();
            characterData.vitality = vitalityStepper.GetValue();
            characterData.stamina = staminaStepper.GetValue();

            Debug.Log(JsonUtility.ToJson( characterData ));
            AppData.character = characterData;

            FieldElement txResult = null;
            try
            {
                //txResult = await DojoService.CreateCharacter();
                DojoService2.CreateCharacter();
            } catch (Exception e)
            {
                Debug.Log(e.Message);
            }

            if (txResult != null)
            {
                Debug.Log("Character is created successfully!");
                LoadScene(2);
            }
        }
    }
}
