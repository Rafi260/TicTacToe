

//when second player gives move, update my Ui accoridng to second playes move.
//Used id vsComputer & vsOnline(Multiplayer)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _UpdatePlayerData : MonoBehaviour
{
    [Header("References")]
    public _VsOnline vsOnline;

    [Header("Prefabs")]
    public GameObject X;
    public GameObject O;
    GameObject g;

    [Header("Ui")]
    public GameObject[] grids = new GameObject[9];


    //x,y is coordinate of opponents move.
    public void PlaceSymbol(int x, int y, int player1)
    {
        print(x + " " + y);
        int gridId;
        gridId = 3 * x + y;

        if (player1 == 1)
        {
            g = Instantiate(O, grids[gridId].transform.position, Quaternion.identity);
        }
        else
        {
            g = Instantiate(X, grids[gridId].transform.position, Quaternion.identity);

        }

        if (vsOnline)
        {
            vsOnline.AddToXOs(g);
        }

 
        grids[gridId].SetActive(false);
    }

}
