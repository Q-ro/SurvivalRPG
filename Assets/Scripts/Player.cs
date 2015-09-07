using UnityEngine;
using System.Collections;

public class Player : MovingObject {

    // How much dmg the player deals to destructible objects
    public int objectDamage = 1;
    //The amount of points refilled by collecting edible items
    public int pointsPerFood = 10;
    public int pointsPersoda = 20;
    //Delay to be used when restarting a level
    public float restartLevelDelay = 1f;

    //An instance to our animator, to be able to switch between the different animations
    private Animator animator;

    //Store the current "hunger" level of the player
    private int food;
    //Stores the health level of the player
    private int health;
    //Sotres the stamina level of the player
    private int stamina;

	/// <summary>
	/// Initialize player specific logic 
	/// </summary>
	protected override void Start () 
    {
        //Get the player's animator to be used to trigger animation changes
        animator = GetComponent<Animator>();

        //Get the Player's "hunger" level between levels
        food = GameManager.instance.playerFoodPoints;

        //Continue with the inizialization process on the base class
        base.Start();
	
	}

    /// <summary>
    /// Called when the player is disabled
    /// </summary>
    private void OnDisable()
    {
        //Saves the current playe's status on game manager so that it can be access on the next level
        GameManager.instance.playerFoodPoints = food;
        GameManager.instance.playerHealthPoints = health;
        GameManager.instance.playerStaminaPoints = stamina;

    }

    protected override void AttempMove<T>(int xDireccion, int yDireccion)
    {
        //Stores the object we collided with (if any)
        RaycastHit2D hit;

        //Movement cost 1 food point
        if (food >= 0)
        {
            food--;
        }
        //If the player is starving, he gets hurt instead
        else
        {
            health--;
            //Check is he died from taking too much damage
            CheckGameOver();
        }

        base.AttempMove<T>(xDireccion, yDireccion);
        
        //The player's turn ends after 1 movement
        GameManager.instance.PlayersTurn = false;

    }

    private void CheckGameOver()
    {
        if(health <= 0)
        {
            GameManager.instance.GameOver();
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        int horizontal = 0;
        int vertical = 0;

        if (GameManager.instance.PlayersTurn) return;

        horizontal = (int)Input.GetAxis("Horizontal");
        vertical = (int)Input.GetAxis("Vertical");

        //Lock the movement to X and Y axis (no vertical movement)
        if(horizontal !=0)
        {
            vertical = 0;
        }
        //if (vertical!= 0)
        //{
        //    horizontal = 0;
        //}

        if(horizontal !=0 || vertical != 0)
        {
            AttempMove<DesturctibleTerrain>(horizontal, vertical);
        }
	}

    protected override void OnCantMove <T> (T component)
    {
        //Set hitObstacle to equal the component passed in as a parameter.
        DesturctibleTerrain hitObstacle = component as DesturctibleTerrain;

        //Damage the object
        hitObstacle.DamageObject(objectDamage);

        //Switch the animator state to chop
        animator.SetTrigger("playerChop");
    }

    /// <summary>
    /// Advance the stage to the next level
    /// </summary>
    private void NextLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    private void LoseFood(int loss)
    {
        animator.SetTrigger("playerHit");

        if (food >= 0)
        {
            food-= loss;
        }
        //If the player is starving, he gets hurt instead
        else
        {
            health-=loss;
            //Check is he died from taking too much damage
            CheckGameOver();
        }
    }
}
