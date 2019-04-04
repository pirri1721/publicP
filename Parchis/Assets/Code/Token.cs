using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Token : MonoBehaviour { //, IPointerClickHandler {

    public bool free = false;
    public int currentSlot;
    public Color color;
    public Player player;
    public int tokenIndex = 0;

    public BoardManager bM;

    private Vector3 jailPosition; 

    public bool enabledMove = false;
    int nextMove = 0;
    public bool end = false;

    public Outline outline;
    public bool stair;
    private bool inRadius;
    //private LayerMask lm = ~(1 << LayerMask.NameToLayer("Walls"));

    // Use this for initialization
    void Start () {

        jailPosition = transform.position;
        outline = GetComponent<Outline>();
        outline.enabled = false;
        //currentSlot = 0;

        //Test
        //nextMove = 4;
        //enabledMove = true;
	}

    public void Tint()
    {
        color = player.color;
        this.gameObject.GetComponent<MeshRenderer>().material.color = player.color;
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            MouseDown();
        }

        /*
        if (inRadius)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Clicked();
            }
        }*/
	}

    private void MouseDown()
    {
        Ray ray = bM.ui.GetActiveCamera().GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        Debug.DrawLine(ray.origin, ray.direction * 10, Color.black, 5f);
        
        if (Physics.Raycast(ray, out hitInfo)) //, lm))
        {
            Debug.Log(hitInfo.collider.gameObject.name);
            if(hitInfo.collider.gameObject == this.gameObject)
            {
                Clicked();
            }
        }
    }

    public bool TokenCheckMove(int diceNumb)
    {

        if (bM.BMCheckMove(currentSlot, diceNumb))
        {
            //CreateShadow(thisToken)
            //if (CheckEats(token, currentSlot+diceNumb))
            EnableMove(diceNumb);
            return true;
        }
        else
        {
            return false;
        }
       
    }

    public void EnableMove(int diceNumb)
    {
        //Add shader
        outline.enabled = true;
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

        //nextMove = 0;
    }

    public void ReturnJail()
    {
        transform.DOMove(jailPosition, 2f);
        free = false;
        currentSlot = 0;
    }

    public bool IsAlly(Token token)
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

    public void Clicked()
    {
        if (enabledMove)
        {
            Move();
            Debug.Log("Clicked when possible");
        }
        else Debug.Log("Clicked");
    }

    private void OnMouseEnter()
    {

        Debug.Log("Enter");
        inRadius = true;
    }

    private void OnMouseExit()
    {
        inRadius = false;
    }

    /*
    public void OnPointerClick(PointerEventData eventData)
    {
        

        if (enabledMove)
        {
            Move();
            Debug.Log("Clicked when possible");
        }else
        Debug.Log("Clicked");
    }*/

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

        if(bM.slots[player.exitSlotIndex].tokens.Count == 2)
        {
            Token currentInSlotToken = bM.slots[player.exitSlotIndex].tokens[0];

            if (IsAlly(currentInSlotToken))
            {
                player.AddBarrier(this, currentInSlotToken);
                currentInSlotToken.player.AddBarrier(currentInSlotToken, this);
            }
        }
    }
}
