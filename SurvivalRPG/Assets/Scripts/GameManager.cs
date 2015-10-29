using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public float levelStartDelay = 2f;          // Delay before starting level.
    public float turnDelay = 0.1f;              // Delay between turns.      
    private Text levelText;                     // The text to be displayed on floor transitions
    private GameObject levelImage;              // A refference to the level image, so that we can activate and desacticvate it
    private bool doingSetup;                    // Flag that determines if the board is still setting itself up

    //Player stats
    public int playerFoodPoints = 100;
    public int playerHealthPoints = 100;
    public int playerStaminaPoints = 100;          

    
    public static GameManager instance = null;  // Static instance of game manager, allows for access to this class from other scrips
    public BoardManager boardScript;            // A refference to board manager, used to set up the level
    public int level = 1;                       // The current game level (set to 2 to spawn at least 1 enemy on the first screen)

    private List<Enemy> enemies;                // List of game enemies
    private bool enemiesMoving;                 // Flag that keeps track of the enemies's turns


    [HideInInspector]
    public bool playersTurn = true;             // Flag that keeps track of the player's turn


    

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
		Debug.Log ("im awake");
	
        //If there is no previous instance, make this the instance
        if(instance == null)
        {
            instance = this;
        }
        //If there is already an instances and it's not this, kill it.            
        else if(instance != this)
        {
            Destroy(this);
        }

        //Set this to not be destroyed when reloading the scene
        DontDestroyOnLoad(gameObject);

        //get a refference to our BoardManager, and store it
        boardScript = GetComponent<BoardManager>();

        enemies = new List<Enemy>();

        //Initialize the game
        InitGame();
    }


    /// <summary>
    /// Initializes the board for each level.
    /// </summary>
    void InitGame()
    {
		Debug.Log ("Time to roll");

        doingSetup = true;

        // Get a refference to the Level image and text
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        // Set the text to the current game level
        levelText.text = "Floor " + level;
        levelImage.SetActive(true);
        // remove the "title card" after a short delay
        Invoke("FinishSetup", levelStartDelay);

        // Resets the list for new enemies to be stored.
        enemies.Clear();

        // Set up the scene with the current level
        boardScript.SetUpScene(level);
    }

    /// <summary>
    /// 
    /// </summary>
    void FinishSetup()
    {
        // Hide the level intro card
        levelImage.SetActive(false);
        // Set the falg to false, to allow the player to move
        doingSetup = false;
    }

    /// <summary>
    /// Move the enemies on the board
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveEnemies()
    {
        // Set the flag so that the game wait for all enemies movement to be done before proceeding any fourther
        enemiesMoving = true;
        // Make the game wait for the duration of the turn delay
        yield return new WaitForSeconds(turnDelay);

        // If there are no enemies, simulate the delay between turns, as if there were enemies in the map
        if(enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        // Move the enemies one by one
        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();

            // Wait for the previous enemy to stop moving before moving the next one
            yield return new WaitForSeconds(enemies[i].moveTime);
        }


        playersTurn = true;
        enemiesMoving = false;

    }

    /// <summary>
    /// Triggers when the player dies, displays the game over screen
    /// </summary>
    public void GameOver()
    {
        levelText.text = "You died horribly at floor " + level;
        levelImage.SetActive(true);
        enabled = false;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(playersTurn || enemiesMoving || doingSetup)
        {
            return;
        }

        StartCoroutine(MoveEnemies());
	}

    public void AddEnemyToList(Enemy enemy)
    {
        enemies.Add(enemy);
    }

	public void loadNextFloor ()
	{
		//Add one to our level number.
		level++;
		//Call InitGame to initialize our level.
		InitGame();
	}

    //This is called each time a scene is loaded.
    void OnLevelWasLoaded(int index)
    {
        //Add one to our level number.
        //level++;
        //Call InitGame to initialize our level.
        //InitGame();
    }
}
