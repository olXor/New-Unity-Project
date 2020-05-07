using UnityEngine;
using System.Collections;

namespace cg {
    //[CreateAssetMenu(menuName = "ActionCreators/FightAsBishop")]
    public class FightAsBishopCreator : ActionCreator {
        public override Action createAction() {
            return new GameObject("FightAsBishop").AddComponent<FightAsBishop>();
        }
    }
}
