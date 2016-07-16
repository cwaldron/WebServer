﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WebServer
{
    /// <summary>
    /// Extends the string class.
    /// </summary>
    public static class StringExtensions
    {
        #region Extension Methods

        /// <summary>
        /// Renders the string using the placeholder for the property names.
        /// </summary>
        /// <param name="format">The string to format.</param>
        /// <param name="values">The object to pull the values from. Usually an anonymous type.</param>
        /// <returns>The rendered string.</returns>
        public static string Render(this string format, object values = null)
        {
            return format.Render(null, values);
        }

        /// <summary>
        /// Renders the string using the placeholder for the property names.
        /// </summary>
        /// <param name="format">The string to format.</param>
        /// <param name="provider">The provider to use for formatting dates and numeric values.</param>
        /// <param name="values">The object to pull the values from. Usually an anonymous type.</param>
        /// <returns>The rendered string.</returns>
        public static string Render(this string format, IFormatProvider provider, object values = null)
        {
            return format.Render(provider, ToObjectMap(values));
        }

        /// <summary>
        /// Formats the string using the placeholder for the property names.
        /// </summary>
        /// <param name="format">The string to format.</param>
        /// <param name="values">The dictionary to pull the values from.</param>
        /// <returns>The rendered string.</returns>
        public static string Render(this string format, IDictionary<string, object> values)
        {
            return format.Render(null, values);
        }

        /// <summary>
        /// Renders the string using the placeholder for the property names.
        /// </summary>
        /// <param name="format">The string to format.</param>
        /// <param name="provider">The provider to use for formatting dates and numeric values.</param>
        /// <param name="values">The dictionary to pull the values from.</param>
        /// <returns>The rendered string.</returns>
        public static string Render(this string format, IFormatProvider provider, IDictionary<string, object> values)
        {
            if (values == null) return format;
            var results = ParseFormat(format);
            return string.Format(provider, results.Item1, results.Item2.Select(x => values[x]).ToArray());
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Parse the format string.
        /// </summary>
        /// <param name="source">source format string</param>
        /// <returns>
        /// A tuple consisting of the altered format string and a symbol table.
        /// </returns>
        private static Tuple<string, List<string>> ParseFormat(string source)
        {
            int state = 0;
            int index = 0;
            var target = new StringBuilder();
            var symbol = new StringBuilder();
            var format = new StringBuilder();
            var placeholder = 0;
            var symbolTable = new List<string>();

            while (index < source.Length)
            {
                switch (state)
                {
                    case 0: // Initial
                        if (source[index] == '{')
                        {
                            symbol = new StringBuilder();
                            state = 1;
                        }
                        else if (source[index] == '}')
                        {
                            if (Lookahead(source, index, '}'))
                            {
                                target.Append(source[index++]);
                            }
                            else
                            {
                                throw new FormatException();
                            }
                        }

                        target.Append(source[index]);
                        index += 1;
                        continue;

                    case 1: // Start symbol parse.
                        if (source[index] == '{')
                        {
                            target.Append(source[index++]);
                            state = 0;
                            continue;
                        }
                        if (char.IsLetter(source[index]))
                        {
                            state = 2;
                            symbol.Append(source[index]);
                            index += 1;
                            continue;
                        }
                        throw new FormatException();

                    case 2: // Build symbol
                        if (char.IsLetter(source[index]) || char.IsDigit(source[index]))
                        {
                            state = 2;
                            symbol.Append(source[index]);
                            index += 1;
                            continue;
                        }

                        if (source[index] == '}')
                        {
                            // Process symbol
                            symbolTable.Add(symbol.ToString());
                            target.Append(placeholder++);
                            target.Append(source[index]);
                            state = 0;
                            index += 1;
                            continue;
                        }

                        if (source[index] == ':')
                        {
                            // Start parse of format parameters
                            symbolTable.Add(symbol.ToString());
                            state = 3;
                            format = new StringBuilder();
                            index += 1;
                            continue;
                        }

                        throw new FormatException();

                    case 3: // Parse format parameters
                        if (source[index] == '}')
                        {
                            // Complete parse of format parameters
                            target.AppendFormat("{0}:{1}", placeholder++, format);
                            target.Append(source[index]);
                            state = 0;
                            index += 1;
                            continue;
                        }

                        format.Append(source[index]);
                        index += 1;
                        continue;

                    default: // Invalid state
                        throw new FormatException();
                }
            }

            if (state != 0)
                throw new FormatException();

            return new Tuple<string, List<string>>(target.ToString(), symbolTable);
        }

        /// <summary>
        /// Lookahead one character.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="index"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        private static bool Lookahead(string format, int index, char character)
        {
            return index < format.Length && format[index + 1] == character;
        }

        /// <summary>
        /// Converts anonymous object properties to an object map.
        /// </summary>
        /// <param name="anonymousObject"></param>
        /// <returns>object map</returns>
        private static IDictionary<string, object> ToObjectMap(object anonymousObject)
        {
            var objectMap = new Dictionary<string, object>();
            if (anonymousObject != null)
            {
                TypeDescriptor.GetProperties(anonymousObject)
                    .OfType<PropertyDescriptor>()
                    .ToList()
                    .ForEach(x => objectMap.Add(x.Name, x.GetValue(anonymousObject)));
            }

            return objectMap;
        }

        #endregion
    }
}
