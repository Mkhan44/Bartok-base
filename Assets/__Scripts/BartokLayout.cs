using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//SlotDef class is not based on MonoBehaviour, so it doesn't need its own file.
[System.Serializable] //Makes SlotDef able to be seen in the UNity Inspector
public class SlotDef{
	public float x;
	public float y;
	public bool faceUp = false;
	public string layerName = "Default";
	public int layerID = 0;
	public int id;
	public List<int> hiddenBy = new List<int> (); //Unused in Bartok.
	public float rot; //rotation of hands.
	public string type = "slot";
	public Vector2 stagger;
	public int player; //player number of a hand.
	public Vector3 pos; //pos derived from x, y, & multiplier.
}

public class BartokLayout : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
