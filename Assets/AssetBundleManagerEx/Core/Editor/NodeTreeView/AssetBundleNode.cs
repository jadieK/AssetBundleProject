using UnityEngine;
using UnityEditor;
using System.Collections;

namespace NodeTreeView
{
	public class AssetBundleNode : BaseNode 
	{
		private RootNode _root = null;
		private bool doBuild = false;

		public AssetBundleNode(RootNode root) : base()
		{
			_root = root;
		}

		public AssetBundleBuilderUtil.CompressType CurrentCompressType
		{
			get
			{
				return _root.CurrentCompressType;
			}
		}

		protected override void DrawExtra (int layer, BaseNode currentSelected)
		{
			base.DrawExtra (layer, currentSelected);
			if (GUILayout.Button ("Build", GUILayout.ExpandWidth (false)))
			{
				doBuild = true;
//				AssetBundleBuilderUtil.BuildAssetBundleByConfig (this);
			}
		}

		protected override void DoAction ()
		{
			base.DoAction ();
			if (doBuild)
			{
				doBuild = false;
				AssetBundleBuilderUtil.BuildAssetBundleByConfig (this);
			}
				
		}

		public AssetBundleBuild GetAssetBundleBuild()
		{
			AssetBundleBuild config = new AssetBundleBuild ();
			config.assetBundleName = nodeText;
			config.assetNames = new string[childNodes.Count];
			for (int assetIndex = 0; assetIndex < childNodes.Count; assetIndex++)
			{
				config.assetNames [assetIndex] = (childNodes [assetIndex] as AssetNode).AssetPath;
			}
			return config;
		}
	}
}
