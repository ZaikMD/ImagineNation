/*
 * Created by: Kris MAtis
 * menu for multi player character selection
 * 
 */

#region ChangeLog
/*
 * 29/10/2014 edit: made the button script
 */
#endregion

using UnityEngine;
using System.Collections;

public class ButtonChangeMenuAndSetLevelData : ButtonChangeMenuV2
{
	public Levels LevelSetting = Levels.None;
	public Sections SectionSetting = Sections.None;
	public CheckPoints checkPointSetting = CheckPoints.None;

	public override void use (PlayerInput usedBy = PlayerInput.None)
	{
		base.use ();

		if(LevelSetting != Levels.None && LevelSetting != Levels.Count)
		{
			GameData.Instance.CurrentLevel = LevelSetting;
		}

		if(SectionSetting != Sections.None && SectionSetting != Sections.Count)
		{
			GameData.Instance.CurrentSection = SectionSetting;
		}

		if(checkPointSetting != CheckPoints.None && checkPointSetting != CheckPoints.Count)
		{
			GameData.Instance.CurrentCheckPoint = checkPointSetting;
		}
	}
}
