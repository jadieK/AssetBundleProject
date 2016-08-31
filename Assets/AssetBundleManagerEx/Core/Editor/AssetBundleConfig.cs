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
	public List<AssetConfig> assetConfigList = new List<AssetConfig> ();
}
