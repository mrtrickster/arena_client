using SimpleGraphQL;
using System.Threading.Tasks;
using UnityEngine;

public class ToriiService
{
    private static GraphQLClient toriiClient = new GraphQLClient("https://api.cartridge.gg/x/arena-0-2-1-sepolia/torii/graphql");

    public static async Task<string> GetCharacterModelsAll(string playerAddress)
    {
        string query = $@"query {{ arenaCharacterInfoModels ( where: {{ player: ""{playerAddress}"" }} first: 1000 ) {{ totalCount }} }}";
        return await SendToriiRequest(query);
    }

    public static async Task<string> GetCharacterInfoModel(string playerAddress)
    {
        string query = $@"query {{ arenaCharacterInfoModels ( where: {{ player: ""{playerAddress}"" }} first: 1000 ) {{ edges {{ node {{ player name attributes {{ strength agility vitality stamina }} strategy level experience points golds }} }} }} }}";
        return await SendToriiRequest(query);
    }

    public static async Task<string> GetArenaModelsAll()
    {
        string query = $@"query {{ arenaArenaModels ( first: 1000 ) {{ edges {{ node {{ id player name current_tier characters_number winner is_closed red_side_num blue_side_num }} }} }} }}";
        return await SendToriiRequest(query);
    }

    public static async Task<string> GetArenaModel(int arenaId)
    {
        string query = $@"query {{ arenaArenaModels ( where: {{ id: {arenaId} }} first: 1000 ) {{ edges {{ node {{ id player name current_tier characters_number winner is_closed red_side_num blue_side_num }} }} }} }}";
        return await SendToriiRequest(query);
    }

    public static async Task<string> GetArenaCharacters(int arenaId)
    {
        string query = $@"query {{ arenaArenaCharacterModels ( where: {{ arena_id: {arenaId} }} first: 1000 order: {{ field: CID direction: ASC }}) {{ edges {{ node {{ arena_id cid name level hp energy attributes {{ strength agility vitality stamina }} character_owner strategy position {{ x y }} direction action initiative consecutive_rest_count side }} }} }} }}";
        return await SendToriiRequest(query);
    }

    public static async Task<string> GetArenaCharacter(int arenaId, int characterId)
    {
        string query = $@"query {{ arenaArenaCharacterModels ( where: {{ arena_id: {arenaId} cid: {characterId} }} first: 1000 ) {{ edges {{ node {{ arena_id cid name level hp energy attributes {{ strength agility vitality stamina }} character_owner strategy position {{ x y }} direction action initiative consecutive_rest_count side }} }} }} }}";
        return await SendToriiRequest(query);
    }

    public static async Task<string> GetArenaRegistered(string playerAddress, int arenaId)
    {
        string query = $@"query {{ arenaArenaRegisteredModels ( where: {{ character_owner: ""{playerAddress}"" arena_id: {arenaId} }} first: 1000 ) {{ edges {{ node {{ arena_id character_owner registered }} }} }} }}";
        return await SendToriiRequest(query);
    }

    public static async Task<string> GetBattleLogs(int arenaId)
    {
        Debug.Log("GraphQLClient.GetBattleLogs()");
        string query = $@"query {{ eventMessages ( keys: [""0x{arenaId.ToString("X")}""] first: 1000 ) {{ totalCount edges {{ node {{ models {{ ... on arena_BattleLog {{ arena_id turn battle_log }} }} }} }} }} }}";
        Debug.Log(query);
        return await SendToriiRequest(query);
    }

    private static async Task<string> SendToriiRequest(string query)
    {
        return await toriiClient.Send(new Request { Query = query });
    }
}
