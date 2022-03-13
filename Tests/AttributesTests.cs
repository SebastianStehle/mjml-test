﻿using Mjml.Net;
using Mjml.Net.Helpers;
using Tests.Properties;
using Xunit;

namespace Tests
{
    public class AttributesTests
    {
        [Fact]
        public void Should_render_font_with_attributes()
        {
            var source = @"
 <mjml plain=""plain"">
  <mj-head>
    <mj-attributes>
      <mj-font name=""Raleway"" href=""https://fonts.googleapis.com/css?family=Raleway"" />
    </mj-attributes>
    <mj-font />
  </mj-head>
  <mj-body>
  </mj-body>
</mjml>
";

            var result = TestHelper.Render(source, new FontHelper());

            AssertHelpers.HtmlAssert(Resources.Font, result);
        }

        [Fact]
        public void Should_render_font_with_class_attribute()
        {
            var source = @"
 <mjml plain=""plain"">
  <mj-head>
    <mj-attributes>
      <mj-class name=""blue"" href=""https://fonts.googleapis.com/css?family=Raleway"" />
    </mj-attributes>
    <mj-font mj-class=""red blue"" />
  </mj-head>
  <mj-body>
  </mj-body>
</mjml>
";

            var result = TestHelper.Render(source, new FontHelper());

            AssertHelpers.HtmlAssert(Resources.Font, result);
        }

        [Fact]
        public void Should_render_font_with_multiple_attributes()
        {
            var source = @"
 <mjml plain=""plain"">
  <mj-head>
    <mj-attributes>
      <mj-other />
    </mj-attributes>
    <mj-attributes>
      <mj-font name=""Mono"" href=""https://fonts.googleapis.com/css?family=Mono"" />
    </mj-attributes>
    <mj-attributes>
      <mj-font name=""Raleway"" href=""https://fonts.googleapis.com/css?family=Raleway"" />
    </mj-attributes>
    <mj-font />
  </mj-head>
  <mj-body>
  </mj-body>
</mjml>
";

            var result = TestHelper.Render(source, new FontHelper());

            AssertHelpers.HtmlAssert(Resources.Font, result);
        }
    }
}
