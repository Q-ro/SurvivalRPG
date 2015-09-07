using UnityEngine;
using System.Collections;

public class DesturctibleTerrain : MonoBehaviour {

    //Sprite to be shown when the object takes damage
    public Sprite dmgsprite;
    //The object health
    public int hp = 4;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageObject(int loss)
    {
        spriteRenderer.sprite = dmgsprite;
        //Remove health acordingly
        hp -= loss;
        //If the object died, kill it
        if(hp <= 0)
        {
            gameObject.SetActive(false);
        }

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
