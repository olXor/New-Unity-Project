using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace cg
{
    public class Player : MonoBehaviour
    {
        public Color32 color;
        public GameManager gameManager { get; set; }
        protected List<Action> onTurnActions;
        protected List<Action> offTurnActions;

        public Square handSquare { get; set; }
        public DeckInstance deck { get; set; }
        public Graveyard graveyard { get; set; }

        protected bool initialized = false;

        public virtual void initialize()
        {
            if (initialized)
                return;

            initialized = true;

            onTurnActions = new List<Action>();
            offTurnActions = new List<Action>();
        }

        public void OnTurnTick(float d)
        {
            if (!initialized)
                return;

            for (int i = 0; i < onTurnActions.Count; i++)
            {
                onTurnActions[i].execute(d);
            }
        }

        public void OffTurnTick(float d)
        {
            if (!initialized)
                return;

            for (int i = 0; i < offTurnActions.Count; i++)
            {
                offTurnActions[i].execute(d);
            }
        }
    }
}
