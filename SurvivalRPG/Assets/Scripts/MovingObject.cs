using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {

    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rigidBody;
    private float inverseMoveTime;

	// Use this for initialization
	protected virtual void Start () 
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
	}

    protected IEnumerator SmoothMovement(Vector3 end) 
    {
        float squareRemaningDistance = (transform.position - end).sqrMagnitude;

        while(squareRemaningDistance > 0)
        {
            Vector3 newPosition = Vector3.MoveTowards(rigidBody.position,end,inverseMoveTime * Time.deltaTime);
            rigidBody.MovePosition(newPosition);
            squareRemaningDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }
    }

    protected bool Move (int xDireccion, int yDireccion,  out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDireccion, yDireccion);

        boxCollider.enabled = false;

        hit = Physics2D.Linecast(start, end, blockingLayer);

        boxCollider.enabled = true;

        if(hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }

        return false;
    }

    protected virtual void AttempMove <T> (int xDireccion, int yDireccion)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDireccion, yDireccion, out hit);

        if(hit.transform != null)
        {
            T hitComponent = hit.transform.GetComponent<T>();

            if(!canMove && hitComponent != null)
            {
                OnCantMove(hitComponent);
            }
        }
        else
        {
            return;
        }
    }


    protected abstract void OnCantMove<T>(T component)
           where T : Component;
	
	// Update is called once per frame
	void Update () {
	
	}
}
