using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GameObject startGamePanel;    
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject playerWinPanel;
    [Space(10)]
    [SerializeField] private Button exitButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private PlayerInput playerInput;    

    private void Start()
    {         
        startGamePanel.SetActive(true);
        gameOverPanel.SetActive(false);
        restartButton.interactable = false;
        playerInput.enabled = false;
    }

    public void StartButtonPressed()
    {
        StartCoroutine(StartGame());
        exitButton.interactable = false;
    }

    public void ExitButtonPressed()
    {
        Application.Quit();
    }

    public void RestartButtonPressed()
    {
        StartCoroutine(RestartLevel());
    }

    public void GameOverEvent()
    {      
        StartCoroutine(GameOver());
    }

    public void PlayerWIn()
    {     
        StartCoroutine(PlayerWonGame());
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(4f);        
        gameOverPanel.SetActive(true);
        restartButton.interactable = false;
    }

    private IEnumerator PlayerWonGame()
    {
        yield return new WaitForSeconds(0.2f);
        playerInput.enabled = false;
        playerWinPanel.SetActive(true);
        restartButton.interactable = false;
    }

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.5f);
        startGamePanel.SetActive(false);        
        restartButton.interactable = true;
        playerInput.enabled = true;
    }
}
