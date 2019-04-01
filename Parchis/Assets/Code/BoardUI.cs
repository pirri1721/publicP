using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardUI : MonoBehaviour {

    public BoardManager bM;
    public Text turnText; //current managed by editor

    private Canvas canvas;
    private GraphicRaycaster GR;

	// Use this for initialization
	void Start () {
        canvas = this.gameObject.GetComponent<Canvas>();
        GR = this.gameObject.GetComponent<GraphicRaycaster>();
	}

    public void UpdateTurnText(string name, Color color)
    {
        turnText.text = "It's " + name + "'s turn";
        turnText.transform.parent.GetComponent<Image>().color = color;
    }

    public void DisableGR()
    {
        GR.enabled = false;
    }

    public void EnableGR()
    {
        GR.enabled = true;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
