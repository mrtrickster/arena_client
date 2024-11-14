using UnityEngine;

public class LobbyTile : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI lobbyNameLabel;
    [SerializeField] private TMPro.TextMeshProUGUI lobbyTierLabel;
    [SerializeField] private TMPro.TextMeshProUGUI redTeamLabel;
    [SerializeField] private TMPro.TextMeshProUGUI blueTeamLabel;
    [SerializeField] private GameObject teamButtonsBlock;
    [SerializeField] private UnityEngine.UI.Button redTeamButton;
    [SerializeField] private UnityEngine.UI.Button blueTeamButton;
    [SerializeField] private UnityEngine.UI.Button enterButton;
    private LobbyData lobbyData;

    public void Init(LobbyData data)
    {
        lobbyData = data;
        UpdateElements();
    }

    public void UpdateElements()
    {
        lobbyNameLabel.text = lobbyData.name;
        lobbyTierLabel.text = lobbyData.tier;
        redTeamLabel.text = lobbyData.redPlayers + " of 3 players";
        blueTeamLabel.text = lobbyData.bluePlayers + " of 3 players";
        Debug.Log("lobby data available: " + lobbyData.available);
        redTeamButton.interactable = lobbyData.redPlayers < 3;
        blueTeamButton.interactable = lobbyData.bluePlayers < 3;
        teamButtonsBlock.SetActive(lobbyData.available);
        enterButton.gameObject.SetActive(!lobbyData.available);
    }

    public async void OnJoinButtonClick(int side)
    {
        Debug.Log(JsonUtility.ToJson( lobbyData ));
        AppData.lobby = lobbyData;
        DojoService2.JoinLobby(side);
        //
        var redPlayers = lobbyData.redPlayers;
        var bluePlayers = lobbyData.bluePlayers;
        int iterations = 0;
        while (lobbyData.redPlayers == redPlayers && lobbyData.bluePlayers == bluePlayers)
        {
            var result = await ToriiService.GetArenaModel(lobbyData.id);
            var responce = JsonUtility.FromJson<ArenaLobbiesListData>(result);
            if (responce.data.arenaArenaModels.edges.Length > 0)
            {
                lobbyData.redPlayers = responce.data.arenaArenaModels.edges[0].node.red_side_num;
                lobbyData.bluePlayers = responce.data.arenaArenaModels.edges[0].node.blue_side_num;
            }
            Debug.Log(lobbyData.redPlayers + " == " + redPlayers + "; " + lobbyData.bluePlayers + " == " + bluePlayers);
            iterations++;
            if (iterations == 30) break;
        }
        lobbyData.available = false;
        //UpdateElements();
        UnityEngine.SceneManagement.SceneManager.LoadScene(4);
    }

    public void OnEnterButtonClick()
    {
        AppData.lobby = lobbyData;
        lobbyData.available = false;
        //UpdateElements();
        UnityEngine.SceneManagement.SceneManager.LoadScene(4);
    }
}
