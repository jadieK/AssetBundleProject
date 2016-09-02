using UnityEngine;
using System.Collections;

public static class AssetBundleBuilderConfig
{
	public const string ASSETBUNDLE_CONFIG_OUTPUT_PATH = "Assets/StreamingAssets/";
	public const string ASSETBUNDLE_CONFIG_PATH = "Assets/Editor/";
	public const string ASSETBUNDLE_CONFIG_FILE_NAME = "AssetBundleConfig.abc";

	public static UnityEditor.BuildTarget AssetBundlePlatform = UnityEditor.BuildTarget.iOS;
}
