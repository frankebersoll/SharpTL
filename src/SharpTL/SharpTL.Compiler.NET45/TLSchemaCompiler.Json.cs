// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TLSchemaCompiler.Json.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using ServiceStack.Text;

namespace SharpTL.Compiler
{
    public partial class TLSchemaCompiler
    {
        public string CompileFromJson(string json)
        {
            return Compile(GetTLSchemaFromJson(json));
        }

        public TLSchema GetTLSchemaFromJson(string json)
        {
            JsonObject tlSchemaJsonObject = JsonObject.Parse(json);

            var constructors = CreateConstructorsFromJsonArrayObjects(tlSchemaJsonObject.ArrayObjects("constructors"));
            var methods = CreateMethodsFromJsonArrayObjects(tlSchemaJsonObject.ArrayObjects("methods"));
            var types = CreateTLTypes(constructors);

            return new TLSchema
            {
                Constructors = constructors,
                Methods = methods,
                Types = types
            };
        }

        private List<TLCombinator> CreateConstructorsFromJsonArrayObjects(JsonArrayObjects objects)
        {
            return CreateCombinatorsFromJsonArrayObjects(objects, "predicate");
        }

        private List<TLCombinator> CreateMethodsFromJsonArrayObjects(JsonArrayObjects objects)
        {
            return CreateCombinatorsFromJsonArrayObjects(objects, "method");
        }

        private static List<TLCombinator> CreateCombinatorsFromJsonArrayObjects(JsonArrayObjects objects, string nameKey)
        {
            return
                objects.ConvertAll(
                    x =>
                        new TLCombinator
                        {
                            Number = (uint) x.JsonTo<int>("id"),
                            Name = x.Get(nameKey),
                            Parameters = x.ArrayObjects("params").ConvertAll(param => new TLCombinatorParameter {Name = param.Get("name"), Type = param.Get("type")}),
                            Type = x.Get("type")
                        });
        }
    }
}
