using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace cg
{
    public class ScreenSquare : Square {
        public void setHighlight(bool highlight) {
            if (highlight)
                GetComponent<Image>().enabled = true;
            else
                GetComponent<Image>().enabled = false;
        }
    }
}
