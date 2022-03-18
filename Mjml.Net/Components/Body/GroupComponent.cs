﻿using Mjml.Net.Extensions;
using Mjml.Net.Helpers;

namespace Mjml.Net.Components.Body
{
    public partial class GroupComponent : Component, IProvidesWidth
    {
        public override string ComponentName => "mj-group";

        [Bind("background-color", BindType.Color)]
        public string? BackgroundColor;

        [Bind("css-class")]
        public string? CssClass;

        [Bind("direction", BindType.Direction)]
        public string Direction = "ltr";

        [Bind("vertical-align", BindType.VerticalAlign)]
        public string? VerticalAlign;

        [Bind("width", BindType.PixelsOrPercent)]
        public string? Width;

        public ContainerWidth ContainerWidth;

        public double CurrentWidth;

        public override void Render(IHtmlRenderer renderer, GlobalContext context)
        {
            ContainerWidth = context.GetContainerWidth();

            var (width, widthString, pixels) = GetParsedWidth();

            CurrentWidth = pixels;

            renderer.StartElement("div") // Style div
                .Class(GetColumnClass(width, widthString, context))
                .Class("mj-outlook-group-fix")
                .Class(CssClass)
                .Style("background-color", BackgroundColor)
                .Style("direction", Direction)
                .Style("display", "inline-block")
                .Style("font-size", "0")
                .Style("line-height", "0")
                .Style("text-align", "left")
                .Style("vertical-align", VerticalAlign)
                .Style("width", "100%");

            renderer.Content("<!--[if mso | IE]>");

            renderer.StartElement("table")
                .Attr("bgcolor", BackgroundColor == "none" ? null : BackgroundColor)
                .Attr("border", "0")
                .Attr("cellpadding", "0")
                .Attr("cellspacing", "0")
                .Attr("role", "presentation");

            renderer.StartElement("tr");

            foreach (var child in ChildNodes)
            {
                var childWidth = GetElementWidth(child);

                context.Push();
                context.SetContainerWidth(childWidth);

                if (child.Raw)
                {
                    renderer.Content("<![endif]-->");
                    child.Render(renderer, context);
                    renderer.Content("<!--[if mso | IE]>");
                }
                else
                {
                    renderer.StartElement("td")
                        .Style("align", child.Node.GetAttribute("align"))
                        .Style("vertical-align", child.Node.GetAttribute("vertical-align"))
                        .Style("width", $"{childWidth}px");

                    renderer.Content("<![endif]-->");

                    child.Render(renderer, context);

                    renderer.Content("<!--[if mso | IE]>");
                    renderer.EndElement("td");
                }

                context.Pop();
            }

            renderer.EndElement("tr");
            renderer.EndElement("table");

            renderer.Content("<![endif]-->");

            renderer.EndElement("div");
        }

        private double GetElementWidth(IComponent component)
        {
            var width = 0d;

            if (component is IProvidesWidth providesWidth)
            {
                width = providesWidth.GetWidthAsPixel();
            }

            if (Width != null)
            {
                var parsed = UnitParser.Parse(Width);

                if (parsed.Unit == Unit.Pixels)
                {
                    width = 100 * parsed.Value / CurrentWidth;
                }
            }
            else
            {
                width = CurrentWidth / Math.Max(1, component.ChildNodes.Count(x => !x.Raw));
            }

            return width;
        }

        private static string GetColumnClass((double Value, Unit Unit) width, string originalWidth, GlobalContext context)
        {
            string className;

            if (width.Unit == Unit.Percent)
            {
                className = $"mj-column-per-{width.Value}";
            }
            else
            {
                className = $"mj-column-px-{width.Value}";
            }

            context.SetGlobalData(originalWidth, MediaQuery.Width(className, originalWidth));

            return className;
        }

        private ((double Value, Unit Unit), string, double) GetParsedWidth()
        {
            var widthValue = 0d;
            var widthUnit = Unit.Pixels;
            var widthString = string.Empty;
            var pixels = 0d;

            if (Width != null)
            {
                (widthValue, widthUnit) = UnitParser.Parse(Width);

                // No need to interpolate it again.
                widthString = Width;
            }
            else
            {
                widthValue = 100 / Math.Max(1, Parent?.ChildNodes.Count(x => !x.Raw) ?? 1);
                widthUnit = Unit.Percent;
                widthString = $"{widthValue}%";
            }

            if (widthUnit != Unit.Pixels)
            {
                pixels = ContainerWidth.Value * widthValue / 100;
            }
            else
            {
                pixels = widthValue;
            }

            return ((widthValue, widthUnit), widthString, pixels);
        }

        double IProvidesWidth.GetWidthAsPixel()
        {
            return CurrentWidth;
        }

        public override string? GetInheritingAttribute(string name)
        {
            switch (name)
            {
                case "mobileWidth":
                    return "mobileWidth";
            }

            return null;
        }
    }
}
