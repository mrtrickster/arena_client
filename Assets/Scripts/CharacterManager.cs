using UnityEngine;

public class CharacterManager : SceneController
{
    public void OnCharacterCreated(string transactionResult)
    {
        Debug.Log("OnCharacterCreated");
        Debug.Log(transactionResult);
        LoadScene(2);
    }

    public void OnStrategyHashUpdated(string transactionResult)
    {
        Debug.Log("OnStrategyHashUpdated");
    }
}
