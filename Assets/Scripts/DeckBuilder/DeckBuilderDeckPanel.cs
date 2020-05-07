using UnityEngine;
using System.Collections.Generic;

namespace cg {
    public class DeckBuilderDeckPanel : MonoBehaviour {
        public DeckBuilderCardListVis templateVis;
        public Deck deck { get; set; }
        List<DeckBuilderCardListVis> visList { get; } = new List<DeckBuilderCardListVis>();

        public SceneController sceneController;

        public void Start() {
            StartCoroutine(completeLoad());
        }

        private System.Collections.IEnumerator completeLoad() {
            yield return new WaitUntil(() => sceneController.preloadLoaded);

            GameObject globalData = GameObject.Find("GlobalData");
            deck = globalData.transform.Find("Deck1").GetComponent<Deck>();
            updateVis();
        }

        public void updateVis() {
            for (int i = 0; i < visList.Count; i++)
                Destroy(visList[i].gameObject);
            visList.Clear();

            for (int i = 0; i < deck.cards.Count; i++) {
                DeckBuilderCardListVis vis = Instantiate(templateVis, Vector3.zero, Quaternion.identity);
                vis.transform.SetParent(transform.parent);
                vis.loadCard(deck.cards[i]);
                vis.deckListPanel = this;
                visList.Add(vis);
            }

            updateCardPositions();
        }

        public void addCard(Card card) {
            deck.addCard(card);
            updateVis();
        }

        public void removeCard(Card card) {
            deck.removeCard(card);
            updateVis();
        }

        public void updateCardPositions() {
            if (visList.Count == 0)
                return;

            RectTransform rt = GetComponent<RectTransform>();
            float minX = rt.localPosition.x - rt.rect.width * rt.localScale.x / 2;
            float maxX = rt.localPosition.x + rt.rect.width * rt.localScale.x / 2;
            float minY = rt.localPosition.y - rt.rect.height * rt.localScale.y / 2;
            float maxY = rt.localPosition.y + rt.rect.height * rt.localScale.y / 2;

            RectTransform cardRT = visList[0].GetComponent<RectTransform>();

            for (int i = 0; i < visList.Count; i++) {
                float xPosition = (minX + maxX) / 2;
                float yPosition = maxY - (0.5f + i) * cardRT.rect.height * cardRT.localScale.y;
                visList[i].transform.localPosition = new Vector3(xPosition, yPosition);
            }
        }

        public void saveDeck() {
        }
    }
}
