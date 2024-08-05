using System;
using UnityEngine.AddressableAssets;

namespace Motk.Client.Core.Descriptors.Serialization
{
	public static class AssetReferenceSerializationExtensions
	{
		public static bool IsAssetReferenceType(Type type)
		{
			return typeof(AssetReference).IsAssignableFrom(type) ||
			       type.BaseType != null &&
			       type.BaseType.IsGenericType &&
			       type.BaseType.GetGenericTypeDefinition() == typeof(AssetReferenceT<>);
		}
	}
}
