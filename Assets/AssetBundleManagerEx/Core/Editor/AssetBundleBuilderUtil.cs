using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public static class AssetBundleBuilderUtil 
{
	public enum CompressType
	{
		Uncompress = 0,
		LZMA = 1,
		LZ4 = 2,
		CompressTypeCount = 3
	};

	public static CompressType NextCompressType(CompressType currentType)
	{
		switch(currentType)
		{
			case CompressType.Uncompress:
				return CompressType.LZMA;
			case CompressType.LZMA:
				return CompressType.LZ4;
			case CompressType.LZ4:
				return CompressType.Uncompress;
			default:
				return CompressType.LZMA;	
		}
	}

	public static BuildAssetBundleOptions GetAssetBundleBuildOption(CompressType compressType)
	{
		//			BuildAssetBundleOptions result = BuildAssetBundleOptions.None;
		switch (compressType)
		{
			case AssetBundleBuilderUtil.CompressType.Uncompress:// Uncompressed
				return BuildAssetBundleOptions.UncompressedAssetBundle;
			case AssetBundleBuilderUtil.CompressType.LZMA:// LZMA
				return BuildAssetBundleOptions.None;
			case AssetBundleBuilderUtil.CompressType.LZ4:// LZ4
				return BuildAssetBundleOptions.ChunkBasedCompression;
			default:
				return BuildAssetBundleOptions.None;
		}
	}

	public static AssetBundleConfigList GetAssetBundleConfigList(string configFilePath)
	{
		FileStream inputFile = File.Open (configFilePath, FileMode.Open);
		byte[] inputData = new byte[inputFile.Length];
		inputFile.Read (inputData, 0, inputData.Length);
		inputFile.Close ();
		return LitJson.JsonMapper.ToObject<AssetBundleConfigList>(System.Text.Encoding.Default.GetString (inputData));
	}

	public static NodeTreeView.RootNode GenerateTreeViewNodes(string configFilePath)
	{
		AssetBundleConfigList assetBundleConfigList = GetAssetBundleConfigList (configFilePath);
		NodeTreeView.RootNode rootNode = new NodeTreeView.RootNode ();
		rootNode.nodeText = "root";
		rootNode.CurrentCompressType = assetBundleConfigList.compressType;
		//		foreach (AssetBundleConfig curABC in assetBundleConfigList)
		for(int i = 0; i < assetBundleConfigList.assetBundleConfig.Count;i++)
		{
			AssetBundleConfig curABC = assetBundleConfigList.assetBundleConfig [i];
			NodeTreeView.AssetBundleNode abNode = new NodeTreeView.AssetBundleNode (rootNode);
			rootNode.childNodes.Add (abNode);
			abNode.nodeText = curABC.assetBundleName;
			for (int assetIndex = 0; assetIndex < curABC.assetConfigList.Count; assetIndex++)
			{
				AssetConfig curAssetConfig = curABC.assetConfigList [assetIndex];
				if (System.IO.File.Exists (curAssetConfig.assetPath))
				{
					NodeTreeView.AssetNode aNode = new NodeTreeView.AssetNode (curAssetConfig.assetPath);
					aNode.nodeText = Path.GetFileName(curAssetConfig.assetPath);
					string[] dependencies = AssetDatabase.GetDependencies (curAssetConfig.assetPath);
					NodeTreeView.BaseNode depRootNode = new NodeTreeView.BaseNode ();
					depRootNode.nodeText = "Dependencies";
					aNode.childNodes.Add (depRootNode);
					for (int depIndex = 0; depIndex < dependencies.Length; depIndex++)
					{
						NodeTreeView.DepNode depNode = new NodeTreeView.DepNode (dependencies [depIndex]);
						depNode.nodeText = Path.GetFileName (dependencies [depIndex]);
						depRootNode.childNodes.Add (depNode);
					}
					abNode.childNodes.Add (aNode);
				}
			}
		}
		return rootNode;
	}

	public static AssetBundleConfigList GenerateAssetBundleConfigList(NodeTreeView.RootNode root)
	{
		AssetBundleConfigList result = new AssetBundleConfigList();
		result.compressType = root.CurrentCompressType;
		for (int assetBundleIndex = 0; assetBundleIndex < root.childNodes.Count; assetBundleIndex++)
		{
			NodeTreeView.AssetBundleNode curAssetBundleNode = root.childNodes [assetBundleIndex] as NodeTreeView.AssetBundleNode;
			if (curAssetBundleNode == null)
			{
				Debug.LogError ("Generate Asset Bundle Config Failed! The asset bundle layer is wrong!");
			}
			else
			{
				AssetBundleConfig curABC = new AssetBundleConfig ();
				curABC.assetBundleName = curAssetBundleNode.nodeText;
				for (int assetIndex = 0; assetIndex < curAssetBundleNode.childNodes.Count; assetIndex++)
				{
					NodeTreeView.AssetNode curAssetNode = curAssetBundleNode.childNodes [assetIndex] as NodeTreeView.AssetNode;
					if (curAssetNode == null)
					{
						Debug.LogError ("Generate Asset Bundle Config Failed! The asset layer is wrong!");
					}
					else
					{
						AssetConfig curAC = new AssetConfig ();
						curAC.assetGUID = curAssetNode.AssetGUID;
						curAC.assetPath = curAssetNode.AssetPath;
						curABC.assetConfigList.Add (curAC);
					}
				}
				result.assetBundleConfig.Add (curABC);
			}
		}
		return result;
	}

	public static void BuildAssetBundleByConfig(string assetBundleName, NodeTreeView.RootNode rootNode)
	{
		AssetBundleConfigList dataList = AssetBundleBuilderUtil.GenerateAssetBundleConfigList (rootNode);
		BuildAssetBundleByConfig (assetBundleName, dataList);
	}

	public static void BuildAssetBundleByConfig(string asssetBundleName, AssetBundleConfigList configList)
	{
		AssetBundleConfig currentConfig = configList.assetBundleConfig.Find (delegate(AssetBundleConfig obj)
			{
				return obj.assetBundleName == asssetBundleName;
			});

		if (currentConfig != null)
		{
			AssetBundleBuild[] targetBuild = new AssetBundleBuild[1];
			targetBuild [0].assetBundleName = currentConfig.assetBundleName;
			targetBuild [0].assetNames = new string[currentConfig.assetConfigList.Count];
			for (int assetIndex = 0; assetIndex < currentConfig.assetConfigList.Count; assetIndex++)
			{
				targetBuild [0].assetNames [assetIndex] = currentConfig.assetConfigList [assetIndex].assetPath;
			}
			BuildPipeline.BuildAssetBundles(AssetBundleBuilderConfig.ASSETBUNDLE_CONFIG_OUTPUT_PATH, targetBuild, AssetBundleBuilderUtil.GetAssetBundleBuildOption(configList.compressType) ,AssetBundleBuilderConfig.AssetBundlePlatform);
		}
	}



	public static void BuildAssetBundleByConfig(NodeTreeView.AssetBundleNode assetBundleNode)
	{
		AssetBundleBuild[] targetBuild = new AssetBundleBuild[]{ assetBundleNode.GetAssetBundleBuild () };
		BuildPipeline.BuildAssetBundles (AssetBundleBuilderConfig.ASSETBUNDLE_CONFIG_OUTPUT_PATH, targetBuild, AssetBundleBuilderUtil.GetAssetBundleBuildOption(assetBundleNode.CurrentCompressType) ,AssetBundleBuilderConfig.AssetBundlePlatform);
	}
}
