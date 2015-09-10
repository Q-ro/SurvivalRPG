using UnityEngine;
using System.Collections;

public class Enemy : MovingObject {

	//The Food cost of fighting this enemy
	public int playerFoodCost;

	//Stores an instance to our animator, to be able to switch between the different animations
	private Animator animator;

	//Stores the enemy transform to be used during movement in the map sreen
	private Transform target;

	//Flag to stablish if a turn should be skiped
	private bool skipMove; 

	//The enemy stats
	private int health;
	private int stamina;

	// Use this for initialization
	protected override void Start () 
	{
		//GameManager.instance;

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
		// Enemies are only allowed to move once per turn
		if (skipMove) 
		{
			skipMove = false;
			return;
		}

		base.AttempMove <T>(xDireccion, yDireccion);

		skipMove = true;
	}

	/// <summary>
	/// Moves the enemy.
	/// </summary>
	public void MoveEnemy()
	{
		int xDirection, yDirection;

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

	protected override void OnCantMove<T> (T component)
	{

		Player hitPlayer = (Player)component;
		hitPlayer.LoseFood (playerFoodCost);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
