using UnityEditor;
using UnityEngine;

namespace HierarchyDecorator
{
	public class TagInfo : HierarchyInfo
	{
		protected override void DrawInfo(Rect rect, GameObject instance, Settings settings)
		{
			if (rect.x < (LabelRect.x + LabelRect.width))
			{
				return;
			}

			EditorGUI.LabelField(rect, instance.tag == "Untagged" ? "-" : instance.tag, Style.CenteredSmallLabel);

			// とりあえず表示だけ
		}

		protected override int GetGridCount()
		{
			return 3;
		}

		protected override bool DrawerIsEnabled(Settings settings, GameObject instance)
		{
			if (settings.styleData.HasStyle(instance.name) && !settings.styleData.displayTags)
			{
				return false;
			}
			return settings.globalData.showTags;
		}
	}
}