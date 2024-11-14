using UnityEngine;

public class LobbyManager : SceneController
{
    public void OnLobbyCreated(string transactionResult)
    {
        Debug.Log("OnLobbyCreated");
        Debug.Log(transactionResult);
        LoadScene(3);
    }
    public void OnLobbyJoined(string transactionResult)
    {
        Debug.Log("OnLobbyJoined");
        Debug.Log(transactionResult);
        //LoadScene(4);
    }
    public void OnFightStarted(string transactionResult)
    {
        Debug.Log("OnFightStarted");
        Debug.Log(transactionResult);
        LoadScene(5);
    }
}
