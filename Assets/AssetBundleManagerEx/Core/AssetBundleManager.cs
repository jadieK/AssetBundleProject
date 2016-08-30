using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AssetBundleManager : MonoBehaviour {

//	private Dictionary<string, AssetBundleEx> _assetBundlePool = new List<AssetBundle> ();
//	AssetBundle ab;
	public delegate void onAssetBundleLoaded(AssetBundleEx ab);

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

	public AssetBundleEx LoadFromFile(string path)
	{
		Debug.Log ("Get Asset Bundle From : " + path);
		AssetBundle local_ab = AssetBundle.LoadFromFile (path);
		if (local_ab == null) {
			Debug.LogError ("Cannot Load Asset Budnle From : " + path);
		} else {
			Debug.Log ("Success Load Asset Bundle From : " + path);
//			_assetBundlePool.Add (ab);
		}
		AssetBundleEx result = new AssetBundleEx ();
		result._ab = local_ab;
		result._urls.Add (path);
		return result;
	}

	public void LoadFromCacheOrDownload(string path, onAssetBundleLoaded func)
	{
		Debug.Log ("Get Asset Bundle From : " + path);
		StartCoroutine (DoLoadFromCacheOrDownload (path, func));
	}

	IEnumerator DoLoadFromCacheOrDownload(string path, onAssetBundleLoaded func)
	{
		Debug.Log ("Loading Asset Bundle....");
		WWW www = WWW.LoadFromCacheOrDownload (path, 0);
		while (!www.isDone) {
			yield return null;
		}
		AssetBundle local_ab = www.assetBundle;
		if (local_ab == null) {
			Debug.LogError ("Cannot Load Asset Budnle From : " + path);
		} else {
			Debug.Log ("Success Load Asset Bundle From : " + path);
//			_assetBundlePool.Add (ab);
		}
		www.Dispose ();
		www = null;
		AssetBundleEx result = new AssetBundleEx ();
		result._ab = local_ab;
		result._urls.Add (path);
		func (result);
	}

}