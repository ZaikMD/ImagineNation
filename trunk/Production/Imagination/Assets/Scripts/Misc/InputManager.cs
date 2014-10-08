/*
*InputManager
*
*resposible for reading inputs
*
*Created by: Kris Matis
*/

#region ChangeLog
/*
* 8/10/2014 Edit: Fully Commented - Kris Matis.
*
* 
*/
#endregion

using UnityEngine;
using System.Collections;

using GamepadInput;

public static class InputManager
{
	const string MOUSE_X_STRING = "Mouse X";
	const string MOUSE_Y_STRING = "Mouse Y";

	//each input follows the following format

	//Generic functions that read all input for get, down, and up
	//=====================================================================================================
	//specific functions that read a single input for get, down, and up

	//=====================================================================================================
	//=====================================================================================================

	#region Jump
	#region Generic Inputs
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
	#endregion generic Generic
    //=====================================================================================================


	#region Specific Inputs
	public static bool getJump(PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case PlayerInput.Keyboard:
			return Input.GetKey(KeyCode.Space);
			
		case PlayerInput.GamePadOne:
			return GamePad.GetButton(GamePad.Button.A, GamePad.Index.One);
			
		case PlayerInput.GamePadTwo:
			return GamePad.GetButton(GamePad.Button.A, GamePad.Index.Two);
			
		case PlayerInput.GamePadThree:
			return GamePad.GetButton(GamePad.Button.A, GamePad.Index.Three);
			
		case PlayerInput.GamePadFour:
			return GamePad.GetButton(GamePad.Button.A, GamePad.Index.Four);

		case PlayerInput.All:
			return getJump();
		}
		return false;
	}

	public static bool getJumpDown(PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case PlayerInput.Keyboard:
			return Input.GetKeyDown(KeyCode.Space);
			
		case PlayerInput.GamePadOne:
			return GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.One);
			
		case PlayerInput.GamePadTwo:
			return GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Two);
			
		case PlayerInput.GamePadThree:
			return GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Three);
			
		case PlayerInput.GamePadFour:
			return GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Four);

		case PlayerInput.All:
			return getJumpDown();
		}
		return false;
	}

	public static bool getJumpUp(PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case PlayerInput.Keyboard:
			return Input.GetKeyUp(KeyCode.Space);
			
		case PlayerInput.GamePadOne:
			return GamePad.GetButtonUp(GamePad.Button.A, GamePad.Index.One);
			
		case PlayerInput.GamePadTwo:
			return GamePad.GetButtonUp(GamePad.Button.A, GamePad.Index.Two);
			
		case PlayerInput.GamePadThree:
			return GamePad.GetButtonUp(GamePad.Button.A, GamePad.Index.Three);
			
		case PlayerInput.GamePadFour:
			return GamePad.GetButtonUp(GamePad.Button.A, GamePad.Index.Four);

		case PlayerInput.All:
			return getJumpUp();
		}
		return false;
	}
	#endregion
    //=====================================================================================================
    //=====================================================================================================
	#endregion

    #region Attack
    #region Generic Inputs
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
    #endregion
    //=====================================================================================================
    #region Specific Input
    public static bool getAttack(PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case PlayerInput.Keyboard:
			return Input.GetMouseButton(0);
			
		case PlayerInput.GamePadOne:
			return GamePad.GetButton(GamePad.Button.X, GamePad.Index.One);
			
		case PlayerInput.GamePadTwo:
			return GamePad.GetButton(GamePad.Button.X, GamePad.Index.Two);
			
		case PlayerInput.GamePadThree:
			return GamePad.GetButton(GamePad.Button.X, GamePad.Index.Three);
			
		case PlayerInput.GamePadFour:
			return GamePad.GetButton(GamePad.Button.X, GamePad.Index.Four);

		case PlayerInput.All:
			return getAttack();
		}
		return false;
	}
	
	public static bool getAttackDown(PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case PlayerInput.Keyboard:
			return Input.GetMouseButtonDown(0);
			
		case PlayerInput.GamePadOne:
			return GamePad.GetButtonDown(GamePad.Button.X, GamePad.Index.One);
			
		case PlayerInput.GamePadTwo:			
			return GamePad.GetButtonDown(GamePad.Button.X, GamePad.Index.Two);
			
		case PlayerInput.GamePadThree:
			return GamePad.GetButtonDown(GamePad.Button.X, GamePad.Index.Three);
			
		case PlayerInput.GamePadFour:
			return GamePad.GetButtonDown(GamePad.Button.X, GamePad.Index.Four);

		case PlayerInput.All:
			return getAttackDown();
		}
		return false;
	}
	
	public static bool getAttackUp(PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case PlayerInput.Keyboard:
			return Input.GetMouseButtonUp(0);
			
		case PlayerInput.GamePadOne:
			return GamePad.GetButtonUp(GamePad.Button.X, GamePad.Index.One);
			
		case PlayerInput.GamePadTwo:
			return GamePad.GetButtonUp(GamePad.Button.X, GamePad.Index.Two);
			
		case PlayerInput.GamePadThree:
			return GamePad.GetButtonUp(GamePad.Button.X, GamePad.Index.Three);
			
		case PlayerInput.GamePadFour:
			return GamePad.GetButtonUp(GamePad.Button.X, GamePad.Index.Four);

		case PlayerInput.All:
			return getAttackUp();
		}
		return false;
	}
    #endregion
    //=====================================================================================================
    //=====================================================================================================
    #endregion

    #region Character Switch
    #region Generic Inputs
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
    #endregion

    //=====================================================================================================
    #region Specific Input
    public static bool getCharacterSwitch(PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case PlayerInput.Keyboard:
			return Input.GetKey(KeyCode.Tab);
			
		case PlayerInput.GamePadOne:
			return GamePad.GetButton(GamePad.Button.Y, GamePad.Index.One);
			
		case PlayerInput.GamePadTwo:
			return GamePad.GetButton(GamePad.Button.Y, GamePad.Index.Two);
			
		case PlayerInput.GamePadThree:
			return GamePad.GetButton(GamePad.Button.Y, GamePad.Index.Three);
			
		case PlayerInput.GamePadFour:
			return GamePad.GetButton(GamePad.Button.Y, GamePad.Index.Four);

		case PlayerInput.All:
			return getCharacterSwitch();
		}
		return false;
	}
	
	public static bool getCharacterSwitchDown(PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case PlayerInput.Keyboard:
			return Input.GetKeyDown(KeyCode.Tab);
			
		case PlayerInput.GamePadOne:
			return GamePad.GetButtonDown(GamePad.Button.Y, GamePad.Index.One);
			
		case PlayerInput.GamePadTwo:
			return GamePad.GetButtonDown(GamePad.Button.Y, GamePad.Index.Two);
			
		case PlayerInput.GamePadThree:
			return GamePad.GetButtonDown(GamePad.Button.Y, GamePad.Index.Three);
			
		case PlayerInput.GamePadFour:
			return GamePad.GetButtonDown(GamePad.Button.Y, GamePad.Index.Four);

		case PlayerInput.All:
			return getCharacterSwitchDown();
		}
		return false;
	}
	
	public static bool getCharacterSwitchUp(PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case PlayerInput.Keyboard:
			return Input.GetKeyUp(KeyCode.Tab);
			
		case PlayerInput.GamePadOne:
			return GamePad.GetButtonUp(GamePad.Button.Y, GamePad.Index.One);
			
		case PlayerInput.GamePadTwo:
			return GamePad.GetButtonUp(GamePad.Button.Y, GamePad.Index.Two);
			
		case PlayerInput.GamePadThree:
			return GamePad.GetButtonUp(GamePad.Button.Y, GamePad.Index.Three);
			
		case PlayerInput.GamePadFour:
			return GamePad.GetButtonUp(GamePad.Button.Y, GamePad.Index.Four);

		case PlayerInput.All:
			return getCharacterSwitchUp();
		}
		return false;
	}
    #endregion
    //=====================================================================================================
    //=====================================================================================================
    #endregion

    #region Show Hud
    #region
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
    #endregion
    //=====================================================================================================
    #region Specific Input
    public static bool getShowHud(PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case PlayerInput.Keyboard:
			return Input.GetKey(KeyCode.F);
			
		case PlayerInput.GamePadOne:
			return GamePad.GetButton(GamePad.Button.B, GamePad.Index.One);
			
		case PlayerInput.GamePadTwo:
			return GamePad.GetButton(GamePad.Button.B, GamePad.Index.Two);
			
		case PlayerInput.GamePadThree:
			return GamePad.GetButton(GamePad.Button.B, GamePad.Index.Three);
			
		case PlayerInput.GamePadFour:
			return GamePad.GetButton(GamePad.Button.B, GamePad.Index.Four);

		case PlayerInput.All:
			return getShowHud();
		}
		return false;
	}
	
	public static bool getShowHudDown(PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case PlayerInput.Keyboard:
			return Input.GetKeyDown(KeyCode.F);
			
		case PlayerInput.GamePadOne:
			return GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.One);
			
		case PlayerInput.GamePadTwo:
			return GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Two);
			
		case PlayerInput.GamePadThree:
			return GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Three);
			
		case PlayerInput.GamePadFour:
			return GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Four);

		case PlayerInput.All:
			return getShowHudDown();
		}
		return false;
	}
	
	public static bool getShowHudUp(PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case PlayerInput.Keyboard:
			return Input.GetKeyUp(KeyCode.F);
			
		case PlayerInput.GamePadOne:
			return GamePad.GetButtonUp(GamePad.Button.B, GamePad.Index.One);
			
		case PlayerInput.GamePadTwo:
			return GamePad.GetButtonUp(GamePad.Button.B, GamePad.Index.Two);
			
		case PlayerInput.GamePadThree:
			return GamePad.GetButtonUp(GamePad.Button.B, GamePad.Index.Three);
			
		case PlayerInput.GamePadFour:
			return GamePad.GetButtonUp(GamePad.Button.B, GamePad.Index.Four);

		case PlayerInput.All:
			return getShowHudUp();
		}
		return false;
	}
    #endregion
    //=====================================================================================================
    //=====================================================================================================
    #endregion

    #region Pause
    #region  Generic Inputs
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
    #endregion

    //=====================================================================================================
    #region Specific Input
    public static bool getPause(PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case PlayerInput.Keyboard:
			return Input.GetKey(KeyCode.Escape);
			
		case PlayerInput.GamePadOne:
			return GamePad.GetButton(GamePad.Button.Start, GamePad.Index.One);
			
		case PlayerInput.GamePadTwo:
			return GamePad.GetButton(GamePad.Button.Start, GamePad.Index.Two);
			
		case PlayerInput.GamePadThree:
			return GamePad.GetButton(GamePad.Button.Start, GamePad.Index.Three);
			
		case PlayerInput.GamePadFour:
			return GamePad.GetButton(GamePad.Button.Start, GamePad.Index.Four);

		case PlayerInput.All:
			return getPause();
		}
		return false;
	}
	
	public static bool getPauseDown(PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case PlayerInput.Keyboard:
			return Input.GetKeyDown(KeyCode.Escape);
			
		case PlayerInput.GamePadOne:
			return GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.One);
			
		case PlayerInput.GamePadTwo:
			return GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Two);
			
		case PlayerInput.GamePadThree:
			return GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Three);
			
		case PlayerInput.GamePadFour:
			return GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Four);

		case PlayerInput.All:
			return getPauseDown();
		}
		return false;
	}
	
	public static bool getPauseUp(PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case PlayerInput.Keyboard:
			return Input.GetKeyUp(KeyCode.Escape);
			
		case PlayerInput.GamePadOne:
			return GamePad.GetButtonUp(GamePad.Button.Start, GamePad.Index.One);
			
		case PlayerInput.GamePadTwo:
			return GamePad.GetButtonUp(GamePad.Button.Start, GamePad.Index.Two);
			
		case PlayerInput.GamePadThree:
			return GamePad.GetButtonUp(GamePad.Button.Start, GamePad.Index.Three);
			
		case PlayerInput.GamePadFour:
			return GamePad.GetButtonUp(GamePad.Button.Start, GamePad.Index.Four);

		case PlayerInput.All:
			return getPauseUp();
		}
		return false;
	}
    #endregion
    
    //=====================================================================================================
    //=====================================================================================================
    #endregion

    #region Move
    #region Generic Inputs
    public static Vector2 getMove()
    {
		Vector2 input = turnWASDIntoVector2 ();
		if(input.magnitude == 0)
		{
        	return GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any);
		}
		return input;
    }
    #endregion
    //==========================================================================================
    #region Specific Inputs
    public static Vector2 getMove(PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case PlayerInput.Keyboard:
			return turnWASDIntoVector2 ();
		case PlayerInput.GamePadOne:
			return GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.One);
			
		case PlayerInput.GamePadTwo:
			return GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Two);
			
		case PlayerInput.GamePadThree:
			return GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Three);
			
		case PlayerInput.GamePadFour:
			return GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Four);	

		case PlayerInput.All:
			return getMove();
		}
		return new Vector2 (0, 0);
	}

    static Vector2 turnWASDIntoVector2 ()
	{
		Vector2 input = new Vector2 (0.0f, 0.0f);
		if(Input.GetKey (KeyCode.D))
		{
			input.x += 1.0f;
		}

		if(Input.GetKey (KeyCode.A))
		{
			input.x -= 1.0f;
		}

		if(Input.GetKey (KeyCode.W))
		{
			input.y += 1.0f;
		}

		if(Input.GetKey (KeyCode.S))
		{
			input.y -= 1.0f;
		}
		return input;
    }
    #endregion
    #endregion

    #region Camera
    #region Generic Inputs
    public static Vector2 getCamera()
    {
		Vector2 input = new Vector2(Input.GetAxis(MOUSE_X_STRING), Input.GetAxis(MOUSE_Y_STRING));
        if (input.magnitude == 0)
        {
            return GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any);
        }
        return input;
    }
    #endregion
    //==========================================================================================
    #region Specific Inputs
    public static Vector2 getCamera(PlayerInput inputToRead)
    {
        switch (inputToRead)
        {
            case PlayerInput.Keyboard:
			return new Vector2(Input.GetAxis(MOUSE_X_STRING), Input.GetAxis(MOUSE_Y_STRING));
            case PlayerInput.GamePadOne:
                return GamePad.GetAxis(GamePad.Axis.RightStick, GamePad.Index.One);

            case PlayerInput.GamePadTwo:
                return GamePad.GetAxis(GamePad.Axis.RightStick, GamePad.Index.Two);

            case PlayerInput.GamePadThree:
                return GamePad.GetAxis(GamePad.Axis.RightStick, GamePad.Index.Three);

            case PlayerInput.GamePadFour:
                return GamePad.GetAxis(GamePad.Axis.RightStick, GamePad.Index.Four);

            case PlayerInput.All:
                return getCamera();
        }
        return new Vector2(0, 0);
    }
    #endregion
    #endregion
}
