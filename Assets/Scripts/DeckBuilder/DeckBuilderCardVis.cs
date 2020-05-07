using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

namespace cg {
    public class DeckBuilderCardVis : MonoBehaviour {
        public Image picture;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI powerText;

        Card card;
        public DeckBuilderDeckPanel deckListPanel { get; set; }

        public void loadCard(Card newCard) {
            card = newCard;
            picture.sprite = card.picture;
            nameText.text = card.cardName;
            powerText.text = card.basePower.ToString();
        }

        public void addToList() {
            if (deckListPanel != null)
                deckListPanel.addCard(card);
        }
    }
}
