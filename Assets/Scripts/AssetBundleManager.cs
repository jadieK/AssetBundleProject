using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AssetBundleManager : MonoBehaviour {

	AssetBundle ab;

	public Text usingStreamingAsssetPath;
	public Text cacheCompress;
	public InputField assetBundlePath;

	private string _assetBundlePath = "";
	private bool _usingStreamAssetPath = false;
	private bool _cacheCompress = false;
	// Use this for initialization
	void Start () {
		Caching.compressionEnabled = _cacheCompress;
		usingStreamingAsssetPath.text = _usingStreamAssetPath.ToString ();
		cacheCompress.text = _cacheCompress.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnInputEnded()
	{
		_assetBundlePath = assetBundlePath.text;
	}

	public void OnSwitchUsingStreamingAssetsPath()
	{
		_usingStreamAssetPath = !_usingStreamAssetPath;
		usingStreamingAsssetPath.text = _usingStreamAssetPath.ToString ();
		assetBundlePath.gameObject.SetActive(!_usingStreamAssetPath);
	}

	public void OnSwitchCacheCompress()
	{
		_cacheCompress = !_cacheCompress;
		Caching.compressionEnabled = _cacheCompress;
		cacheCompress.text = _cacheCompress.ToString ();

	}

	public void OnLoadFromFile()
	{
		string path = "";	
		if (_usingStreamAssetPath) {
			path = Path.Combine (Application.streamingAssetsPath, "cube");
		} else {
			path = Path.Combine (_assetBundlePath, "cube");
		}
		Debug.Log ("Get Asset Bundle From : " + path);
		ab = AssetBundle.LoadFromFile (path);
		if (ab == null) {
			Debug.LogError ("Cannot Load Asset Budnle From : " + path);
		} else {
			Debug.Log ("Success Load Asset Bundle From : " + path);
		}
	}

	public void OnLoadFromCacheOrDownload()
	{
		string path = "";	
		if (_usingStreamAssetPath) {
			path = Path.Combine (Application.streamingAssetsPath, "cube");
		} else {
			path = Path.Combine (_assetBundlePath, "cube");
		}
		Debug.Log ("Get Asset Bundle From : " + path);
		StartCoroutine (DoLoadFromCacheOrDownload (path));

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
		}
		www.Dispose ();
		www = null;
	}

	public void OnCleanCache()
	{
		Caching.CleanCache ();
	}



	public void OnUnloadAssetBundle()
	{
		ab.Unload (true);
		ab = null;
	}
}
