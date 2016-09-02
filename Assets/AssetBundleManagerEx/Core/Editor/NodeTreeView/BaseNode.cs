using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace NodeTreeView
{
	public class BaseNode
	{
		private const int layerIndent = 15;
		private const int arrayIndent = 10;

		public bool hasExpanded = false;
		public string nodeText = "DefaultNode";
		public List<BaseNode> childNodes = new List<BaseNode> ();

		private static Texture2D _openIcon = null;
		private static Texture2D _closedIcon = null;
		private static GUIStyle _iconStyle = null;
		private static GUIStyle _labelStyle = null;
		private static GUIStyle _selectedStyle = null;
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
//			_selectedStyle = UnityEditor.EditorStyles.textArea;//new GUIStyle ();//
			_selectedStyle = new GUIStyle();
			_selectedStyle.normal.background = CreateTexture (600, 1, Color.grey);
		}

		public void DrawNode(int layer, BaseNode currentSelected)
		{
			
			if (currentSelected == this)
			{
				GUILayout.BeginHorizontal (_selectedStyle);
			}
			else
			{
				GUILayout.BeginHorizontal();
			}
			GUILayout.Space (layer * layerIndent);
			if (childNodes.Count != 0 && GUILayout.Button (hasExpanded ? _openIcon : _closedIcon, _iconStyle, GUILayout.ExpandWidth (false)))
			{
				hasExpanded = !hasExpanded;
			}
			GUIContent content = new GUIContent ();
			content.text = nodeText;
			_selectionRect = GUILayoutUtility.GetRect(content, _labelStyle);
			GUI.Box(_selectionRect, nodeText, _labelStyle);
			DrawExtra (layer, currentSelected);
			GUILayout.EndHorizontal ();
			DoAction ();
		}

		protected virtual void DrawExtra(int layer, BaseNode currentSelected)
		{
		}

		protected virtual void DoAction()
		{
		}

		public bool CheckSelect(out BaseNode selectedNode)
		{
			selectedNode = null;
			bool isLeftClick = Event.current.type == EventType.mouseDown && _selectionRect.Contains(Event.current.mousePosition);
			bool isRightClick = Event.current.type == EventType.ContextClick && _selectionRect.Contains(Event.current.mousePosition);
			if (isLeftClick)
			{
				OnLeftSelected ();
				selectedNode = this;
				return true;
			}
			else if (isRightClick)
			{
				OnRightSelected ();
			}
			else
			{
				for (int childIndex = 0; childIndex < childNodes.Count; childIndex++)
				{
					if (childNodes [childIndex].CheckSelect (out selectedNode))
					{
						return true;
					}
				}
			}
			return false;
		}

		protected virtual void OnLeftSelected()
		{
			
		}

		protected virtual void OnRightSelected()
		{
		}

		public static Texture2D CreateTexture(int width, int height, Color color)
		{
			Color[] pixels = new Color[width*height];

			for(int i = 0; i < pixels.Length; i++)
			{
				pixels[i] = color;
			}

			Texture2D result = new Texture2D(width, height);
			result.SetPixels(pixels);
			result.Apply();

			return result;
		}
	}
}
