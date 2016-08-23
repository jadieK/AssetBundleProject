using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AssetBundleManager : MonoBehaviour {

//	private Dictionary<string, AssetBundleEx> _assetBundlePool = new List<AssetBundle> ();
	AssetBundle ab;
	private static AssetBundleManager _instance = null;
	public static AssetBundleManager Instance
	{
		get
		{ 
			return _instance;
		}
	}
	void Awake()
	{
		if (_instance == null) {
			_instance = this;
		} else {
			Object.DestroyImmediate (_instance);
			_instance = this;
		}
	}

	public void LoadFromFile(string path)
	{
		Debug.Log ("Get Asset Bundle From : " + path);
		ab = AssetBundle.LoadFromFile (path);
		if (ab == null) {
			Debug.LogError ("Cannot Load Asset Budnle From : " + path);
		} else {
			Debug.Log ("Success Load Asset Bundle From : " + path);
//			_assetBundlePool.Add (ab);
		}
	}

	public void LoadFromCacheOrDownload(string path)
	{
		Debug.Log ("Get Asset Bundle From : " + path);
		StartCoroutine (DoLoadFromCacheOrDownload (path));
	}

	public void UnloadAssetBundle()
	{
		ab.Unload (true);
		ab = null;
	}

	IEnumerator DoLoadFromCacheOrDownload(string path)
	{
		Debug.Log ("Loading Asset Bundle....");
		WWW www = WWW.LoadFromCacheOrDownload (path, 0);
		while (!www.isDone) {
			yield return null;
		}
		ab = www.assetBundle;
		if (ab == null) {
			Debug.LogError ("Cannot Load Asset Budnle From : " + path);
		} else {
			Debug.Log ("Success Load Asset Bundle From : " + path);
//			_assetBundlePool.Add (ab);
		}
		www.Dispose ();
		www = null;
	}


}