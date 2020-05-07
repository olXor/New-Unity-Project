using UnityEngine;
using System.Collections;

namespace cg {
    //[CreateAssetMenu(menuName = "ActionCreators/DeployVanilla")]
    public class DeployVanillaCreator : ActionCreator {
        public override Action createAction() {
            return new GameObject("DeployVanilla").AddComponent<DeployVanilla>();
        }
    }
}
