using UnityEditor;
using UnityEngine;

namespace HierarchyDecorator
{
	public class TagInfo : HierarchyInfo
	{
		GUIStyle _style;
		protected override void DrawInfo(Rect rect, GameObject instance, Settings settings)
		{
			if (rect.x < (LabelRect.x + LabelRect.width))
			{
				return;
			}
			if (_style == null)
				_style = new GUIStyle(Style.SmallDropdown) { wordWrap = true };
			var label_text = instance.tag == "Untagged" ? "-" : instance.tag;
			CalculateFontSizeToFitWidth(_style, rect.width, label_text);
			EditorGUI.LabelField(rect, label_text, _style);

			// とりあえず表示だけ
		}

		protected override int GetGridCount()
		{
			return 5;
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
