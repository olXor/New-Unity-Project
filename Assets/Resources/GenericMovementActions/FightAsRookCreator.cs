using UnityEngine;
using System.Collections;

namespace cg {
    //[CreateAssetMenu(menuName = "ActionCreators/FightAsRook")]
    public class FightAsRookCreator : ActionCreator {
        public override Action createAction() {
            return new GameObject("FightAsRook").AddComponent<FightAsRook>();
        }
    }
}
