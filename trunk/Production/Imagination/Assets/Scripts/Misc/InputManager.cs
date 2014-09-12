using UnityEngine;
using System.Collections;

using GamepadInput;

public static class InputManager
{
    public static bool getJump()
    {
        if (Input.GetKey(KeyCode.Space) || GamePad.GetButton(GamePad.Button.A, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    public static bool getJumpDown()
    {
        if (Input.GetKeyDown(KeyCode.Space) || GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    public static bool getJumpUp()
    {
        if (Input.GetKeyUp(KeyCode.Space) || GamePad.GetButtonUp(GamePad.Button.A, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    //=====================================================================================================

	public static bool getJump(Enums.PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case Enums.PlayerInput.Keyboard:
			return Input.GetKey(KeyCode.Space);
			break;
		case Enums.PlayerInput.GamePadOne:
			return GamePad.GetButton(GamePad.Button.A, GamePad.Index.One);
			break;
		case Enums.PlayerInput.GamePadTwo:
			return GamePad.GetButton(GamePad.Button.A, GamePad.Index.Two);
			break;
		case Enums.PlayerInput.GamePadThree:
			return GamePad.GetButton(GamePad.Button.A, GamePad.Index.Three);
			break;
		case Enums.PlayerInput.GamePadFour:
			return GamePad.GetButton(GamePad.Button.A, GamePad.Index.Four);
			break;
		}
		return false;
	}

	public static bool getJumpDown(Enums.PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case Enums.PlayerInput.Keyboard:
			return Input.GetKeyDown(KeyCode.Space);
			break;
		case Enums.PlayerInput.GamePadOne:
			return GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.One);
			break;
		case Enums.PlayerInput.GamePadTwo:
			return GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Two);
			break;
		case Enums.PlayerInput.GamePadThree:
			return GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Three);
			break;
		case Enums.PlayerInput.GamePadFour:
			return GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Four);
			break;
		}
		return false;
	}

	public static bool getJumpUp(Enums.PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case Enums.PlayerInput.Keyboard:
			return Input.GetKeyUp(KeyCode.Space);
			break;
		case Enums.PlayerInput.GamePadOne:
			return GamePad.GetButtonUp(GamePad.Button.A, GamePad.Index.One);
			break;
		case Enums.PlayerInput.GamePadTwo:
			return GamePad.GetButtonUp(GamePad.Button.A, GamePad.Index.Two);
			break;
		case Enums.PlayerInput.GamePadThree:
			return GamePad.GetButtonUp(GamePad.Button.A, GamePad.Index.Three);
			break;
		case Enums.PlayerInput.GamePadFour:
			return GamePad.GetButtonUp(GamePad.Button.A, GamePad.Index.Four);
			break;
		}
		return false;
	}

    //=====================================================================================================
    //=====================================================================================================

    public static bool getAttack()
    {
        if (Input.GetMouseButton(0) || GamePad.GetButton(GamePad.Button.X, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    public static bool getAttackDown()
    {
        if (Input.GetMouseButtonDown(0) || GamePad.GetButtonDown(GamePad.Button.X, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    public static bool getAttackUp()
    {
        if (Input.GetMouseButtonUp(0) || GamePad.GetButtonUp(GamePad.Button.X, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    //=====================================================================================================

	public static bool getAttack(Enums.PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case Enums.PlayerInput.Keyboard:
			return Input.GetMouseButton(0);
			break;
		case Enums.PlayerInput.GamePadOne:
			return GamePad.GetButton(GamePad.Button.X, GamePad.Index.One);
			break;
		case Enums.PlayerInput.GamePadTwo:
			return GamePad.GetButton(GamePad.Button.X, GamePad.Index.Two);
			break;
		case Enums.PlayerInput.GamePadThree:
			return GamePad.GetButton(GamePad.Button.X, GamePad.Index.Three);
			break;
		case Enums.PlayerInput.GamePadFour:
			return GamePad.GetButton(GamePad.Button.X, GamePad.Index.Four);
			break;
		}
		return false;
	}
	
	public static bool getAttackDown(Enums.PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case Enums.PlayerInput.Keyboard:
			return Input.GetMouseButtonDown(0);
			break;
		case Enums.PlayerInput.GamePadOne:
			return GamePad.GetButtonDown(GamePad.Button.X, GamePad.Index.One);
			break;
		case Enums.PlayerInput.GamePadTwo:			
			return GamePad.GetButtonDown(GamePad.Button.X, GamePad.Index.Two);
			break;
		case Enums.PlayerInput.GamePadThree:
			return GamePad.GetButtonDown(GamePad.Button.X, GamePad.Index.Three);
			break;
		case Enums.PlayerInput.GamePadFour:
			return GamePad.GetButtonDown(GamePad.Button.X, GamePad.Index.Four);
			break;
		}
		return false;
	}
	
	public static bool getAttackUp(Enums.PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case Enums.PlayerInput.Keyboard:
			return Input.GetMouseButtonUp(0);
			break;
		case Enums.PlayerInput.GamePadOne:
			return GamePad.GetButtonUp(GamePad.Button.X, GamePad.Index.One);
			break;
		case Enums.PlayerInput.GamePadTwo:
			return GamePad.GetButtonUp(GamePad.Button.X, GamePad.Index.Two);
			break;
		case Enums.PlayerInput.GamePadThree:
			return GamePad.GetButtonUp(GamePad.Button.X, GamePad.Index.Three);
			break;
		case Enums.PlayerInput.GamePadFour:
			return GamePad.GetButtonUp(GamePad.Button.X, GamePad.Index.Four);
			break;
		}
		return false;
	}

    //=====================================================================================================
    //=====================================================================================================

    public static bool getCharacterSwitch()
    {
        if (Input.GetKey(KeyCode.Tab) || GamePad.GetButton(GamePad.Button.Y, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    public static bool getCharacterSwitchDown()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || GamePad.GetButtonDown(GamePad.Button.Y, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    public static bool getCharacterSwitchUp()
    {
        if (Input.GetKeyUp(KeyCode.Tab) || GamePad.GetButtonUp(GamePad.Button.Y, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    //=====================================================================================================

	public static bool getCharacterSwitch(Enums.PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case Enums.PlayerInput.Keyboard:
			return Input.GetKey(KeyCode.Tab);
			break;
		case Enums.PlayerInput.GamePadOne:
			return GamePad.GetButton(GamePad.Button.Y, GamePad.Index.One);
			break;
		case Enums.PlayerInput.GamePadTwo:
			return GamePad.GetButton(GamePad.Button.Y, GamePad.Index.Two);
			break;
		case Enums.PlayerInput.GamePadThree:
			return GamePad.GetButton(GamePad.Button.Y, GamePad.Index.Three);
			break;
		case Enums.PlayerInput.GamePadFour:
			return GamePad.GetButton(GamePad.Button.Y, GamePad.Index.Four);
			break;
		}
		return false;
	}
	
	public static bool getCharacterSwitchDown(Enums.PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case Enums.PlayerInput.Keyboard:
			return Input.GetKeyDown(KeyCode.Tab);
			break;
		case Enums.PlayerInput.GamePadOne:
			return GamePad.GetButtonDown(GamePad.Button.Y, GamePad.Index.One);
			break;
		case Enums.PlayerInput.GamePadTwo:
			return GamePad.GetButtonDown(GamePad.Button.Y, GamePad.Index.Two);
			break;
		case Enums.PlayerInput.GamePadThree:
			return GamePad.GetButtonDown(GamePad.Button.Y, GamePad.Index.Three);
			break;
		case Enums.PlayerInput.GamePadFour:
			return GamePad.GetButtonDown(GamePad.Button.Y, GamePad.Index.Four);
			break;
		}
		return false;
	}
	
	public static bool getCharacterSwitchUp(Enums.PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case Enums.PlayerInput.Keyboard:
			return Input.GetKeyUp(KeyCode.Tab);
			break;
		case Enums.PlayerInput.GamePadOne:
			return GamePad.GetButtonUp(GamePad.Button.Y, GamePad.Index.One);
			break;
		case Enums.PlayerInput.GamePadTwo:
			return GamePad.GetButtonUp(GamePad.Button.Y, GamePad.Index.Two);
			break;
		case Enums.PlayerInput.GamePadThree:
			return GamePad.GetButtonUp(GamePad.Button.Y, GamePad.Index.Three);
			break;
		case Enums.PlayerInput.GamePadFour:
			return GamePad.GetButtonUp(GamePad.Button.Y, GamePad.Index.Four);
			break;
		}
		return false;
	}

    //=====================================================================================================
    //=====================================================================================================

    public static bool getShowHud()
    {
        if (Input.GetKey(KeyCode.F) || GamePad.GetButton(GamePad.Button.B, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    public static bool getShowHudDown()
    {
        if (Input.GetKeyDown(KeyCode.F) || GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    public static bool getShowHudUp()
    {
        if (Input.GetKeyUp(KeyCode.F) || GamePad.GetButtonUp(GamePad.Button.B, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    //=====================================================================================================

	public static bool getShowHud(Enums.PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case Enums.PlayerInput.Keyboard:
			return Input.GetKey(KeyCode.F);
			break;
		case Enums.PlayerInput.GamePadOne:
			return GamePad.GetButton(GamePad.Button.B, GamePad.Index.One);
			break;
		case Enums.PlayerInput.GamePadTwo:
			return GamePad.GetButton(GamePad.Button.B, GamePad.Index.Two);
			break;
		case Enums.PlayerInput.GamePadThree:
			return GamePad.GetButton(GamePad.Button.B, GamePad.Index.Three);
			break;
		case Enums.PlayerInput.GamePadFour:
			return GamePad.GetButton(GamePad.Button.B, GamePad.Index.Four);
			break;
		}
		return false;
	}
	
	public static bool getShowHudDown(Enums.PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case Enums.PlayerInput.Keyboard:
			return Input.GetKeyDown(KeyCode.F);
			break;
		case Enums.PlayerInput.GamePadOne:
			return GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.One);
			break;
		case Enums.PlayerInput.GamePadTwo:
			return GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Two);
			break;
		case Enums.PlayerInput.GamePadThree:
			return GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Three);
			break;
		case Enums.PlayerInput.GamePadFour:
			return GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Four);
			break;
		}
		return false;
	}
	
	public static bool getShowHudUp(Enums.PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case Enums.PlayerInput.Keyboard:
			return Input.GetKeyUp(KeyCode.F);
			break;
		case Enums.PlayerInput.GamePadOne:
			return GamePad.GetButtonUp(GamePad.Button.B, GamePad.Index.One);
			break;
		case Enums.PlayerInput.GamePadTwo:
			return GamePad.GetButtonUp(GamePad.Button.B, GamePad.Index.Two);
			break;
		case Enums.PlayerInput.GamePadThree:
			return GamePad.GetButtonUp(GamePad.Button.B, GamePad.Index.Three);
			break;
		case Enums.PlayerInput.GamePadFour:
			return GamePad.GetButtonUp(GamePad.Button.B, GamePad.Index.Four);
			break;
		}
		return false;
	}

    //=====================================================================================================
    //=====================================================================================================

    public static bool getPause()
    {
        if (Input.GetKey(KeyCode.Escape) || GamePad.GetButton(GamePad.Button.Start, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    public static bool getPauseDown()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    public static bool getPauseUp()
    {
        if (Input.GetKeyUp(KeyCode.Escape) || GamePad.GetButtonUp(GamePad.Button.Start, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    //=====================================================================================================

	public static bool getPause(Enums.PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case Enums.PlayerInput.Keyboard:
			return Input.GetKey(KeyCode.Escape);
			break;
		case Enums.PlayerInput.GamePadOne:
			return GamePad.GetButton(GamePad.Button.Start, GamePad.Index.One);
			break;
		case Enums.PlayerInput.GamePadTwo:
			return GamePad.GetButton(GamePad.Button.Start, GamePad.Index.Two);
			break;
		case Enums.PlayerInput.GamePadThree:
			return GamePad.GetButton(GamePad.Button.Start, GamePad.Index.Three);
			break;
		case Enums.PlayerInput.GamePadFour:
			return GamePad.GetButton(GamePad.Button.Start, GamePad.Index.Four);
			break;
		}
		return false;
	}
	
	public static bool getPauseDown(Enums.PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case Enums.PlayerInput.Keyboard:
			return Input.GetKeyDown(KeyCode.Escape);
			break;
		case Enums.PlayerInput.GamePadOne:
			return GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.One);
			break;
		case Enums.PlayerInput.GamePadTwo:
			return GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Two);
			break;
		case Enums.PlayerInput.GamePadThree:
			return GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Three);
			break;
		case Enums.PlayerInput.GamePadFour:
			return GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Four);
			break;
		}
		return false;
	}
	
	public static bool getPauseUp(Enums.PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case Enums.PlayerInput.Keyboard:
			return Input.GetKeyUp(KeyCode.Escape);
			break;
		case Enums.PlayerInput.GamePadOne:
			return GamePad.GetButtonUp(GamePad.Button.Start, GamePad.Index.One);
			break;
		case Enums.PlayerInput.GamePadTwo:
			return GamePad.GetButtonUp(GamePad.Button.Start, GamePad.Index.Two);
			break;
		case Enums.PlayerInput.GamePadThree:
			return GamePad.GetButtonUp(GamePad.Button.Start, GamePad.Index.Three);
			break;
		case Enums.PlayerInput.GamePadFour:
			return GamePad.GetButtonUp(GamePad.Button.Start, GamePad.Index.Four);
			break;
		}
		return false;
	}

    //=====================================================================================================
    //=====================================================================================================

    public static Vector2 getMove()
    {
        //TODO: figure out how to convert buttons into an axis and compare the two axises
        return GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any);
    }

    public static Vector2 getMove(GamePad.Index index)
    {
        return GamePad.GetAxis(GamePad.Axis.LeftStick, index);
    }

	public static Vector2 getMove(Enums.PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		//case Enums.PlayerInput.Keyboard:
			//Vector2 input = new Vector2(Input.GetKey(KeyCode.A) - Input.GetKey(KeyCode.D));
			//return 
			//break;
		case Enums.PlayerInput.GamePadOne:
			return GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.One);
			break;
		case Enums.PlayerInput.GamePadTwo:
			return GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Two);
			break;
		case Enums.PlayerInput.GamePadThree:
			return GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Three);
			break;
		case Enums.PlayerInput.GamePadFour:
			return GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Four);
			break;
		}
		return new Vector2 (0, 0);
	}
}
