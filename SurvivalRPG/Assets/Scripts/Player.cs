using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MovingObject {

	int horizontal = 0, vertical = 0;   // A flag that keeps track of player's movement orientation
    public int objectDamage = 1;        // How much dmg the player deals to destructible objects

    //The amount of points refilled by collecting edible items
    public int pointsPerFood = 10;
    public int pointsPersoda = 20;

    public float nextLevelDelay = 1f;   // Delay to be used when adavancing to the next level
    private Animator animator;          // Store an instance to our animator, to be able to switch between the different animations

    // Sound effects
    public AudioClip moveSound1;
    public AudioClip moveSound2;

    public AudioClip damagedSound1;
    public AudioClip damagedSound2;

    public AudioClip pickupSound;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;
    public AudioClip nextlevelSound;

    //Player stats
    // Stores the current "hunger" level of the player
    private int _food;
    
    public int food
    {
        get { return _food; }
        private set { _food = value; foodText.text = "Food " + food; }
    }

    private int _health;                 // Stores the health level of the player
    public int health
    {
        get { return _health; }
        private set { _health = value; healthText.text = "Health " + _health; }
    } 
       
    private int stamina;                // Stores the stamina level of the player

    public Text foodText;
    public Text healthText;

    private Vector2 touchOrigin = -Vector2.one;

	/// <summary>
	/// Initialize player specific logic 
	/// </summary>
	protected override void Start () 
    {
        //Get the player's animator to be used to trigger animation changes
        animator = GetComponent<Animator>();

        this.foodText = GameObject.Find("FoodText").GetComponent("Text") as Text;
        this.healthText = GameObject.Find("HealthText").GetComponent("Text") as Text;

        //Get the Player's "hunger" level between levels
        food = GameManager.instance.playerFoodPoints;
        health = GameManager.instance.playerHealthPoints;
        stamina = GameManager.instance.playerStaminaPoints;

       

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
        if (food > 0)
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

        // If the player was able to take a step
        if(Move(xDireccion,yDireccion,out hit))
        {
            // then play a random step soundeffect from the list
            //SoundManager.instance.RandomSFX(moveSound1, moveSound2);
        }
        
        //The player's turn ends after 1 movement
        GameManager.instance.playersTurn = false;

    }

    private void CheckGameOver()
    {
        if(health <= 0)
        {
            SoundManager.instance.musicSource.Stop();
            SoundManager.instance.PlaySingle(gameOverSound);
            GameManager.instance.GameOver();
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
		//Skip the update cycle if it's not the players turn yet
        if (!GameManager.instance.playersTurn) 
		{
			return;
		}

		horizontal 	= 	(int)Input.GetAxisRaw ("Horizontal");
		vertical	=	(int)Input.GetAxisRaw ("Vertical");

        #if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR

        //Lock the movement to X and Y axis (no vertical movement)
        if(horizontal !=0)
        {
            vertical = 0;
        }
        //if (vertical!= 0)
        //{
        //    horizontal = 0;
        //}

        #else

        if(Input.touchCount > 0)
        {
            Touch myTouch = Input.touches[0];

            if(myTouch.phase == TouchPhase.Began)
            {
                touchOrigin = myTouch.position;
            }
            else if(myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
            {
                Vector2 touchEnd = myTouch.position;
                float x = touchEnd.x - touchOrigin.x;
                float y = touchEnd.y - touchOrigin.y;
                touchOrigin.x = -1;
                if(Mathf.Abs(x) > Mathf.Abs(y))
                {
                    if (x > 0)
                    {
                        horizontal = 1;
                    }
                    else
                    {
                        horizontal = -1;
                    }
                        
                }
                else
                {
                    if (y > 0)
                    {
                        vertical = 1;
                    }
                    else
                    {
                        vertical = -1;
                    }
                }
            }
        }

        #endif

        if(horizontal !=0 || vertical != 0)
        {
            AttempMove<DesturctibleTerrain>(horizontal, vertical);
        }

		//"reset" player's movement
//        horizontal = 0;
//        vertical = 0;
	}

    protected override void OnCantMove <T> (T component)
    {
        //Set hitObstacle to equal the component passed in as a parameter.
        DesturctibleTerrain hitObstacle = component as DesturctibleTerrain;

        //Damage the object
        hitObstacle.DamageObject(objectDamage);

        //Switch the animator state to chop
        animator.SetTrigger("PlayerChop");
    }

    /// <summary>
    /// Advance the stage to the next level
    /// </summary>
    private void NextLevel()
    {
        //Application.LoadLevel(Application.loadedLevel);
		GameManager.instance.loadNextFloor();
    }

	/// <summary>
	/// Check 2D Collision with scene triggers (such as exit)
	/// </summary>
	/// <param name="trigger">the trigger being hit</param>
	private void OnTriggerEnter2D (Collider2D trigger)
	{
		//Advance to the next level if the trigger is an exit
		if (trigger.tag == "Exit") 
        {
			//Disables the player object since the level is over
			//this.enabled = false;

			trigger.enabled = false;

			Destroy(trigger);

			//Start the next level after a short delay
			Invoke ("NextLevel", nextLevelDelay);

            SoundManager.instance.PlaySingle(nextlevelSound);

		} 
		//Replenish the player's food meter
		else if (trigger.tag == "Food")
        {
			food += pointsPerFood;
            SoundManager.instance.PlaySingle(pickupSound);

			//Disable the item
			trigger.gameObject.SetActive (false);
		}
		//Replenish the player's stamina meter
		else if (trigger.tag == "Soda") 
		{
			stamina += pointsPersoda;
            SoundManager.instance.PlaySingle(pickupSound);

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
        animator.SetTrigger("PlayerHit");
        SoundManager.instance.RandomSFX(damagedSound1, damagedSound2);

        if (food > 0)
        {
            // If the food lost  diferential is more than or equal to zero, decrese player's food level
            if(food - loss >= 0)
            {
                food -= loss;

            }
            // Else set Food to Zero and damage the player by the residue of the operation
            else 
            {
                int residualDamage = loss - food;
                health -= residualDamage;
                food = 0;
                //Check is he died from taking too much damage
                CheckGameOver();
            }
            
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
