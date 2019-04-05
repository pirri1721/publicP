using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchInfo : Singleton<MatchInfo>
{
    public string ole = "string";

    public bool IA;
    public bool allies;
    public PlayerDefinition[] playerDefinitions;

    public struct PlayerDefinition
    {
        public string name_id;
        public Image image;
        public int charIndex;
        public Color color;
        public int colorIndex;
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        Debug.Log(ole);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PlayerDefinition ReturnByColorIndex(int colorIndex)
    {
        for(int i=0; i < playerDefinitions.Length; i++)
        {
            if(playerDefinitions[i].colorIndex == colorIndex)
            {
                return playerDefinitions[i];
            }
        }
        Debug.LogError("look here");
        return playerDefinitions[0];
    }
}
