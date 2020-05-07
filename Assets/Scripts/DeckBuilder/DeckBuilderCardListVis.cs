using UnityEngine;
using System.Collections;
using TMPro;

namespace cg {
    public class DeckBuilderCardListVis : MonoBehaviour {
        public TextMeshProUGUI nameText;

        Card card;
        public DeckBuilderDeckPanel deckListPanel { get; set; }

        public void loadCard(Card newCard) {
            card = newCard;
            nameText.text = card.cardName;
        }

        public void removeFromList() {
            if (deckListPanel != null)
                deckListPanel.removeCard(card);
        }
    }
}
