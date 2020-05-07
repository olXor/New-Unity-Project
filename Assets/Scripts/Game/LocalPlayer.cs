using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace cg
{
    public class LocalPlayer : Player
    {
        public ScreenManager screenManager { get; set; }

        public override void initialize()
        {
            if (initialized)
                return;

            base.initialize();

            MouseOverDetection mouseOver = new GameObject("MouseOverDetection").AddComponent<MouseOverDetection>();
            mouseOver.screenManager = screenManager;
            mouseOver.defaultTargetingInfo.player = this;

            onTurnActions.Add(mouseOver);
        }

        void Start()
        {
        }
    }
}
