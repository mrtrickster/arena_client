using Dojo.Starknet;
using dojo_bindings;
using System.Threading.Tasks;
using UnityEngine;

public class DojoService2
{
    public static void CreateCharacter()
    {
        Debug.Log("CreateCharacter");
        string characterName = StringConverter.TextToFelt(AppData.character.name).ToString();
        string strategyHash = AppData.character.strategyHash;
        string strength = AppData.character.strength.ToString();
        string agility = AppData.character.agility.ToString();
        string vitality = AppData.character.vitality.ToString();
        string stamina = AppData.character.stamina.ToString();
        string[] calldata = new string[] {
            characterName,
            strength,
            agility,
            vitality,
            stamina,
            strategyHash
        };
        string calldataString = JsonUtility.ToJson(new ArrayWrapper { array = calldata });
        JSInteropManager.SendTransactionArgentX(AppData.dojoData.contractAddress, "createCharacter", calldataString, "CharacterManager", "OnCharacterCreated");
    }

    public static void CreateLobby()
    {
        Debug.Log("CreateLobby " + AppData.lobby.name);
        string lobbyName = StringConverter.TextToFelt(AppData.lobby.name).ToString();
        string[] calldata = new string[] {
            lobbyName
        };
        string calldataString = JsonUtility.ToJson(new ArrayWrapper { array = calldata });
        JSInteropManager.SendTransactionArgentX(AppData.dojoData.contractAddress, "createArena", calldataString, "LobbyManager", "OnLobbyCreated");
    }

    public static void JoinLobby(int side)
    {
        Debug.Log("JoinLobby " + AppData.lobby.id + " on " + side + " side");
        string lobbyId = AppData.lobby.id.ToString();
        string team = side.ToString();
        string[] calldata = new string[] {
            lobbyId,
            team
        };
        string calldataString = JsonUtility.ToJson(new ArrayWrapper { array = calldata });
        JSInteropManager.SendTransactionArgentX(AppData.dojoData.contractAddress, "register", calldataString, "LobbyManager", "OnLobbyJoined");
    }

    public static void UpdateStrategyHash()
    {
        Debug.Log("UpdateStrategyHash() " + AppData.character.strategyHash);
        string[] calldata = new string[] {
            AppData.character.strategyHash
        };
        string calldataString = JsonUtility.ToJson(new ArrayWrapper { array = calldata });
        JSInteropManager.SendTransactionArgentX(AppData.dojoData.contractAddress, "update_strategy", calldataString, "CharacterManager", "OnStrategyHashUpdated");
    }

    public static void Play()
    {
        Debug.Log("Play() " + AppData.lobby.id);
        string[] calldata = new string[] {
            AppData.lobby.id.ToString()
        };
        string calldataString = JsonUtility.ToJson(new ArrayWrapper { array = calldata });
        JSInteropManager.SendTransactionArgentX(AppData.dojoData.playContractAddress, "play", calldataString, "LobbyManager", "OnFightStarted");
    }

    public static void Close()
    {
        Debug.Log("Close() " + AppData.lobby.id);
        string[] calldata = new string[] {
            AppData.lobby.id.ToString()
        };
        string calldataString = JsonUtility.ToJson(new ArrayWrapper { array = calldata });
        JSInteropManager.SendTransactionArgentX(AppData.dojoData.contractAddress, "closeArena", calldataString, "LobbyManager", "OnArenaClosed");
    }

    public static async Task<FieldElement> Play(int arenaId)
    {
        Debug.Log($"starting arena {arenaId.ToString("x")} by player " + AppData.account.Address.Hex());
        return await AppData.account.ExecuteRaw(new dojo.Call[] {
            new dojo.Call{
                to = AppData.dojoData.playContractAddress,
                selector = "play",
                calldata = new dojo.FieldElement[] {
                    new FieldElement(arenaId.ToString("x")).Inner
                }
            }
        });
    }

    public static async Task<FieldElement> PlayAndClose(int arenaId)
    {
        Debug.Log($"starting arena {arenaId} by player " + AppData.account.Address.Hex());
        return await AppData.account.ExecuteRaw(new dojo.Call[] {
            new dojo.Call{
                to = AppData.dojoData.playContractAddress,
                selector = "play",
                calldata = new dojo.FieldElement[] {
                    new FieldElement(arenaId.ToString("x")).Inner
                }
            },
            new dojo.Call
            {
                to = AppData.dojoData.contractAddress,
                selector = "closeArena",
                calldata = new dojo.FieldElement[] {
                    new FieldElement(arenaId.ToString("x")).Inner
                }
            }
        });
    }
}
