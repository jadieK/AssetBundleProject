using UnityEngine;
using UnityEditor;
using System.Collections;

namespace NodeTreeView
{
	public class DepNode : BaseNode {
		private string _assetPath = "";
		private Object _assetObject = null;
		public DepNode(string assetPath) : base()
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
