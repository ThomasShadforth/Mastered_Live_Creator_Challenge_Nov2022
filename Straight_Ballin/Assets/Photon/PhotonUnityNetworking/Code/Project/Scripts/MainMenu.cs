using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject _mainMenuPanel;
    [SerializeField] GameObject _randomMatchmakingPanel;
    [SerializeField] GameObject _customMatchmakingPanel;

    [SerializeField] DelayLobbyController _delayLobbyController;

    // Start is called before the first frame update
    void Start()
    {
        _delayLobbyController = FindObjectOfType<DelayLobbyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMenu()
    {
        _mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenRandomMatchMaking()
    {
        _randomMatchmakingPanel.SetActive(true);
        _delayLobbyController.SetButtonsDefault();
    }

    public void CloseRandomMatchMaking()
    {
        _randomMatchmakingPanel.SetActive(false);
    }
}
