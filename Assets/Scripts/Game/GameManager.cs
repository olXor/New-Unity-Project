using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace cg
{
    public class GameManager : MonoBehaviour
    {
        public BoardManager initialBoardManager;
        public CardInstance cardTemplate;

        protected BoardManager boardManager = null;
        protected List<List<Player>> players;   //in turn order
        protected List<List<bool>> turnsDone;
        protected int turn = 0;


        public virtual void updateBoardManager(BoardManager newManager)
        {
            boardManager = newManager;
            boardManager.setGameManager(this);
        }

        public BoardManager getScreenManager()
        {
            return boardManager;
        }

        protected virtual void Start()
        {
            updateBoardManager(initialBoardManager);
            boardManager.initialize();

            turnsDone = new List<List<bool>>();
            for (int i = 0; i < players.Count; i++)
            {
                turnsDone.Add(new List<bool>());
                for (int j = 0; j < players[i].Count; j++)
                {
                    turnsDone[i].Add(false);
                }
            }

            for(int i = 0; i < players.Count; i++)
            {
                for (int j = 0; j < players[i].Count; j++)
                {
                    players[i][j].gameManager = this;
                }
            }
        }

        protected virtual void Update()
        {
            for (int i = 0; i < players.Count; i++)
            {
                for (int j = 0; j < players[i].Count; j++)
                {
                    if (i == turn)
                        players[i][j].OnTurnTick(Time.deltaTime);
                    else
                        players[i][j].OffTurnTick(Time.deltaTime);
                }
            }

            bool doneTurn = true;
            for (int i = 0; i < players[turn].Count; i++)
            {
                if (!turnsDone[turn][i])
                {
                    doneTurn = false;
                    break;
                }
            }

            if (doneTurn)
            {
                turn = (turn + 1) % players.Count;
                for (int i = 0; i < turnsDone[turn].Count; i++)
                    turnsDone[turn][i] = false;
            }
        }

        public bool isPlayerTurn(Player player)
        {
            return players[turn].Contains(player);
        }

        public void finishPlayerTurn(Player player)
        {
            int index = players[turn].IndexOf(player);
            if (index >= 0)
                turnsDone[turn][index] = true;
        }

        public void setTurn(int newTurn)
        {
            turn = newTurn;
            for (int i = 0; i < turnsDone[turn].Count; i++)
                turnsDone[turn][i] = false;
        }
    }
}
