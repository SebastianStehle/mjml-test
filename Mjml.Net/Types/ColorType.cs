﻿using System.Text.RegularExpressions;

namespace Mjml.Net.Types
{
    public sealed class ColorType : IType
    {
        private static readonly HashSet<string> Colors = new HashSet<string>
        {
            "aliceblue",
            "antiquewhite",
            "aqua",
            "aquamarine",
            "azure",
            "beige",
            "bisque",
            "black",
            "blanchedalmond",
            "blue",
            "blueviolet",
            "brown",
            "burlywood",
            "cadetblue",
            "chartreuse",
            "chocolate",
            "coral",
            "cornflowerblue",
            "cornsilk",
            "crimson",
            "cyan",
            "darkblue",
            "darkcyan",
            "darkgoldenrod",
            "darkgray",
            "darkgreen",
            "darkgrey",
            "darkkhaki",
            "darkmagenta",
            "darkolivegreen",
            "darkorange",
            "darkorchid",
            "darkred",
            "darksalmon",
            "darkseagreen",
            "darkslateblue",
            "darkslategray",
            "darkslategrey",
            "darkturquoise",
            "darkviolet",
            "deeppink",
            "deepskyblue",
            "dimgray",
            "dimgrey",
            "dodgerblue",
            "firebrick",
            "floralwhite",
            "forestgreen",
            "fuchsia",
            "gainsboro",
            "ghostwhite",
            "gold",
            "goldenrod",
            "gray",
            "green",
            "greenyellow",
            "grey",
            "honeydew",
            "hotpink",
            "indianred",
            "indigo",
            "inherit",
            "ivory",
            "khaki",
            "lavender",
            "lavenderblush",
            "lawngreen",
            "lemonchiffon",
            "lightblue",
            "lightcoral",
            "lightcyan",
            "lightgoldenrodyellow",
            "lightgray",
            "lightgreen",
            "lightgrey",
            "lightpink",
            "lightsalmon",
            "lightseagreen",
            "lightskyblue",
            "lightslategray",
            "lightslategrey",
            "lightsteelblue",
            "lightyellow",
            "lime",
            "limegreen",
            "linen",
            "magenta",
            "maroon",
            "mediumaquamarine",
            "mediumblue",
            "mediumorchid",
            "mediumpurple",
            "mediumseagreen",
            "mediumslateblue",
            "mediumspringgreen",
            "mediumturquoise",
            "mediumvioletred",
            "midnightblue",
            "mintcream",
            "mistyrose",
            "moccasin",
            "navajowhite",
            "navy",
            "oldlace",
            "olive",
            "olivedrab",
            "orange",
            "orangered",
            "orchid",
            "palegoldenrod",
            "palegreen",
            "paleturquoise",
            "palevioletred",
            "papayawhip",
            "peachpuff",
            "peru",
            "pink",
            "plum",
            "powderblue",
            "purple",
            "rebeccapurple",
            "red",
            "rosybrown",
            "royalblue",
            "saddlebrown",
            "salmon",
            "sandybrown",
            "seagreen",
            "seashell",
            "sienna",
            "silver",
            "skyblue",
            "slateblue",
            "slategray",
            "slategrey",
            "snow",
            "springgreen",
            "steelblue",
            "tan",
            "teal",
            "thistle",
            "tomato",
            "transparent",
            "turquoise",
            "violet",
            "wheat",
            "white",
            "whitesmoke",
            "yellow",
            "yellowgreen",
        };

        private static readonly Regex Rgba = new Regex(@"^rgba\(\d{1,3},\s?\d{1,3},\s?\d{1,3},\s?\d(\.\d{1,3})?\)?$", RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static readonly Regex Rgb = new Regex(@"^rgb\(\d{1,3},\s?\d{1,3},\s?\d{1,3}\)?$", RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static readonly Regex Hex = new Regex(@"^#([0-9a-f]{3}){1,2}?$", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        public bool Validate(string value)
        {
            // Unfortunately we cannot avoid the allocation here, but it is only necessary in strict validation mode.
            var trimmed = value.Trim();

            if (Colors.Contains(trimmed))
            {
                return true;
            }

            return Rgba.IsMatch(trimmed) || Rgb.IsMatch(trimmed) || Hex.IsMatch(trimmed);
        }

        public string Coerce(string value)
        {
            var trimmed = value.AsSpan().Trim();

            if (trimmed.Length == 4 && trimmed[0] == '#')
            {
                return new string(new char[]
                {
                    trimmed[0],
                    trimmed[1],
                    trimmed[1],
                    trimmed[2],
                    trimmed[2],
                    trimmed[3],
                    trimmed[3]
                });
            }

            return value;
        }
    }
}
