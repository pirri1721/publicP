using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Color color;
    public Token[] tokens = new Token[4];
    public int exitSlotIndex = 0;


    public BoardManager bm;
    public Player ally;

    public Token lastTokenUsed;
    // Use this for initialization
    void Start () {

        //TEST
        
        tokens[0] = GameObject.Find("Token").gameObject.GetComponent<Token>();
	}
	
	// Update is called once per frame
	void Update () {

    }

    

    public void CheckMoves(int diceNumb)
    {
        for (int i = 0; i < tokens.Length; i++)
        {
            if (tokens[i].free)
            {
                tokens[i].CheckMove(diceNumb);
            }
        }
    }

    public bool CheckJail()
    {
        for (int i = 0; i < tokens.Length; i++)
        {
            if (!tokens[i].free)
            {
                bm.TokenFree(tokens[i]);
                tokens[i].UpdateCurrentSlot(exitSlotIndex);
                tokens[i].free = true;
                return true;
            }
        }

        return false;
    }

    public void JailToken(int tokenIndex)
    {

    }

    public void SelectToken()
    {
        
    }
}
