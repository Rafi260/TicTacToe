using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _VsComputer : MonoBehaviour
{
    _GameLogic _gameLogic = new _GameLogic();

    [Header("Class references")]
    public Menu menu;
    public _UpdatePlayerData updatePlayerData;

    [HideInInspector] public int currentPlayer = 0;
    [HideInInspector] public int player1;
    [HideInInspector] public bool bRun = true, bDraw = false;

    private void Start()
    {
        _gameLogic.IntializeGrid();
    }

    //Let player choose symbol at the beginnig of game.
    //If choose O, player moves first, otherwise computer gives move.
    public void ChoosePlayerSymbol(int symbol)
    {
        player1 = symbol;
        _gameLogic.setPlayer1(symbol);

        if (symbol == 1)
        {
            Invoke("PcMove", .5f);

        }

    }


    //player move
    public void _PlaceMove(int x, int y)
    {
        if (bRun)
        {
            _gameLogic.PlaceMove(x, y, currentPlayer);
            bRun = _gameLogic.running;
            bDraw = _gameLogic.draw;


            //cheking for win lose draw
            if (bRun == false)
            {
                if (bDraw == true)
                {
                    menu.DrawScreen();
                    return;
                }
                else
                {
                    menu.SetWinningLine(_gameLogic.winingLine);
                    menu.WinScreen();
                    return;
                }
            }

            if (bRun)
            {
                currentPlayer = 1 - currentPlayer;
                PcMove();
                bRun = _gameLogic.running;
                bDraw = _gameLogic.draw;

                if (bRun == false)
                {
                    if (bDraw == true)
                    {
                        menu.DrawScreen();
                        return;
                    }
                    else
                    {
                        menu.SetWinningLine(_gameLogic.winingLine);
                        menu.LoseScreen();
                        return;
                    }
                }
            }

        }


    }


    //Logic for computer move.
    public void PcMove()
    {

        //wining move
        //checks if computer has already placed two symbol in any winnig line.
        //if yes, then check if computer can place third symbol in that line to win.
        for (int i = 0; i < 8; ++i)
        {

            if (_gameLogic.SymbolCount(i, _gameLogic.player2) == 2 && _gameLogic.SymbolCount(i, _gameLogic.blank) == 1)
            {
                Tuple<int, int> coOrdinate = _gameLogic.FindBlank(i);
                _gameLogic.PlaceMove(coOrdinate.Item1, coOrdinate.Item2, currentPlayer);
                updatePlayerData.PlaceSymbol(coOrdinate.Item1, coOrdinate.Item2, player1);
                currentPlayer = 1 - currentPlayer;
                return;
            }
        }

        //preventing player win
        //checks if player has placed two symbol in any winnig line and the third place is blank.
        //if yes, computer will place its move in that black place 
        for (int i = 0; i < 8; ++i)
        {
            if (_gameLogic.SymbolCount(i, _gameLogic.player1) == 2 && _gameLogic.SymbolCount(i, _gameLogic.blank) == 1)
            {
                // move
                Tuple<int, int> coOrdinate = _gameLogic.FindBlank(i);
                _gameLogic.PlaceMove(coOrdinate.Item1, coOrdinate.Item2, currentPlayer);
                updatePlayerData.PlaceSymbol(coOrdinate.Item1, coOrdinate.Item2, player1);
                currentPlayer = 1 - currentPlayer;
                return;
            }
        }

        // random move
        //if there is no winnig chance for computer or the player, it place symbol in a random blank space
        for (int i = 0; i < 8; ++i)
        {
            if (_gameLogic.SymbolCount(i, _gameLogic.blank) > 0)
            {
                Tuple<int, int> coOrdinate = _gameLogic.FindBlank(i);
                _gameLogic.PlaceMove(coOrdinate.Item1, coOrdinate.Item2, currentPlayer);
                updatePlayerData.PlaceSymbol(coOrdinate.Item1, coOrdinate.Item2, player1);
                currentPlayer = 1 - currentPlayer;
                return;
            }

        }
        return;
    }


}
