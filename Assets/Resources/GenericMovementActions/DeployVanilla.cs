using UnityEngine;
using System.Collections.Generic;

namespace cg {
    public class DeployVanilla : Action {
        public override bool execute(float d, ActionTargetingInfo targetInfo = null) {
            if (!isValidTarget(d, targetInfo))
                return false;

            ActionTargetingInfo tInfo = (targetInfo ?? defaultTargetingInfo);

            tInfo.sourceCard.boardManager.moveCardToSquare(tInfo.sourceCard, tInfo.targetSquares[0]);

            return true;
        }

        public override bool isValidTarget(float d, ActionTargetingInfo targetInfo = null) {
            ActionTargetingInfo tInfo = (targetInfo ?? defaultTargetingInfo);

            if (tInfo.targetSquares.Count != 1)
                return false;
            if (tInfo.targetSquares[0] == null || tInfo.sourceCard == null || tInfo.sourceCard.square == null)
                return false;
            if (tInfo.sourceCard.owner == null || tInfo.player == null || tInfo.sourceCard.owner != tInfo.player)
                return false;

            //target must be unoccupied
            if (tInfo.targetSquares[0].cards.Count != 0)
                return false;

            if (tInfo.sourceCard.square.boardLocation != BoardLocationEnum.player1Hand && tInfo.sourceCard.square.boardLocation != BoardLocationEnum.player2Hand)    //doesn't check which hand it is, because assumes that's handled in game manager (and by correctly assigning owners)
                return false;

            if (tInfo.sourceCard.square.boardLocation == BoardLocationEnum.player1Hand && tInfo.targetSquares[0].row >= tInfo.sourceCard.boardManager.squares[0].Count/2)
                return false;
            if (tInfo.sourceCard.square.boardLocation == BoardLocationEnum.player2Hand && tInfo.targetSquares[0].row < (tInfo.sourceCard.boardManager.squares[0].Count - tInfo.sourceCard.boardManager.squares[0].Count / 2))
                return false;

            return true;
        }
    }
}
