// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TLSchemaCompiler.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpTL.Serializers;

namespace SharpTL.Compiler
{
    /// <summary>
    ///     TL-schema.
    /// </summary>
    public partial class TLSchemaCompiler
    {
        private readonly string _defaultNamespace;
        private readonly Encoding _encoding;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TLSchemaCompiler" /> class.
        /// </summary>
        /// <param name="defaultNamespace">Default namespace.</param>
        public TLSchemaCompiler(string defaultNamespace) : this(defaultNamespace, Encoding.UTF8)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TLSchemaCompiler" /> class.
        /// </summary>
        /// <param name="defaultNamespace">Default namespace.</param>
        /// <param name="encoding">Default encoding.</param>
        public TLSchemaCompiler(string defaultNamespace, Encoding encoding)
        {
            _defaultNamespace = defaultNamespace;
            _encoding = encoding;
        }

        public string Compile(TLSchema tlSchema)
        {
            var template = new SharpTLDefaultTemplate(new TemplateVars {Schema = tlSchema, Namespace = _defaultNamespace});
            return template.TransformText();
        }

        private static List<TLType> CreateTLTypes(IEnumerable<TLCombinator> constructors)
        {
            var typesDict = new Dictionary<string, HashSet<uint>>();

            foreach (TLCombinator constructor in constructors)
            {
                HashSet<uint> constructorNumbers;
                string type = constructor.Type;
                if (!typesDict.ContainsKey(type))
                {
                    constructorNumbers = new HashSet<uint>();
                    typesDict.Add(type, constructorNumbers);
                }
                else
                {
                    constructorNumbers = typesDict[type];
                }

                uint number = constructor.Number;
                if (!constructorNumbers.Contains(number))
                {
                    constructorNumbers.Add(number);
                }
            }

            return typesDict.Select(typeConstructors => new TLType {Name = typeConstructors.Key, ConstructorNumbers = typeConstructors.Value.ToList()}).ToList();
        }

        private static void UpdateTypes(TLSchema schema)
        {
            
        }

        private string GetCSharpTypeName(uint constructorNumber)
        {
            foreach (ITLSerializer serializer in BuiltIn.BaseTypeSerializers)
            {
                var singleConstructorSerializer = serializer as ITLSingleConstructorSerializer;
                var multiConstructorSerializer = serializer as ITLMultiConstructorSerializer;
                if ((singleConstructorSerializer != null && constructorNumber == singleConstructorSerializer.ConstructorNumber) ||
                    multiConstructorSerializer != null && multiConstructorSerializer.ConstructorNumbers.Contains(constructorNumber))
                {
                    return serializer.SupportedType.FullName;
                }
            }
            return null;
        }
    }
}
