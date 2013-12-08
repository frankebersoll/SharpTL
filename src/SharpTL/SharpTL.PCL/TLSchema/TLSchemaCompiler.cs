// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TLSchemaCompiler.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Text;
using System.Text.RegularExpressions;
using SharpTL.Utils;

namespace SharpTL.TLSchema
{
    public class TLCombinatorParameter
    {
        public string Name { get; set; }
        
        public string Type { get; set; }

        public int Order { get; set; }

        public bool IsOptional { get; set; }
    }

    public class TLDeclaration
    {
        public string Text { get; set; }

        public string CombinatorName { get; set; }

        public uint CombinatorNumber { get; set; }

        public string TypeName { get; set; }
    }

    /// <summary>
    ///     TL-schema.
    /// </summary>
    public class TLSchemaCompiler
    {
        private static readonly Regex TLSchemaPartsRegex = new Regex(@"(?<Types>[\w\W\r\n]*)---functions---(?<Functions>[\w\W\r\n]*)");
        private static readonly Regex ReturnRegex = new Regex(@"\s*(\r\n|\n\r|\r|\n)+\s*");

        private static readonly Regex DeclarationRegex =
            new Regex(@"[\s]*(?<Declaration>(?<CombinatorName>[\w-[#]]*)(?:\s|#(?<CombinatorNumber>[0-9a-fA-F]{1,8})\s*)?" +
                @"(?<Parameters>[\w\W-[;]]+?)??\s*?=\s*(?<TypeName>[\w\W]+?));");

        private static readonly Regex SingleLineCommentRegex = new Regex("//.*$", RegexOptions.Multiline);
        private static readonly Regex MultiLineCommentRegex = new Regex(@"/\*.*?\*/", RegexOptions.Singleline);

        private static readonly Encoding DefaultEncoding = Encoding.UTF8;

        /// <summary>
        ///     Compile TL-schema to C# object model.
        /// </summary>
        /// <param name="tlSchema">TL-schema.</param>
        /// <returns>Compiled schema.</returns>
        public string Compile(string tlSchema)
        {
            tlSchema = RemoveComments(tlSchema);
            tlSchema = RemoveNewlines(tlSchema);

            Match partsMatch = TLSchemaPartsRegex.Match(tlSchema);
            if (!partsMatch.Success)
            {
                throw new InvalidTLSchemaException();
            }
            string typesPart = partsMatch.Groups["Types"].Value.Trim();
            string functionsPart = partsMatch.Groups["Functions"].Value.Trim();

            foreach (Match declarationMatch in DeclarationRegex.Matches(typesPart))
            {
                string declarationText = declarationMatch.Groups["Declaration"].Value.Trim();
                uint combinatorNumber = Crc32.Compute(declarationText, DefaultEncoding);

                var declaration = new TLDeclaration {Text = declarationText, CombinatorName = declarationMatch.Groups["CombinatorName"].Value.Trim()};

                Group combinatorNumberMatch = declarationMatch.Groups["CombinatorNumber"];
                declaration.CombinatorNumber = combinatorNumberMatch.Success ? Convert.ToUInt32(combinatorNumberMatch.Value, 16) : combinatorNumber;

                Group parametersMatch = declarationMatch.Groups["Parameters"];
                if (parametersMatch.Success)
                {
//                    declaration.
                }
            }

            var sb = new StringBuilder();


            return sb.ToString();
        }

        private static string RemoveComments(string text)
        {
            text = SingleLineCommentRegex.Replace(text, string.Empty);
            return MultiLineCommentRegex.Replace(text, string.Empty);
        }

        private static string RemoveNewlines(string text)
        {
            return ReturnRegex.Replace(text, " ");
        }
    }
}
