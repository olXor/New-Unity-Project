using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cg
{
    [CreateAssetMenu(menuName = "Card")]
    public class Card : ScriptableObject
    {
        public string cardName;
        public Sprite picture;
        public string abilityText;
        public string flavorText;
        public int basePower;

        public List<ActionCreator> deployActionCreators;
        public List<ActionCreator> movementActionCreators;
        public List<ActionCreator> otherActionCreators;
    }
}
