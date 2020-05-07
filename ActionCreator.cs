using UnityEngine;

namespace cg {
    public abstract class ActionCreator : ScriptableObject {
        public abstract Action createAction();
    }
}