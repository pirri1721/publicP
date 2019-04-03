using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Slot : MonoBehaviour {

    public int index;

    public bool safe;
    public List<Color> specialColor;
    public bool specialMove;
    public int specialIndex;

    public bool roadEnd;
    //pre-rules
    //rules

    //public Token[] tokens = new Token[2];
    public List<Token> tokens;
    public BoardManager bM;
    public bool stairTile;

    // Use this for initialization
    void Awake () {
        tokens = new List<Token>(2);
        specialColor = new List<Color>();
        //Debug.Log(tokens.Count);
    }

    public bool IsAvaible()
    {
        //TODO
        //CheckForEnemies

        if (tokens.Count == 2)
        {
            return false;
        }
        else return true;
    }

    public bool ThisSlotAvaible(int followMove)
    {
        if(roadEnd && followMove > 0)
        {
            return false;
        }

        if(tokens.Count == 2)
        {
            return false;
        }

        /*
        if(tokens[0] != null && tokens[1] != null)
        {
            return false;
        }*/

        if(followMove > 0)
        {
            
            /*
            if (specialMove)
            {
                    bM.CheckMove(specialIndex, followMove - 1);
            }
            else*/
            //bM.CheckMove(index,followMove - 1);
            return bM.slots[index+1].ThisSlotAvaible(followMove-1);
        }
        
        //pre-rules
        //rules
        //return bool?
         //index = slots[index].NextIndex(token);
        return true;
    }

    public int NextIndex(Token thisToken)
    {
        for(int i = 0; i < specialColor.Count; i++)
        {
            if (specialMove && thisToken.color == specialColor[i])
            {
                return specialIndex;
            }
        }
        
        return index + 1;
    }

    public void AddingToken(Token token)
    {
        tokens.Add(token);
        //add check
        if(tokens.Count == 2)
        {
            StartCoroutine(AdjustTokens());
        }
    }

    public void RemovingToken(Token token)
    {
        if (!bM.eat)
        {
            if (tokens.Count == 2)
            {
                if (tokens[0] == token)
                {
                    tokens[1].transform.DOMoveX(this.transform.position.x, 0.5f);
                }
                else
                {
                    tokens[0].transform.DOMoveX(this.transform.position.x, 0.5f);
                }

                if (token.player.barriers.Count > 0)
                {
                    for (int i = 0; i < token.player.barriers.Count; i++)
                    {
                        if (token.player.barriers[i].token1 == token)
                        {
                            Token otherToken = token.player.barriers[i].token2;

                            token.player.RemoveBarrier(i);
                            otherToken.player.RemoveBarrier(otherToken);
                        }
                    }
                }
            }


        }
        bM.eat = false;
        tokens.Remove(token);
        //tokens[0].transform.DOMoveX(this.transform.position.x, 0.5f);

    }

    public IEnumerator AdjustTokens()
    {
        yield return new WaitForSeconds(0.5f);

        if(tokens.Count == 2 && bM.eat == false)
        {

            Transform firstToken = tokens[0].transform;
            Transform secondToken = tokens[1].transform;

            firstToken.SetParent(this.transform);
            secondToken.SetParent(this.transform);
            /*
            firstToken.rotation = this.transform.rotation;
            secondToken.rotation = this.transform.rotation;
            yield return new WaitForEndOfFrame();
            */
            if (this.transform.rotation.y > 0)
            {
                firstToken.DOLocalMoveZ(this.transform.position.z - 0.2f, 0.5f);
                secondToken.DOLocalMoveZ(this.transform.position.z + 0.2f, 0.5f);
            }
            else
            {
                firstToken.DOLocalMoveX(this.transform.position.x - 0.2f, 0.5f);
                secondToken.DOLocalMoveX(this.transform.position.x + 0.2f, 0.5f);
            }

            firstToken.SetParent(null);
            secondToken.SetParent(null);
        }
        else
        {
            bM.eat = false;
            Debug.Log("wtf");
        }
    }

}
