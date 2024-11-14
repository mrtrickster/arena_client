using UnityEngine;

[System.Serializable]
public class ArenaCharactersListData
{
    public Data data;

    [System.Serializable]
    public class Data
    {
        public Models arenaArenaCharacterModels;

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
                    public int arena_id;
                    public int cid;
                    public string name;
                    public int level;
                    public int hp;
                    public int energy;
                    public Attributes attributes;
                    public string character_owner;
                    public string strategy;
                    public Position position;
                    public string direction;
                    public string action;
                    public int initiative;
                    public int consecutive_rest_count;
                    public string side;

                    [System.Serializable]
                    public class Attributes
                    {
                        public int strength;
                        public int agility;
                        public int vitality;
                        public int stamina;
                    }

                    [System.Serializable]
                    public class Position
                    {
                        public int x;
                        public int y;
                    }
                }
            }
        }
    }
}
