using UnityEngine;
using UnityEngine.UI;

public class PlayerTile : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI memberNameLabel;
    [SerializeField] private TMPro.TextMeshProUGUI strengthLabel;
    [SerializeField] private TMPro.TextMeshProUGUI agilityLabel;
    [SerializeField] private TMPro.TextMeshProUGUI vitalityLabel;
    [SerializeField] private TMPro.TextMeshProUGUI staminaLabel;
    private CharacterData characterData;

    public async void Init(CharacterData data)
    {
        Debug.Log("PlayerTile.Init()");
        characterData = data;
        GetComponent<ContentSizeFitter>().enabled = false;
        var result = await ToriiService.GetArenaCharacter(AppData.lobby.id, data.id);
        Debug.Log(result);
        var responce = JsonUtility.FromJson<ArenaCharactersListData>(result);
        var node = responce.data.arenaArenaCharacterModels.edges[0].node;
        characterData.id = node.cid;
        characterData.name = StringConverter.DecodeFeltHex(node.name);
        characterData.level = node.level;
        characterData.health = node.hp;
        characterData.energy = node.energy;
        characterData.address = node.character_owner;
        characterData.team = node.side;
        characterData.strength = node.attributes.strength;
        characterData.agility = node.attributes.agility;
        characterData.vitality = node.attributes.vitality;
        characterData.stamina = node.attributes.stamina;
        UpdateElements();
        GetComponent<ContentSizeFitter>().enabled = true;
    }

    public void UpdateElements()
    {
        memberNameLabel.text = characterData.name;
        strengthLabel.text = characterData.strength.ToString();
        agilityLabel.text = characterData.agility.ToString();
        vitalityLabel.text = characterData.vitality.ToString();
        staminaLabel.text = characterData.stamina.ToString();
    }
}
