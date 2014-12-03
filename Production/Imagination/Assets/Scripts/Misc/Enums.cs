/*
*
*
*resposible for holding enums multiple scripts need
*
*Created by: Kris Matis
 *
 * 23/10/2014 kris matis :made the inputs bit based
*/

#region ChangeLog
/*
* added counts and none to levels,section,and checkpoints
*
* 
*/
#endregion

using UnityEngine;
using System.Collections;

public enum PlayerInput
{
	Keyboard = 1,
	GamePadOne = 2,
	GamePadTwo = 4,
	GamePadThree = 8,
	GamePadFour = 16,
    All = Keyboard | GamePadOne | GamePadTwo | GamePadThree | GamePadFour,
    Count = 5,// <============== IT IS CRITICAL TO UPDATE THIS IF THE NUMBER CHANGES,     ALSO UPDATE THE CONSTANT ARRAY
    None = 0
}

public enum Characters
{
	Zoe,
	Derek,
	Alex
}

public enum Players
{
	PlayerOne,
	PlayerTwo
}

public enum Levels
{
	Level_1,
	Count,
	None
}

public enum Sections
{
	Sections_1,
	Sections_2,
	Sections_3,
	Count,
	None
}

public enum CheckPoints
{
	CheckPoint_1,
	CheckPoint_2,
	CheckPoint_3,
	Count,
	None
}

