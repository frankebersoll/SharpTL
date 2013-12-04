// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TLVectorSerializer.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace SharpTL.Serializers
{
    public interface ITLVectorSerializer : ITLSerializer
    {
        void Write(object vector, TLSerializationContext context, TLSerializationMode? serializationModeOverride, TLSerializationMode? itemsSerializationModeOverride);
    }

    public class TLVectorSerializer<T> : TLBoxedTypeSerializerBase, ITLVectorSerializer
    {
        private const TLSerializationMode DefaultItemsSerializationMode = TLSerializationMode.Boxed;

        // ReSharper disable once StaticFieldInGenericType
        private static readonly Type SupportedTypeInternal = typeof (List<T>);

        public override uint ConstructorNumber
        {
            get { return 0x1CB5C415; }
        }

        public override Type SupportedType
        {
            get { return SupportedTypeInternal; }
        }

        public void Write(object vector, TLSerializationContext context, TLSerializationMode? serializationModeOverride, TLSerializationMode? itemsSerializationModeOverride)
        {
            WriteHeader(context, serializationModeOverride);
            WriteBodyInternal(vector, context, itemsSerializationModeOverride);
        }

        protected override object ReadBody(TLSerializationContext context)
        {
            int length = context.Streamer.ReadInt();
            var list = (List<T>) Activator.CreateInstance(SupportedTypeInternal, length);

            for (int i = 0; i < length; i++)
            {
                var item = TLRig.Deserialize<T>(context);
                list.Add(item);
            }

            return list;
        }

        protected override void WriteBody(object obj, TLSerializationContext context)
        {
            WriteBodyInternal(obj, context, DefaultItemsSerializationMode);
        }

        private void WriteBodyInternal(object obj, TLSerializationContext context, TLSerializationMode? itemsSerializationModeOverride = null)
        {
            var vector = obj as List<T>;
            if (vector == null)
            {
                // TODO: log wrong type.
                throw new InvalidOperationException("This serializer supports only List<> types.");
            }
            int length = vector.Count;

            // Length.
            context.Streamer.WriteInt(length);

            // Child objects.
            for (int i = 0; i < length; i++)
            {
                TLRig.Serialize(vector[i], context, itemsSerializationModeOverride);
            }
        }
    }
}
