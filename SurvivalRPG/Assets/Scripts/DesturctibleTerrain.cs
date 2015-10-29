using UnityEngine;
using System.Collections;

public class DesturctibleTerrain : MonoBehaviour {

    //Sprite to be shown when the object takes damage
    public Sprite dmgsprite;
    //The object health
    public int hp = 4;

    public AudioClip chopSound1;
    public AudioClip chopSound2;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageObject(int loss)
    {
        SoundManager.instance.RandomSFX(chopSound1, chopSound2);
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
