using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Css;
using AngleSharp.Dom;
using AngleSharp.Dom.Css;
using AngleSharp.Parser.Html;
using AngleSharp.Parser.Css;
using System.IO;
using AngleSharp.Dom.Events;

namespace AngleSharp
{

    public interface Document : IDocument { }
    public interface Element : IElement { }
    public interface HtmlFormElement : IElement { }
}