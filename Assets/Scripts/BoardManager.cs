using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count (int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    //Amount of rows and columns for the game board
    public int columns = 8;
    public int rows = 8;

    //Range of wall obstacles to spawn
    public Count wallCount = new Count(5, 9);
    //Range of items to spwan
    public Count foodCount = new Count(1, 5);

    // Prefab to spawn for exit
    public GameObject exit; 

    //Arrays of Game objects to be used
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    //A parent for our game objects, used to store refferences to the transform of our game objects
    //as well as for cleanliness sake
    private Transform boardHolder;

    //A gird of all the vailable positions to spawn object into
    private List<Vector3> gridPositions = new List<Vector3>();

    /// <summary>
    /// Pepares the grid for a new board to be created
    /// </summary>
    void InitializeList()
    {
        //Make sure the grid is empty
        gridPositions.Clear();

        //Go through all the columns
        for(int x = 1 ; x < columns -1; x++)
        {
            //Go through all the rows
            for(int y = 1; y < rows -1; y++)
            {
                //Add a new "available position" to our grid
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    /// <summary>
    /// Sets up the outter edges and floor tiles for the boards
    /// </summary>
    void BoarSetup()
    {
        // A variable to store the prefab to be instantiated by our board
        GameObject toInstantiate;

        // Instantiate Board and set boardHolder to its transform
        boardHolder = new GameObject("Board").transform;

        //Go through all the columns starting with the outter ones (-1)
        for(int x = -1; x < columns+1; x++)
        {

            //Go through all the rows starting with the outter ones (-1)
            for(int y = -1; y < rows+1; y++)
            {
                //create the outter walls
                if(x == -1 || x == columns  || y == -1 || y == rows )
                {
                    // Select a random wall tile prefab and perpare to instantiate it
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }
                //create the floor
                else
                {
                    // Select a random floor tile prefab and perpare to instantiate it
                    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                }

                //Instantiate the game object using the selected prefab
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                //Set the parent of our instantiated object to boardHolder, this is just organizational to avoid cluttering hierarchy.
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    /// <summary>
    /// Returns a random position from gridPosition
    /// </summary>
    /// <returns></returns>
    Vector3 RandomPosition()
    {
        //stores a random index between 0 and the amount of possible positions stores in gridPosition
        int randomIndex = Random.Range(0, gridPositions.Count);

        //Stores the position the random position from gridPosition 
        Vector3 randomPosition = gridPositions[randomIndex];

        // Remove the element so that i cant be reselected in future iterations
        gridPositions.RemoveAt(randomIndex);

        return randomPosition;
    }

    /// <summary>
    /// Lays out object at random from an array, using a range of possible object to be spawned
    /// </summary>
    /// <param name="tileArray">A list of objects to choose from</param>
    /// <param name="minimum">The minimum amount of objects to be spawned</param>
    /// <param name="maximum">The maximum amount of objects to be spawned</param>
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {

        Vector3 randomPosition; //Stores a random available position from gridPosition
        GameObject tileChoice; //Stores a random tile from the given tileArray

        // A random number of objects to be created withing the given range
        int objectCount = Random.Range(minimum, maximum + 1);
        
        for(int i = 0; i < objectCount; i++)
        {
            //Get an available position for the object to be spawned
            randomPosition = RandomPosition();

            //choose a random tile
            tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            //Instantiate the tile choosen at an abailable position
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    /// <summary>
    /// Initializes the level
    /// </summary>
    public void SetUpScene(int level)
    {

        // Create the outter walls and floors
        BoarSetup();

        // Setup a list of all available spaces to spawn object into
        InitializeList();

        //Instantiate a random amount of walls
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);

        //Instantiate a random amount of food
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

        //Determine number of enemies based on current level and a logarithmic progression
        int enemyCount = (int)Mathf.Log(level, 2f);

        //Instantiate a random number of enemies
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

        //Instantiate the exit on the upper rigth corner
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
