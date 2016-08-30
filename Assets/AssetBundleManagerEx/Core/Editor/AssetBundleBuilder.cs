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
		foreach (string abName in AssetDatabase.GetAllAssetBundleNames ()) 
		{
			AssetBundleConfig curABC = new AssetBundleConfig ();
			exportData.Add (curABC);

			curABC.assetBundleName = abName;
			curABC.assetPathsList.AddRange(AssetDatabase.GetAssetPathsFromAssetBundle (abName));
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

		FileStream inputFile = File.Open (inputPath, FileMode.Open);
		byte[] inputData = new byte[inputFile.Length];
		inputFile.Read (inputData, 0, inputData.Length);
		inputFile.Close ();
		List<AssetBundleConfig> importData = LitJson.JsonMapper.ToObject<List<AssetBundleConfig>>(System.Text.Encoding.Default.GetString (inputData));
		foreach (AssetBundleConfig curABC in importData)
		{
			foreach (string curAssetPath in curABC.assetPathsList)
			{
				AssetImporter.GetAtPath (curAssetPath).assetBundleName = curABC.assetBundleName;
			}
		}
		AssetDatabase.Refresh ();
	}

	[MenuItem ("Tools/CustomConfig/ConfigWindow")]
	static void ShowConfigWindow()
	{
		NodeTreeView.BaseNode bn = new NodeTreeView.BaseNode ();
		bn.nodeText = "AA";
		NodeTreeView.BaseNode ch = new NodeTreeView.BaseNode ();
		ch.nodeText = "BB";
		bn.childNodes.Add (ch);
		NodeTreeView.NodeTreeViewWindow.ShowTreeViewWindow ("Test", bn);
	}

	[MenuItem ("Tools/CleanAssetBundle")]
	static void CleanAssetBundle()
	{
		FileUtil.DeleteFileOrDirectory ("Assets/StreamingAssets");
	}

	[MenuItem ("Tools/UploadToServer")]
	static void UploadToServer()
	{
		System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo ("Assets/StreamingAssets");
		foreach (System.IO.FileInfo curFileInfo in  dirInfo.GetFiles())
		{
			string sourcePath = curFileInfo.FullName;
			string targetPath = "/Users/chuangao/httpserver/" + curFileInfo.Name;
			Debug.Log ("Upload File From " + sourcePath + " To " + targetPath);
			System.IO.File.Copy (sourcePath, targetPath, true);
		}

//		FileUtil.MoveFileOrDirectory("Asset/StreamingAssets/", "/Users/chuangao/httpserver/");
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
