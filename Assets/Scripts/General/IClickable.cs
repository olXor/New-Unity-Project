using UnityEngine;
using System.Collections;

namespace cg
{
    public interface IClickable
    {
        void OnLeftClick();
        void OnLeftRelease();
        void OnRightClick();
        void OnRightRelease();
        void OnHighlight();
    }
}
