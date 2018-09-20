using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Token : MonoBehaviour, IPointerClickHandler {

    public bool free = false;
    public int currentSlot;
    public Color color;
    public Player player;
    public int tokenIndex = 0;

    public BoardManager bM;

    private Vector3 jailPosition; 

    public bool enabledMove = false;
    int nextMove = 0;

	// Use this for initialization
	void Start () {

        jailPosition = transform.position;
        //currentSlot = 0;

        //Test
        //nextMove = 4;
        //enabledMove = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {

            nextMove = 4;
            enabledMove = true;
        }
	}

    public void CheckMove(int diceNumb)
    {

        if (!bM.CheckMove(currentSlot, diceNumb))
        {
            
        }
        else
        {
            //CreateShadow(thisToken)
            EnableMove(diceNumb);
        }
       
    }

    public void EnableMove(int diceNumb)
    {
        //Add shader
        //addpointer
        enabledMove = true;
        nextMove = diceNumb;
    }


    public void Move()
    {
        player.DisableMoves();

        //currentSlot = currentSlot + nextMove;
        

        bM.MoveToken(this, nextMove);
        //currentSlot = currentSlot + nextMove;

        nextMove = 0;
    }

    internal void ReturnJail()
    {
        transform.DOMove(jailPosition, 2f);
        free = false;
        currentSlot = 0;
    }

    internal bool IsAlly(Token token)
    {
        if(token.player == this.player || token.player == this.player.ally)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        

        if (enabledMove)
        {
            Move();
            Debug.Log("Clicked when possible");
        }else
        Debug.Log("Clicked");
    }

    public void UpdateCurrentSlot(int destinationSlot)
    {
        currentSlot = destinationSlot;
    }

    
    public void FreeToken()
    {
        currentSlot = player.exitSlotIndex;
        free = true;
        transform.DOMove(bM.slots[player.exitSlotIndex].transform.position, 1);
        bM.slots[player.exitSlotIndex].AddingToken(this);
    }
}
