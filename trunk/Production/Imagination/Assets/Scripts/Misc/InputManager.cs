/*
*InputManager
*
*resposible for reading inputs
*
*Created by: Kris Matis
 *
 * 23/10/2014 kris matis : added bit based input reading
*/

#region ChangeLog
/*
* 8/10/2014 Edit: Fully Commented - Kris Matis.
*
* 21/10/*2014 Edit : added camera snapping inputs
 * 
 * 21/10/2014 edit : added the menu inputs
*/
#endregion

using UnityEngine;
using System.Collections;

using GamepadInput;

public static class InputManager
{
	const string MOUSE_X_STRING = "Mouse X";
	const string MOUSE_Y_STRING = "Mouse Y";

    const int MOUSE_LEFT = 0;
    const int MOUSE_RIGHT = 1;

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

    #region Bits
    public static bool getJump(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if(getJump(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool getJumpDown(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getJumpDown(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool getJumpUp(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getJumpUp(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    #endregion
    //=====================================================================================================
	#endregion

    #region Attack
    #region Generic Inputs
    public static bool getAttack()
    {
        if (Input.GetMouseButton(MOUSE_LEFT) || GamePad.GetButton(GamePad.Button.X, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    public static bool getAttackDown()
    {
        if (Input.GetMouseButtonDown(MOUSE_LEFT) || GamePad.GetButtonDown(GamePad.Button.X, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    public static bool getAttackUp()
    {
        if (Input.GetMouseButtonUp(MOUSE_LEFT) || GamePad.GetButtonUp(GamePad.Button.X, GamePad.Index.Any))
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
            return Input.GetMouseButton(MOUSE_LEFT);
			
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
            return Input.GetMouseButtonDown(MOUSE_LEFT);
			
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
            return Input.GetMouseButtonUp(MOUSE_LEFT);
			
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
    #region Bits
    public static bool getAttack(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if(getAttack(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool getAttackDown(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getAttackDown(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool getAttackUp(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getAttackUp(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    #endregion
    //=====================================================================================================
	#endregion

    #region Heavy attack
    #region Generic Inputs
    public static bool getHeavyAttack()
    {
        if (Input.GetMouseButton(MOUSE_RIGHT) || GamePad.GetButton(GamePad.Button.Y, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    public static bool getHeavyAttackDown()
    {
        if (Input.GetMouseButtonDown(MOUSE_RIGHT) || GamePad.GetButtonDown(GamePad.Button.Y, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

    public static bool getHeavyAttackUp()
    {
        if (Input.GetMouseButtonUp(MOUSE_RIGHT) || GamePad.GetButtonUp(GamePad.Button.Y, GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }
    #endregion

    //=====================================================================================================
    #region Specific Input
    public static bool getHeavyAttack(PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case PlayerInput.Keyboard:
			return Input.GetMouseButton(MOUSE_RIGHT);
			
		case PlayerInput.GamePadOne:
			return GamePad.GetButton(GamePad.Button.Y, GamePad.Index.One);
			
		case PlayerInput.GamePadTwo:
			return GamePad.GetButton(GamePad.Button.Y, GamePad.Index.Two);
			
		case PlayerInput.GamePadThree:
			return GamePad.GetButton(GamePad.Button.Y, GamePad.Index.Three);
			
		case PlayerInput.GamePadFour:
			return GamePad.GetButton(GamePad.Button.Y, GamePad.Index.Four);

		case PlayerInput.All:
            return getHeavyAttack();
		}
		return false;
	}

    public static bool getHeavyAttackDown(PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case PlayerInput.Keyboard:
                return Input.GetMouseButtonDown(MOUSE_RIGHT);
			
		case PlayerInput.GamePadOne:
			return GamePad.GetButtonDown(GamePad.Button.Y, GamePad.Index.One);
			
		case PlayerInput.GamePadTwo:
			return GamePad.GetButtonDown(GamePad.Button.Y, GamePad.Index.Two);
			
		case PlayerInput.GamePadThree:
			return GamePad.GetButtonDown(GamePad.Button.Y, GamePad.Index.Three);
			
		case PlayerInput.GamePadFour:
			return GamePad.GetButtonDown(GamePad.Button.Y, GamePad.Index.Four);

		case PlayerInput.All:
            return getHeavyAttackDown();
		}
		return false;
	}

    public static bool getHeavyAttackUp(PlayerInput inputToRead)
	{
		switch(inputToRead)
		{
		case PlayerInput.Keyboard:
                return Input.GetMouseButtonUp(MOUSE_RIGHT);
			
		case PlayerInput.GamePadOne:
			return GamePad.GetButtonUp(GamePad.Button.Y, GamePad.Index.One);
			
		case PlayerInput.GamePadTwo:
			return GamePad.GetButtonUp(GamePad.Button.Y, GamePad.Index.Two);
			
		case PlayerInput.GamePadThree:
			return GamePad.GetButtonUp(GamePad.Button.Y, GamePad.Index.Three);
			
		case PlayerInput.GamePadFour:
			return GamePad.GetButtonUp(GamePad.Button.Y, GamePad.Index.Four);

		case PlayerInput.All:
            return getHeavyAttackUp();
		}
		return false;
	}
    #endregion
    //=====================================================================================================
    #region Bits
    public static bool getHeavyAttack(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getHeavyAttack(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool getHeavyAttackDown(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getHeavyAttackDown(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool getHeavyAttackUp(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getHeavyAttackUp(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    #endregion
    //=====================================================================================================
    #endregion

    #region Show Hud
    #region generic inputs
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
    #region Bits
    public static bool getShowHud(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getShowHud(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool getShowHudDown(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getShowHudDown(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool getShowHudUp(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getShowHudUp(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    #endregion
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
    #region Bits
    public static bool getPause(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getPause(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool getPauseDown(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getPauseDown(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool getPauseUp(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getPauseUp(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    #endregion
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
    //==========================================================================================
    #region Bits
    public static Vector2 getMove(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getMove(Constants.PLAYER_INPUT_ARRAY[i]) != Vector2.zero)
                {
                    return getMove(Constants.PLAYER_INPUT_ARRAY[i]);
                }
            }
        }
        return Vector2.zero;
    }
    #endregion
    #endregion

    #region Camera Movement
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
    //==========================================================================================
    #region Bits
    public static Vector2 getCamera(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getCamera(Constants.PLAYER_INPUT_ARRAY[i]) != Vector2.zero)
                {
                    return getCamera(Constants.PLAYER_INPUT_ARRAY[i]);
                }
            }
        }
        return Vector2.zero;
    }
    #endregion
    #endregion

    #region Camera Snap
    #region Generic Inputs
    public static bool getCameraSnap()
    {
        return GamePad.GetButton(GamePad.Button.RightStick, GamePad.Index.Any) || Input.GetKey(KeyCode.Tab);
    }

    public static bool getCameraSnapDown()
    {
        return GamePad.GetButtonDown(GamePad.Button.RightStick, GamePad.Index.Any) || Input.GetKeyDown(KeyCode.Tab);
    }

    public static bool getCameraSnapUp()
    {
        return GamePad.GetButtonUp(GamePad.Button.RightStick, GamePad.Index.Any) || Input.GetKeyUp(KeyCode.Tab);
    }
    #endregion
    //==========================================================================================
    #region Specific Inputs
    public static bool getCameraSnap(PlayerInput inputToRead)
    {
        switch (inputToRead)
        {
            case PlayerInput.Keyboard:
                return Input.GetKey(KeyCode.Tab);

            case PlayerInput.GamePadOne:
                return GamePad.GetButton(GamePad.Button.RightStick, GamePad.Index.One);

            case PlayerInput.GamePadTwo:
                return GamePad.GetButton(GamePad.Button.RightStick, GamePad.Index.Two);

            case PlayerInput.GamePadThree:
                return GamePad.GetButton(GamePad.Button.RightStick, GamePad.Index.Three);

            case PlayerInput.GamePadFour:
                return GamePad.GetButton(GamePad.Button.RightStick, GamePad.Index.Four);

            case PlayerInput.All:
                return getCameraSnap();
        }
        return false;
    }

    //==========================================================================================
    public static bool getCameraSnapDown(PlayerInput inputToRead)
    {
        switch (inputToRead)
        {
            case PlayerInput.Keyboard:
                return Input.GetKeyDown(KeyCode.Tab);

            case PlayerInput.GamePadOne:
                return GamePad.GetButtonDown(GamePad.Button.RightStick, GamePad.Index.One);

            case PlayerInput.GamePadTwo:
                return GamePad.GetButtonDown(GamePad.Button.RightStick, GamePad.Index.Two);

            case PlayerInput.GamePadThree:
                return GamePad.GetButtonDown(GamePad.Button.RightStick, GamePad.Index.Three);

            case PlayerInput.GamePadFour:
                return GamePad.GetButtonDown(GamePad.Button.RightStick, GamePad.Index.Four);

            case PlayerInput.All:
                return getCameraSnapDown();
        }
        return false;
    }

    //==========================================================================================
    public static bool getCameraSnapUp(PlayerInput inputToRead)
    {
        switch (inputToRead)
        {
            case PlayerInput.Keyboard:
                return Input.GetKeyUp(KeyCode.Tab);

            case PlayerInput.GamePadOne:
                return GamePad.GetButtonUp(GamePad.Button.RightStick, GamePad.Index.One);

            case PlayerInput.GamePadTwo:
                return GamePad.GetButtonUp(GamePad.Button.RightStick, GamePad.Index.Two);

            case PlayerInput.GamePadThree:
                return GamePad.GetButtonUp(GamePad.Button.RightStick, GamePad.Index.Three);

            case PlayerInput.GamePadFour:
                return GamePad.GetButtonUp(GamePad.Button.RightStick, GamePad.Index.Four);

            case PlayerInput.All:
                return getCameraSnapUp();
        }
        return false;
    }
    #endregion
    //=====================================================================================================
    #region Bits
    public static bool getCameraSnap(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getCameraSnap(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool getCameraSnapDown(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getCameraSnapDown(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool getCameraSnapUp(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getCameraSnapUp(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    #endregion
    #endregion

    //=======================================================================================================

    #region Menu Change Selection
    #region Generic Inputs
    public static Vector2 getMenuChangeSelection()
    {
        Vector2 input = turnWASDIntoVector2();
        if (input.magnitude == 0)
        {
            return GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any);
        }
        return input;
    }
    #endregion
    //==========================================================================================
    #region Specific Inputs
    public static Vector2 getMenuChangeSelection(PlayerInput inputToRead)
    {
        switch (inputToRead)
        {
            case PlayerInput.Keyboard:
                return turnWASDIntoVector2();
            case PlayerInput.GamePadOne:
                return GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.One);

            case PlayerInput.GamePadTwo:
                return GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Two);

            case PlayerInput.GamePadThree:
                return GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Three);

            case PlayerInput.GamePadFour:
                return GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Four);

            case PlayerInput.All:
                return getMenuChangeSelection();
        }
        return new Vector2(0, 0);
    }
    #endregion
    //==========================================================================================
    #region Bits
    public static Vector2 getMenuChangeSelection(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getMenuChangeSelection(Constants.PLAYER_INPUT_ARRAY[i]) != Vector2.zero)
                {
                    return getMenuChangeSelection(Constants.PLAYER_INPUT_ARRAY[i]);
                }
            }
        }
        return Vector2.zero;
    }

    public static Vector2 getMenuChangeSelection(int bitsToRead, out PlayerInput input)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getMenuChangeSelection(Constants.PLAYER_INPUT_ARRAY[i]) != Vector2.zero)
                {
                    input = (PlayerInput)Constants.PLAYER_INPUT_ARRAY[i];
                    return getMenuChangeSelection(Constants.PLAYER_INPUT_ARRAY[i]);
                }
            }
        }
        input = PlayerInput.None;
        return Vector2.zero;
    }
    #endregion
    #endregion
   
    #region Menu Start
    #region Generic Inputs
    public static bool getMenuStart()
    {
        return GamePad.GetButton(GamePad.Button.Start, GamePad.Index.Any) || Input.GetKey(KeyCode.Return);
    }

    public static bool getMenuStartDown()
    {
        return GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Any) || Input.GetKeyDown(KeyCode.Return);
    }

    public static bool getMenuStartUp()
    {
        return GamePad.GetButtonUp(GamePad.Button.Start, GamePad.Index.Any) || Input.GetKeyUp(KeyCode.Return);
    }
    #endregion
    //==========================================================================================
    #region Specific Inputs
    public static bool getMenuStart(PlayerInput inputToRead)
    {
        switch (inputToRead)
        {
            case PlayerInput.Keyboard:
                return Input.GetKey(KeyCode.Return);

            case PlayerInput.GamePadOne:
                return GamePad.GetButton(GamePad.Button.Start, GamePad.Index.One);

            case PlayerInput.GamePadTwo:
                return GamePad.GetButton(GamePad.Button.Start, GamePad.Index.Two);

            case PlayerInput.GamePadThree:
                return GamePad.GetButton(GamePad.Button.Start, GamePad.Index.Three);

            case PlayerInput.GamePadFour:
                return GamePad.GetButton(GamePad.Button.Start, GamePad.Index.Four);

            case PlayerInput.All:
                return getMenuStart();
        }
        return false;
    }

    //==========================================================================================
    public static bool getMenuStartDown(PlayerInput inputToRead)
    {
        switch (inputToRead)
        {
            case PlayerInput.Keyboard:
                return Input.GetKeyDown(KeyCode.Return);

            case PlayerInput.GamePadOne:
                return GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.One);

            case PlayerInput.GamePadTwo:
                return GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Two);

            case PlayerInput.GamePadThree:
                return GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Three);

            case PlayerInput.GamePadFour:
                return GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Four);

            case PlayerInput.All:
                return getMenuStartDown();
        }
        return false;
    }

    //==========================================================================================
    public static bool getMenuStartUp(PlayerInput inputToRead)
    {
        switch (inputToRead)
        {
            case PlayerInput.Keyboard:
                return Input.GetKeyUp(KeyCode.Return);

            case PlayerInput.GamePadOne:
                return GamePad.GetButtonUp(GamePad.Button.Start, GamePad.Index.One);

            case PlayerInput.GamePadTwo:
                return GamePad.GetButtonUp(GamePad.Button.Start, GamePad.Index.Two);

            case PlayerInput.GamePadThree:
                return GamePad.GetButtonUp(GamePad.Button.Start, GamePad.Index.Three);

            case PlayerInput.GamePadFour:
                return GamePad.GetButtonUp(GamePad.Button.Start, GamePad.Index.Four);

            case PlayerInput.All:
                return getMenuStartUp();
        }
        return false;
    }
    #endregion
    //=====================================================================================================
    #region Bits
    public static bool getMenuStart(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getMenuStart(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool getMenuStartDown(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getMenuStartDown(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool getMenuStartUp(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getMenuStartUp(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    //=====================================================================================================

    public static bool getMenuStart(int bitsToRead, out PlayerInput input)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getMenuStart(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    input = (PlayerInput)Constants.PLAYER_INPUT_ARRAY[i];
                    return true;
                }
            }
        }
        input = PlayerInput.None;
        return false;
    }

    public static bool getMenuStartDown(int bitsToRead, out PlayerInput input)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getMenuStartDown(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    input = (PlayerInput)Constants.PLAYER_INPUT_ARRAY[i];
                    return true;
                }
            }
        }
        input = PlayerInput.None;
        return false;
    }

    public static bool getMenuStartUp(int bitsToRead, out PlayerInput input)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getMenuStartUp(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    input = (PlayerInput)Constants.PLAYER_INPUT_ARRAY[i];
                    return true;
                }
            }
        }
        input = PlayerInput.None;
        return false;
    }
    #endregion
    #endregion

    #region Menu Accept
    #region Generic Inputs
    public static bool getMenuAccept()
    {
        return GamePad.GetButton(GamePad.Button.A, GamePad.Index.Any) || Input.GetKey(KeyCode.Space);
    }

    public static bool getMenuAcceptDown()
    {
        return GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Any) || Input.GetKeyDown(KeyCode.Space);
    }

    public static bool getMenuAcceptUp()
    {
        return GamePad.GetButtonUp(GamePad.Button.A, GamePad.Index.Any) || Input.GetKeyUp(KeyCode.Space);
    }
    #endregion
    //==========================================================================================
    #region Specific Inputs
    public static bool getMenuAccept(PlayerInput inputToRead)
    {
        switch (inputToRead)
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
                return getMenuAccept();
        }
        return false;
    }

    //==========================================================================================
    public static bool getMenuAcceptDown(PlayerInput inputToRead)
    {
        switch (inputToRead)
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
                return getMenuAcceptDown();
        }
        return false;
    }

    //==========================================================================================
    public static bool getMenuAcceptUp(PlayerInput inputToRead)
    {
        switch (inputToRead)
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
                return getMenuAcceptUp();
        }
        return false;
    }
    #endregion
    //=====================================================================================================
    #region Bits
    public static bool getMenuAccept(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getMenuAccept(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool getMenuAcceptDown(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getMenuAcceptDown(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool getMenuAcceptUp(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getMenuAcceptUp(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }
    //=====================================================================================================

    public static bool getMenuAccept(int bitsToRead, out PlayerInput input)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getMenuAccept(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    input = (PlayerInput)Constants.PLAYER_INPUT_ARRAY[i];
                    return true;
                }
            }
        }
        input = PlayerInput.None;
        return false;
    }

    public static bool getMenuAcceptDown(int bitsToRead, out PlayerInput input)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getMenuAcceptDown(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    input = (PlayerInput)Constants.PLAYER_INPUT_ARRAY[i];
                    return true;
                }
            }
        }
        input = PlayerInput.None;
        return false;
    }

    public static bool getMenuAcceptUp(int bitsToRead, out PlayerInput input)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getMenuAcceptUp(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    input = (PlayerInput)Constants.PLAYER_INPUT_ARRAY[i];
                    return true;
                }
            }
        }
        input = PlayerInput.None;
        return false;
    }
    #endregion
    #endregion

    #region Menu Back
    #region Generic Inputs
    public static bool getMenuBack()
    {
        return GamePad.GetButton(GamePad.Button.B, GamePad.Index.Any) || Input.GetKey(KeyCode.Escape);
    }

    public static bool getMenuBackDown()
    {
        return GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Any) || Input.GetKeyDown(KeyCode.Escape);
    }

    public static bool getMenuBackUp()
    {
        return GamePad.GetButtonUp(GamePad.Button.B, GamePad.Index.Any) || Input.GetKeyUp(KeyCode.Escape);
    }
    #endregion
    //==========================================================================================
    #region Specific Inputs
    public static bool getMenuBack(PlayerInput inputToRead)
    {
        switch (inputToRead)
        {
            case PlayerInput.Keyboard:
                return Input.GetKey(KeyCode.Escape);

            case PlayerInput.GamePadOne:
                return GamePad.GetButton(GamePad.Button.B, GamePad.Index.One);

            case PlayerInput.GamePadTwo:
                return GamePad.GetButton(GamePad.Button.B, GamePad.Index.Two);

            case PlayerInput.GamePadThree:
                return GamePad.GetButton(GamePad.Button.B, GamePad.Index.Three);

            case PlayerInput.GamePadFour:
                return GamePad.GetButton(GamePad.Button.B, GamePad.Index.Four);

            case PlayerInput.All:
                return getMenuBack();
        }
        return false;
    }

    //==========================================================================================
    public static bool getMenuBackDown(PlayerInput inputToRead)
    {
        switch (inputToRead)
        {
            case PlayerInput.Keyboard:
                return Input.GetKeyDown(KeyCode.Escape);

            case PlayerInput.GamePadOne:
                return GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.One);

            case PlayerInput.GamePadTwo:
                return GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Two);

            case PlayerInput.GamePadThree:
                return GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Three);

            case PlayerInput.GamePadFour:
                return GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Four);

            case PlayerInput.All:
                return getMenuBackDown();
        }
        return false;
    }

    //==========================================================================================
    public static bool getMenuBackUp(PlayerInput inputToRead)
    {
        switch (inputToRead)
        {
            case PlayerInput.Keyboard:
                return Input.GetKeyUp(KeyCode.Escape);

            case PlayerInput.GamePadOne:
                return GamePad.GetButtonUp(GamePad.Button.B, GamePad.Index.One);

            case PlayerInput.GamePadTwo:
                return GamePad.GetButtonUp(GamePad.Button.B, GamePad.Index.Two);

            case PlayerInput.GamePadThree:
                return GamePad.GetButtonUp(GamePad.Button.B, GamePad.Index.Three);

            case PlayerInput.GamePadFour:
                return GamePad.GetButtonUp(GamePad.Button.B, GamePad.Index.Four);

            case PlayerInput.All:
                return getMenuBackUp();
        }
        return false;
    }
    #endregion
    //=====================================================================================================
    #region Bits
    public static bool getMenuBack(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getMenuBack(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool getMenuBackDown(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getMenuBackDown(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool getMenuBackUp(int bitsToRead)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getMenuBackUp(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }
    //=====================================================================================================

    public static bool getMenuBack(int bitsToRead, out PlayerInput input)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getMenuBack(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    input = (PlayerInput)Constants.PLAYER_INPUT_ARRAY[i];
                    return true;
                }
            }
        }
        input = PlayerInput.None;
        return false;
    }

    public static bool getMenuBackDown(int bitsToRead, out PlayerInput input)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getMenuBackDown(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    input = (PlayerInput)Constants.PLAYER_INPUT_ARRAY[i];
                    return true;
                }
            }
        }
        input = PlayerInput.None;
        return false;
    }

    public static bool getMenuBackUp(int bitsToRead, out PlayerInput input)
    {
        for (int i = 0; i < (int)PlayerInput.Count; i++)
        {
            if (((int)Constants.PLAYER_INPUT_ARRAY[i] & bitsToRead) > 0)
            {
                if (getMenuBackUp(Constants.PLAYER_INPUT_ARRAY[i]))
                {
                    input = (PlayerInput)Constants.PLAYER_INPUT_ARRAY[i];
                    return true;
                }
            }
        }
        input = PlayerInput.None;
        return false;
    }
    #endregion
    #endregion
}
