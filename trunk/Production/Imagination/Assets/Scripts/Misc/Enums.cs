﻿/*
*
*
*resposible for holding enums multiple scripts need
*
*Created by: Kris Matis
*/

#region ChangeLog
/*
* 
*
* 
*/
#endregion

using UnityEngine;
using System.Collections;

public enum PlayerInput
{
	Keyboard,
	GamePadOne,
	GamePadTwo,
	GamePadThree,
	GamePadFour,
	All,
    None
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
	Level_1
}

public enum Sections
{
	Sections_1,
	Sections_2,
	Sections_3
}

public enum CheckPoints
{
	CheckPoint_1,
	CheckPoint_2,
	CheckPoint_3
}
