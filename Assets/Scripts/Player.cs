using UnityEngine;
using System.Collections;

public class Player : MovingObject {

	int horizontal = 0, vertical = 0;
	// How much dmg the player deals to destructible objects
    public int objectDamage = 1;
    //The amount of points refilled by collecting edible items
    public int pointsPerFood = 10;
    public int pointsPersoda = 20;
    //Delay to be used when adavancing to the next level
    public float nextLevelDelay = 1f;

    //Store an instance to our animator, to be able to switch between the different animations
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
		//Skip the update cycle if it's not the players turn yet
        if (!GameManager.instance.PlayersTurn) 
		{
			return;
		}

		horizontal 	= 	(int)Input.GetAxisRaw ("Horizontal");
		vertical	=	(int)Input.GetAxisRaw ("Vertical");

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

		//"reset" player's movement
		//horizontal = 0, vertical = 0;
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

	/// <summary>
	/// Check 2D Collision with scene triggers (such as exit)
	/// </summary>
	/// <param name="trigger">the trigger being hit</param>
	private void OnTriggerEnter2D (Collider2D trigger)
	{
		//Advance to the next level if the trigger is an exit
		if (trigger.tag == "Exit") {
			//Start the next level after a short delay
			Invoke ("NextLevel", nextLevelDelay);


			//Disables the player object since the level is over
			enabled = false;
		} 
		//Replenish the player's food meter
		else if (trigger.tag == "Food") {
			food += pointsPerFood;
			//Disable the item
			trigger.gameObject.SetActive (false);
		}
		//Replenish the player's stamina meter
		else if (trigger.tag == "Soda") 
		{
			stamina += pointsPersoda;
			//Disable the item
			trigger.gameObject.SetActive (false);
		}
	}

	/// <summary>
	/// Manages the hunger meter
	/// </summary>
	/// <param name="loss">How much food is lost</param>
    public void LoseFood(int loss)
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
