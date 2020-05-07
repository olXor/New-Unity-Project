using UnityEngine;
using System.Collections.Generic;

namespace cg {
    public class DeckBuilderCardPanel : MonoBehaviour {
        public DeckBuilderCardVis templateVis;
        public DeckBuilderDeckPanel deckPanel;

        Card[] allCards;
        List<DeckBuilderCardVis> shownCards { get; } = new List<DeckBuilderCardVis>();

        public void Start() {
            allCards = Resources.LoadAll<Card>("Cards/IndividualCards");
            for (int i = 0; i < allCards.Length; i++) {
                DeckBuilderCardVis vis = Instantiate(templateVis, Vector3.zero, Quaternion.identity);
                vis.transform.SetParent(transform.parent);
                vis.loadCard(allCards[i]);
                vis.deckListPanel = deckPanel;
                shownCards.Add(vis);
            }

            updateCardPositions();
        }

        public void updateCardPositions() {
            if (shownCards.Count == 0)
                return;

            RectTransform rt = GetComponent<RectTransform>();
            float minX = rt.localPosition.x - rt.rect.width * rt.localScale.x / 2;
            float maxX = rt.localPosition.x + rt.rect.width * rt.localScale.x / 2;
            float minY = rt.localPosition.y - rt.rect.height * rt.localScale.y / 2;
            float maxY = rt.localPosition.y + rt.rect.height * rt.localScale.y / 2;

            RectTransform cardRT = shownCards[0].GetComponent<RectTransform>();

            int numCardsPerRow = (int)((maxX - minX) / cardRT.rect.width / cardRT.localScale.x);

            for (int i = 0; i < shownCards.Count; i++) {
                float xPosition = ((0.5f + (i % numCardsPerRow)) / numCardsPerRow) * (maxX - minX) + minX;
                float yPosition = maxY - (0.5f + (i / numCardsPerRow)) * cardRT.rect.height * cardRT.localScale.y;
                shownCards[i].transform.localPosition = new Vector3(xPosition, yPosition);
            }
        }
    }
}
