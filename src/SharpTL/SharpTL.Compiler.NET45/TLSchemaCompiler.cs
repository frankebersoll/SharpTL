// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TLSchemaCompiler.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using SharpTL.BaseTypes;
using SharpTL.Serializers;

namespace SharpTL.Compiler
{
    /// <summary>
    ///     TL-schema.
    /// </summary>
    public partial class TLSchemaCompiler
    {
        private static readonly Regex VectorRegex = new Regex(@"^(?:(?<Boxed>V)|(?<Bare>v))ector<(?<ItemsType>\w[\w\W-[\s]]*)>$", RegexOptions.Compiled);
        private static readonly Regex BareTypeRegex = new Regex(@"^%(?<TypeName>\w+)$", RegexOptions.Compiled);
        private static readonly Regex Int128Regex = new Regex(@"^int128$", RegexOptions.Compiled);
        private static readonly Regex Int256Regex = new Regex(@"^int256$", RegexOptions.Compiled);
        private readonly string _defaultNamespace;
        private readonly Encoding _encoding;
        private readonly Dictionary<string, TLType> _tlTypesCache = new Dictionary<string, TLType>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="TLSchemaCompiler" /> class.
        /// </summary>
        /// <param name="defaultNamespace">Default namespace.</param>
        protected TLSchemaCompiler(string defaultNamespace) : this(defaultNamespace, Encoding.UTF8)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TLSchemaCompiler" /> class.
        /// </summary>
        /// <param name="defaultNamespace">Default namespace.</param>
        /// <param name="encoding">Default encoding.</param>
        protected TLSchemaCompiler(string defaultNamespace, Encoding encoding)
        {
            _defaultNamespace = defaultNamespace;
            _encoding = encoding;
        }

        protected string Compile(TLSchema tlSchema)
        {
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
            if (_tlTypesCache.TryGetValue(typeName, out type))
                return type;

            string builtInName = typeName;

            // Vector.
            Match match = VectorRegex.Match(typeName);
            if (match.Success)
            {
                var itemsType = GetTLType(match.Groups["ItemsType"].Value);
                builtInName = string.Format("List<{0}>", itemsType.BuiltInName);
            }

            // int128.
            match = Int128Regex.Match(typeName);
            if (match.Success)
            {
                builtInName = typeof(Int128).FullName;
            }

            // int256.
            match = Int256Regex.Match(typeName);
            if (match.Success)
            {
                builtInName = typeof(Int256).FullName;
            }

            // % bare type.
            // TODO;

            type = new TLType(typeName) { BuiltInName = builtInName };
            _tlTypesCache.Add(typeName, type);
            
            return type;
        }
    }
}
