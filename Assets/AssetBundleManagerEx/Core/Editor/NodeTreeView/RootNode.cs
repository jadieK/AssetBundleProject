using UnityEngine;
using UnityEditor;
using System.Collections;

namespace NodeTreeView
{
	public class RootNode : BaseNode 
	{
		AssetBundleBuilderUtil.CompressType _currentCompressType = AssetBundleBuilderUtil.CompressType.LZMA;
		public AssetBundleBuilderUtil.CompressType CurrentCompressType
		{
			set
			{
				_currentCompressType = value;
			}
			get
			{
				return _currentCompressType;
			}
		}
		protected override void DrawExtra (int layer, BaseNode currentSelected)
		{
			base.DrawExtra (layer, currentSelected);
			if (GUILayout.Button (_currentCompressType.ToString(), GUILayout.ExpandWidth(false)))
			{
				_currentCompressType = AssetBundleBuilderUtil.NextCompressType (_currentCompressType);
			}
		}
	}
}
