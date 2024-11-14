using Dojo.Starknet;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LobbySelector : SceneController
{
    [SerializeField] private LobbyTile lobbyTilePrefab;
    [SerializeField] private TMPro.TMP_InputField lobbyNameInput;
    [SerializeField] private TopMenu topMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        topMenu.DisableButtons();
        topMenu.EnableButtons(new int[]{ 1, 3 });
        transform.parent.GetComponent<ContentSizeFitter>().enabled = false;

        var result = await ToriiService.GetArenaModelsAll();
        Debug.Log(result);
        var responce = JsonUtility.FromJson<ArenaLobbiesListData>(result);
        foreach (var edge in responce.data.arenaArenaModels.edges)
        {
            var node = edge.node;
            LobbyData lobbyData = new();
            lobbyData.id = node.id;
            lobbyData.name = StringConverter.DecodeFeltHex( node.name );
            lobbyData.tier = node.current_tier;
            lobbyData.totalPlayers = node.characters_number;
            lobbyData.redPlayers = node.red_side_num;
            lobbyData.bluePlayers = node.blue_side_num;
            lobbyData.winner = StringConverter.DecodeFeltHex(node.winner);

            //check if player already joined this lobby
            bool inLobby = await CheckIfPlayerIsInLobby(lobbyData.id);
            lobbyData.available = !inLobby;

            LobbyTile lobbyTile = Instantiate(lobbyTilePrefab, transform);
            lobbyTile.Init(lobbyData);
        }

        //

        transform.parent.GetComponent<ContentSizeFitter>().enabled = true;
    }

    async Task<bool> CheckIfPlayerIsInLobby(int lobbyId)
    {
        //var result = await ToriiService.GetArenaRegistered(AppData.burnerAccount.address, lobbyId);
        var result = await ToriiService.GetArenaRegistered(AppData.walletAddress, lobbyId);
        Debug.Log(result);
        var responce = JsonUtility.FromJson<ArenaRegisteredData>(result);
        if (responce.data.arenaArenaRegisteredModels.edges.Length == 0) return false;
        return responce.data.arenaArenaRegisteredModels.edges[0].node.registered;
    }

    public void OnCreateLobbyButtonClick()
    {
        if (IsLobbyNameValid())
        {
            LobbyData lobbyData = new();

            lobbyData.name = lobbyNameInput.text;

            Debug.Log(JsonUtility.ToJson(lobbyData));
            AppData.lobby = lobbyData;

            FieldElement txResult = null;
            try
            {
                //txResult = await DojoService2.CreateLobby();
                DojoService2.CreateLobby();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

            if (txResult != null)
            {
                Debug.Log("Lobby is created successfully!");
                LoadScene(3);
            }
        }
    }

    private bool IsLobbyNameValid()
    {
        return lobbyNameInput.text.Length > 3;
    }
}
