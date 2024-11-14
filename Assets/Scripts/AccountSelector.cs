using Dojo.Starknet;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class AccountSelector : SceneController
{
    public List<AccountData> burnerAccounts;
    public string rpcUrl = "https://api.cartridge.gg/x/starknet/sepolia";
    [SerializeField] private TMPro.TMP_Dropdown dropdown;
    [SerializeField] private UnityEngine.UI.Button button;

    [System.Serializable]
    public class JsonData
    {
        public List<AccountData> accounts;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dropdown.ClearOptions();
        AddOptions();
    }

    private async void AddOptions()
    {
        var optionsList = new List<TMPro.TMP_Dropdown.OptionData>();
        var accountsJsonData = await GetJsonDataAsync("https://impulsedao.xyz/warpacks/arena/accounts.json");
        burnerAccounts = accountsJsonData.accounts;
        foreach (var burnerAccount in burnerAccounts)
        {
            optionsList.Add(new TMPro.TMP_Dropdown.OptionData(burnerAccount.address));
        }
        dropdown.options = optionsList;
    }

    private async Task<JsonData> GetJsonDataAsync(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield(); // Await until request completes

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + request.error);
                return null;
            }

            // Decode JSON response
            string jsonResponse = request.downloadHandler.text;
            return JsonUtility.FromJson<JsonData>(jsonResponse);
        }
    }

    public async void OnSelectButtonClick()
    {
        AppData.burnerAccount = burnerAccounts[dropdown.value];
        AppData.account = new Account(new JsonRpcClient(rpcUrl), new SigningKey(AppData.burnerAccount.privateKey), new FieldElement(AppData.burnerAccount.address));
        AppData.dojoData = new DojoData();
        //check if this account already has a character created
        var result = await ToriiService.GetCharacterModelsAll(AppData.burnerAccount.address);
        var responce = JsonUtility.FromJson<PlayerCharactersCountData>(result);
        Debug.Log(responce.data.arenaCharacterInfoModels.totalCount + " characters found");
        //
        if (responce.data.arenaCharacterInfoModels.totalCount == 0)
        {
            //no characters were created for this player, let's create one
            LoadScene(1);
        }
        else
        {
            //this player's character was created, let's show his profile
            LoadScene(2);
        }
        //UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}