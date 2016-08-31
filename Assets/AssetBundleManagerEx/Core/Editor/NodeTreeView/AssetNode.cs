using UnityEngine;
using UnityEditor;
using System.Collections;

namespace NodeTreeView
{
	public class AssetNode : BaseNode 
	{
		private string _assetPath = "";
		private Object _assetObject = null;
		public AssetNode(string assetPath) : base()
		{
			_assetObject = AssetDatabase.LoadMainAssetAtPath(assetPath);
		}

		protected override void OnSelected ()
		{
			base.OnSelected ();
			Selection.activeObject = _assetObject;
		}
	}
}