using UnityEngine;
using System.Collections;

namespace cg {
    //[CreateAssetMenu(menuName = "ActionCreators/MoveAsBishop")]
    public class MoveAsBishopCreator : ActionCreator {
        public override Action createAction() {
            return new GameObject("MoveAsBishop").AddComponent<MoveAsBishop>();
        }
    }
}
