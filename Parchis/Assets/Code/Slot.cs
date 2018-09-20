using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        if(tokens.Count == 2)
        {
            StartCoroutine(AdjustTokens());
        }
    }

    public void RemovingToken(Token token)
    {

        if(tokens.Count == 2)
        {
            if(tokens[0] == token)
            {
                tokens[1].transform.DOMoveX(this.transform.position.x,0.5f);
            }
            else
            {
                tokens[1].transform.DOMoveX(this.transform.position.x, 0.5f);
            }
        }
        tokens.Remove(token);
        
    }

    public IEnumerator AdjustTokens()
    {
        yield return new WaitForSeconds(0.5f);

        Transform firstToken = tokens[0].transform;
        Transform secondToken = tokens[1].transform;

        firstToken.SetParent(this.transform);
        secondToken.SetParent(this.transform);
        /*
        firstToken.rotation = this.transform.rotation;
        secondToken.rotation = this.transform.rotation;
        yield return new WaitForEndOfFrame();
        */
        if(this.transform.rotation.y >  0)
        {
            firstToken.DOLocalMoveZ(this.transform.position.z - 0.2f, 0.5f);
            secondToken.DOLocalMoveZ(this.transform.position.z + 0.2f, 0.5f);
        }
        else
        {
            firstToken.DOLocalMoveX(this.transform.position.x - 0.2f, 0.5f);
            secondToken.DOLocalMoveX(this.transform.position.x + 0.2f, 0.5f);
        }

        firstToken.SetParent(null);
        secondToken.SetParent(null);
    }

}
