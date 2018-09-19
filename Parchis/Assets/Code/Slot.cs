using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour {

    public int index;

    public bool safe;
    public Color specialColor;
    public bool specialMove;
    public int specialIndex;
    //pre-rules
    //rules

    //public Token[] tokens = new Token[2];
    public List<Token> tokens;
    public BoardManager bM;

	// Use this for initialization
	void Awake () {
        tokens = new List<Token>(2);
        //Debug.Log(tokens.Count);
    }

    public bool IsAvaible()
    {
        if (tokens.Count == 2)
        {
            return false;
        }
        else return true;
    }

    public bool ThisSlotAvaible(int followMove)
    {
        if(tokens.Count == 2)
        {
            return false;
        }

        /*
        if(tokens[0] != null && tokens[1] != null)
        {
            return false;
        }*/

        if(followMove > 0)
        {
            if (specialMove)
            {
                    bM.CheckMove(specialIndex, followMove - 1);
            }
            else
            bM.CheckMove(index,followMove - 1);
        }
        
        //pre-rules
        //rules
        //return bool?

        return true;
    }

    internal int NextIndex(Token thisToken)
    {
        if (specialMove && thisToken.color == specialColor)
        {
            return specialIndex;
        }
        else return index + 1;
    }

    public void AddingToken(Token token)
    {
        tokens.Add(token);
        //add check
    }

    public void RemovingToken(Token token)
    {
        tokens.Remove(token);
        //add check
    }

}
