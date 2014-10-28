/*
 * Created by: Kris MAtis
 * menu for multi player character selection
 * 
 */

#region ChangeLog
/*
 * 27/10/2014 edit: script made
 * 
 * 28/10/2014 edit: what i did while sick was terrible so im resarting
 * 28/10/2014 edit: mostly done
 */
#endregion

using UnityEngine;
using System.Collections;

public class MenuV2PlayerSelect : MenuV2 
{
    const int PLAYER_ONE = 0;
    const int PLAYER_TWO = 1;

    public PlayerSelectArrow[] PlayerArrows;

    public GameObject[] PlayerPedestalMountPoint;

    public GameObject PedestalKeyboardPrefab;
    public GameObject PedestalGamepadPreafab;

    struct input
    {
        public Vector2 Move;
        public bool Accept;
        public bool Back;

        public input(Vector2 move, bool accept, bool back)
        {
            Move = move;
            Accept = accept;
            Back = back;
        }
    };

    // Update is called once per frame
    protected override void  update()
    {
        for (int i = 0; i < PlayerArrows.Length; i++)
        {
            //get the input
            input input = new input(Vector2.zero, false, false);

            if (i == PLAYER_ONE)
            {
                input.Move = InputManager.getMenuChangeSelection(GameData.Instance.m_PlayerOneInput);
                input.Accept = InputManager.getMenuAcceptDown(GameData.Instance.m_PlayerOneInput);
                input.Back = InputManager.getMenuBackDown(GameData.Instance.m_PlayerOneInput);
            }
            else if (i == PLAYER_TWO)
            {
                input.Move = InputManager.getMenuChangeSelection(GameData.Instance.m_PlayerTwoInput);
                input.Accept = InputManager.getMenuAcceptDown(GameData.Instance.m_PlayerTwoInput);
                input.Back = InputManager.getMenuBackDown(GameData.Instance.m_PlayerTwoInput);
            }

            m_Timer -= Time.deltaTime;
            if (input.Move != Vector2.zero)
            {
                if(m_Timer > 0.0f)
                {
                    input.Move = Vector2.zero;
                }
                else
                {
                    m_Timer = DELAY_TIME;
                }
            }


            if (!PlayerArrows[i].IsMounted)
            {
                //check if moving
                if (input.Move.y > 0.0f)
                {
                    PlayerArrows[i].moveUp();
                    return;
                }
                else if (input.Move.y < 0.0f)
                {
                    PlayerArrows[i].moveDown();
                    return;
                }


                //check if mounting
                if (input.Move.x != 0.0f)
                {
                    Vector2 middleToMount = new Vector2((PlayerArrows[i].SelectionMountPoint.position - PlayerArrows[i].transform.position).x, 0.0f);

                    if (Vector2.Dot(input.Move.normalized, middleToMount.normalized) > 0.0f)
                    {//player swiped towards thier side
                        if (!PlayerArrows[i].IsMounted)
                        {
                            PlayerArrows[i].select();
                        }
                    }
                }
            }
            else
            {
                if (!PlayerArrows[i].getSelection().IsConfirmed)
                {
                    //check if unmounting
                    if (input.Move.x != 0.0f)
                    {
                        Vector2 middleToMount = new Vector2((PlayerArrows[i].SelectionMountPoint.position - PlayerArrows[i].transform.position).x, 0.0f);

                        if (Vector2.Dot(input.Move.normalized, middleToMount.normalized) < 0.0f)
                        {//player swiped away from their side
                            if (PlayerArrows[i].IsMounted)
                            {
                                PlayerArrows[i].deselect();
                            }
                            return;
                        }
                    }

                    //check if confriming
                    if (input.Accept)
                    {
                        PlayerArrows[i].getSelection().IsConfirmed = true;
                    }
                }
                else
                {
                    //check if un-confirming
                    if (input.Back)
                    {
                        PlayerArrows[i].getSelection().IsConfirmed = true;
                    }
                }
            }
        }

        if (PlayerArrows[PLAYER_ONE].getSelection().IsConfirmed && PlayerArrows[PLAYER_TWO].getSelection().IsConfirmed)
        {
            //TODO: set things
            //change menu
            //reset things
            Debug.Log("done");
        }
    }
}
