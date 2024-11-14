using UnityEngine;
using UnityEngine.UI;

public class MemberTile : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image userpicImage;
    [SerializeField] private TMPro.TextMeshProUGUI memberNameLabel;
    [SerializeField] private TMPro.TextMeshProUGUI playerAddressLabel;
    [SerializeField] private TMPro.TextMeshProUGUI playerLevelLabel;
    [SerializeField] private TMPro.TextMeshProUGUI playerHealthLabel;
    [SerializeField] private TMPro.TextMeshProUGUI playerEnergyLabel;
    private CharacterData characterData;

    private void Awake()
    {
        GetComponent<HorizontalLayoutGroup>().enabled = false;
    }

    public void Init(CharacterData data)
    {

        characterData = data;
        UpdateElements();

        GetComponent<HorizontalLayoutGroup>().enabled = true;
    }

    public void UpdateElements()
    {
        memberNameLabel.text = characterData.name;
        playerAddressLabel.text = characterData.address;
        playerLevelLabel.text = characterData.level + " LVL";
        playerHealthLabel.text = "HP " + characterData.health;
        playerEnergyLabel.text = "NRG " + characterData.energy;
        if (characterData.isDead)
        {
            userpicImage.color = new Color(1, 0, 0);
        }
    }
}
