using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Text usingStreamingAsssetPath;
	public Text cacheCompress;
	public InputField assetBundlePath;

	private string _assetBundlePath = "";
	private bool _usingStreamAssetPath = false;
	private bool _cacheCompress = false;


	private static UIManager _instance = null;
	public static UIManager Instance
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

	// Use this for initialization
	void Start () {
		Caching.compressionEnabled = _cacheCompress;
		usingStreamingAsssetPath.text = _usingStreamAssetPath.ToString ();
		cacheCompress.text = _cacheCompress.ToString ();
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
		AssetBundleManager.Instance.LoadFromFile (path);
	}

	public void OnLoadFromCacheOrDownload()
	{
		string path = "";	
		if (_usingStreamAssetPath) {
			path = Path.Combine (Application.streamingAssetsPath, "cube");
		} else {
			path = Path.Combine (_assetBundlePath, "cube");
		}
		AssetBundleManager.Instance.LoadFromCacheOrDownload (path);
	}
		
	public void OnCleanCache()
	{
		Caching.CleanCache ();
	}



	public void OnUnloadAssetBundle()
	{
		AssetBundleManager.Instance.UnloadAssetBundle ();
	}
}
