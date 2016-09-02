using UnityEngine;
using UnityEditor;
using NodeTreeView;
using System.Collections;
using System.Collections.Generic;

public class AssetBundleBuilderWindow : EditorWindow 
{
	private delegate void NextAction();

	private NextAction _nextAction = null;
	private static RootNode _root;

	public static void ShowDefaultBuilderWindow()
	{
		if (_root == null)
		{
			_root = new RootNode ();
		}
		EditorWindow.GetWindow (typeof(AssetBundleBuilderWindow), false, "Asset Bundle Config");
	}

	void OnGUI()
	{
		DrawOptions ();

		if (_root != null)
		{
			NodeTreeView.NodeTreeView.DrawNodeTreeView (_root);
		}
		else if(EditorPrefs.HasKey("jadiek_asset_config"))
		{
			Debug.Log ("Asset bundle config rebuild!");
			_root = AssetBundleBuilderUtil.GenerateTreeViewNodes (EditorPrefs.GetString("jadiek_asset_config"));
			NodeTreeView.NodeTreeView.DrawNodeTreeView (_root);
		}
	}

	private void DrawOptions()
	{
		_nextAction = null;
		GUILayout.BeginVertical ();
		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("OpenConfig"))
		{
			_nextAction = LoadCustomConfig;
		}
		if (GUILayout.Button ("SaveConfig"))
		{
			_nextAction = SaveCustomConfig;
		}
		if (GUILayout.Button ("ApplyToSystem"))
		{
		}
		if (GUILayout.Button ("BuildAssetBundle"))
		{
			_nextAction = BuildAssetBundle;
		}
		GUILayout.EndHorizontal ();
		GUILayout.EndVertical ();
		if (_nextAction != null)
		{
			_nextAction (); 
		}
	}

	private void LoadCustomConfig()
	{
		string path = EditorUtility.OpenFilePanel ("Select Asset Bundle Config", AssetBundleBuilderConfig.ASSETBUNDLE_CONFIG_PATH, "abc");
		if (!string.IsNullOrEmpty (path))
		{
			EditorPrefs.SetString ("jadiek_asset_config", path);
			_root = AssetBundleBuilderUtil.GenerateTreeViewNodes (path);
		}
	}

	private void SaveCustomConfig()
	{
		string path = EditorUtility.SaveFilePanel ("Select Asset Bundle Config", AssetBundleBuilderConfig.ASSETBUNDLE_CONFIG_PATH, AssetBundleBuilderConfig.ASSETBUNDLE_CONFIG_FILE_NAME,"abc");
		if (!string.IsNullOrEmpty (path))
		{
			EditorPrefs.SetString ("jadiek_asset_config", path);
			string jsonData = LitJson.JsonMapper.ToJson(AssetBundleBuilderUtil.GenerateAssetBundleConfigList (_root));
			System.IO.FileStream outputFile = System.IO.File.Open (path, System.IO.FileMode.Truncate);
			byte[] outputData = System.Text.Encoding.Default.GetBytes (jsonData);
			outputFile.Write (outputData, 0, outputData.Length);
			outputFile.Flush ();
			outputFile.Close ();
		}
	}

	private void BuildAssetBundle()
	{
		AssetBundleConfigList dataList = AssetBundleBuilderUtil.GenerateAssetBundleConfigList (_root);
		AssetBundleBuild[] targetBuild = new AssetBundleBuild[dataList.assetBundleConfig.Count];
		for (int dataIndex = 0; dataIndex < dataList.assetBundleConfig.Count; dataIndex++)
		{
			
			AssetBundleConfig currentAssetBundleConfig = dataList.assetBundleConfig [dataIndex];
			Debug.Log ("Building Asset Bundle " + currentAssetBundleConfig.assetBundleName);
			targetBuild [dataIndex].assetBundleName = currentAssetBundleConfig.assetBundleName;
			targetBuild [dataIndex].assetNames = new string[currentAssetBundleConfig.assetConfigList.Count];
			for (int assetIndex = 0; assetIndex < currentAssetBundleConfig.assetConfigList.Count; assetIndex++)
			{
				targetBuild [dataIndex].assetNames [assetIndex] = currentAssetBundleConfig.assetConfigList [assetIndex].assetPath;
			}
		}
		BuildPipeline.BuildAssetBundles (AssetBundleBuilderConfig.ASSETBUNDLE_CONFIG_OUTPUT_PATH, targetBuild, AssetBundleBuilderUtil.GetAssetBundleBuildOption (dataList.compressType), AssetBundleBuilderConfig.AssetBundlePlatform);
	}
}
