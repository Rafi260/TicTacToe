using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//two local player plays on same device

public class _VsOffline : _GameLogic
{
    public Menu menu;
    [HideInInspector] public int currentPlayer = 0;
    public GameObject restart;


    public void Start()
    {
        IntializeGrid();
        setPlayer1(0);
    }


    public void PlayerMove(int x, int y)
    {
        if (running == true)
        {
            PlaceMove(x, y, currentPlayer);
        }
        if (running == false)
        {
            if (draw == true)
            {
                menu.DrawScreen();
            }
            else
            {
                if (currentPlayer == 0)
                {
                    menu.SetWinningLine(winingLine);
                }
                else if (currentPlayer == 1)
                {
                    menu.SetWinningLine(winingLine);
                }
            }
            restart.SetActive(true);
        }
        currentPlayer = 1 - currentPlayer;
    }
}
