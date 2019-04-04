using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardUI : MonoBehaviour {

    public BoardManager bM;
    public Text turnText; //current managed by editor

    public GameObject mainCamera;
    public GameObject dinamycCamera;

    private Canvas canvas;
    private GraphicRaycaster GR;

	// Use this for initialization
	void Start () {
        canvas = this.gameObject.GetComponent<Canvas>();
        GR = this.gameObject.GetComponent<GraphicRaycaster>();

        dinamycCamera.SetActive(false);
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

    public void CameraButton()
    {
        if (mainCamera.activeInHierarchy)
        {
            mainCamera.SetActive(false);
            dinamycCamera.SetActive(true);
        }
        else
        {
            mainCamera.SetActive(true);
            dinamycCamera.SetActive(false);
        }
    }

    public GameObject GetActiveCamera()
    {
        if (mainCamera.activeInHierarchy)
        {
            return mainCamera;
        }
        else
        {
            return dinamycCamera;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
