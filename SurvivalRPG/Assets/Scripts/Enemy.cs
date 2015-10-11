using UnityEngine;
using System.Collections;

public class Enemy : MovingObject {


   

    private Animator animator;  // Stores an instance to our animator, to be able to switch between the different animations
    private Transform target;   // Stores the enemy transform to be used during movement in the map sreen
    private bool skipMove;      // Flag to stablish if a turn should be skiped

	//The enemy stats
	private int health;         // Stores the enemy hitpoints
	private int stamina;        // Stores the stamina ("mana")
    public int playerFoodCost;  // The Food cost of fighting this enemy

    public AudioClip attackSound1;
    public AudioClip attackSound2;
    public AudioClip moveSound1;
    public AudioClip moveSound2;

	// Use this for initialization
	protected override void Start () 
	{
        //Makes the enemy add itself to the list of enemies
        GameManager.instance.AddEnemyToList(this);

		// Get the enemy animator.
		animator = GetComponent<Animator>();
		// Set the target of the enemy movement to the current position of the player
		target = GameObject.FindGameObjectWithTag ("Player").transform;

		base.Start ();	
	}

	/// <summary>
	/// Moves the enemy towards a designated target (player)
	/// </summary>
	/// <param name="xDireccion">X direccion.</param>
	/// <param name="yDireccion">Y direccion.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	protected override void AttempMove<T> (int xDireccion, int yDireccion)
	{
        //Stores the object we collided with (if any)
        RaycastHit2D hit;

		// Enemies are only allowed to move once per turn
		if (skipMove) 
		{
			skipMove = false;
			return;
		}

		base.AttempMove <T>(xDireccion, yDireccion);

        if (Move(xDireccion, yDireccion, out hit))
        {
            // then play a random step soundeffect from the list
            SoundManager.instance.RandomSFX(moveSound1, moveSound2);
        }

        // Stops turns from happenign till every one ahs taken their turns
		skipMove = true;
	}

	/// <summary>
	/// Moves the enemy.
	/// </summary>
	public void MoveEnemy()
	{
		int xDirection = 0, yDirection = 0;

        //Is the player is at the same X position but a different Y then move towards Y
		if (Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon) {
			if (target.position.y > transform.position.y) 
			{
				yDirection = 1;
			} 
			else 
			{
				yDirection = -1;
			}
		}
        // Otherwise move on the X axis towards the player
		else 
		{
			if (target.position.x > transform.position.x) 
			{
				xDirection = 1;
			} 
			else 
			{
				xDirection = -1;
			}

		}

		AttempMove<Player> (xDirection, yDirection);

	}

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">A generic compenet of the type player</typeparam>
    /// <param name="component"></param>
	protected override void OnCantMove<T> (T component)
	{
		Player hitPlayer = component as Player;

        animator.SetTrigger("EnemyAttack");

        SoundManager.instance.RandomSFX(attackSound1, attackSound2);

        //Decreese the player's food by the cost of batteling this enemy
		hitPlayer.LoseFood (playerFoodCost);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
