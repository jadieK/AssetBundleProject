using UnityEngine;
using UnityEditor;
using System.IO;
using LitJson;
using System.Collections;
using System.Collections.Generic;

public class AssetBundleBuilderMenu
{
	[MenuItem ("Tools/QuickBuild/Uncompressed")]
	static void BuildAssetBundleUncompressed()
	{
		System.IO.Directory.CreateDirectory (AssetBundleBuilderConfig.ASSETBUNDLE_CONFIG_OUTPUT_PATH);
		BuildPipeline.BuildAssetBundles (AssetBundleBuilderConfig.ASSETBUNDLE_CONFIG_OUTPUT_PATH, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.iOS);
	}

	[MenuItem ("Tools/QuickBuild/LZ4")]
	static void BuildAssetBundleLZ4()
	{
		System.IO.Directory.CreateDirectory (AssetBundleBuilderConfig.ASSETBUNDLE_CONFIG_OUTPUT_PATH);
		BuildPipeline.BuildAssetBundles (AssetBundleBuilderConfig.ASSETBUNDLE_CONFIG_OUTPUT_PATH, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.iOS);
	}

	[MenuItem ("Tools/QuickBuild/LZMA")]
	static void BuildAssetBundleLZMA()
	{
		System.IO.Directory.CreateDirectory (AssetBundleBuilderConfig.ASSETBUNDLE_CONFIG_OUTPUT_PATH);
		BuildPipeline.BuildAssetBundles (AssetBundleBuilderConfig.ASSETBUNDLE_CONFIG_OUTPUT_PATH, BuildAssetBundleOptions.None, BuildTarget.iOS);
	}

	[MenuItem ("Tools/CustomConfig/ExportConfig")]
	static void ExportConfig()
	{
		string outputPath = EditorUtility.SaveFilePanel ("Export AssetBundle Config", AssetBundleBuilderConfig.ASSETBUNDLE_CONFIG_PATH, AssetBundleBuilderConfig.ASSETBUNDLE_CONFIG_FILE_NAME, "abc");

		if (string.IsNullOrEmpty (outputPath))
		{
			return;
		}

		AssetBundleConfigList exportData = new AssetBundleConfigList();
//		foreach (string abName in AssetDatabase.GetAllAssetBundleNames ()) 
		string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames ();
		for(int i = 0; i < assetBundleNames.Length;i++)
		{
			string abName = assetBundleNames [i];
			AssetBundleConfig curABC = new AssetBundleConfig ();
			exportData.assetBundleConfig.Add (curABC);

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

		FileStream output = File.Open (outputPath, FileMode.Truncate);
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

		AssetBundleConfigList importData = AssetBundleBuilderUtil.GetAssetBundleConfigList (inputPath);
		for (int i = 0;i < importData.assetBundleConfig.Count;i++)
		{
			AssetBundleConfig curABC = importData.assetBundleConfig [i];
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
		FileUtil.DeleteFileOrDirectory (AssetBundleBuilderConfig.ASSETBUNDLE_CONFIG_OUTPUT_PATH);
	}
}
