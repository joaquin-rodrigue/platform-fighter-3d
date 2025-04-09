using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages player class select, player spawning, and win conditions.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Player objects, and other references for the players
    [SerializeField] private GameObject playerOne;
    [SerializeField] private GameObject playerTwo;
    private PlayerController playerOneController;
    private PlayerController playerTwoController;
    [SerializeField] private Material playerOneMaterial;
    [SerializeField] private Material playerTwoMaterial;
    private int playerOneScore = 0;
    private int playerTwoScore = 0;

    /*[SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject bigEnemyPrefab;
    private GameObject enemy;
    private float timer = 0;*/

    // Prefabs for each player class
    [SerializeField] private GameObject brawlerClass;
    [SerializeField] private GameObject lancerClass;
    [SerializeField] private GameObject mageClass;
    [SerializeField] private GameObject heavyClass;

    // Text and UI objects
    [SerializeField] private TMP_Text playerOneScoreText;
    [SerializeField] private TMP_Text playerTwoScoreText;
    [SerializeField] private TMP_Text playerOneClassText;
    [SerializeField] private TMP_Text playerTwoClassText;
    [SerializeField] private GameObject playerSelectScreen;
    [SerializeField] private TMP_Text winScreenText;

    // Whether the game is started + player class selections
    private bool gameStarted = false;
    private int playerOneClass = -1;
    private int playerTwoClass = -1;

    [SerializeField] private CinemachineTargetGroup targetGroup;

    /// <summary>
    /// Used to change a player's current class selection. Note that to be usable by Unity UI buttons, 
    /// this method has only one parameter.
    /// </summary>
    /// <param name="choice">The class choice as an integer. For player one, this is between 4 and 7. For player two, this is between 0 and 3.</param>
    public void SelectCharacter(int choice)
    {
        if (choice / 4 > 0)
        {
            playerOneClass = choice % 4;
        }
        else
        {
            playerTwoClass = choice;
        }

        switch (choice)
        {
            case 0:
                playerTwoClassText.text = "Brawler";
                break;
            case 1:
                playerTwoClassText.text = "Lancer";
                break;
            case 2:
                playerTwoClassText.text = "Mage";
                break;
            case 3:
                playerTwoClassText.text = "Heavy";
                break;
            case 4:
                playerOneClassText.text = "Brawler";
                break;
            case 5:
                playerOneClassText.text = "Lancer";
                break;
            case 6:
                playerOneClassText.text = "Mage";
                break;
            case 7:
                playerOneClassText.text = "Heavy";
                break;
        }
    }

    /// <summary>
    /// Starts the game. Will not start the game if one of the two players has not chosen a class.
    /// </summary>
    public void StartGame()
    {
        if (playerOneClass == -1 || playerTwoClass == -1)
        {
            return;
        }

        playerOneScoreText.gameObject.SetActive(true);
        playerTwoScoreText.gameObject.SetActive(true);
        playerSelectScreen.SetActive(false);
        
        switch (playerOneClass)
        {
            case 0:
                playerOne = Instantiate(brawlerClass);
                break;
            case 1:
                playerOne = Instantiate(lancerClass);
                break;
            case 2:
                playerOne = Instantiate(mageClass);
                break;
            case 3:
                playerOne = Instantiate(heavyClass);
                break;
        }
        switch (playerTwoClass)
        {
            case 0:
                playerTwo = Instantiate(brawlerClass);
                break;
            case 1:
                playerTwo = Instantiate(lancerClass);
                break;
            case 2:
                playerTwo = Instantiate(mageClass);
                break;
            case 3:
                playerTwo = Instantiate(heavyClass);
                break;
        }

        playerTwo.transform.position = new Vector3(7.5f, 1.5f, 4);
        playerOne.transform.position = new Vector3(-7.5f, 1.5f, -4);
        playerOneController = playerOne.GetComponent<PlayerController>();
        playerTwoController = playerTwo.GetComponent<PlayerController>();
        playerTwo.GetComponentInChildren<MeshRenderer>().material = playerTwoMaterial;
        playerOne.GetComponentInChildren<MeshRenderer>().material = playerOneMaterial;
        playerOneController.SetPlayerID(1);
        playerTwoController.SetPlayerID(2);
        targetGroup.AddMember(playerOne.transform, 5, 1.2f);
        targetGroup.AddMember(playerTwo.transform, 5, 1.2f);
        gameStarted = true;
    }

    /// <summary>
    /// Sets score text, and checks for players being either dead or out of bounds to increase score.
    /// Also checks if a player has won.
    /// </summary>
    void Update()
    {
        if (!gameStarted)
        {
            return;
        }

        if (playerTwo.transform.position.y < -10 || playerTwoController.GetHealth() <= 0)
        {
            playerOneScore++;

            if (playerOneScore > 5)
            {
                WinScreen(1);
            }
            playerTwo.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            playerTwo.transform.position = new Vector3(7.5f, 1.5f, 4);
            playerTwoController.ResetHealth();
        }
        else if (playerOne.transform.position.y < -10 || playerOneController.GetHealth() <= 0)
        {
            playerTwoScore++;

            if (playerTwoScore > 5)
            {
                WinScreen(2);
            }
            playerOne.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            playerOne.transform.position = new Vector3(-7.5f, 1.5f, -4);
            playerOneController.ResetHealth();
        }

        playerOneScoreText.text = "Player 1 Score: " + playerOneScore + " Health: " + playerOneController.GetHealth();
        playerTwoScoreText.text = "Player 2 Score: " + playerTwoScore + " Health: " + playerTwoController.GetHealth();
    }

    /// <summary>
    /// Displays the win screen.
    /// </summary>
    /// <param name="playerNum">The player who won the game. This is not indexed from zero because I'm not saying 'Player 0 won'</param>
    private void WinScreen(int playerNum)
    {
        // TODO: asdf
        winScreenText.gameObject.SetActive(true);
        winScreenText.text = "PLAYER " + playerNum + " WINS";
        StartCoroutine(ResetGame());
    }

    /// <summary>
    /// Resets the game.
    /// </summary>
    /// <returns>Waits for 4 seconds to reload the game.</returns>
    private IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(0);
    }
}
