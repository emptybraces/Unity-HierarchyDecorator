using UnityEditor;
using UnityEngine;

namespace HierarchyDecorator
{
	public class HideFlagsInfo : HierarchyInfo
	{
		protected override void DrawInfo(Rect rect, GameObject instance, Settings settings)
		{
			if (rect.x < (LabelRect.x + LabelRect.width))
			{
				return;
			}

			if (instance.hideFlags == HideFlags.None)
				EditorGUI.LabelField(rect, "-", Style.CenteredSmallLabel);
			else
				EditorGUI.LabelField(rect, "!", Style.CenteredLabelRed);

			if (settings.globalData.clickToSelectHideFlags)
			{
				Event e = Event.current;
				bool hasClicked = rect.Contains(e.mousePosition) && e.type == EventType.MouseDown;

				if (!hasClicked)
				{
					return;
				}

				GameObject[] selection = Selection.gameObjects;

				if (selection.Length < 2)
				{
					Selection.SetActiveObjectWithContext(instance, null);
				}

				GenericMenu menu = new GenericMenu();
				// とりあえずHideFlags.DontSaveInBuildだけ
				var has_dontsaveinbuild = instance.hideFlags.HasFlag(HideFlags.DontSaveInBuild);
				menu.AddItem(new GUIContent((has_dontsaveinbuild ? "*" : "") + "DontSaveInBuild"), false, () =>
				{
					Undo.RecordObjects(Selection.gameObjects, "HideFlags Updated");
					foreach (var go in Selection.gameObjects)
					{
						if (!has_dontsaveinbuild)
							go.hideFlags |= HideFlags.DontSaveInBuild;
						else
							go.hideFlags &= ~HideFlags.DontSaveInBuild;
					}
					if (Selection.gameObjects.Length == 1)
						Selection.SetActiveObjectWithContext(null, null);
				});
				menu.ShowAsContext();
				e.Use();
			}
		}

		protected override int GetGridCount()
		{
			return 1;
		}

		protected override bool DrawerIsEnabled(Settings settings, GameObject instance)
		{
			if (settings.styleData.HasStyle(instance.name) && !settings.styleData.displayHideFlags)
			{
				return false;
			}
			return settings.globalData.showHideFlags;
		}
	}
}