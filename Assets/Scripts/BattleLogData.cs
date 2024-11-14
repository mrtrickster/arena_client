using UnityEngine;

[System.Serializable]
public class BattleLogData
{
    public Data data;

    [System.Serializable]
    public class Data
    {
        public Messages eventMessages;

        [System.Serializable]
        public class Messages
        {
            public Edge[] edges;

            [System.Serializable]
            public class Edge
            {
                public Node node;

                [System.Serializable]
                public class Node
                {
                    public Model[] models;

                    [System.Serializable]
                    public class Model
                    {
                        public int arena_id;
                        public int turn;
                        public int[] battle_log;
                    }
                }
            }
        }
    }
}
