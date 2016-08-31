using UnityEngine;
using UnityEditor;
using NodeTreeView;
using System.Collections;

public class AssetBundleBuilderWindow : EditorWindow 
{
	private static BaseNode _root = null;
	public static void ShowDefaultBuilderWindow()
	{
		_root = new BaseNode ();
		EditorWindow.GetWindow (typeof(AssetBundleBuilderWindow), false, "Asset Bundle Config");
	}

	void OnGUI()
	{
		DrawOptions ();

		if (_root != null)
		{
			NodeTreeView.NodeTreeView.DrawNodeTreeView (_root);
		}

	}

	private void DrawOptions()
	{
		
		GUILayout.BeginVertical ();
		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("OpenConfig"))
		{
			LoadCustomConfig ();
		}
		if (GUILayout.Button ("SaveConfig"))
		{
		}
		if (GUILayout.Button ("ApplyToSystem"))
		{
		}
		if (GUILayout.Button ("BuildAssetBundle"))
		{
		}
		GUILayout.EndHorizontal ();
		GUILayout.EndVertical ();

	}

	private void LoadCustomConfig()
	{
		string path = EditorUtility.OpenFilePanel ("Select Asset Bundle Config", AssetBundleBuilderConfig.ASSETBUNDLE_CONFIG_PATH, "abc");
		if (!string.IsNullOrEmpty (path))
		{
			_root = AssetBundleBuilder.GenerateTreeViewNodes (path);
		}
	}
}
