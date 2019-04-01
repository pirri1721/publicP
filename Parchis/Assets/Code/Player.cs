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
    public CharacterController charController;
    internal List<Barrier> barriers;

    public struct Barrier
    {
        public Token token1;
        public Token token2;
    }

    public Barrier BarrierConstructor(Token tokenA, Token tokenB)
    {
        Barrier barrier = new Barrier();
        barrier.token1 = tokenA;
        barrier.token2 = tokenB;
        return barrier;
    }

    public void AbbBarrier(Token tokenA, Token tokenB)
    {
        barriers.Add(BarrierConstructor(tokenA, tokenB));
    }

    public Token GetOwnTokenFromWall(int indexBarrierQuery)
    {
        Token ownToken;
        Barrier barrierQuery = barriers[indexBarrierQuery];

        if (barrierQuery.token1.player == this)
        {
            ownToken = barrierQuery.token1;
        }
        else
        {
            ownToken = barrierQuery.token2;
        }

        return ownToken;
    }

    // Use this for initialization
    void Start () {

        for (int i = 0; i < this.transform.childCount; i++)
        {
            tokens[i] = transform.GetChild(i).GetComponent<Token>();
            tokens[i].bM = bM;
            tokens[i].player = this;
            tokens[i].Tint();
            tokens[i].tokenIndex = i;
        }

        //Free first token
        tokens[0].UpdateCurrentSlot(exitSlotIndex);
        tokens[0].free = true;
        //bM.FreeToken(tokens[0]);
        tokens[0].FreeToken();


        //charController.gameObject.transform.LookAt(bM.transform);

        charController.gameObject.transform.rotation = Quaternion.LookRotation(bM.transform.position-transform.position,transform.up);
        //TEST

        //tokens[0] = GameObject.Find("Token").gameObject.GetComponent<Token>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public bool CheckMoves(int diceNumb)
    {
        bool movePosibility = false;

        for (int i = 0; i < tokens.Length; i++)
        {
            if (tokens[i].free && !tokens[i].end)
            {
                if (tokens[i].TokenCheckMove(diceNumb))
                {
                    movePosibility = true;
                }
            }
        }

        return movePosibility;
    }

    public bool CheckJail()
    {
        if (AllTokensJailed())
        {
            charController.WakeUp();

            int i = 0;
            bM.FreeToken(tokens[i]);
            tokens[i].UpdateCurrentSlot(exitSlotIndex);
            tokens[i].free = true;

            return true;
        }
        else
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

        if (AllTokensJailed())
        {
            charController.Sit();
        }
    }

    public bool AllTokensFree()
    {
        for(int i =0; i < tokens.Length; i++)
        {
            if (!tokens[i].free) return false;
        }

        return true;
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

    public bool AllTokensEnd()
    {
        for (int i = 0; i < tokens.Length; i++)
        {
            if (!tokens[i].end)
            {
                return false;
            }
        }
        return true;
    }

    public void GetCharacter(CharacterController character)
    {
        charController = character;
        charController.gameObject.transform.position = transform.position;
        charController.gameObject.transform.rotation = Quaternion.LookRotation(bM.transform.position);
    }

}
