using System;

namespace cg {
    public class MoveAsBishop : Action {
        public override bool execute(float d, ActionTargetingInfo targetInfo) {
            if (!isValidTarget(d, targetInfo))
                return false;

            ActionTargetingInfo tInfo = (targetInfo ?? defaultTargetingInfo);

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

            //target must be unoccupied
            if (targetSquare.isOccupied())
                return false;

            if (sourceSquare.boardLocation != BoardLocationEnum.board)
                return false;
            if (Math.Abs(sourceSquare.col - targetSquare.col) != Math.Abs(sourceSquare.row - targetSquare.row))
                return false;
            if (sourceSquare.col == targetSquare.col && sourceSquare.row == targetSquare.row)
                return false;

            int rowDir = (targetSquare.row - sourceSquare.row) / Math.Abs(targetSquare.row - sourceSquare.row);
            int colDir = (targetSquare.col - sourceSquare.col) / Math.Abs(targetSquare.col - sourceSquare.col);
            for (int diff = 1; diff < Math.Abs(sourceSquare.col - targetSquare.col); diff++) {
                int row = sourceSquare.row + diff * rowDir;
                int col = sourceSquare.col + diff * colDir;

                if (tInfo.sourceCard.boardManager.squares[col][row].isOccupied())
                    return false;
            }

            return true;
        }
    }
}
