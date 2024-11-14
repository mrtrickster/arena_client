using UnityEngine;

[System.Serializable]
public class LobbyData
{
    public int id;
    public string name;
    public string tier;
    public int totalPlayers;
    public int redPlayers;
    public int bluePlayers;
    public bool available;
    public string winner;
}
