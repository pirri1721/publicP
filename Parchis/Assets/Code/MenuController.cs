using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{


    public Button NextButton;
    public Button PreviousButton;

    public GameObject charBlockerPanel;
    public GameObject colorBlockerPanel;

    public Image[] chars;
    public Image[] colors;

    private PlayerDefinition currentPlayer;
    private PlayerDefinition[] players;
    public struct PlayerDefinition
    {
        public Image image;
        public Color color;
    }
    private int index = 0;
    private int State { get { return state; } set { SetState(value); } }
    private int state;

    private void SetState(int value)
    {
        if (state == 0)
        {
            charBlockerPanel.SetActive(false);
            colorBlockerPanel.SetActive(true);
            NextButton.interactable = false;
        }
        else if (state == 1)
        {
            charBlockerPanel.SetActive(true);
            colorBlockerPanel.SetActive(false);
            NextButton.interactable = false;
        }
        else if (state == 2)
        {
            charBlockerPanel.SetActive(true);
            colorBlockerPanel.SetActive(true);
            NextButton.interactable = true;
        }
    }

    private int size = 4; 
    // Start is called before the first frame update
    void Start()
    {
        players = new PlayerDefinition[size];
        for(int i = 0; i< players.Length; i++)
        {
            players[i] = new PlayerDefinition();
        }
    }

    public void NextState()
    {
        if (State == 2)
        {
            if(index < size-1)
            {
                //NEXT PlayerDef/index
                index++;
                State = 0;
            }
            else
            {
                //NEXT VIEW
            }
        }
        else Debug.LogError("look here");
    }

    public void PreviousState()
    {
        if (State == 0)
        {
            if (index > 0)
            {
                //PREVIOUS PlayerDef/index
                index--;
                State = 2;
            }
            else
            {
                //PREVIOUS View
            }
        }
        else if (State == 1)
        {
            State = 0;
        }
        else if (State == 2)
        {
            State = 1;
        }
        else Debug.LogError("look here");
    }

    public void CharButton(int i)
    {
        players[index].image = chars[i];
    }

    public void ColorButton(int i)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
