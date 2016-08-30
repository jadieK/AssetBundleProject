using UnityEngine;
using System.Collections.Generic;

public struct AssetBundleEx {
	public AssetBundle _ab;
	public string _name;
	public List<string> _urls;

	public void Init()
	{
		_ab = null;
		_name = "";
		_urls = new List<string> ();
	}


	public void Unload(bool removeResource)
	{
		_ab.Unload (removeResource);
		_ab = null;
		_urls.Clear ();
		_name = "";
	}
}
