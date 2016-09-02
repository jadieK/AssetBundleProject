using UnityEngine;
using UnityEditor;
using System.Collections;

namespace NodeTreeView
{
	public class AssetNode : BaseNode 
	{
		private string _assetPath = "";
		private Object _assetObject = null;
		public string AssetPath
		{
			get
			{ 
				return _assetPath;
			}
		}

		public string AssetGUID
		{
			get
			{
				return AssetDatabase.AssetPathToGUID (_assetPath);
			}
		}

		public AssetNode(string assetPath) : base()
		{
			_assetPath = assetPath;
			_assetObject = AssetDatabase.LoadMainAssetAtPath(assetPath);
		}

		protected override void OnLeftSelected ()
		{
			base.OnLeftSelected ();
			EditorGUIUtility.PingObject (_assetObject);
//			Selection.activeObject = _assetObject;
		}
	}
}