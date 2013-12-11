// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TLSchemaCompiler.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SharpTL.Serializers;

namespace SharpTL.Compiler
{
    /// <summary>
    ///     TL-schema.
    /// </summary>
    public partial class TLSchemaCompiler
    {
        private static readonly Regex VectorRegex = new Regex(@"^(?:(?<Boxed>V)|(?<Bare>v))ector<(?<ItemsType>\w[\w\W-[\s]]*)>$", RegexOptions.Compiled);
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

        private static string GetBuiltInTypeName(string typeName)
        {
            Match match = VectorRegex.Match(typeName);
            if (match.Success)
            {
                typeName = string.Format("List<{0}>", GetBuiltInTypeName(match.Groups["ItemsType"].Value));
            }
            return typeName;
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
                type = new TLType(typeName) {BuiltInName = GetBuiltInTypeName(typeName)};
                _tlTypesCache.Add(typeName, type);
            }
            return type;
        }
    }
}
