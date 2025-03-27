using UnityEditor;
using UnityEngine;

namespace HierarchyDecorator
{
	public class HideFlagsInfo : HierarchyInfo
	{
		const int GRID_SIZE = 1;

		// --- Settings
		bool _isEnabled;

		// --- Methods

		protected override void OnDrawInit(HierarchyItem item, Settings settings)
		{
		}

		protected override bool DrawerIsEnabled(HierarchyItem item, Settings settings)
		{
			_isEnabled = settings.globalData.showHideFlags;
			if (settings.styleData.HasStyle(item.DisplayName))
			{
				_isEnabled &= settings.styleData.displayHideFlags;
			}
			return _isEnabled;
		}

		protected override int CalculateGridCount()
		{
			return GRID_SIZE;
		}

		protected override bool ValidateGrid()
		{
			if (GridCount < 1) // Not big enough for either element
			{
				return false;
			}
			return true;
		}

		protected override void DrawInfo(Rect rect, HierarchyItem item, Settings settings)
		{
			var instance = item.GameObject;
			if (instance.hideFlags == HideFlags.None)
				EditorGUI.LabelField(rect, "-", Style.CenteredSmallLabel);
			else
				EditorGUI.LabelField(rect, "!", Style.CenteredLabelRed);

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
}
