using UnityEngine;

public class TopMenu : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button lobbiesButton;
    [SerializeField] private UnityEngine.UI.Button myLogsButton;
    [SerializeField] private UnityEngine.UI.Button leaderboardButton;
    [SerializeField] private UnityEngine.UI.Button myProfileButton;

    public void EnableButton(int index)
    {
        switch (index)
        {
            case 0:
                lobbiesButton.interactable = true;
                break;
            case 1:
                myLogsButton.interactable = true;
                break;
            case 2:
                leaderboardButton.interactable = true;
                break;
            case 3:
                myProfileButton.interactable = true;
                break;
        }
    }


    public void DisableButton(int index)
    {
        switch (index)
        {
            case 0:
                lobbiesButton.interactable = false;
                break;
            case 1:
                myLogsButton.interactable = false;
                break;
            case 2:
                leaderboardButton.interactable = false;
                break;
            case 3:
                myProfileButton.interactable = false;
                break;
        }
    }

    public void EnableButtons(int[] indices)
    {
        foreach (var i in indices)
        {
            EnableButton(i);
        }
    }

    public void DisableButtons(int[] indices)
    {
        foreach (var i in indices)
        {
            DisableButton(i);
        }
    }

    public void DisableButtons()
    {
        for (var i = 0; i < 4; i++)
        {
            DisableButton(i);
        }
    }
}
