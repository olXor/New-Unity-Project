namespace cg {
    public class FightAsRook : Action {
        public override bool execute(float d, ActionTargetingInfo targetInfo) {
            if (!isValidTarget(d, targetInfo))
                return false;

            ActionTargetingInfo tInfo = (targetInfo ?? defaultTargetingInfo);

            if (tInfo.sourceCard == null || tInfo.targetSquares.Count < 1 || tInfo.targetSquares[0].cards.Count != 1 || tInfo.targetSquares[0].cards[0] == null)
                return false;

            if (tInfo.sourceCard.fight(tInfo.targetSquares[0].cards[0]) && !tInfo.sourceCard.isDead)
                tInfo.sourceCard.boardManager.moveCardToSquare(tInfo.sourceCard, tInfo.targetSquares[0]);

            return true;
        }

        public override bool isValidTarget(float d, ActionTargetingInfo targetInfo) {
            ActionTargetingInfo tInfo = (targetInfo ?? defaultTargetingInfo);

            if (tInfo.targetSquares.Count != 1)
                return false;

            Square sourceSquare = tInfo.sourceCard.square;
            Square targetSquare = tInfo.targetSquares[0];

            if (tInfo.targetSquares.Count != 1)
                return false;
            if (targetSquare == null || tInfo.sourceCard == null || sourceSquare == null)
                return false;
            if (tInfo.sourceCard.owner == null || tInfo.player == null || tInfo.sourceCard.owner != tInfo.player)
                return false;

            if (sourceSquare.boardLocation != BoardLocationEnum.board)
                return false;

            //target must be occupied by opponent
            if (!targetSquare.isOccupied())
                return false;

            if (tInfo.sourceCard == null || tInfo.targetSquares.Count < 1 || tInfo.targetSquares[0].cards.Count != 1 || tInfo.targetSquares[0].cards[0] == null)
                return false;
            if (tInfo.targetSquares[0].cards[0].owner == tInfo.sourceCard.owner)
                return false;

            if (sourceSquare.col == targetSquare.col && sourceSquare.row == targetSquare.row)
                return false;
            if (sourceSquare.col != targetSquare.col && sourceSquare.row != targetSquare.row)
                return false;

            if (sourceSquare.col == targetSquare.col) {
                int propDir = (sourceSquare.row < targetSquare.row ? 1 : -1);
                for (int row = sourceSquare.row + propDir; (propDir > 0 ? row < targetSquare.row : row > targetSquare.row); row += propDir) {
                    if (tInfo.sourceCard.boardManager.squares[sourceSquare.col][row].isOccupied())
                        return false;
                }
            }
            else if (sourceSquare.row == targetSquare.row) {
                int propDir = (sourceSquare.col < targetSquare.col ? 1 : -1);
                for (int col = sourceSquare.col + propDir; (propDir > 0 ? col < targetSquare.col : col > targetSquare.col); col += propDir) {
                    if (tInfo.sourceCard.boardManager.squares[col][sourceSquare.row].isOccupied())
                        return false;
                }
            }

            return true;
        }
    }
}
