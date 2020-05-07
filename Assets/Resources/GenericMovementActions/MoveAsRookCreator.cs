using UnityEngine;
using System.Collections;

namespace cg {
    //[CreateAssetMenu(menuName = "ActionCreators/MoveAsRook")]
    public class MoveAsRookCreator : ActionCreator {
        public override Action createAction() {
            return new GameObject("MoveAsRook").AddComponent<MoveAsRook>();
        }
    }
}
