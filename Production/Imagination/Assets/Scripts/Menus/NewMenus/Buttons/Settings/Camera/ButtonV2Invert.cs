using UnityEngine;
using System.Collections;

public abstract class buttonV2Invert : ButtonV2
{
    public GameObject i_Default;
    public GameObject i_Inverted;

    bool m_PlayerOneToggle = false;
    bool m_PlayerTwoToggle = false;

    public override void use(PlayerInput usedBy = PlayerInput.None)
    {
        if (PlayerInfo.getPlayer(usedBy).m_Player == Players.PlayerOne)
        {
            m_PlayerOneToggle = !m_PlayerOneToggle;
        }
        else if (PlayerInfo.getPlayer(usedBy).m_Player == Players.PlayerTwo)
        {
            m_PlayerTwoToggle = !m_PlayerTwoToggle;
        }
    }

    protected override void update()
    {
        base.update();

        if (PlayerInfo.getPlayer((PlayerInput)ParentMenu.ReadInputFromBits).m_Player == Players.PlayerOne)
        {
            i_Default.SetActive(!m_PlayerOneToggle);
            i_Inverted.SetActive(m_PlayerOneToggle); 
        }
        else if (PlayerInfo.getPlayer((PlayerInput)ParentMenu.ReadInputFromBits).m_Player == Players.PlayerTwo)
        {
            i_Default.SetActive(!m_PlayerTwoToggle);
            i_Inverted.SetActive(m_PlayerTwoToggle); 
        }
    }
}
