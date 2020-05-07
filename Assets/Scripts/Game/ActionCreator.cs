using UnityEngine;
using System.Collections;

namespace cg {
    public abstract class ActionCreator : ScriptableObject {
        public abstract Action createAction();
    }
}
