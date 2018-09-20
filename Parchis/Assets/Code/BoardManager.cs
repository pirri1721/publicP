using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoardManager : MonoBehaviour {

    private Player[] players = new Player[4];
    //HookeableRules

    private Player currentPlayer;
    private int currentPlayerIndex = 0;

    private GameObject SlotsGO;
    public Slot[] slots;



    private bool diceUsed = false;
    private bool repeatTurn = false;
    private int repeatedTurns = 0;
    private bool killedToken = false;
    private Token lastTokenUsed;

    // Use this for initialization
    void Start () {
        
        //Slots
        SlotsGO = GameObject.Find("Slots").gameObject;
        int slotsN = SlotsGO.transform.childCount;
        slots = new Slot[slotsN];

        for(int i = 0; i < slotsN; i++)
        {
            slots[i] = SlotsGO.transform.GetChild(i).gameObject.AddComponent<Slot>();
            slots[i].index = i;
            slots[i].bM = this;
        }
        //Add key slot rules
        //add safes

        for(int i=0; i<slotsN; i++)
        {
            if(i == 4 ||
                i == 11 ||
                i == 16 ||
                i == 21 ||
                i == 28 ||
                i == 33 ||
                i == 38 ||
                i == 45 ||
                i == 50 ||
                i == 55 ||
                i == 62 ||
                i == 67 )
            {
                slots[i].safe = true;
            }
        }

        //add exitSlotsIndex - currently InEditor managed



        //Players
        for(int i = 0; i < this.transform.childCount; i++)
        {
            players[i] = transform.GetChild(i).GetComponent<Player>();
            players[i].bM = this;
        }
        currentPlayer = players[0];
        currentPlayerIndex = 0;

        //TEST
        /*
        Token token = GameObject.Find("Token").gameObject.GetComponent<Token>();
        token.bM = this;
        */
	}

    internal void FreeToken(Token token)
    {
        slots[currentPlayer.exitSlotIndex].AddingToken(token);
        token.gameObject.transform.DOMove(slots[currentPlayer.exitSlotIndex].transform.position, 0.5f);
    }

    public bool CheckMove(int slot, int diceNumb)
    {

        if (!slots[slot+1].ThisSlotAvaible(diceNumb))
        {
            return false;
        }

        return true;
    }

    public void MoveToken(Token token, int amount)
    {
        int index = token.currentSlot;
        slots[index].RemovingToken(token);


        float time = 1.0f / amount;
        Sequence moveSequence = DOTween.Sequence();

        for (int i = 0; i < amount; i++)
        {
            //TWEEN
            /*
            if (i == 0)
            {
                moveSequence.Append(token.gameObject.transform.DOMove(slots[index].transform.position, time));
            }
            */

            index = slots[index].NextIndex(token);

            moveSequence.Append(token.gameObject.transform.DOMove(slots[index].transform.position, time));
            
        }
        //token.currentSlot = index;
        slots[index].AddingToken(token);
        moveSequence.Play<Sequence>();
        currentPlayer.lastTokenUsed = token;
        lastTokenUsed = token;

        if (CheckEats(token, index))
        {

        }
        else
        {
            NextTurn();
            
        }
        
        
    }

    private bool CheckEats(Token token, int index)
    {
        Slot currentSlot = slots[index];
        if (currentSlot.safe)
        {
            token.currentSlot = index;
            return false;
        }
        else
        {
            if (currentSlot.tokens[0] != null)
            {
                Token currentToken = currentSlot.tokens[0];
                if (!token.IsAlly(currentToken))
                {
                    //DeadAnimation
                    //remove token eated
                    currentToken.player.JailToken(currentToken.tokenIndex);
                    currentSlot.RemovingToken(currentToken);

                    //make eaten move

                    token.currentSlot = index;
                    currentSlot.AddingToken(token);
                    currentPlayer.CheckMoves(20);
                    return true;
                }
            }

            token.currentSlot = index;
            return false;
        }
        
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!diceUsed)
            {
                ThrowDice();
            }
        }

        //test
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (!diceUsed)
            {
                ThrowDice(5);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (!diceUsed)
            {
                ThrowDice(6);
            }
        }
    }

    public void ThrowDice()
    {
        diceUsed = true;
        repeatTurn = false;

        int diceNumb = UnityEngine.Random.Range(1, 7);

        Debug.Log(diceNumb);
        if (diceNumb == 5)
        {
            if (slots[currentPlayer.exitSlotIndex].IsAvaible())
            {
                //TODO
                //CheckForEnemies
                if (currentPlayer.CheckJail())
                {
                    
                    //slots[currentPlayer.exitSlotIndex].AddingToken()
                    NextTurn();
                }
            }
        }
        if (diceNumb == 6)
        {
            RepeatTurn();

            //If all tokens free --> diceNumb = 7
            if (currentPlayer.AllTokensFree()) diceNumb = 7;
        }

        if (killedToken)
        {
            killedToken = false;
            NextTurn();
        }
        else
        {
            currentPlayer.CheckMoves(diceNumb);
        }

        
    }

    //testingDice
    public void ThrowDice(int diceNumb)
    {
        diceUsed = true;
        repeatTurn = false;

        //int diceNumb = UnityEngine.Random.Range(1, 7);

        Debug.Log(diceNumb);
        if (diceNumb == 5)
        {
            if (slots[currentPlayer.exitSlotIndex].IsAvaible())
            {
                //TODO
                //CheckForEnemies
                if (currentPlayer.CheckJail())
                {

                    //slots[currentPlayer.exitSlotIndex].AddingToken()
                    NextTurn();
                }
            }
        }
        else
        {
            if(currentPlayer.AllTokensJailed())
            {
                NextTurn();
            }
            else
            {
                if (diceNumb == 6)
                {
                    RepeatTurn();

                    //If all tokens free --> diceNumb = 7
                    if (currentPlayer.AllTokensFree()) diceNumb = 7;
                }

                if (killedToken)
                {
                    killedToken = false;
                    NextTurn();
                }
                else
                {
                    currentPlayer.CheckMoves(diceNumb);
                }
            }

        }

    }

    public void RepeatTurn()
    {
        
        repeatedTurns++;
        if(repeatedTurns > 2)
        {
            //se mató - lastTokenUsed --> Jail
            lastTokenUsed.ReturnJail();

            killedToken = true;
            repeatTurn = false;
        }
        else
        {
            repeatTurn = true;
        }
        
    }

    public void NextTurn()
    {
        if (!repeatTurn)
        {
            currentPlayerIndex++;
            if (currentPlayerIndex == 4)
            {
                currentPlayerIndex = 0;
            }

            currentPlayer = players[currentPlayerIndex];
            repeatedTurns = 0;
        }

        //UI animation -- currentPlayer turn
        diceUsed = false;
    }

    public void EndGame()
    {

    }
}
