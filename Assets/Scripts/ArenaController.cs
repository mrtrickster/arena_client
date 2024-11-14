using UnityEngine;
using static ArenaCharactersListData.Data.Models.Edge;

public class ArenaController : SceneController
{
    [SerializeField] private TMPro.TextMeshProUGUI arenaNameLabel;
    [SerializeField] private MemberTile memberTilePrefab;
    [SerializeField] private Transform redTeamBlock;
    [SerializeField] private Transform blueTeamBlock;
    [SerializeField] private TMPro.TextMeshProUGUI statusTextLabel;
    [SerializeField] private UnityEngine.UI.Button startBattleButton;
    [SerializeField] private TopMenu topMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        Debug.Log("ArenaController.Start()");
        topMenu.DisableButtons();
        topMenu.EnableButtons(new int[] { 0, 3 });
        arenaNameLabel.text = AppData.lobby.name;
        var result = await ToriiService.GetArenaCharacters(AppData.lobby.id);
        Debug.Log(result);
        var responce = JsonUtility.FromJson<ArenaCharactersListData>(result);
        AppData.lobby.totalPlayers = responce.data.arenaArenaCharacterModels.edges.Length;
        foreach (var edge in responce.data.arenaArenaCharacterModels.edges)
        {
            CreateMemberTile(edge.node);
        }
        UpdateElements();
    }

    private void CreateMemberTile(Node node)
    {
        Debug.Log("ArenaController.CreateMemberTile()");
        CharacterData characterData = new();
        characterData.id = node.cid;
        characterData.name = StringConverter.DecodeFeltHex(node.name);
        characterData.level = node.level;
        characterData.health = node.hp;
        characterData.energy = node.energy;
        characterData.address = node.character_owner;
        characterData.team = node.side;
        MemberTile memberTile = Instantiate(memberTilePrefab, characterData.team == "Red" ? redTeamBlock : blueTeamBlock);
        memberTile.Init(characterData);
    }

    private void UpdateElements()
    {
        statusTextLabel.gameObject.SetActive(AppData.lobby.totalPlayers < 6);
        startBattleButton.gameObject.SetActive(AppData.lobby.totalPlayers == 6);
        startBattleButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = AppData.lobby.winner == "0" ? "Start battle!" : "View battle";
    }

    public async void OnStartBattleButtonClick()
    {
        //check if battle is already started by someone (or closed)
        var result = await ToriiService.GetArenaModel(AppData.lobby.id);
        Debug.Log(result);
        var responce = JsonUtility.FromJson<ArenaLobbiesListData>(result);
        var node = responce.data.arenaArenaModels.edges[0].node;
        if (node.characters_number == 6)
        {
            if (node.is_closed == false)
            {
                //seems like battle is not started yet
                //let's start and then close it
                if (node.winner == "0x0")
                {
                    DojoService2.Play();
                }
                else
                {
                    LoadScene(5);
                }
            }
        }
    }
}
