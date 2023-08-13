using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// when you touch a grid , a corrosponding symbol is spawned on that grid,
// and disable that grid so that player cant place further symbol on the same grid
//also send Coordinate value of that grid to main game function to populate grid value.

public class _UserInteraction : MonoBehaviour
{
    [Header("Class References")]
    public _VsComputer _vsComputer;
    public _VsOnline _vsOnline;
    public _VsOffline _vsOffline;

    [Header("Prefab")]
    public GameObject X;
    public GameObject O ;
    GameObject g;

    [Header("Game Level")]
    public bool computer;
    public bool online;
    public bool offline;

    [Header("Coordinate value of grid")]
    public int coOrd_x;
    public int coOrd_y;

    [Header("Sound")]
    public AudioClip clip;
    public AudioSource source;


    private void OnMouseDown()
    {
        //for vsComputer level
        if (computer)
        {
            if (_vsComputer.currentPlayer == _vsComputer.player1 && _vsComputer.bRun)
            {
                if (_vsComputer.player1 == 0)
                {
                    Ins(O);
                }
                else
                {
                    Ins(X);
                }
                
                _vsComputer._PlaceMove(coOrd_x, coOrd_y);
                gameObject.SetActive(false);

            }
        }

        //for Multiplayer level
        else if (online)
        {
            if (_vsOnline.myTurn && _vsOnline.bRun && _vsOnline.canGiveMove)
            {
                if (_vsOnline.player1 == 0)
                {
                    Ins(O);
                }
                else
                {
                    Ins(X);
                }
               
                _vsOnline._PlaceMove(coOrd_x, coOrd_y);
                gameObject.SetActive(false);

            }
        }

        //for local 1v1 level
        else
        {
            if (_vsOffline.running)
            {
                if (_vsOffline.currentPlayer == 0)
                {
                    Ins(O);

                }
                else
                {
                    Ins(X);
                }

               
                _vsOffline.PlayerMove(coOrd_x, coOrd_y);
                gameObject.SetActive(false);
            }


        }
    }


    //Spawn X O on grids
    void Ins(GameObject g)
    {
        PlaySound();
        g = Instantiate(g, gameObject.transform.position, Quaternion.identity);
        if (online)
        {
            _vsOnline.AddToXOs(g);
        }
    }
    
    void PlaySound()
    {
        source.PlayOneShot(clip);
    }


}
