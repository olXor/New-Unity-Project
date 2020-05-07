using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace cg
{
    public class OneVOneManager : GameManager
    {
        private ScreenManager screenManager = null;  //same as boardManager, because I am bad at inheritance
        public Player player1;
        public Player player2;
        public DeckInstance player1Deck;
        public DeckInstance player2Deck;
        public Graveyard player1Graveyard;
        public Graveyard player2Graveyard;
        public SceneController sceneController;

        public override void updateBoardManager(BoardManager newManager)
        {
            if (!(newManager is ScreenManager))
                throw new System.Exception("OneVOneManager's boardManager must be screenManager");

            screenManager = (ScreenManager)newManager;
            base.updateBoardManager(newManager);
        }

        protected override void Start()
        {
            players = new List<List<Player>>();
            players.Add(new List<Player>());
            players[0].Add(player1);
            players.Add(new List<Player>());
            players[1].Add(player2);

            base.Start();

            if (player1 is LocalPlayer)
                ((LocalPlayer)player1).screenManager = screenManager;
            if (player2 is LocalPlayer)
                ((LocalPlayer)player2).screenManager = screenManager;

            player1.initialize();
            player2.initialize();

            boardManager.players.Add(player1);
            boardManager.players.Add(player2);
            player1Deck.boardManager = boardManager;
            boardManager.playerDecks.Add(player1Deck);
            boardManager.playerGraveyards.Add(player1Graveyard);
            player2Deck.boardManager = boardManager;
            boardManager.playerDecks.Add(player2Deck);
            boardManager.playerGraveyards.Add(player2Graveyard);
            boardManager.registerPlayers();

            StartCoroutine(completeLoad());
        }

        private System.Collections.IEnumerator completeLoad() {
            yield return new WaitUntil(() => sceneController.preloadLoaded);

            GameObject globalData = GameObject.Find("GlobalData");
            Deck deck = globalData.transform.Find("Deck1").GetComponent<Deck>();


            if (deck != null) {
                for (int i = 0; i < deck.cards.Count; i++) {
                    CardInstance c = Instantiate(cardTemplate);
                    c.owner = player1;
                    c.transformToCard(deck.cards[i]);
                    boardManager.addCardToBoard(c);
                    boardManager.playerDecks[0].addCard(c, 0);
                }

                for (int i = 0; i < deck.cards.Count; i++) {
                    CardInstance c = Instantiate(cardTemplate);
                    c.owner = player2;
                    c.transformToCard(deck.cards[i]);
                    boardManager.addCardToBoard(c);
                    boardManager.playerDecks[1].addCard(c, 0);
                }
            }
            boardManager.drawToHandFill();
            setTurn(0);
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}
