using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class BattleController : SceneController
{
    [SerializeField] private PlayerTile playerTilePrefab;
    [SerializeField] private UnitController redUnitPrefab;
    [SerializeField] private UnitController blueUnitPrefab;
    [SerializeField] private Transform redTeamBlock;
    [SerializeField] private Transform battleLogsBlock;
    [SerializeField] private TMPro.TextMeshProUGUI battleLogsLabel;
    [SerializeField] private Transform blueTeamBlock;
    [SerializeField] private Transform battleField;
    [SerializeField] private ResultWindow resultWindow;
    [SerializeField] private TopMenu topMenu;
    [SerializeField] private List<UnitController> unitsList;
    [SerializeField] private List<PlayerTile> tilesList;

    [SerializeField] private UnityEngine.UI.Button showLogsBlockButton;
    [SerializeField] private UnityEngine.UI.Button hideLogsBlockButton;

    private List<List<int>> battleLog;
    private int currentTurn = 0;
    private List<List<int>> turnList;
    private int currentStep = 0;

    private List<string> textColors = new List<string>() { "red", "blue" };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        Debug.Log("BattleController.Start()");
        topMenu.DisableButtons();
        battleLogsLabel.text = "Starting the battle\n";
        unitsList = new();
        tilesList = new();
        await GetArenaInfo();
        var result = await ToriiService.GetArenaCharacters(AppData.lobby.id);
        var response = JsonUtility.FromJson<ArenaCharactersListData>(result);
        int index = 0;
        foreach (var edge in response.data.arenaArenaCharacterModels.edges)
        {
            var node = edge.node;
            CharacterData characterData = new();
            characterData.id = node.cid;
            characterData.name = StringConverter.DecodeFeltHex(node.name);
            characterData.level = node.level;
            characterData.health = node.hp;
            characterData.energy = node.energy;
            characterData.initHealth = node.hp;
            characterData.initEnergy = node.energy;
            characterData.address = node.character_owner;
            characterData.team = node.side;
            characterData.position = new Vector3Int(node.position.x, 0, node.position.y);
            characterData.rotation = node.direction;
            UnitController unit = Instantiate(characterData.team == "Red" ? redUnitPrefab : blueUnitPrefab, battleField);
            unitsList.Add(unit);
            unit.team = characterData.team;
            unit.unitName = characterData.name;
            unit.address = characterData.address;
            unit.initHealth = characterData.initHealth;
            unit.initEnergy = characterData.initEnergy;
            unit.SetHealth(characterData.health);
            unit.SetEnergy(characterData.energy);
            unit.SetPosition(characterData.position);
            unit.lastPosition = characterData.position;
            unit.SetDirection(characterData.rotation);
            PlayerTile playerTile = Instantiate(playerTilePrefab, characterData.team == "Red" ? redTeamBlock : blueTeamBlock);
            tilesList.Add(playerTile);
            playerTile.Init(characterData);
            index++;
        }
        await GetBattleLogs();
    }

    private async Task GetArenaInfo()
    {
        var result = await ToriiService.GetArenaModel(AppData.lobby.id);
        Debug.Log(result);
        var responce = JsonUtility.FromJson<ArenaLobbiesListData>(result);
        var node = responce.data.arenaArenaModels.edges[0].node;
        AppData.lobby.winner = StringConverter.DecodeFeltHex(node.winner);
        Debug.Log("and the winner is... " + AppData.lobby.winner);
    }

    private async Task GetBattleLogs()
    {
        Debug.Log("BattleController.GetBattleLogs()");
        var result = await ToriiService.GetBattleLogs(AppData.lobby.id);
        var response = JsonUtility.FromJson<BattleLogData>(result);
        if (response.data.eventMessages.edges.Length > 0)
        {
            battleLog = new();
            foreach (var edge in response.data.eventMessages.edges)
            {
                var node = edge.node;
                if (node.models[0].arena_id == AppData.lobby.id)
                {
                    battleLog.Add(node.models[0].battle_log.ToList());
                }
            }
            battleLog.Reverse();
            foreach (List<int> turnLog in battleLog)
            {
                Debug.Log("turn log: " + string.Join(", ", turnLog));
            }
            //
            PlayTurn(battleLog[currentTurn]);
        } else
        {
            Debug.LogError("No battle logs found in response, check Torii, please");
        }
    }

    public void PlayTurn(List<int> turnLog)
    {
        Debug.Log("BattleController.PlayTurn()");
        battleLogsLabel.text += $"Turn #{currentTurn + 1}\n";
        turnList = GetTurnList(turnLog, 8);
        currentStep = 0;
        PlayTurnStep(turnList[currentStep]);
    }

    private List<List<int>> GetTurnList(List<int> input, int chunkSize)
    {
        List<List<int>> result = new();
        for (int i = 0; i < input.Count; i += chunkSize)
        {
            // Create a new list containing the next `chunkSize` elements
            List<int> chunk = input.GetRange(i, Mathf.Min(chunkSize, input.Count - i));
            result.Add(chunk);
        }
        return result;
    }

    private void PlayTurnStep(List<int> turnStepData)
    {
        var id = turnStepData[0];
        var action = turnStepData[1];
        var direction = turnStepData[2];
        var health = turnStepData[3];
        var energy = turnStepData[4];
        var position = new Vector3Int(turnStepData[5], 0, turnStepData[6]);
        Debug.Log("playing turn " + currentTurn + " step " + currentStep + ": " + string.Join(", ", turnStepData));
        var unit = unitsList[id - 1];
        unit.SetDirection(direction);
        string unitStatus = "<color=\"" + unit.team.ToLower() +"\">" + unit.unitName + "</color>";
        bool statusSet = false;
        if (health == 0)
        {
            var tile = tilesList[id - 1];
            tile.GetComponent<CanvasGroup>().alpha = 0.3f;
        }
        switch (action)
        {
            case 0:
                //being beaten
                unitStatus += $" is being hit";
                statusSet = true;
                unit.SetAction(action);
                break;
            case 1:
                //quick attack
                unitStatus += $" quick-attacks";
                statusSet = true;
                unit.SetAction(action);
                break;
            case 2:
                //precise attack
                unitStatus += $" attacks precisely";
                statusSet = true;
                unit.SetAction(action);
                break;
            case 3:
                //heavy attack
                unitStatus += $" attacks heavily";
                statusSet = true;
                unit.SetAction(action);
                break;
            case 4:
                //move
                Debug.Log("unit #" + id + " is moving from " + string.Join(", ", unit.lastPosition) + " to " + string.Join(", ", position));
                if (position != unit.lastPosition)
                {
                    unitStatus += $" is moving {UnitController.Directions[direction]}";
                    statusSet = true;
                    unit.SetAction(action);
                    unit.lastPosition = position;
                }
                break;
            case 5:
                //rest
                unitStatus += $" is taking a rest";
                statusSet = true;
                unit.SetAction(action);
                break;
        }

        var healhDelta = health - unit.lastHealth;
        if (healhDelta > 0) unitStatus += $" gaining {healhDelta} hp";
        if (healhDelta < 0) unitStatus += $" losing {healhDelta} hp";
        unit.SetHealth(health);

        var energyDelta = energy - unit.lastEnergy;

        if (healhDelta != 0 && energyDelta != 0) unitStatus += " and";

        if (energyDelta > 0) unitStatus += $" gaining {energyDelta} nrg";
        if (energyDelta < 0) unitStatus += $" losing {energyDelta} nrg";
        unit.SetEnergy(energy);

        if (statusSet)
        {
            battleLogsLabel.text += unitStatus + "\n";
        }
        unit.WaitFinishStep();
        Invoke("PlayNextStep", 1f);
    }

    private void PlayNextStep()
    {
        if (currentStep < turnList.Count - 1)
        {
            currentStep++;
            PlayTurnStep(turnList[currentStep]);
        } else
        {
            currentStep = 0;
            if (currentTurn < battleLog.Count - 1)
            {
                currentTurn++;
                battleLogsLabel.text += "\n";
                PlayTurn(battleLog[currentTurn]);
            } else
            {
                battleLogsLabel.text += "\nThe battle is complete!\n\n";
                Debug.Log("battle complete!");
                
                resultWindow.gameObject.SetActive(true);
                resultWindow.UpdateUnits(unitsList);
                topMenu.EnableButtons(new int[] { 0, 3 });
            }
        }
    }

    public void ShowLogsBlock()
    {
        
    }

    public void HideLogsBlock()
    {

    }
}
