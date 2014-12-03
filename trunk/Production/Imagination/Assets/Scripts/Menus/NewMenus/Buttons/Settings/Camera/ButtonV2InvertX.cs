using UnityEngine;
using System.Collections;

public class ButtonV2InvertX : buttonV2Invert
{
    public override void use(PlayerInput usedBy = PlayerInput.None)
    {
        base.use(usedBy);
        if (PlayerInfo.getPlayer(usedBy).m_Player == Players.PlayerOne)
        {
            GameData.Instance.invertPlayerOneX();
        }
        else if (PlayerInfo.getPlayer(usedBy).m_Player == Players.PlayerTwo)
        {
            GameData.Instance.invertPlayerTwoX();
        }
    }
}
