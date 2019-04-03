﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoardManager : MonoBehaviour {

    private BoardUI ui;

    private Player[] players = new Player[4];
    //HookeableRules

    private Player currentPlayer;
    private int currentPlayerIndex = 0;

    private GameObject SlotsGO;
    public Slot[] slots;

    private Dice dice;

    private bool diceUsed = false;
    private bool repeatTurn = false;
    private int repeatedTurns = 0;
    private bool killedToken = false;
    private Token lastTokenUsed;
    private bool sixDrop;
    public bool eat;

    // Use this for initialization
    void Start () {

        ui = GameObject.Find("Canvas").gameObject.GetComponent<BoardUI>();

        //Dice
        dice = GameObject.Find("Dice").gameObject.GetComponent<Dice>();
        dice.bM = this;

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

        //Color slots rules
        slots[16].specialMove = true;
        slots[16].specialIndex = 76;
        slots[16].specialColor.Add(Color.blue);

        slots[33].specialMove = true;
        slots[33].specialIndex = 84;
        slots[33].specialColor.Add(Color.red);

        slots[50].specialMove = true;
        slots[50].specialIndex = 92;
        slots[50].specialColor.Add(Color.green);

        slots[67].specialMove = true;
        slots[67].specialIndex = 0;
        slots[67].specialColor.Add(Color.blue);
        slots[67].specialColor.Add(Color.red);
        slots[67].specialColor.Add(Color.green);

        for(int i = 68; i < 100; i++)
        {
            slots[i].stairTile = true;
        }

        slots[75].roadEnd = true;
        slots[83].roadEnd = true;
        slots[91].roadEnd = true;
        slots[99].roadEnd = true;



        //add exitSlotsIndex - currently InEditor managed


        //Players
        for (int i = 0; i < this.transform.childCount; i++)
        {
            players[i] = transform.GetChild(i).GetComponent<Player>();
            players[i].bM = this;
            if (i == 0) players[i].color = Color.yellow;
            if (i == 1) players[i].color = Color.blue;
            if (i == 2) players[i].color = Color.red;
            if (i == 3) players[i].color = Color.green;
        }
        currentPlayer = players[0];
        currentPlayerIndex = 0;

        //TEST
        /*
        Token token = GameObject.Find("Token").gameObject.GetComponent<Token>();
        token.bM = this;
        */
	}

    public void BMFreeToken(Token token)
    {
        token.FreeToken();
        //slots[currentPlayer.exitSlotIndex].AddingToken(token);
        //token.gameObject.transform.DOMove(slots[currentPlayer.exitSlotIndex].transform.position, 0.5f);
    }

    public bool BMCheckMove(int slot, int diceNumb)
    {

        if (slots[slot+1].ThisSlotAvaible(diceNumb-1))
        {

            return true;
        }

        return false;
    }

    public void MoveToken(Token token, int amount)
    {
        int index = token.currentSlot;
        
        /*
        if(currentPlayer.barriers.Count > 0)
        {
            for(int i = 0; i< currentPlayer.barriers.Count; i++)
            {
                if(currentPlayer.barriers[i].token1 == token)
                {
                    Token otherToken = currentPlayer.barriers[i].token2;
                    otherToken.player.RemoveBarrier(otherToken);

                    currentPlayer.RemoveBarrier(i);
                }
            }
        }*/

        slots[index].RemovingToken(token);


        float time = 1.0f / amount;
        Sequence moveSequence = DOTween.Sequence();

        for (int i = 0; i < amount; i++)
        {
            //TWEEN

            index = slots[index].NextIndex(token);

            moveSequence.Append(token.gameObject.transform.DOMove(slots[index].transform.position, time));
            
        }

        slots[index].AddingToken(token);
        if (slots[index].roadEnd)
        {
            token.end = true;
            if (currentPlayer.AllTokensEnd())
            {
                //TODO
                //WIN
                Debug.LogError("WIN!");
            }
        }
        if (slots[index].stairTile)
        {
            token.stair = true;
        }

        moveSequence.Play<Sequence>();
        currentPlayer.lastTokenUsed = token;
        lastTokenUsed = token;

        token.currentSlot = index;

        if (slots[index].tokens.Count > 1)
        {

            if (CheckEats(token, index))
            {
                //IA - ChooseOneMove
            }
            else
            {
                NextTurn();

            }
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
            //ADD Bariers
            if (currentSlot.tokens[0] != null)
            {
                Token currentInSlotToken = currentSlot.tokens[0];
                
                //otherPlayer
                Player otherPlayer = currentInSlotToken.player;

                otherPlayer.AddBarrier(currentInSlotToken, token);
                currentPlayer.AddBarrier(token, currentInSlotToken);

                //ADJUST
            }

            //return false;
        }
        else
        {
            if (currentSlot.tokens[0] != null)
            {
                Token currentInSlotToken = currentSlot.tokens[0];
                if (!token.IsAlly(currentInSlotToken))
                {
                    //DeadAnimation
                    //remove token eated
                    currentInSlotToken.player.JailToken(currentInSlotToken.tokenIndex);
                    eat = true;
                    currentSlot.RemovingToken(currentInSlotToken);

                    //make eaten move

                    if (!currentPlayer.CheckMoves(20))
                    {
                        return false;
                    }
                    else
                    {
                        eat = true;
                        return true;
                    }

                }
                else
                {
                    //isAlly
                    Player ally = currentInSlotToken.player;

                    ally.AddBarrier(currentInSlotToken, token);
                    currentPlayer.AddBarrier(token, currentInSlotToken);

                    //ADJUST
                }
            }

            //token.currentSlot = index;
            //return false;
        }

        return false;
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!diceUsed)
            {
                //ThrowDice();

            }
        }

        //test
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!diceUsed)
            {
                dice.GetNumb(1);
                ui.DisableGR();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (!diceUsed)
            {
                dice.GetNumb(2);
                ui.DisableGR();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (!diceUsed)
            {
                dice.GetNumb(3);
                ui.DisableGR();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (!diceUsed)
            {
                dice.GetNumb(4);
                ui.DisableGR();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (!diceUsed)
            {
                dice.GetNumb(5);
                ui.DisableGR();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (!diceUsed)
            {
                dice.GetNumb(6);
                ui.DisableGR();
            }
        }
    }

    public void ThrowDice(int diceNumb)
    {
        diceUsed = true;
        repeatTurn = false;
        sixDrop = false;

        //int diceNumb = UnityEngine.Random.Range(1, 7);

        Debug.Log(diceNumb);
        if (diceNumb == 5 && slots[currentPlayer.exitSlotIndex].IsAvaible() && !currentPlayer.AllTokensFree())
        {
            
            if (currentPlayer.CheckJail())
            {

                //slots[currentPlayer.exitSlotIndex].AddingToken()
                NextTurn();
            }
            else
            {
                Debug.LogError("look here");
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
                    sixDrop = true;
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
                    bool openableBarrier = false;

                    if (sixDrop)
                    {
                        if (currentPlayer.barriers.Count > 0)
                        {
                            //TODO

                            for (int i = 0; i < currentPlayer.barriers.Count; i++)
                            {
                                Token playerToken = currentPlayer.GetOwnTokenFromWall(i);

                                if (playerToken.TokenCheckMove(diceNumb) && !openableBarrier)
                                {
                                    openableBarrier = true;
                                }
                            }
                        }

                        sixDrop = false;
                    }

                    if (openableBarrier)
                    {
                        //IA - ChooseOneMove
                    }
                    else
                    {
                        if (currentPlayer.CheckMoves(diceNumb))
                        {
                            //IA - ChooseOneMove
                        }
                        else
                        {
                            NextTurn();
                        }
                    }
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
            //lastTokenUsed.ReturnJail();

            if (!lastTokenUsed.stair)
            {
                currentPlayer.JailToken(lastTokenUsed.tokenIndex);
            }

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
            currentPlayer.charController.Turn();
            currentPlayerIndex++;
            if (currentPlayerIndex == 4)
            {
                currentPlayerIndex = 0;
            }

            currentPlayer = players[currentPlayerIndex];
            currentPlayer.charController.Turn();
            repeatedTurns = 0;
        }

        //UI animation -- currentPlayer turn
        diceUsed = false;

        ui.UpdateTurnText(currentPlayer.name, currentPlayer.color);
        EnableLaunchButton();
    }

    public void EnableLaunchButton()
    {
        dice.ResetDice();
        //TODO
        //enableButton
        ui.EnableGR();
    }

    public void LaunchButtonAction()
    {
        Debug.Log("LaunchButtonAction");
        dice.Launch();
        //TODO
        //disableButton
        ui.DisableGR();
    }

    public void EndGame()
    {

    }
}
