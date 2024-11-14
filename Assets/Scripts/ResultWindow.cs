using System.Collections.Generic;
using UnityEngine;
using static ArenaCharactersListData.Data.Models.Edge;

public class ResultWindow : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI arenaNameLabel;
    [SerializeField] private MemberTile memberTilePrefab;
    [SerializeField] private Transform redTeamBlock;
    [SerializeField] private Transform blueTeamBlock;
    [SerializeField] private TMPro.TextMeshProUGUI statusTextLabel;
    [SerializeField] private UnityEngine.UI.Button closeBattleButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        Debug.Log("ResultWindow.Start()");
        arenaNameLabel.text = AppData.lobby.name;
        /*
        var result = await ToriiService.GetArenaCharacters(AppData.lobby.id);
        Debug.Log(result);
        var responce = JsonUtility.FromJson<ArenaCharactersListData>(result);
        AppData.lobby.totalPlayers = responce.data.arenaArenaCharacterModels.edges.Length;
        foreach (var edge in responce.data.arenaArenaCharacterModels.edges)
        {
            CreateMemberTile(edge.node);
        }
        */
        UpdateElements();
    }

    public void UpdateUnits(List<UnitController> units)
    {
        foreach (var unit in units)
        {
            CreateMemberTile(unit);
        }
    }

    private void CreateMemberTile(UnitController unit)
    {
        Debug.Log("ResultWindow.CreateMemberTile()");
        CharacterData characterData = new();
        characterData.name = unit.unitName;
        characterData.address = unit.address;
        characterData.isDead = unit.dead;
        characterData.team = unit.team;
        characterData.health = unit.lastHealth;
        characterData.energy = unit.lastEnergy;
        MemberTile memberTile = Instantiate(memberTilePrefab, characterData.team == "Red" ? redTeamBlock : blueTeamBlock);
        memberTile.Init(characterData);
    }

    private void UpdateElements()
    {
        string resultText = "";
        switch (AppData.lobby.winner)
        {
            case "red":
                resultText = "Red team wins!";
                redTeamBlock.GetComponent<CanvasGroup>().alpha = 1;
                blueTeamBlock.GetComponent<CanvasGroup>().alpha = 0.3f;
                break;
            case "blue":
                resultText = "Blue team wins!";
                redTeamBlock.GetComponent<CanvasGroup>().alpha = 0.3f;
                blueTeamBlock.GetComponent<CanvasGroup>().alpha = 1;
                break;
            case "tie":
                resultText = "It's a tie!";
                redTeamBlock.GetComponent<CanvasGroup>().alpha = 1;
                blueTeamBlock.GetComponent<CanvasGroup>().alpha = 1;
                break;
        }
        statusTextLabel.text = resultText;
    }
}
