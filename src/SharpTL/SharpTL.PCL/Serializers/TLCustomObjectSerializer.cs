// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TLCustomObjectSerializer.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SharpTL.Serializers
{
    /// <summary>
    ///     Serializer for TL custom object.
    /// </summary>
    public class TLCustomObjectSerializer : TLSerializerBase
    {
        private readonly uint _constructorNumber;
        private readonly Type _objectType;
        private readonly TLPropertyInfo[] _properties;

        public TLCustomObjectSerializer(uint constructorNumber, Type objectType, IEnumerable<TLPropertyInfo> properties,
            TLSerializationMode serializationMode = TLSerializationMode.Boxed)
        {
            _constructorNumber = constructorNumber;
            _objectType = objectType;
            _properties = properties.OrderBy(info => info.Order).ToArray();
            SerializationMode = serializationMode;
        }

        public override uint ConstructorNumber
        {
            get { return _constructorNumber; }
        }

        public override Type SupportedType
        {
            get { return _objectType; }
        }

        protected override void WriteBody(object obj, TLSerializationContext context)
        {
            for (int i = 0; i < _properties.Length; i++)
            {
                TLPropertyInfo tlPropertyInfo = _properties[i];
                PropertyInfo propertyInfo = tlPropertyInfo.PropertyInfo;

                Type propType = propertyInfo.PropertyType;
                object propertyValue = propertyInfo.GetValue(obj);

                // Check for TLVector attribute.
                var tlVectorAttribute = tlPropertyInfo.PropertyInfo.GetCustomAttribute<TLVectorAttribute>();
                if (tlVectorAttribute != null)
                {
                    TLSerializationMode? itemsSerializationMode = tlVectorAttribute.ItemsSerializationMode;

                    var serializer = context.Rig.GetSerializerByObjectType(propType) as ITLVectorSerializer;
                    if (serializer == null)
                    {
                        throw new NotSupportedException("Vector serializer doesn't implement ITLVectorSerializer interface.");
                    }
                    if (serializer.SupportedType != propType)
                    {
                        throw new NotSupportedException(string.Format("Current vector serializer doesn't support type: {0}. It supports: {1}", propType, serializer.SupportedType));
                    }

                    serializer.Write(propertyValue, context, tlPropertyInfo.SerializationModeOverride, itemsSerializationMode);
                }
                else
                {
                    TLRig.Serialize(propertyValue, context, tlPropertyInfo.SerializationModeOverride);
                }
            }
        }

        protected override object ReadBody(TLSerializationContext context)
        {
            object obj = Activator.CreateInstance(_objectType);
            for (int i = 0; i < _properties.Length; i++)
            {
                TLPropertyInfo tlPropertyInfo = _properties[i];
                object propertyValue = TLRig.Deserialize(tlPropertyInfo.PropertyInfo.PropertyType, context);
                tlPropertyInfo.PropertyInfo.SetValue(obj, propertyValue);
            }
            return obj;
        }
    }
}
