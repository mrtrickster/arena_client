using UnityEngine;

[System.Serializable]
public class PlayerCharactersCountData
{
    public Data data;

    [System.Serializable]
    public class Data
    {
        public Models arenaCharacterInfoModels;

        [System.Serializable]
        public class Models
        {
            public int totalCount;
        }
    }
}
