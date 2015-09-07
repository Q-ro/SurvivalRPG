using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    //Static instance of game manager, allows for access to this class from other scrips
    public static GameManager instance = null;
    //A refference to board manager, used to set up the level
    public BoardManager boardScript;

    public int playerFoodPoints = 100;
    public int playerHealthPoints = 100;
    public int playerStaminaPoints = 100;

    [HideInInspector]
    public bool PlayersTurn = true;


    //The current game level (set to 2 to spawn at least 1 enemy on the first screen)
    public int level = 2;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
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

        //Initialize the game
        InitGame();
    }


    /// <summary>
    /// Initializes the board for each level.
    /// </summary>
    void InitGame()
    {
        //Set up the scene with the current level
        boardScript.SetUpScene(level);
    }

    /// <summary>
    /// Triggers when the player dies, displays the game over screen
    /// </summary>
    public void GameOver()
    {
        enabled = false;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
