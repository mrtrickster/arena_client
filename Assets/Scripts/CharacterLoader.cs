using UnityEngine;

public class CharacterLoader : SceneController
{
    [SerializeField] private TMPro.TextMeshProUGUI characterNameLabel;
    [SerializeField] private TMPro.TextMeshProUGUI strategyHashLabel;
    [SerializeField] private TMPro.TMP_InputField strategyHashInput;
    [SerializeField] private TMPro.TextMeshProUGUI strengthLabel;
    [SerializeField] private TMPro.TextMeshProUGUI agilityLabel;
    [SerializeField] private TMPro.TextMeshProUGUI vitalityLabel;
    [SerializeField] private TMPro.TextMeshProUGUI staminaLabel;
    [SerializeField] private UnityEngine.UI.Button changeButton;
    [SerializeField] private UnityEngine.UI.Button editButton;
    [SerializeField] private TopMenu topMenu;

    private CharacterData characterData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        topMenu.DisableButtons();
        topMenu.EnableButton(0);
        //var result = await ToriiService.GetCharacterInfoModel(AppData.burnerAccount.address);
        var result = await ToriiService.GetCharacterInfoModel(AppData.walletAddress);
        Debug.Log(result);
        var responce = JsonUtility.FromJson<PlayerCharacterInfoData>(result);
        var node = responce.data.arenaCharacterInfoModels.edges[0].node;
        characterData = new();
        characterData.name = StringConverter.DecodeFeltHex( node.name );
        characterData.strategyHash = node.strategy;
        characterData.strength = node.attributes.strength;
        characterData.agility = node.attributes.agility;
        characterData.vitality = node.attributes.vitality;
        characterData.stamina = node.attributes.stamina;
        AppData.character = characterData;
        UpdateElements();
    }

    public void UpdateElements()
    {
        characterNameLabel.text = characterData.name;
        strategyHashLabel.text = characterData.strategyHash;
        strengthLabel.text = characterData.strength.ToString();
        agilityLabel.text = characterData.agility.ToString();
        vitalityLabel.text = characterData.vitality.ToString();
        staminaLabel.text = characterData.stamina.ToString();
    }

    public void OnChangeStrategyButtonClick()
    {
        strategyHashLabel.transform.parent.gameObject.SetActive(false);
        strategyHashInput.transform.parent.gameObject.SetActive(true);
    }

    public void OnStrategyHashChange()
    {
        if (IsStrategyHashValid())
        {
            var hash = strategyHashInput.text;
            AppData.character.strategyHash = hash;
            DojoService2.UpdateStrategyHash();

            strategyHashLabel.text = hash;
            strategyHashLabel.transform.parent.gameObject.SetActive(true);
            strategyHashInput.transform.parent.gameObject.SetActive(false);
        }
    }

    private bool IsStrategyHashValid()
    {
        //Debug.Log(strategyHashInput.text.Length);
        return strategyHashInput.text.Length > 64 && strategyHashInput.text.IndexOf("0x") == 0;
    }
}
