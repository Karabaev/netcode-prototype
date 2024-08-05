using System;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine.AddressableAssets;

namespace Motk.Client.Core.Descriptors.Serialization
{
	public class AssetReferenceConverter<T> : JsonConverter<T>
		where T : AssetReference
	{
		// ReSharper disable once StaticMemberInGenericType
		private static readonly Type[] GetConstructorArgs = { typeof(string) };
		private readonly ConstructorInfo _constructor = typeof(T).GetConstructor(GetConstructorArgs)!;
		private readonly object[] _constructorArgs = new object[1];

		public override T? ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			var value = reader.ReadAsString();
			if (string.IsNullOrEmpty(value))
				return Create(value);

			var split = value.Split("_", 2);
			var result = Create(split[0]);
			if (split.Length > 1)
			{
				result.SubObjectName = split[1];
			}
			return result;
		}
		
		public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer)
		{
			var serialized = value switch
			{
				{ AssetGUID: { Length: > 0 }, SubObjectName: { Length: > 0 } } => $"{value.AssetGUID}_{value.SubObjectName}",
				{ AssetGUID: { Length: > 0 } } => value.AssetGUID,
				_ => null
			};
			writer.WriteValue(serialized);
		}

		private T Create(string guid)
		{
			_constructorArgs[0] = guid;
			var instance = (T) _constructor.Invoke(_constructorArgs);
			return instance;
		}
	}
}
