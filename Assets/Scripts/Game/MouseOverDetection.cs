using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;

namespace cg
{
    public class MouseOverDetection : Action
    {
        public ScreenManager screenManager = null;

        public override bool execute(float d, ActionTargetingInfo targetInfo)
        {
            if (!isValidTarget(d, targetInfo))
                return false;

            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            IClickable c = null;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            foreach(RaycastResult r in results)
            {
                c = r.gameObject.GetComponentInParent<IClickable>();
                if (c != null)
                    break;
            }

            if(c == null) {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit)
                    c = hit.collider.GetComponentInParent<IClickable>();
                    //c = GameObject.Find(hit.collider.gameObject.name).GetComponentInParent<IClickable>();
            }

            if (screenManager != null)
                screenManager.onMouseEvent(c, (targetInfo == null ? defaultTargetingInfo.player : targetInfo.player));

            return true;
        }

        public override bool isValidTarget(float d, ActionTargetingInfo targetInfo = null) {
            ActionTargetingInfo tInfo = (targetInfo == null ? defaultTargetingInfo : targetInfo);
            if (tInfo == null || tInfo.player == null)
                return false;

            return true;
        }
    }
}
