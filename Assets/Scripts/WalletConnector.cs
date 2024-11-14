using System;
using System.Collections;
using UnityEngine;

public class WalletConnector : SceneController
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bool available = JSInteropManager.IsWalletAvailable();
        if (!available)
        {
            JSInteropManager.AskToInstallWallet();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnButtonConnectWalletArgentX()
    {
        StartCoroutine(ConnectWalletAsync(JSInteropManager.ConnectWalletArgentX));
    }

    public void OnButtonConnectWalletBraavos()
    {
        StartCoroutine(ConnectWalletAsync(JSInteropManager.ConnectWalletBraavos));
    }
    private IEnumerator ConnectWalletAsync(Action connectWalletFunction)
    {
        // Call the JavaScript method to connect the wallet
        connectWalletFunction();

        // Wait for the connection to be established
        yield return new WaitUntil(() => JSInteropManager.IsConnected());

        AppData.walletAddress = JSInteropManager.GetAccount();
        Debug.Log("Connected to wallet: " + AppData.walletAddress);

        OnWalletConnected();
    }

    public async void OnWalletConnected()
    {
        AppData.dojoData = new DojoData();
        //check if this account already has a character created
        var result = await ToriiService.GetCharacterModelsAll(AppData.walletAddress);
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
