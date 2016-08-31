using UnityEngine;
using System.Collections;

namespace NodeTreeView
{
	public static class NodeTreeView
	{
		private static BaseNode _currentSelectedNode = null;
		private static void InitNodeTree()
		{
			_currentSelectedNode = null;
		}

		public static void DrawNodeTreeView(BaseNode root)
		{
			DrawNodeTreeView (root, 0);
			BaseNode checkResult = null;

			if(CheckSelect (root, out checkResult))
			{
				_currentSelectedNode = checkResult;
			}
		}

		private static void DrawNodeTreeView(BaseNode root, int layerCount)
		{
			DrawNode (root, layerCount);
			if (root.hasExpanded)
			{
				foreach (BaseNode curChild in root.childNodes)
				{
					DrawNodeTreeView (curChild, layerCount + 1);
				}
			}
		}

		private static void DrawNode(BaseNode curNode, int layerCount)
		{
			curNode.DrawNode (layerCount, _currentSelectedNode);
		}

		private static bool CheckSelect(BaseNode curNode, out BaseNode selectedNode)
		{
			return curNode.CheckSelect (out selectedNode);
		}
	}
}
