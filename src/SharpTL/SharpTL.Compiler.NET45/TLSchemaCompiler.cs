// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TLSchemaCompiler.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BigMath;
using SharpTL.Serializers;

namespace SharpTL.Compiler
{
    /// <summary>
    ///     TL-schema.
    /// </summary>
    public partial class TLSchemaCompiler
    {
        private static readonly Regex VectorRegex = new Regex(@"^(?:(?<Boxed>V)|(?<Bare>v))ector<(?<ItemsType>%?\w[\w\W-[\s]]*)>$", RegexOptions.Compiled);
        private static readonly Regex BareTypeRegex = new Regex(@"^%(?<Type>\w+)$", RegexOptions.Compiled);
        private static readonly Regex Int128Regex = new Regex(@"^int128$", RegexOptions.Compiled);
        private static readonly Regex Int256Regex = new Regex(@"^int256$", RegexOptions.Compiled);
        private static readonly Regex TLBytesRegex = new Regex(@"^bytes$", RegexOptions.Compiled);
        private readonly string _defaultNamespace;
        private readonly Dictionary<string, TLType> _tlTypesCache = new Dictionary<string, TLType>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="TLSchemaCompiler" /> class.
        /// </summary>
        /// <param name="defaultNamespace">Default namespace.</param>
        protected TLSchemaCompiler(string defaultNamespace)
        {
            _defaultNamespace = defaultNamespace;
        }

        protected string Compile(TLSchema tlSchema)
        {
            SetBuiltInTypeNames(tlSchema);

            var template = new SharpTLDefaultTemplate(new TemplateVars {Schema = tlSchema, Namespace = _defaultNamespace});
            return template.TransformText();
        }

        private List<TLType> UpdateAndGetTLTypes(IEnumerable<TLCombinator> constructors)
        {
            var types = new List<TLType>();
            foreach (TLCombinator constructor in constructors)
            {
                TLType type = constructor.Type;
                if (!type.Constructors.Contains(constructor))
                {
                    type.Constructors.Add(constructor);
                }
                if (!types.Contains(type))
                {
                    types.Add(type);
                }
            }
            return types;
        }

        private static string GetBuiltInTypeName(uint constructorNumber)
        {
            return (from serializer in BuiltIn.BaseTypeSerializers
                let singleConstructorSerializer = serializer as ITLSingleConstructorSerializer
                let multiConstructorSerializer = serializer as ITLMultiConstructorSerializer
                where
                    (singleConstructorSerializer != null && constructorNumber == singleConstructorSerializer.ConstructorNumber) ||
                        multiConstructorSerializer != null && multiConstructorSerializer.ConstructorNumbers.Contains(constructorNumber)
                select serializer.SupportedType.FullName).FirstOrDefault();
        }

        private static bool HasBuiltInSerializer(uint constructorNumber)
        {
            return GetBuiltInTypeName(constructorNumber) != null;
        }

        private TLType GetTLType(string typeName)
        {
            TLType type;
            if (!_tlTypesCache.TryGetValue(typeName, out type))
            {
                type = new TLType(typeName);
                _tlTypesCache.Add(typeName, type);
            }
            return type;
        }

        private void SetBuiltInTypeNames(TLSchema schema)
        {
            foreach (TLCombinatorParameter parameter in schema.Constructors.SelectMany(constructor => constructor.Parameters))
            {
                FixType(parameter.Type, schema);
            }
        }

        private void FixType(TLType type, TLSchema schema)
        {
            List<TLCombinator> constructors = schema.Constructors;
            string typeName = type.Name;

            // Vector.
            Match match = VectorRegex.Match(typeName);
            if (match.Success)
            {
                TLType itemsType = GetTLType(match.Groups["ItemsType"].Value);
                FixType(itemsType, schema);
                type.BuiltInName = string.Format("System.Collections.Generic.List<{0}>", itemsType.BuiltInName);
                return;
            }

            // int128.
            match = Int128Regex.Match(typeName);
            if (match.Success)
            {
                type.BuiltInName = typeof (Int128).FullName;
                return;
            }

            // int256.
            match = Int256Regex.Match(typeName);
            if (match.Success)
            {
                type.BuiltInName = typeof (Int256).FullName;
                return;
            }

            // bytes.
            match = TLBytesRegex.Match(typeName);
            if (match.Success)
            {
                type.BuiltInName = typeof (byte[]).FullName;
                return;
            }

            // % bare types.
            match = BareTypeRegex.Match(type.Name);
            if (match.Success)
            {
                typeName = match.Groups["Type"].Value;
                type.BuiltInName = constructors.Where(combinator => combinator.Type.Name == typeName).Select(combinator => combinator.Name).SingleOrDefault() ?? typeName;
                type.SerializationModeOverride = TLSerializationMode.Bare;
            }
        }
    }
}
