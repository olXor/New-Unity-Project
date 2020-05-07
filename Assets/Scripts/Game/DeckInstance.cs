using UnityEngine;
using System.Collections.Generic;

namespace cg {
    public class DeckInstance : Square {
        public Player player { get; set; }

        //index is from the back of the deck, so that drawing is more runtime efficient
        public void addCard(CardInstance card, int index) {
            if (index < 0 || index > cards.Count)
                return;
            else
                cards.Insert(cards.Count - index, card);
            if (boardManager != null)
                boardManager.moveCardToSquare(card, this);
        }

        public CardInstance drawCard() {
            if (cards.Count == 0)
                return null;
            CardInstance drawnCard = cards[cards.Count - 1];
            cards.RemoveAt(cards.Count - 1);
            return drawnCard;
        }
    }
}
