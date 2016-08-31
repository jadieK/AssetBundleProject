using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace NodeTreeView
{
	public class BaseNode
	{
		public bool hasExpanded = false;
		public string nodeText = "DefaultNode";
		public List<BaseNode> childNodes = new List<BaseNode> ();

		private Texture2D _openIcon = null;
		private Texture2D _closedIcon = null;
		private GUIStyle _iconStyle = null;
		private GUIStyle _labelStyle = null;
		private GUIStyle _selectedStyle = null;
		private Rect _selectionRect;

		public BaseNode()
		{
			float height = 16;
			_openIcon = EditorStyles.foldout.onNormal.background;
			_closedIcon = EditorStyles.foldout.normal.background;
			_iconStyle = new GUIStyle();
			_iconStyle.fixedHeight = height;
			_iconStyle.alignment = TextAnchor.MiddleLeft;
			_labelStyle = new GUIStyle(EditorStyles.label);
			_labelStyle.fixedHeight = height;
			_labelStyle.alignment = TextAnchor.MiddleLeft;
			_selectedStyle = new GUIStyle();
			_selectedStyle.normal.background = EditorStyles.colorField.active.background;
		}

		public void DrawNode(int layer, bool isSelected)
		{
			if (isSelected)
			{
				GUILayout.BeginHorizontal (_selectedStyle);
			}
			else
			{
				GUILayout.BeginHorizontal();
			}
			if (childNodes.Count != 0 && GUILayout.Button (hasExpanded ? _openIcon : _closedIcon, _iconStyle, GUILayout.ExpandWidth (false)))
			{
				hasExpanded = !hasExpanded;
			}
			GUIContent content = new GUIContent ();
			content.text = nodeText;
			_selectionRect = GUILayoutUtility.GetRect(content, _labelStyle);
			GUI.Box(_selectionRect, nodeText, _labelStyle);
			GUILayout.EndHorizontal ();
		}

		public virtual void OnSelected()
		{
			
		}
	}
}
