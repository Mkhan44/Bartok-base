using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bartok : MonoBehaviour {
	
	static public Bartok S;
	public TextAsset deckXML;
	public TextAsset layoutXML;
	public Vector3 layoutCenter = Vector3.zero;
	
	public bool __________;
	
	public Deck deck;
	public List<CardBartok> drawPile;
	public List<CardBartok> discardPile;

	public BartokLayout layout;
	public Transform layoutAnchor;


	void Awake() {
		S = this;
	}
	
	// Use this for initialization
	void Start () {
		
		deck = GetComponent<Deck> (); //Get the deck.
		deck.InitDeck (deckXML.text); //Pass DeckXML to it.
		Deck.Shuffle (ref deck.cards); //This shuffles the deck.
		//The ref keyword passes a reference to deck.cards, which allows deck.cards to be modified by Deck.Shuffle()
		layout = GetComponent<BartokLayout> (); //Get the layout.
		layout.ReadLayout (layoutXML.text); //Pass LayoutXML to it.

		drawPile = UpgradeCardsList (deck.cards);
	}

	//UpgradeCardsList casts the cards in 1CD to be CardBartoks
	//Of course, they were all along, but this lets Unity know it.
	List<CardBartok> UpgradeCardList(List<Card> lCD) {
		List<CardBartok> lCB = new List<CardBartok>();
		foreach(Card tCD in lCD) {
			lCB.Add (tCD as CardBartok);
		}
		return(lCB);
	}
	
}
