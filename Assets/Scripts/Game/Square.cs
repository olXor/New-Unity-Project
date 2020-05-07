using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace cg
{
    public class Square : MonoBehaviour
    {
        public BoardManager boardManager { get; set; }
        public List<CardInstance> cards { get; } = new List<CardInstance>();
        public int row;
        public int col;
        public BoardLocationEnum boardLocation = BoardLocationEnum.none;

        public bool isOccupied() {
            return (cards != null && cards.Count != 0);
        }
    }
}
