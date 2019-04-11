using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;

public class BoardUI : MonoBehaviour {

    public BoardManager bM;
    public Text turnText; //currently managed by editor
    public Text winText;

    public GameObject mainCamera;
    public GameObject dinamycCamera;
    public GameObject animCamera;

    public GameObject PauseWinPanel;
    public GameObject PausePanel;
    public GameObject WinPanel;

    public GameObject ConfirmationPanel;
    public Button ExitButton;
    public Text ConfirmationText;

    public GameObject OptionsPanel;
    AudioSource sound;
    public Slider volumeSlider;

    public GameObject Rerolling;
    public GameObject helpText;

    //private Canvas canvas;
    private GraphicRaycaster GR;

	void Start () {
        //canvas = this.gameObject.GetComponent<Canvas>();
        GR = this.gameObject.GetComponent<GraphicRaycaster>();

        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            mainCamera.SetActive(false);

            StartCoroutine(DisactiveAnimCamera());
        }
        dinamycCamera.SetActive(false);

        volumeSlider.onValueChanged.AddListener(delegate { AdjustVolume(); });

        sound = GameObject.Find("Sound").gameObject.GetComponent<AudioSource>();

        PauseWinPanel.SetActive(false);
        PausePanel.SetActive(false);
        WinPanel.SetActive(false);
        ConfirmationPanel.SetActive(false);
        OptionsPanel.SetActive(false);
        Rerolling.SetActive(false);
        helpText.SetActive(false);
    }

    public IEnumerator DisactiveAnimCamera()
    {
        yield return new WaitForSeconds(8f);
        mainCamera.SetActive(true);
        animCamera.SetActive(false);
    }

    public void AdjustVolume()
    {
        sound.volume = volumeSlider.value;
    }

    public void UpdateTurnText(string name, Color color, bool first)
    {
        if (first)
        {
            turnText.text = "it's " + name + "'s first turn";
        }
        else
        {
            turnText.text = "it's " + name + "'s turn";
        }
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
            if (animCamera)
            {
                animCamera.SetActive(false);
            }

            helpText.SetActive(true);
        }
        else
        {
            mainCamera.SetActive(true);
            dinamycCamera.SetActive(false);
            helpText.SetActive(false);
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
    
    public void OpenWinPanel()
    {
        PauseWinPanel.SetActive(true);
        WinPanel.SetActive(true);
    }

    public void UpdateWinText(string name, Color color)
    {
        OpenWinPanel();

        {
            winText.text = "player " + color.ToString() + "WIN";
            winText.text = name + "WIN";
        }
        //winText.transform.parent.GetComponent<Image>().color = color;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnMainMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenPauseMenu()
    {
        PauseWinPanel.SetActive(true);
        PausePanel.SetActive(true);
    }

    public void Resume()
    {
        PauseWinPanel.SetActive(false);
        PausePanel.SetActive(false);
    }

    public void OpenOptionsMenu()
    {

        PausePanel.SetActive(false);
        OptionsPanel.SetActive(true);
    }

    public void CloseOptionsMenu()
    {
        PausePanel.SetActive(true);
        OptionsPanel.SetActive(false);
    }

    public void OpenConfirmationPanel(bool restart)
    {
        ConfirmationPanel.SetActive(true);
        
        ExitButton.onClick.RemoveAllListeners();

        if (restart)
        {
            ConfirmationText.text = "Are you sure that you want restart the match?";
            ExitButton.onClick.AddListener(() => RestartLevel());
        }
        else
        {
            ConfirmationText.text = "Are you sure that you want leave the match?";
            ExitButton.onClick.AddListener(() => ReturnMainMenuScene());
        }
    }

    public void CloseConfirmationPanel()
    {
        ConfirmationPanel.SetActive(false);
    }

    public void ActiveRerolling()
    {
        StartCoroutine(RerollingCoroutine());
    }

    public IEnumerator RerollingCoroutine()
    {
        Rerolling.SetActive(true);

        yield return new WaitForSeconds(2f);

        Rerolling.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) OpenPauseMenu();
    }
}
