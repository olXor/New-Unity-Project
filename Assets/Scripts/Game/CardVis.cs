using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace cg
{
    public class CardVis : MonoBehaviour
    {
        public Card card;
        public SpriteRenderer picture;
        public TextMeshPro nameText;
        public TextMeshPro powerText;

        public void Awake()
        {
            loadCard(card, null);
        }

        public void loadCard(Card newCard, Player player = null)
        {
            if (newCard == null)
                return;
            card = newCard;
            picture.sprite = card.picture;
            nameText.text = card.cardName;

            if (player != null)
                nameText.faceColor = player.color;
        }
    }
}
