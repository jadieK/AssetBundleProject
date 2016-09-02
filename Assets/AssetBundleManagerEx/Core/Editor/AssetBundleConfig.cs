using UnityEngine;
using System.Collections.Generic;

public class AssetConfig
{
	public string assetGUID = "";
	public string assetPath = "";
}

public class AssetBundleConfig
{
	public string assetBundleName = "";
//	public string assetBundleCompressType = "LZMA";
	public List<AssetConfig> assetConfigList = new List<AssetConfig> ();
}

public class AssetBundleConfigList
{
	public AssetBundleBuilderUtil.CompressType compressType = AssetBundleBuilderUtil.CompressType.LZMA;
	public List<AssetBundleConfig> assetBundleConfig = new List<AssetBundleConfig>();
}