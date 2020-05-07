using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace cg
{
    class BoardPositionAllowance
    {
        public bool p1Hand = true;
        public bool p2Hand = true;
        public bool board = true;
    }

    public enum BoardOverlayType {
        none,
        detail
    }

    public class ScreenManager : BoardManager
    {
        public int xSquares;
        public int ySquares;

        public GameObject board;
        public Square templateSquare;
        public CardDetailVis cardDetail;

        CardInstance dragCard = null;
        BoardOverlayType curBoardOverlay = BoardOverlayType.none;

        public Canvas canvas;

        BoardPositionAllowance interactableBoardPositions;

        public override void initialize()
        {
            if (initialized)
                return;

            RectTransform rt = GetComponent<RectTransform>();
            float minX = rt.localPosition.x - rt.rect.width / 2;
            float maxX = rt.localPosition.x + rt.rect.width / 2;
            float minY = rt.localPosition.y - rt.rect.height / 2;
            float maxY = rt.localPosition.y + rt.rect.height / 2;

            float sqX = (maxX - minX) / xSquares;
            float sqY = (maxY - minY) / ySquares;

            for (int i = 0; i < xSquares; i++)
            {
                List<Square> row = new List<Square>();
                for (int j = 0; j < ySquares; j++)
                {
                    Square sq = Instantiate(templateSquare, Vector3.zero, Quaternion.identity);
                    sq.transform.SetParent(canvas.transform);
                    sq.transform.localPosition = new Vector3(minX + sqX / 2 + i * sqX, minY + sqY / 2 + j * sqY);
                    sq.col = i;
                    sq.row = j;
                    sq.boardLocation = BoardLocationEnum.board;
                    sq.boardManager = this;
                    row.Add(sq);
                }
                squares.Add(row);
            }

            Square p1HandSq = Instantiate(templateSquare, Vector3.zero, Quaternion.identity);
            p1HandSq.transform.SetParent(canvas.transform);
            p1HandSq.transform.localPosition = new Vector3((minX + maxX) / 2, minY - sqY / 2);
            p1HandSq.col = 0;
            p1HandSq.row = -1;
            p1HandSq.boardLocation = BoardLocationEnum.player1Hand;
            p1HandSq.boardManager = this;
            playerHandSquares.Add(p1HandSq);
            Square p2HandSq = Instantiate(templateSquare, Vector3.zero, Quaternion.identity);
            p2HandSq.transform.SetParent(canvas.transform);
            p2HandSq.transform.localPosition = new Vector3((minX + maxX) / 2, maxY + sqY / 2);
            p2HandSq.col = 0;
            p2HandSq.row = -2;
            p2HandSq.boardLocation = BoardLocationEnum.player2Hand;
            p2HandSq.boardManager = this;
            playerHandSquares.Add(p2HandSq);

            interactableBoardPositions = new BoardPositionAllowance();

            base.initialize();
        }

        protected override void Update()
        {
            base.Update();
            if(dragCard != null)
            {
                dragCard.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }

        protected bool allowedToDrag(CardInstance card) {
            if (card.square == null)
                return false;
            if (card.square.boardLocation == BoardLocationEnum.board || card.square.boardLocation == BoardLocationEnum.player1Hand || card.square.boardLocation == BoardLocationEnum.player2Hand)
                return true;
            return false;
        }

        public void setDragCard(CardInstance newCard) {
            if (!allowedToDrag(newCard))
                return;
            if (dragCard == null) {
                dragCard = newCard;
                dragCard.GetComponent<SortingGroup>().sortingLayerName = "DragCard";
                if(dragCard.square == null) {
                }
                else if (dragCard.square.boardLocation == BoardLocationEnum.board)
                    highlightValidSquares(dragCard, BoardActionEnum.movement);
                else if(dragCard.square.boardLocation == BoardLocationEnum.player1Hand || dragCard.square.boardLocation == BoardLocationEnum.player2Hand)
                    highlightValidSquares(dragCard, BoardActionEnum.deploy);
            }
        }

        public void dropDragCard() {
            if (dragCard == null)
                return;
            Square sq = getNearestSquare(dragCard.transform.localPosition);
            bool actionSuccess = false;
            if (dragCard.square.boardLocation == BoardLocationEnum.player1Hand || dragCard.square.boardLocation == BoardLocationEnum.player2Hand)
                actionSuccess = initiateAction(dragCard, sq, BoardActionEnum.deploy);
            else if (dragCard.square.boardLocation == BoardLocationEnum.board)
                actionSuccess = initiateAction(dragCard, sq, BoardActionEnum.movement);
            if (!actionSuccess && dragCard.square != null)
                moveCardToSquare(dragCard, dragCard.square);
            dragCard.GetComponent<SortingGroup>().sortingLayerName = "Default";
            dragCard = null;
            clearHighlightSquares();
        }

        public void highlightValidSquares(CardInstance card, BoardActionEnum actionType) {
            ActionTargetingInfo targetInfo = new ActionTargetingInfo {
                player = card.owner,
                sourceCard = card,
                targetSquares = new List<Square> {
                    squares[0][0]
                }
            };
            for (int i = 0; i < squares.Count; i++) {
                for (int j = 0; j < squares[i].Count; j++) {
                    targetInfo.targetSquares[0] = squares[i][j];
                    if (card.isValidActionTarget(targetInfo, actionType))
                        ((ScreenSquare)squares[i][j]).setHighlight(true);
                }
            }
        }

        public void clearHighlightSquares() {
            for (int i = 0; i < squares.Count; i++) {
                for (int j = 0; j < squares[i].Count; j++) {
                    ((ScreenSquare)squares[i][j]).setHighlight(false);
                }
            }
        }

        //looks at interactableBoardPositions variable, because lol
        private Square getNearestSquare(Vector3 localPosition)
        {
            RectTransform rt = GetComponent<RectTransform>();
            float minX = rt.localPosition.x - rt.rect.width / 2;
            float maxX = rt.localPosition.x + rt.rect.width / 2;
            float minY = rt.localPosition.y - rt.rect.height / 2;
            float maxY = rt.localPosition.y + rt.rect.height / 2;

            float sqX = (maxX - minX) / xSquares;
            float sqY = (maxY - minY) / ySquares;
            if (localPosition.x < minX || localPosition.x >= maxX)
                return null;

            int numX;
            if (localPosition.x < minX)
                numX = 0;
            else if (localPosition.x >= minX + (xSquares - 1) * sqX)
                numX = xSquares - 1;
            else
                numX = (int)((localPosition.x - minX) / sqX);

            int numY;
            if (localPosition.y < minY && interactableBoardPositions.p1Hand) {
                return playerHandSquares[0];
                //numY = 0;
            }
            else if (localPosition.y >= maxY && interactableBoardPositions.p2Hand) {
                return playerHandSquares[1];
                //numY = ySquares - 1;
            }
            else if (interactableBoardPositions.board && localPosition.x >= minX && localPosition.x < maxX && localPosition.y >= minY && localPosition.y < maxY)
                numY = (int)((localPosition.y - minY) / sqY);
            else
                return null;

            return squares[numX][numY];
        }

        public void onMouseEvent(IClickable c, Player player)
        {
            if (dragCard != null && !Input.GetMouseButton(0))
                dropDragCard();

            if(curBoardOverlay == BoardOverlayType.detail) {
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.J)) {
                    curBoardOverlay = BoardOverlayType.none;
                    cardDetail.gameObject.SetActive(false);
                }
                return;
            }

            if (c != null && curBoardOverlay == BoardOverlayType.none) {
                //card detail view
                if (Input.GetKeyDown(KeyCode.J) && (c is CardInstance) && c != null) {
                    cardDetail.loadCard(((CardInstance)c).card, ((CardInstance)c).owner);
                    cardDetail.gameObject.SetActive(true);
                    curBoardOverlay = BoardOverlayType.detail;
                    return;
                }

                //highlight
                if (!Input.GetMouseButton(0))
                    c.OnHighlight();

                //drag
                if (Input.GetMouseButtonDown(0))
                {
                    c.OnLeftClick();
                    if ((c is CardInstance) && ((CardInstance)c).owner == player && gameManager.isPlayerTurn(player))
                        setDragCard((CardInstance)c);
                    return;
                }
                return;
            }
        }

        public override void moveCardToSquare(CardInstance card, Square square)
        {
            Square prevSquare = card.square;
            base.moveCardToSquare(card, square);
            if (square == null)
                card.transform.localPosition = prevSquare.transform.localPosition;
            else if(square != playerHandSquares[0] && square != playerHandSquares[1])
                card.transform.localPosition = square.transform.localPosition;
            if (square == playerHandSquares[0] || prevSquare == playerHandSquares[0])
                updateHandCardPositions(0);
            if (square == playerHandSquares[1] || prevSquare == playerHandSquares[1])
                updateHandCardPositions(1);
        }

        private void updateHandCardPositions(int playerNum)
        {
            if (playerNum != 0 && playerNum != 1)
                throw new System.Exception("updateHandCardPositions: playerNum not 0 or 1");

            RectTransform rt = GetComponent<RectTransform>();
            float minX = rt.localPosition.x - rt.rect.width / 2;
            float maxX = rt.localPosition.x + rt.rect.width / 2;
            float minY = rt.localPosition.y -rt.rect.height / 2;
            float maxY = rt.localPosition.y + rt.rect.height / 2;

            float sqX = (maxX - minX) / xSquares;
            float sqY = (maxY - minY) / ySquares;

            float handY = (playerNum == 0 ? minY - sqY / 2 : maxY + sqY / 2);
            float centerHandX = (minX + maxX) / 2;

            for (int i = 0; i < playerHandSquares[playerNum].cards.Count; i++)
            {
                float locX = centerHandX + sqX * (0.5f + (float)i - (float)playerHandSquares[playerNum].cards.Count / 2);
                playerHandSquares[playerNum].cards[i].transform.localPosition = new Vector3(locX, handY);
            }
        }

        public override void addCardToBoard(CardInstance card)
        {
            card.transform.SetParent(canvas.transform, false);

            base.addCardToBoard(card);
        }
    }
}
