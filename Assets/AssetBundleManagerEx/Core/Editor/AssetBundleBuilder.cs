using UnityEngine;
using UnityEditor;
using System.IO;
using LitJson;
using System.Collections;
using System.Collections.Generic;

public class AssetBundleBuilder
{
	[MenuItem ("Tools/QuickBuild/Uncompressed")]
	static void BuildAssetBundleUncompressed()
	{
		System.IO.Directory.CreateDirectory ("Assets/StreamingAssets");
		BuildPipeline.BuildAssetBundles ("Assets/StreamingAssets", BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.iOS);
	}

	[MenuItem ("Tools/QuickBuild/LZ4")]
	static void BuildAssetBundleLZ4()
	{
		System.IO.Directory.CreateDirectory ("Assets/StreamingAssets");
		BuildPipeline.BuildAssetBundles ("Assets/StreamingAssets", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.iOS);
	}

	[MenuItem ("Tools/QuickBuild/LZMA")]
	static void BuildAssetBundleLZMA()
	{
		System.IO.Directory.CreateDirectory ("Assets/StreamingAssets");
		BuildPipeline.BuildAssetBundles ("Assets/StreamingAssets", BuildAssetBundleOptions.None, BuildTarget.iOS);
	}

	[MenuItem ("Tools/CustomConfig/ExportConfig")]
	static void ExportConfig()
	{
		string outputPath = EditorUtility.SaveFilePanel ("Export AssetBundle Config", AssetBundleBuilderConfig.ASSETBUNDLE_CONFIG_PATH, AssetBundleBuilderConfig.ASSETBUNDLE_CONFIG_FILE_NAME, "abc");

		if (string.IsNullOrEmpty (outputPath))
		{
			return;
		}

		List<AssetBundleConfig> exportData = new List<AssetBundleConfig> ();
//		foreach (string abName in AssetDatabase.GetAllAssetBundleNames ()) 
		string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames ();
		for(int i = 0; i < assetBundleNames.Length;i++)
		{
			string abName = assetBundleNames [i];
			AssetBundleConfig curABC = new AssetBundleConfig ();
			exportData.Add (curABC);

			curABC.assetBundleName = abName;
			string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle (abName);
			for (int assetPathIndex = 0; assetPathIndex < assetPaths.Length; assetPathIndex++)
			{
				string curAssetPath = assetPaths [assetPathIndex];
				AssetConfig ac = new AssetConfig ();
				ac.assetGUID = AssetDatabase.AssetPathToGUID (curAssetPath);
				ac.assetPath = curAssetPath;
				curABC.assetConfigList.Add (ac);
			}
//			curABC.assetPathsList.AddRange(AssetDatabase.GetAssetPathsFromAssetBundle (abName));
		}

		FileStream output = File.Open (outputPath, FileMode.OpenOrCreate);
		byte[] outputData = System.Text.Encoding.Default.GetBytes (LitJson.JsonMapper.ToJson (exportData));
		output.Write (outputData,0,outputData.Length);
		output.Flush ();
		output.Close ();
	}

	[MenuItem ("Tools/CustomConfig/ImportConfig")]
	static void ImportConfig()
	{
		string inputPath = EditorUtility.OpenFilePanel ("Export AssetBundle Config", AssetBundleBuilderConfig.ASSETBUNDLE_CONFIG_PATH, "abc");

		if (string.IsNullOrEmpty (inputPath))
		{
			return;
		}

		List<AssetBundleConfig> importData = GetAssetBundleConfigList (inputPath);
		for (int i = 0;i < importData.Count;i++)
		{
			AssetBundleConfig curABC = importData [i];
//			foreach (string curAssetPath in curABC.assetPathsList)
			for(int assetConfigIndex = 0; assetConfigIndex < curABC.assetConfigList.Count;assetConfigIndex++)
			{
				string curAssetPath = curABC.assetConfigList [assetConfigIndex].assetPath;
				AssetImporter.GetAtPath (curAssetPath).assetBundleName = curABC.assetBundleName;
			}
		}
		AssetDatabase.Refresh ();
	}

	[MenuItem ("Tools/CustomConfig/ConfigWindow")]
	static void ShowConfigWindow()
	{
		AssetBundleBuilderWindow.ShowDefaultBuilderWindow ();
	}

	[MenuItem ("Tools/CleanAssetBundle")]
	static void CleanAssetBundle()
	{
		FileUtil.DeleteFileOrDirectory ("Assets/StreamingAssets");
	}

//	[MenuItem ("Tools/UploadToServer")]
//	static void UploadToServer()
//	{
//		System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo ("Assets/StreamingAssets");
//		foreach (System.IO.FileInfo curFileInfo in  dirInfo.GetFiles())
//		{
//			string sourcePath = curFileInfo.FullName;
//			string targetPath = "/Users/chuangao/httpserver/" + curFileInfo.Name;
//			Debug.Log ("Upload File From " + sourcePath + " To " + targetPath);
//			System.IO.File.Copy (sourcePath, targetPath, true);
//		}
//
////		FileUtil.MoveFileOrDirectory("Asset/StreamingAssets/", "/Users/chuangao/httpserver/");
//	}

	private static List<AssetBundleConfig> GetAssetBundleConfigList(string configFilePath)
	{
		FileStream inputFile = File.Open (configFilePath, FileMode.Open);
		byte[] inputData = new byte[inputFile.Length];
		inputFile.Read (inputData, 0, inputData.Length);
		inputFile.Close ();
		return LitJson.JsonMapper.ToObject<List<AssetBundleConfig>>(System.Text.Encoding.Default.GetString (inputData));
	}

	public static NodeTreeView.BaseNode GenerateTreeViewNodes(string configFilePath)
	{
		List<AssetBundleConfig> assetBundleConfigList = GetAssetBundleConfigList (configFilePath);
		NodeTreeView.BaseNode rootNode = new NodeTreeView.BaseNode ();
		rootNode.nodeText = "root";
//		foreach (AssetBundleConfig curABC in assetBundleConfigList)
		for(int i = 0; i < assetBundleConfigList.Count;i++)
		{
			AssetBundleConfig curABC = assetBundleConfigList [i];
			NodeTreeView.AssetBundleNode abNode = new NodeTreeView.AssetBundleNode ();
			rootNode.childNodes.Add (abNode);
			abNode.nodeText = curABC.assetBundleName;
			for (int assetIndex = 0; assetIndex < curABC.assetConfigList.Count; assetIndex++)
			{
				AssetConfig curAssetConfig = curABC.assetConfigList [assetIndex];
				if (System.IO.File.Exists (curAssetConfig.assetPath))
				{
					NodeTreeView.AssetNode aNode = new NodeTreeView.AssetNode ();
					aNode.assetPath = curAssetConfig.assetPath;
					aNode.nodeText = Path.GetFileName(curAssetConfig.assetPath);
					string[] dependencies = AssetDatabase.GetDependencies (curAssetConfig.assetPath);
					for (int depIndex = 0; depIndex < dependencies.Length; depIndex++)
					{
						NodeTreeView.AssetNode depNode = new NodeTreeView.AssetNode ();
						depNode.assetPath = dependencies [depIndex];
						depNode.nodeText = Path.GetFileName (dependencies [depIndex]);
						aNode.childNodes.Add (depNode);
					}
					abNode.childNodes.Add (aNode);
				}
			}
		}
		return rootNode;
	}
}
