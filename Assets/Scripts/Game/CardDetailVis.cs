using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace cg
{
    public class CardDetailVis : MonoBehaviour
    {
        public Card card;
        public Image picture;
        public TextMeshProUGUI cardName;
        public TextMeshProUGUI abilityText;
        public TextMeshProUGUI flavorText;
        public TextMeshProUGUI basePower;

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
            cardName.text = card.cardName;
            abilityText.text = card.abilityText;
            flavorText.text = card.flavorText;
            basePower.text = card.basePower.ToString();
        }
    }
}
