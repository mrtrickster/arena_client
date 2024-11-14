using UnityEngine;

[System.Serializable]
public class PlayerCharacterInfoData
{
    public Data data;

    [System.Serializable]
    public class Data
    {
        public Models arenaCharacterInfoModels;

        [System.Serializable]
        public class Models
        {
            public Edge[] edges;

            [System.Serializable]
            public class Edge
            {
                public Node node;

                [System.Serializable]
                public class Node
                {
                    public string player;
                    public string name;
                    public Attributes attributes;
                    public string strategy;

                    [System.Serializable]
                    public class Attributes
                    {
                        public int strength;
                        public int agility;
                        public int vitality;
                        public int stamina;
                    }
                }
            }
        }
    }
}
