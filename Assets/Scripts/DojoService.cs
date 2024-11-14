using Dojo.Starknet;
using dojo_bindings;
using System.Threading.Tasks;
using UnityEngine;

public class DojoService
{
    public static async Task<FieldElement> CreateCharacter()
    {
        Debug.Log($"creating character {AppData.character.name} for player " + AppData.account.Address.Hex());
        //if (AppData.account == null) Debug.Log("no account created");
        return await AppData.account.ExecuteRaw(new dojo.Call[] {
            new dojo.Call{
                to = AppData.dojoData.contractAddress,
                selector = "createCharacter",
                calldata = new dojo.FieldElement[] {
                    new FieldElement(StringConverter.EncodeToHex(AppData.character.name)).Inner,
                    new FieldElement(AppData.character.strength.ToString("x")).Inner,
                    new FieldElement(AppData.character.agility.ToString("x")).Inner,
                    new FieldElement(AppData.character.vitality.ToString("x")).Inner,
                    new FieldElement(AppData.character.stamina.ToString("x")).Inner,
                    new FieldElement(AppData.character.strategyHash).Inner
                }
            }
        });
    }

    public static void CreateCharacter(bool useWallet)
    {
        Debug.Log("CreateCharacter");
        string characterName = StringConverter.StringToFelt(AppData.character.name);
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
        Debug.Log("SendTransactionArgentX");
        JSInteropManager.SendTransactionArgentX(AppData.dojoData.contractAddress, "createCharacter", calldataString, "CharacterManager", "OnCharacterCreated");
    }

    public static async Task<FieldElement> CreateLobby(string lobbyName)
    {
        Debug.Log($"creating lobby {lobbyName} for player " + AppData.account.Address.Hex());
        return await AppData.account.ExecuteRaw(new dojo.Call[] {
            new dojo.Call{
                to = AppData.dojoData.contractAddress,
                selector = "createArena",
                calldata = new dojo.FieldElement[] {
                    new FieldElement(StringConverter.EncodeToHex(lobbyName)).Inner
                }
            }
        });
    }

    public static async Task<FieldElement> JoinLobby(int lobbyId, int side)
    {
        Debug.Log($"joining lobby {lobbyId} on {side} side by player " + AppData.account.Address.Hex());
        return await AppData.account.ExecuteRaw(new dojo.Call[] {
            new dojo.Call{
                to = AppData.dojoData.contractAddress,
                selector = "register",
                calldata = new dojo.FieldElement[] {
                    new FieldElement(lobbyId.ToString("x")).Inner,
                    new FieldElement(side.ToString("x")).Inner
                }
            }
        });
    }

    public static async Task<FieldElement> UpdateStrategyHash()
    {
        Debug.Log($"updating strategy hash for player " + AppData.account.Address.Hex());
        return await AppData.account.ExecuteRaw(new dojo.Call[] {
            new dojo.Call{
                to = AppData.dojoData.contractAddress,
                selector = "update_strategy",
                calldata = new dojo.FieldElement[] {
                    new FieldElement(AppData.character.strategyHash).Inner
                }
            }
        });
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
