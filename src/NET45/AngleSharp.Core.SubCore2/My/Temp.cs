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
    public class BrowsingContext : IBrowsingContext
    {   
        public static IBrowsingContext New(IConfiguration config)
        {
            throw new NotImplementedException("RAD");
        }
        public IDocument Active
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IDocument Creator
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IWindow Current
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IBrowsingContext Parent
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Sandboxes Security
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IHistory SessionHistory
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event DomEventHandler Parsed;
        public event DomEventHandler ParseError;
        public event DomEventHandler Parsing;
        public event DomEventHandler Requested;
        public event DomEventHandler Requesting;

        public void AddEventListener(string type, DomEventHandler callback = null, bool capture = false)
        {
            throw new NotImplementedException();
        }

        public bool Dispatch(Event ev)
        {
            throw new NotImplementedException();
        }

        public void InvokeEventListener(Event ev)
        {
            throw new NotImplementedException();
        }

        public void RemoveEventListener(string type, DomEventHandler callback = null, bool capture = false)
        {
            throw new NotImplementedException();
        }
    }

    public interface CssMedium : CssNode, ICssMedium
    {
    }
    public interface CssNode : ICssNode
    {
    }
    public interface CssStyleDeclaration
    {
    }
    public class CssValue
    {
        public CssValue(object o) { }
    }
    public interface CssRule : ICssRule { }

    public interface HtmlDocument { }

    public class CssSelectorConstructor
    {
        public CssSelectorConstructor()
        {
        }
        public CssSelectorConstructor(object o1, object o2, object o3) { }
        public ISelector GetResult()
        {
            throw new NotImplementedException("RAD");
        }
        public CssSelectorConstructor Reset(object o1, object o2, object o3)
        {
            throw new NotImplementedException("RAD");
        }
    }
    public class SimpleSelector : ISelector
    {
        public SimpleSelector(object o1, object o2, object o3)
        { }
        public SimpleSelector(Predicate<IElement> matches, Priority specifify, String code)
        {
        }

        public IEnumerable<ICssNode> Children
        {
            get
            {
                throw new NotImplementedException("RAD");
            }
        }

        public TextView SourceCode
        {
            get
            {
                throw new NotImplementedException("RAD");
            }
        }

        public Priority Specifity
        {
            get
            {
                throw new NotImplementedException("RAD");
            }
        }

        public string Text
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Match(IElement element)
        {
            throw new NotImplementedException();
        }

        public void ToCss(TextWriter writer, IStyleFormatter formatter)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// Extensions for ensuring code portability.
    /// </summary>
    static class PortableExtensions
    {
        [MyAggressiveMethodInliningAttribute]
        public static String ConvertFromUtf32(this Int32 utf32)
        {
            return Char.ConvertFromUtf32(utf32);
        }
    }
    static class HtmlParserExtension
    {
        public static Int32 GetCode(this HtmlParseError code)
        {
            return (Int32)code;
        }
    }

    static class CssParserExtensions
    {

        /// <summary>
        /// Gets the corresponding token type for the function name.
        /// </summary>
        /// <param name="functionName">The name to match.</param>
        /// <returns>The token type for the name.</returns>
        public static CssTokenType GetTypeFromName(this String functionName)
        {
            throw new NotSupportedException("RAD");
            //var creator = default(Func<String, DocumentFunction>);
            //return functionTypes.TryGetValue(functionName, out creator) ? CssTokenType.Url : CssTokenType.Function;
        }
        /// <summary>
        /// Retrieves a number describing the error of a given error code.
        /// </summary>
        /// <param name="code">A specific error code.</param>
        /// <returns>The code of the error.</returns>
        public static Int32 GetCode(this CssParseError code)
        {
            return (Int32)code;
        }
    }
    static class ObjectExtensions
    {
        /// <summary>
        /// Retrieves a string describing the error of a given error code.
        /// </summary>
        /// <param name="code">A specific error code.</param>
        /// <returns>The description of the error.</returns>
        public static String GetMessage<T>(this T code)
            where T : struct
        {
            throw new NotSupportedException("RAD");
            //var type = typeof(T).GetTypeInfo();
            //var field = type.GetDeclaredField(code.ToString());
            //var description = field.GetCustomAttribute<DomDescriptionAttribute>()?.Description;
            //return description ?? "An unknown error occurred.";
        }
    }
    static class ValueExtensions
    {
        public static String ToText(this IEnumerable<CssToken> value)
        {
            return String.Join(String.Empty, value.Select(m => m.ToValue()));
        }

    }


    public class Configuration : IConfiguration
    {
        /// <summary>
        /// Gets an enumeration over the available services.
        /// </summary>
        public IEnumerable<Object> Services
        {
            get
            {
                throw new NotSupportedException("RAD");
            }
        }
        public static IConfiguration Default = new Configuration();
    }

    static class ConfigurationExtensions
    {
        public static Boolean IsScripting(this IConfiguration configuration)
        {
            throw new NotSupportedException("RAD");
        }
    }

    static class ElementExtensions
    {

        /// <summary>
        /// Checks if the element with the provided prefix matches the CSS
        /// namespace.
        /// </summary>
        /// <param name="el">The element to examine.</param>
        /// <param name="prefix">The namespace in question.</param>
        /// <returns>True if the namespace is matched, else false.</returns>
        public static Boolean MatchesCssNamespace(this IElement el, String prefix)
        {
            throw new NotImplementedException("RAD");
            //if (prefix.Is(Keywords.Asterisk))
            //{
            //    return true;
            //}

            //var nsUri = el.GetAttribute(NamespaceNames.XmlNsPrefix) ?? el.NamespaceUri;

            //if (prefix.Is(String.Empty))
            //{
            //    return nsUri.Is(String.Empty);
            //}

            //return nsUri.Is(GetCssNamespace(el, prefix));
        }
    }

}