using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BoardUI : MonoBehaviour {

    public BoardManager bM;
    public Text turnText; //currently managed by editor

    public GameObject mainCamera;
    public GameObject dinamycCamera;
    public GameObject animCamera;

    //private Canvas canvas;
    private GraphicRaycaster GR;

	void Start () {
        //canvas = this.gameObject.GetComponent<Canvas>();
        GR = this.gameObject.GetComponent<GraphicRaycaster>();

        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            mainCamera.SetActive(false);
        }
        dinamycCamera.SetActive(false);
    }

    public void UpdateTurnText(string name, Color color)
    {
        turnText.text = "It's " + name + "'s turn";
        turnText.transform.parent.GetComponent<Image>().color = color;
    }

    public void DestroyMovecamera()
    {
        mainCamera.SetActive(true);
        animCamera.SetActive(false);
    }

    public void DisableGR()
    {
        //GR.enabled = false;
    }

    public void EnableGR()
    {
        //GR.enabled = true;
    }

    public void CameraButton()
    {
        if (mainCamera.activeInHierarchy)
        {
            mainCamera.SetActive(false);
            dinamycCamera.SetActive(true);
            animCamera.SetActive(false);
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
}
