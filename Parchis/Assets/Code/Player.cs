using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Color color;
    public Token[] tokens = new Token[4];
    public int exitSlotIndex = 0;


    public BoardManager bM;
    public Player ally;

    public Token lastTokenUsed;
    // Use this for initialization
    void Start () {

        for (int i = 0; i < this.transform.childCount; i++)
        {
            tokens[i] = transform.GetChild(i).GetComponent<Token>();
            tokens[i].bM = bM;
            tokens[i].player = this;
        }

        //Free first token
        tokens[0].UpdateCurrentSlot(exitSlotIndex);
        tokens[0].free = true;
        //bM.FreeToken(tokens[0]);
        tokens[0].FreeToken();
        
        
        //TEST

        //tokens[0] = GameObject.Find("Token").gameObject.GetComponent<Token>();
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
                bM.FreeToken(tokens[i]);
                tokens[i].UpdateCurrentSlot(exitSlotIndex);
                tokens[i].free = true;
                return true;
            }
        }

        return false;
    }

    internal void DisableMoves()
    {
        for (int i=0; i < tokens.Length; i++)
        {
            tokens[i].enabledMove = false;
        }
        
    }

    public void JailToken(int tokenIndex)
    {
        tokens[tokenIndex].ReturnJail();
    }

    public bool AllTokensFree()
    {
        for(int i =0; i < tokens.Length; i++)
        {
            if (!tokens[i].free) return false;
        }

        return true;
    }

    public void SelectToken()
    {
        
    }

    public bool AllTokensJailed()
    {
        for(int i = 0; i < tokens.Length; i++)
        {
            if (tokens[i].free)
            {
                return false;
            }
        }
        return true;
    }
}
