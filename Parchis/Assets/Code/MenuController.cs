using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject ModeSelectionView;
    #region
    public GameObject CharsSelectionView;
    public Button CharNextButton;
    public Button CharPreviousButton;

    public GameObject charBlockerPanel;
    public GameObject charFramePanel;
    public GameObject colorBlockerPanel;
    public GameObject colorFramePanel;
    public GameObject playerDefinitionsPanel;

    public String[] names;
    public Image[] chars;
    public Color[] colors;

    private GameObject currentPlayer;
    private MatchInfo.PlayerDefinition[] players;

    

    private int index = 0;
    private int State { get { return state; } set { SetState(value); } }
    private int state = 0;
    private Sprite bg;
    #endregion

    public GameObject StageSelectionView;

    public Sprite[] stages;
    public Image stagePortrait;
    private int stageIndex;

    private int size = 4;

    // Start is called before the first frame update
    void Start()
    {
        names = new String[size];
        chars = new Image[size];
        colors = new Color[size];

        names[0] = "Paladin";
        names[1] = "Bow";
        names[2] = "GreatSword";
        names[3] = "Axe";

        players = new PlayerDefinition[size];

        for(int i = 0; i< players.Length; i++)
        {
            int f = i;
            players[f] = new PlayerDefinition();

            chars[f] = CharsSelectionView.transform.GetChild(0).GetChild(f).GetComponent<Image>();
            CharsSelectionView.transform.GetChild(0).GetChild(f).GetComponent<Button>().onClick.AddListener(() => CharButton(f));

            colors[f] = CharsSelectionView.transform.GetChild(1).GetChild(f).GetComponent<Image>().color;
            CharsSelectionView.transform.GetChild(1).GetChild(f).GetComponent<Button>().onClick.AddListener(() => ColorButton(f));
        }

        CharNextButton.onClick.AddListener(() => NextState());
        CharPreviousButton.onClick.AddListener(() => PreviousState());

        charBlockerPanel.SetActive(false);
        colorFramePanel.SetActive(false);

        bg = CharsSelectionView.GetComponent<Image>().sprite;

        stagePortrait.sprite = stages[0];
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
                Debug.Log("NextView");
                //LOAD INFO TO MATCH INFO
                MatchInfo.Instance.playerDefinitions = players;
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
                Debug.Log("PrevView");
            }
        }
        else if (State == 1)
        {
            State = 0;

            CharsSelectionView.transform.GetChild(0).GetChild(players[index].imageIndex).GetComponent<Button>().interactable = true;

            currentPlayer = playerDefinitionsPanel.transform.GetChild(index).gameObject;

            currentPlayer.transform.GetChild(0).GetComponent<Image>().sprite = null;
            //currentPlayer.transform.GetChild(0).GetComponent<Image>().color = Color.clear;
        }
        else if (State == 2)
        {
            CharsSelectionView.transform.GetChild(1).GetChild(players[index].colorIndex).GetComponent<Button>().interactable = true;

            currentPlayer = playerDefinitionsPanel.transform.GetChild(index).gameObject;

            currentPlayer.transform.GetChild(1).GetComponent<Image>().color = Color.white;

            State = 1;
        }
        else Debug.LogError("look here");
    }

    public void CharButton(int i)
    {
        Debug.Log("Char button " + i);

        players[index].name_id = names[i];
        players[index].image = chars[i];
        players[index].imageIndex = i;

        currentPlayer = playerDefinitionsPanel.transform.GetChild(index).gameObject;

        currentPlayer.transform.GetChild(0).GetComponent<Image>().sprite = chars[i].sprite;

        CharsSelectionView.transform.GetChild(0).GetChild(i).GetComponent<Button>().interactable = false;
        State = 1;
    }

    public void ColorButton(int i)
    {
        Debug.Log("Color button " + i);

        players[index].color = colors[i];
        players[index].colorIndex = i;

        currentPlayer = playerDefinitionsPanel.transform.GetChild(index).gameObject;

        currentPlayer.transform.GetChild(1).GetComponent<Image>().color = colors[i];

        CharsSelectionView.transform.GetChild(1).GetChild(i).GetComponent<Button>().interactable = false;
        State = 2;
    }

    private void SetState(int value)
    {
        if (value == 0)
        {
            charBlockerPanel.SetActive(false);
            charFramePanel.SetActive(true);
            colorBlockerPanel.SetActive(true);
            colorFramePanel.SetActive(false);
            CharNextButton.interactable = false;
        }
        else if (value == 1)
        {
            charBlockerPanel.SetActive(true);
            charFramePanel.SetActive(false);
            colorBlockerPanel.SetActive(false);
            colorFramePanel.SetActive(true);
            CharNextButton.interactable = false;
        }
        else if (value == 2)
        {
            charBlockerPanel.SetActive(true);
            charFramePanel.SetActive(false);
            colorBlockerPanel.SetActive(true);
            colorFramePanel.SetActive(false);
            CharNextButton.interactable = true;
        }
        state = value;
    }

    public void StageNextButtonAction()
    {
        if(stageIndex < stages.Length - 1)
        {
            stageIndex++;
        }
        else
        {
            stageIndex = 0;
        }
        stagePortrait.sprite = stages[stageIndex];
    }

    public void StagePrevButtonAction()
    {
        if(stageIndex != 0)
        {
            stageIndex--;
        }
        else
        {
            stageIndex = stages.Length - 1;
        }
        stagePortrait.sprite = stages[stageIndex];
    }

    public void StageButtonAction()
    {
        Debug.Log("Launch Scene");
        if(stageIndex == 0)
        {
            Debug.Log("Launch town");
            SceneManager.LoadScene(0);
        }
        else
        {
            Debug.Log("Launch tavern");
        }
    }

    public void FourVersusButtonAction()
    {
        MatchInfo.Instance.allies = false;
        MatchInfo.Instance.ia = false;
        ModeSelectionView.SetActive(false);
        CharsSelectionView.SetActive(true);
    }

    public void TwoVersusTwoButtonAction()
    {
        MatchInfo.Instance.allies = true;
        MatchInfo.Instance.ia = false;
        ModeSelectionView.SetActive(false);
        CharsSelectionView.SetActive(true);
    }

    public void VersusIAButtonAction()
    {
        MatchInfo.Instance.allies = false;
        MatchInfo.Instance.ia = true;
        ModeSelectionView.SetActive(false);
        CharsSelectionView.SetActive(true);
    }
}
