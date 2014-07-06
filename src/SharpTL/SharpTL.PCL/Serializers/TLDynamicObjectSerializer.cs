using System;
using SharpTL.BaseTypes;

namespace SharpTL.Serializers
{
    public class TLDynamicObjectSerializer : ITLSerializer
    {
        /// <summary>
        /// Supported type.
        /// </summary>
        public Type SupportedType
        {
            get { return typeof(ITLObject); }
        }

        /// <summary>
        ///     Serializes an object.
        /// </summary>
        /// <param name="obj">Object to be serialized.</param>
        /// <param name="context">Serialization context.</param>
        /// <param name="modeOverride">Serialization mode override.</param>
        public void Write(object obj, TLSerializationContext context, TLSerializationMode? modeOverride = null)
        {
            TLRig.Serialize(obj, context, modeOverride);
        }

        /// <summary>
        /// Deserialize an object.
        /// </summary>
        /// <param name="context">Serialization context.</param>
        /// <param name="modeOverride">Serialization mode override.</param>
        /// <returns></returns>
        public object Read(TLSerializationContext context, TLSerializationMode? modeOverride = null)
        {
            if (modeOverride.HasValue && modeOverride.Value == TLSerializationMode.Bare)
            {
                throw new InvalidOperationException("TLMultiConstructorObjectSerializer doesn't support bare type deserialization.");
            }

            return TLRig.Deserialize(context);
        }
    }
}