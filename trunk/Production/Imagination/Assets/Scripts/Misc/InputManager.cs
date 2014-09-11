using UnityEngine;
using System.Collections;

using GamepadInput;

public class InputManager
{
    public bool getJump()
    {
        if (Input.GetKey(KeyCode.Space) || GamePad.GetButton(GamePad.Button.A, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    public bool getJumpDown()
    {
        if (Input.GetKeyDown(KeyCode.Space) || GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    public bool getJumpUp()
    {
        if (Input.GetKeyUp(KeyCode.Space) || GamePad.GetButtonUp(GamePad.Button.A, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    //=====================================================================================================

    public bool getJump(GamePad.Index index)
    {
        return GamePad.GetButton(GamePad.Button.A, index);
    }

    public bool getJumpDown(GamePad.Index index)
    {
        return GamePad.GetButtonDown(GamePad.Button.A, index);
    }

    public bool getJumpUp(GamePad.Index index)
    {
        return GamePad.GetButtonUp(GamePad.Button.A, index);
    }

    //=====================================================================================================
    //=====================================================================================================

    public bool getAttack()
    {
        if (Input.GetMouseButton(0) || GamePad.GetButton(GamePad.Button.X, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    public bool getAttackDown()
    {
        if (Input.GetMouseButtonDown(0) || GamePad.GetButtonDown(GamePad.Button.X, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    public bool getAttackUp()
    {
        if (Input.GetMouseButtonUp(0) || GamePad.GetButtonUp(GamePad.Button.X, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    //=====================================================================================================

    public bool getAttack(GamePad.Index index)
    {
        return GamePad.GetButton(GamePad.Button.X, index);
    }

    public bool getAttackDown(GamePad.Index index)
    {
        return GamePad.GetButtonDown(GamePad.Button.X, index);
    }

    public bool getAttackUp(GamePad.Index index)
    {
        return GamePad.GetButtonUp(GamePad.Button.X, index);
    }

    //=====================================================================================================
    //=====================================================================================================

    public bool getCharacterSwitch()
    {
        if (Input.GetKey(KeyCode.Tab) || GamePad.GetButton(GamePad.Button.Y, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    public bool getCharacterSwitchDown()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || GamePad.GetButtonDown(GamePad.Button.Y, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    public bool getCharacterSwitchUp()
    {
        if (Input.GetKeyUp(KeyCode.Tab) || GamePad.GetButtonUp(GamePad.Button.Y, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    //=====================================================================================================

    public bool getCharacterSwitch(GamePad.Index index)
    {
        return GamePad.GetButton(GamePad.Button.Y, index);
    }

    public bool getCharacterSwitchDown(GamePad.Index index)
    {
        return GamePad.GetButtonDown(GamePad.Button.Y, index);
    }

    public bool getCharacterSwitchUp(GamePad.Index index)
    {
        return GamePad.GetButtonUp(GamePad.Button.Y, index);
    }

    //=====================================================================================================
    //=====================================================================================================

    public bool getShowHud()
    {
        if (Input.GetKey(KeyCode.F) || GamePad.GetButton(GamePad.Button.B, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    public bool getShowHudDown()
    {
        if (Input.GetKeyDown(KeyCode.F) || GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    public bool getShowHudUp()
    {
        if (Input.GetKeyUp(KeyCode.F) || GamePad.GetButtonUp(GamePad.Button.B, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    //=====================================================================================================

    public bool getShowHud(GamePad.Index index)
    {
        return GamePad.GetButton(GamePad.Button.B, index);
    }

    public bool getShowHudDown(GamePad.Index index)
    {
        return GamePad.GetButtonDown(GamePad.Button.B, index);
    }

    public bool getShowHudUp(GamePad.Index index)
    {
        return GamePad.GetButtonUp(GamePad.Button.B, index);
    }

    //=====================================================================================================
    //=====================================================================================================

    public bool getPause()
    {
        if (Input.GetKey(KeyCode.Escape) || GamePad.GetButton(GamePad.Button.Start, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    public bool getPauseDown()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    public bool getPauseUp()
    {
        if (Input.GetKeyUp(KeyCode.Escape) || GamePad.GetButtonUp(GamePad.Button.Start, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    //=====================================================================================================

    public bool getPause(GamePad.Index index)
    {
        return GamePad.GetButton(GamePad.Button.Start, index);
    }

    public bool getPauseDown(GamePad.Index index)
    {
        return GamePad.GetButtonDown(GamePad.Button.Start, index);
    }

    public bool getPauseUp(GamePad.Index index)
    {
        return GamePad.GetButtonUp(GamePad.Button.Start, index);
    }

    //=====================================================================================================
    //=====================================================================================================

    public Vector2 getMove()
    {
        //TODO: figure out how to convert buttons into an axis and compare the two axises
        return GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any);
    }

    public Vector2 getMove(GamePad.Index index)
    {
        return GamePad.GetAxis(GamePad.Axis.LeftStick, index);
    }
}
