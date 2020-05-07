using UnityEngine;
using System.Collections.Generic;

namespace cg {
    public class Deck : MonoBehaviour {
        public List<Card> cards { get; set; } = new List<Card>();

        public void addCard(Card card) {
            cards.Add(card);
        }

        public void removeCard(Card card) {
            cards.Remove(card);
        }
    }
}
