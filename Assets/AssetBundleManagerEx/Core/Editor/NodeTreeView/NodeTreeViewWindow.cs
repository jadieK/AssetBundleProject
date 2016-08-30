using UnityEngine;
using UnityEditor;
using System.Collections;

namespace NodeTreeView
{
	public class NodeTreeViewWindow : EditorWindow {

		private static BaseNode _root = null;
		public static void ShowTreeViewWindow(string title, BaseNode root)
		{
			_root = root;
			EditorWindow.GetWindow (typeof(NodeTreeViewWindow), false, title);
		}

		void OnGUI()
		{
			if (_root != null)
			{
				NodeTreeView.DrawNodeTreeView (_root);
			}
		}

	}
}