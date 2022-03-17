﻿namespace Mjml.Net.Components.Body
{
    public partial class SectionProps
    {
        [Bind("none")]
        public string? None;
    }

    public sealed class SectionComponent : BodyComponentBase<SectionProps>
    {
        public override void Render(IHtmlRenderer renderer, GlobalContext context)
        {
        }
    }
}
