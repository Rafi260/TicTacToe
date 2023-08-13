//responsible for core game mechanism.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _GameLogic : MonoBehaviour
{

    public int blank = -1, player1, player2;
    int[,] grid = new int[3, 3];
    int[] count = new int[8];


    //these are all the combination in grid that if a player fills, he wins.
    /* exmple:  
     *  (X[0] + 0 * dX[0] , Y[0] + 0 * dY[0])  ,   (X[0] + 0 * dX[0] , Y[0] + 0 * dY[0])  ,   (X[0] + 0 * dX[0] , Y[0] + 0 * dY[0])
     *  these are three Coordinates of a 2D grid array indicating wining line. If same symbol is found in these three point, player wins.
     *  the 0,1,2 multiplied with dX and dY is achieved through a loop.
     *  In all four array (X,Y,dX,dY) index is 0. It indicates 0 number winning line. If we pass index 1, we get 1 number winnig line. We can go upto 7 to get 8 winnig lines.
    */

    int[] X = { 0, 1, 2, 0, 0, 0, 0, 0 };
    int[] Y = { 0, 0, 0, 0, 1, 2, 0, 2 };
    int[] dX = { 0, 0, 0, 1, 1, 1, 1, 1 };
    int[] dY = { 1, 1, 1, 0, 0, 0, 1, -1 };

    //to check if the game is runnig, or draw
    public bool running = true, draw = false;
    public int winingLine = -1;

    public void Initialize()
    {
        IntializeGrid();
        InitializeCount();

        running = true;
        draw = false;
        winingLine = -1;
    }

    //setting grid value to -1. Grid value -1 means no symbol has been placed to that grid.
    public void IntializeGrid()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                grid[i, j] = -1;
            }
        }
    }

    //count array is used to determine if a player wins.
    public void InitializeCount()
    {
        for (int i = 0; i < 8; i++)
        {
            count[i] = 0;
        }
    }

    //setting player symbol. 0 means O ,1 means X.
    public void setPlayer1(int player1)
    {
        this.player1 = player1;
        player2 = 1 - player1;
    }


    int getPlayer1()
    {
        return player1;
    }

    //setting grid value according to player symbol. for O value is 0 ,for X value is 1 .
    public void PlaceMove(int x, int y, int symbol)
    {
        grid[x, y] = symbol;

        //after placing value, checks if the player has win or draw
        if (CheckWin(symbol))
        {
            running = false;
        }
        else if (CheckDraw(symbol))
        {
            print(symbol);
            running = false;
            draw = true;
        }

    }


    //If we get value 3 in count, means player has win
    bool CheckWin(int symbol)
    {
        //filling the count array 
        FillCount(symbol);

        for (int i = 0; i < 8; i++)
        {
            if (count[i] == 3)
            {
                winingLine = i;
                return true;
            }

        }
        return false;
    }

    //If there is no black place in grid, if means all grids have been used and the match is a draw
    bool CheckDraw(int symbol)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (grid[i, j] == blank)
                {
                    return false;
                }
            }
        }
        return true;
    }


    //If the player is X, checking how many X are there in every winnig line. If player actually win, we get count 3 for a winnig line.
    void FillCount(int symbol)
    {
        for (int i = 0; i < 3; i++)
        {
            count[i] = GetCount(i, 0, 0, 1, symbol);
        }
        for (int i = 0; i < 3; i++)
        {
            count[i + 3] = GetCount(0, i, 1, 0, symbol);
        }

        count[6] = GetCount(0, 0, 1, 1, symbol);
        count[7] = GetCount(0, 2, 1, -1, symbol);
    }

    int GetCount(int x, int y, int delX, int delY, int symbol)
    {
        int count = 0;
        for (int i = 0; i < 3; i++)
        {
            int _x = x + i * delX;
            int _y = y + i * delY;
            if (grid[_x, _y] == symbol)
            {
                count++;
            }
        }
        return count;
    }

    //========================== vs Computer ===============================
    //these func used in vs computer


    //checks how many X or O are there in a winning line. 
    public int SymbolCount(int line, int symbol)
    {
        int cnt = 0;
        for (int i = 0; i < 3; ++i)
        {
            int x = X[line] + i * dX[line];
            int y = Y[line] + i * dY[line];
            if (grid[x, y] == symbol) ++cnt;
        }
        return cnt;
    }


    //computer finds any black space in grid
    public Tuple<int, int> FindBlank(int line)
    {
        Tuple<int, int> coOrdinate = new Tuple<int, int>(-1, -1);

        for (int i = 0; i < 3; ++i)
        {
            int x = X[line] + i * dX[line];
            int y = Y[line] + i * dY[line];
            if (grid[x, y] == blank) coOrdinate = new Tuple<int, int>(x, y);
        }
        return coOrdinate;
    }
}
