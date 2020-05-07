using UnityEngine;
using System.Collections.Generic;

namespace cg
{
    public class CardInstance : MonoBehaviour, IClickable
    {
        public CardVis cardVis;
        private Card _card;
        public Card card {
            get => _card;
            set {
                cardVis.loadCard(value, owner);
                //cardVis.card = value;
                _card = value;
            }
        }
        public BoardManager boardManager { get; set; }

        public Square square { get; set; } = null;
        public Player owner { get; set; } = null;

        public List<Action> deployActions;
        public List<Action> movementActions;
        public List<Action> otherActions;

        private int _currentPower = 0;
        public int currentPower {
            get => _currentPower;
            set {
                _currentPower = value;
                cardVis.powerText.text = _currentPower.ToString();
            }
        }

        public bool isDead { get; set; }

        public void resetActions() {
            if (card != null) {
                deployActions.Clear();
                for (int i = 0; i < card.deployActionCreators.Count; i++)
                    deployActions.Add(card.deployActionCreators[i].createAction());
                movementActions.Clear();
                for (int i = 0; i < card.movementActionCreators.Count; i++)
                    movementActions.Add(card.movementActionCreators[i].createAction());
                otherActions.Clear();
                for (int i = 0; i < card.otherActionCreators.Count; i++)
                    otherActions.Add(card.otherActionCreators[i].createAction());
            }
        }

        public void resetState() {
            isDead = false;
            resetPower();
        }

        public void resetPower() {
            currentPower = card.basePower;
        }

        public void transformToCard(Card newCard, bool stateReset = true, bool actionReset = true) {
            card = newCard;
            if (actionReset)
                resetActions();
            if (stateReset)
                resetState();
        }

        public bool isValidActionTarget(ActionTargetingInfo targetInfo, BoardActionEnum actionType) {
            List<Action> actionList;
            if (actionType == BoardActionEnum.deploy) {
                actionList = deployActions;
            }
            else if (actionType == BoardActionEnum.movement) {
                actionList = movementActions;
            }
            else if (actionType == BoardActionEnum.other) {
                actionList = otherActions;
            }
            else
                return false;

            for (int i = 0; i < actionList.Count; i++) {
                if (actionList[i].isValidTarget(0, targetInfo))
                    return true;
            }

            return false;
        }

        public bool die() {
            resetState();
            resetActions();
            isDead = true;
            if (owner.graveyard != null)
                boardManager.moveCardToSquare(this, owner.graveyard);
            return true;
        }

        //returns true on death
        public bool takeDamage(int damage) {
            currentPower -= damage;
            if (currentPower < 0) {
                return die();
            }
            return false;
        }

        public bool dealDamage(CardInstance target, int damage) {
            target.takeDamage(damage);
            return target.isDead;
        }

        //returns true if target died
        public bool fight(CardInstance target) {
            return target.die();
            /*
            for (int i = 0; i < 10; i++) {
                if (dealDamage(target, currentPower))
                    return true;
                if (target.dealDamage(this, target.currentPower))
                    return false;
            }
            return false;
            */
        }

        public void OnHighlight()
        {
            //cardVis.loadCard(defaultCard);
        }

        public void OnLeftClick()
        {
        }

        public void OnLeftRelease()
        {
            throw new System.NotImplementedException();
        }

        public void OnRightClick()
        {
            throw new System.NotImplementedException();
        }

        public void OnRightRelease()
        {
            throw new System.NotImplementedException();
        }
    }
}
