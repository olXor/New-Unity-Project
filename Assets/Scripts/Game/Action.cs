using UnityEngine;
using System.Collections.Generic;

namespace cg
{
    public class ActionTargetingInfo
    {
        public Player player { get; set; }
        public CardInstance sourceCard { get; set; }
        public List<Square> targetSquares { get; set; }
    }

    public abstract class Action : MonoBehaviour
    {
        public Action()
        {
            defaultTargetingInfo = new ActionTargetingInfo();
        }

        public ActionTargetingInfo defaultTargetingInfo { get; set; }
        public abstract bool execute(float d, ActionTargetingInfo targetInfo = null);
        public abstract bool isValidTarget(float d, ActionTargetingInfo targetInfo = null);
    }
}
