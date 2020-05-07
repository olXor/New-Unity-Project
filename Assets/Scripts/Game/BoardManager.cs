using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace cg
{
    public enum BoardLocationEnum {
        none = 0,
        board = 1,
        player1Hand = 2,
        player2Hand = 3,
        player1Deck = 4,
        player2Deck = 5,
        player1Graveyard = 6,
        player2Graveyard = 7
    }

    public enum BoardActionEnum {
        none = 0,
        deploy = 1,
        movement = 2,
        other = 3
    }

    public class BoardManager : MonoBehaviour
    {
        protected GameManager gameManager;
        public List<List<Square>> squares { get; } = new List<List<Square>>();
        public List<Player> players { get; } = new List<Player>();
        public List<Square> playerHandSquares { get; } = new List<Square>();
        public List<DeckInstance> playerDecks { get; } = new List<DeckInstance>();
        public List<Graveyard> playerGraveyards { get; } = new List<Graveyard>();

        public int handSize = 7;

        protected bool initialized = false;

        public virtual void initialize()
        {
            initialized = true;
        }

        protected virtual void Start()
        {
            initialize();
        }

        protected virtual void Update()
        {
        }

        public void setGameManager(GameManager newManager)
        {
            gameManager = newManager;
        }

        public void registerPlayers() {
            if (players.Count != playerHandSquares.Count || players.Count != playerDecks.Count || players.Count != playerGraveyards.Count)
                throw new System.Exception("BoardManager: players.Count doesn't match playerHandSquare, playerDeck, and playerGraveyard counts");
            for (int i = 0; i < players.Count; i++) {
                if (players[i] == null)
                    throw new System.Exception("BoardManager: tried to register null player");
                if (playerHandSquares[i] == null)
                    throw new System.Exception("BoardManager: tried to register null playerHandSquare");
                if (playerDecks[i] == null)
                    throw new System.Exception("BoardManager: tried to register null playerDeck");
                if (playerGraveyards[i] == null)
                    throw new System.Exception("BoardManager: tried to register null playerGraveyard");

                    players[i].handSquare = playerHandSquares[i];
                players[i].deck = playerDecks[i];
                playerDecks[i].player = players[i];
                players[i].graveyard = playerGraveyards[i];
                playerGraveyards[i].player = players[i];
            }
        }

        public void drawToHandFill() {
            for (int i = 0; i < playerHandSquares.Count; i++) {
                while (playerHandSquares[i].cards.Count < handSize && drawCard(i)) { }
            }
        }

        public virtual bool drawCard(int playerIndex) {
            if (playerIndex >= playerHandSquares.Count || playerIndex >= playerDecks.Count)
                return false;
            CardInstance drawnCard = playerDecks[playerIndex].drawCard();
            if (drawnCard == null)
                return false;
            moveCardToSquare(drawnCard, playerHandSquares[playerIndex]);
            return true;
        }

        public virtual void moveCardToSquare(CardInstance card, Square square)
        {
            Square prevSquare = card.square;
            if (prevSquare == square || square == null)
                return;

            if (prevSquare != null)
                prevSquare.cards.Remove(card);
            square.cards.Add(card);
            card.square = square;
        }

        //row == -1 for player 1 hand, -2 for player 2 hand
        public void moveCardToSquareByPos(CardInstance card, int row, int col)
        {
            Square sq;
            if (row == -1)
                sq = playerHandSquares[0];
            else if (row == -2)
                sq = playerHandSquares[1];
            else if (col >= 0 && col < squares.Count && row >= 0 && row < squares[col].Count)
                sq = squares[col][row];
            else
                throw new System.Exception("moveCardToSquareByPos: invalid square position");

            moveCardToSquare(card, sq);
        }

        public virtual void addCardToBoard(CardInstance card)
        {
            card.boardManager = this;
        }

        public bool initiateDeployAction(CardInstance card, Square square) {
            if (!gameManager.isPlayerTurn(card.owner))
                return false;
            if (square.boardLocation != BoardLocationEnum.board)
                return false;   //must deploy to square on board
            ActionTargetingInfo tInfo = new ActionTargetingInfo {
                player = card.owner,
                sourceCard = card,
                targetSquares = new List<Square> {
                    square
                }
            };

            for (int i = 0; i < card.deployActions.Count; i++) {
                if (card.deployActions[i].execute(0, tInfo)) {
                    drawToHandFill();
                    return true;
                }
            }

            return false;
        }

        public bool initiateDeployActionByPos(CardInstance card, int row, int col)
        {
            Square sq;
            if (col >= 0 && col < squares.Count && row >= 0 && row < squares[col].Count)
                sq = squares[col][row];
            else
                return false;
                //throw new System.Exception("initiateDeployActionByPos: must deploy to square on board");

            return initiateDeployAction(card, sq);
        }

        public bool initiateMovementAction(CardInstance card, Square square) {
            if (!gameManager.isPlayerTurn(card.owner))
                return false;
            if (square.boardLocation != BoardLocationEnum.board)
                return false;   //must move to square on board
            ActionTargetingInfo tInfo = new ActionTargetingInfo {
                player = card.owner,
                sourceCard = card,
                targetSquares = new List<Square> {
                    square
                }
            };

            for (int i = 0; i < card.movementActions.Count; i++) {
                if (card.movementActions[i].execute(0, tInfo))
                    return true;
            }

            return false;
        }

        public bool initiateMovementActionByPos(CardInstance card, int row, int col)
        {
            Square sq;
            if (col >= 0 && col < squares.Count && row >= 0 && row < squares[col].Count)
                sq = squares[col][row];
            else
                return false;
                //throw new System.Exception("initiateMovementActionByPos: must deploy to square on board");

            return initiateMovementAction(card, sq);
        }

        public bool initiateAction(CardInstance card, Square square, BoardActionEnum actionType, bool endTurnOnSuccess = true) {
            if (square == null)
                return false;
            bool success = false;
            if(actionType == BoardActionEnum.deploy) {
                success = initiateDeployAction(card, square);
            }
            else if(actionType == BoardActionEnum.movement) {
                success = initiateMovementAction(card, square);
            }
            if (endTurnOnSuccess && success)
                gameManager.finishPlayerTurn(card.owner);

            return success;
        }
    }
}
