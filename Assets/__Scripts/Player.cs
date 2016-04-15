using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq; //Enables LINQ queries, which will be explained soon.

//The player can either be human or an ai
public enum PlayerType {
	human,
	ai
}

//The individual player of the game
//Note player does NOT extend MonoBehaviour.

public class Player {

	public PlayerType type = PlayerType.ai;
	public int playerNum;

	public List<CardBartok> hand; //The cards in this player's hand.

	public SlotDef handSlotDef;

	//Add a card to the hand.
	public CardBartok AddCard(CardBartok eCB) {
		if (hand == null)
			hand = new List<CardBartok> ();

		//Add the card to the hand.
		hand.Add (eCB);
		FanHand ();
		return(eCB);

		//Sort the cards by rank using LINQ if this is a human.
		if (type == PlayerType.human) {
			CardBartok[] cards = hand.ToArray (); //Copy hand to a new array.

			cards = cards.OrderBy (cd => cd.rank).ToArray ();

			hand = new List<CardBartok> (cards);
			eCB.SetSortingLayerName ("10"); //This sorts the moving card to the top. +
		eCB.eventualSortLayer = handSlotDef.layerName;
		}

	
	}

	


	//Remove a card from the hand.
	public CardBartok RemoveCard(CardBartok cb) {
		hand.Remove (cb);


		FanHand ();
		return(cb);

	}

	public void FanHand() {
	//StartRot is the rotation about Z of the first card.
		float startRot = 0;
		startRot = handSlotDef.rot;
		if (hand.Count > 1) {
			startRot += Bartok.S.handFanDegrees * (hand.Count-1) /2;
		}
		//Then each card is rotated handFanDegrees from that to fan the cards.

		//Move all the cards to their new positions.
		Vector3 pos;
		float rot;
		Quaternion rotQ;
		for (int i=0; i<hand.Count; i++) {
			rot = startRot - Bartok.S.handFanDegrees*i; //Rot about the z axis.
			// ^also adds the rotations of the different players' hands.
			rotQ = Quaternion.Euler (0,0,rot);
			//^Quaternion representing the same rotation as rot.

			//pos is a V3 half a card height above [0,0,0] (i.e., [0,1.75,0]
			pos = Vector3.up *CardBartok.CARD_HEIGHT /2f;

			pos = rotQ *pos;

			pos += handSlotDef.pos;

			pos.z = -0.5f*i;

			//The line below makes sure that the card starts moving immediately.

			if(Bartok.S.phase !=TurnPhase.idle) {
				hand[i].timeStart = 0;
			}

			hand[i].MoveTo (pos , rotQ);
			hand[i].state = CBState.toHand;

			hand[i].transform.localPosition = pos;
			hand[i].transform.rotation = rotQ;
			hand[i].state = CBState.hand;

			hand[i].faceUp = (type == PlayerType.human);
			hand[i].eventualSortLayer = i*4;
			hand[i].SetSortOrder (i*4);

		}

		//The TakeTurn() function enables the AI of the computer players.
		public void TakeTurn() {
			Utils.try(Utils.RoundToPlaces(Time.Time), "Player.TakeTurn");

			if(type == PlayerType.human)return;
			Bartok.S.phase = TurnPhase.waiting;

			CardBartok cb;

			List<CardBartok> validCards = new List<CardBartok>();
			foreach(CardBartok tCB in hand) {
			if(Bartok.S.ValidPlay (tCB)) {
					validCards.Add (tCB);
				}
			}
			//If there are no valid cards.
			if(validCards.Count == 0) {
				cb = AddCard (Bartok.S.Draw ());
				cb.callbackPlayer = this;
				return;
			}

			cb = validCards[Random.Range (0, validCards.Count)];
			RemoveCard (cb);
			Bartok.S.MoveToTarget(cb);
			cb.callbackPlayer = this;
		}

		public void CBCallback(CardBartok tCB) {
			Utils.tr (Utils.RoundToPlaces (Time.time), "Player.CBCallback()", tCB.name, "Player " + playerNum);

			Bartok.S.PassTurn ();
		}
	}



