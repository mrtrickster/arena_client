using UnityEngine;

[System.Serializable]
public class ArenaLobbiesListData
{
    public Data data;

    [System.Serializable]
    public class Data
    {
        public Models arenaArenaModels;

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
                    public int id;
                    public string player;
                    public string name;
                    public string current_tier;
                    public int characters_number;
                    public string winner;
                    public bool is_closed;
                    public int red_side_num;
                    public int blue_side_num;
                }
            }
        }
    }
}