using UnityEngine;
using UnityEditor;
using System.Collections;

public class AssetBundleBuilder
{
	[MenuItem ("Tools/BuildAssetBundle/Uncompressed")]
	static void BuildAssetBundleUncompressed()
	{
		System.IO.Directory.CreateDirectory ("Assets/StreamingAssets");
		BuildPipeline.BuildAssetBundles ("Assets/StreamingAssets", BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.iOS);
	}

	[MenuItem ("Tools/BuildAssetBundle/LZ4")]
	static void BuildAssetBundleLZ4()
	{
		System.IO.Directory.CreateDirectory ("Assets/StreamingAssets");
		BuildPipeline.BuildAssetBundles ("Assets/StreamingAssets", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.iOS);
	}

	[MenuItem ("Tools/BuildAssetBundle/LZMA")]
	static void BuildAssetBundleLZMA()
	{
		System.IO.Directory.CreateDirectory ("Assets/StreamingAssets");
		BuildPipeline.BuildAssetBundles ("Assets/StreamingAssets", BuildAssetBundleOptions.None, BuildTarget.iOS);
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
