﻿using System.Text;

namespace Mjml.Net
{
    public static partial class XmlFixer
    {
        public static string Process(string mjml, MjmlOptions options)
        {
            var sb = DefaultPools.StringBuilders.Get();

            try
            {
                var span = mjml.AsSpan();

                static ReadOnlySpan<char> FixNextSequence(ReadOnlySpan<char> span, StringBuilder sb, MjmlOptions options)
                {
                    var nextTagStart = span.IndexOf('<');

                    if (nextTagStart < 0)
                    {
                        // Add everything that is left.
                        sb.Append(span);
                        return default;
                    }

                    if (nextTagStart > 0)
                    {
                        // Add everything to the character.
                        sb.Append(span[..nextTagStart]);

                        // Move to the character.
                        span = span[nextTagStart..];
                    }

                    var first = span[0];

                    if (StartWithIgnoreWhitespace(span, "<br*>", out var charsRead))
                    {
                        var afterBr = span[charsRead..];

                        if (!StartWithIgnoreWhitespace(afterBr, "*</br*>", out _))
                        {
                            sb.Append("<br />");
                            return afterBr;
                        }
                        else
                        {
                            sb.Append(first);
                        }
                    }
                    else
                    {
                        sb.Append(first);
                    }

                    // Skip the start character.
                    return span[1..];
                }

                while (span.Length > 0)
                {
                    span = FixNextSequence(span, sb, options);
                }

                return sb.ToString();
            }
            finally
            {
                DefaultPools.StringBuilders.Return(sb);
            }
        }

        private static bool StartWithIgnoreWhitespace(ReadOnlySpan<char> input, string test, out int charsRead)
        {
            var i = 0;
            foreach (var testCharacter in test)
            {
                if (i >= input.Length)
                {
                    charsRead = -1;
                    return false;
                }

                var c = input[i];

                if (testCharacter == '*')
                {
                    while (char.IsWhiteSpace(c) && i < input.Length - 2)
                    {
                        i++;
                        c = input[i];
                    }

                    continue;
                }

                if (char.ToUpperInvariant(testCharacter) != char.ToUpperInvariant(c))
                {
                    charsRead = -1;
                    return false;
                }

                i++;
            }

            charsRead = i;
            return true;
        }
    }
}
