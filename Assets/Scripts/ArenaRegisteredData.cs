using UnityEngine;

[System.Serializable]
public class ArenaRegisteredData
{
    public Data data;

    [System.Serializable]
    public class Data
    {
        public Models arenaArenaRegisteredModels;

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
                    public string character_owner;
                    public bool registered;
                }
            }
        }
    }
}
