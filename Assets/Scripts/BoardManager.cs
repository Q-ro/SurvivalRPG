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
    public int column = 8;
    public int rows = 8;

    //Range of obstacles to spawn
    public Count wallCount = new Count(5, 9);
    //Range of items to spwan
    public Count foodCount = new Count(1, 5);

    //Game objects to be used
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    //A parent for our game objects, for cleanliness sake
    private Transform boardHolder;

    //A gird of all the vailable positions to spawn object into
    private List<Vector3> gridPositions = new List<Vector3>();

    /// <summary>
    /// Function that initialize the grid for our game
    /// </summary>
    void InitializeList()
    {
        //Make sure the grid is empty
        gridPositions.Clear();


    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
