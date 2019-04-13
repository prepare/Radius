using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Html.Dom;

using LayoutFarm;
using LayoutFarm.CustomWidgets;
using LayoutFarm.UI;
using LayoutFarm.Composers;
using LayoutFarm.WebDom;

namespace HtmlTest1
{
    public partial class Form1 : Form
    {
        Demo_UIHtmlBox _uiHtmlBoxApp;
        AppHostWinForm _appHost;

        LayoutFarm.UI.UISurfaceViewportControl _latestviewport;
        Form _latest_formCanvas;

        public Form1()
        {
            InitializeComponent();
        }
        void InitHtmlControl()
        {


        }
        private void button1_Click(object sender, EventArgs e)
        {
            //1. load and parse with AngleSharp's HtmlParser
            var parser = new HtmlParser();
            string htmlfile = @"D:\projects\HtmlRenderer\Source\Test8_HtmlRenderer.Demo\Samples\0_acid1_dev\00.htm";
            string htmlstr = File.ReadAllText(htmlfile);
            AngleSharp.Html.Dom.IHtmlDocument htmldoc = parser.ParseDocument(htmlstr);

            dbugIterAllChild(htmldoc);

        }
        static void dbugIterAllChild(AngleSharp.Dom.INode node)
        {
            System.Diagnostics.Debug.WriteLine(node.ToString());
            //2. Render with Our HtmlRenderer
            switch (node.NodeType)
            {
                case NodeType.Element:
                    {
                        AngleSharp.Html.Dom.HtmlElement elem = (AngleSharp.Html.Dom.HtmlElement)node;

                        foreach (AngleSharp.Dom.INode child in node.ChildNodes)
                        {
                            //iterate
                            dbugIterAllChild(child);
                        }
                    }
                    break;
                case NodeType.Document:
                    {
                        AngleSharp.Html.Dom.IHtmlDocument elem = (AngleSharp.Html.Dom.IHtmlDocument)node;
                        foreach (AngleSharp.Dom.INode child in node.ChildNodes)
                        {
                            //iterate
                            dbugIterAllChild(child);
                        }
                    }
                    break;
                case NodeType.Text:
                    {
                        AngleSharp.Dom.IText textnode = (AngleSharp.Dom.IText)node;
                        System.Diagnostics.Debug.WriteLine(textnode.Text);
                    }
                    break;
                default:
                    {

                    }
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            //1. create blank form
            YourImplementation.DemoFormCreatorHelper.CreateReadyForm(
                InnerViewportKind.GLES,
                out _latestviewport, out _latest_formCanvas);

            _appHost = new AppHostWinForm(_latestviewport);

            _latest_formCanvas.FormClosed += (s1, e1) =>
            {
                //when owner form is disposed
                //we should clear our resource?

                _latest_formCanvas = null;
                _latestviewport = null;
            };
        }

        class Demo_UIHtmlBox : App
        {

            HtmlBox _htmlBox;
            string _htmltext;
            string _documentRootPath;
            AppHost _host;
            LayoutFarm.Composers.ExternalHtmlTreeWalker _externalHtmlTreeWalker;
            protected override void OnStart(AppHost host)
            {
                //html box
                _host = host;
                var loadingQueueMx = new LayoutFarm.ContentManagers.ImageLoadingQueueManager();
                loadingQueueMx.AskForImage += loadingQueue_AskForImg;

                LayoutFarm.HtmlBoxes.HtmlHost htmlHost = HtmlHostCreatorHelper.CreateHtmlHost(host,
                       (s, e) => loadingQueueMx.AddRequestImage(e.ImageBinder),
                       contentMx_LoadStyleSheet);

                //
                _htmlBox = new HtmlBox(htmlHost, 1024, 800);
                _htmlBox.SetLocation(0, 10); //test
                host.AddChild(_htmlBox);
                if (_externalHtmlTreeWalker != null)
                {
                    _htmlBox.LoadHtml(_externalHtmlTreeWalker);
                }
                else
                {
                    if (_htmltext == null)
                    {
                        _htmltext = @"<html><head></head><body>NOT FOUND!</body></html>";
                    }
                    _htmlBox.LoadHtmlString(_htmltext);
                }
            }


            void loadingQueue_AskForImg(object sender, LayoutFarm.ContentManagers.ImageRequestEventArgs e)
            {
                //load resource -- sync or async? 
                //if we enable cache in loadingQueue (default=> enable)
                //if the loading queue dose not have the req img 
                //then it will raise event to here

                //we can resolve the req image to specific img
                //eg. 
                //1. built -in img from control may has special protocol
                //2. check if the req want a local file
                //3. or if req want to download from the network
                //

                //examples ...

                string absolutePath = null;
                if (e.ImagSource.StartsWith("built_in://imgs/"))
                {
                    //substring
                    absolutePath = _documentRootPath + "\\" + e.ImagSource.Substring("built_in://imgs/".Length);
                }
                else
                {
                    absolutePath = _documentRootPath + "\\" + e.ImagSource;
                }

                if (!System.IO.File.Exists(absolutePath))
                {
                    return;
                }
                //load
                //lets host do img loading... 

                //we can do img resolve or caching here

                e.SetResultImage(_host.LoadImage(absolutePath));
            }
            void contentMx_LoadStyleSheet(object sender, LayoutFarm.ContentManagers.TextRequestEventArgs e)
            {
                string absolutePath = _documentRootPath + "\\" + e.Src;
                if (!System.IO.File.Exists(absolutePath))
                {
                    return;
                }
                //if found
                e.TextContent = System.IO.File.ReadAllText(absolutePath);
            }
            public void LoadHtml(string documentRootPath, string htmltext)
            {
                _documentRootPath = System.IO.Path.GetDirectoryName(documentRootPath);
                _htmltext = htmltext;
            }
            public void LoadHtml(string documentRootPath, LayoutFarm.Composers.ExternalHtmlTreeWalker externalHtmlTreeWalker)
            {
                _documentRootPath = System.IO.Path.GetDirectoryName(documentRootPath);
                _externalHtmlTreeWalker = externalHtmlTreeWalker;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //test simple html renderer
            if (_uiHtmlBoxApp != null) return;
            _uiHtmlBoxApp = new Demo_UIHtmlBox();
            //----------

            var parser = new HtmlParser();
            string htmlfile = @"D:\projects\HtmlRenderer\Source\Test8_HtmlRenderer.Demo\Samples\0_acid1_dev\00.htm";
            string htmlstr = File.ReadAllText(htmlfile);

            _uiHtmlBoxApp.LoadHtml(".", htmlstr);
            _appHost.StartApp(_uiHtmlBoxApp);
            _latestviewport.TopDownRecalculateContent();
            _latestviewport.PaintMe();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //2. create app host
            if (_uiHtmlBoxApp != null) return;
            _uiHtmlBoxApp = new Demo_UIHtmlBox();
            //----------

            var parser = new HtmlParser();
            string htmlfile = @"D:\projects\HtmlRenderer\Source\Test8_HtmlRenderer.Demo\Samples\0_acid1_dev\00.htm";
            string htmlstr = File.ReadAllText(htmlfile);
            AngleSharp.Html.Dom.IHtmlDocument htmldoc = parser.ParseDocument(htmlstr);

            //early test
            var angleSharpTreeWalker = new AngleSharpTreeWalker(htmldoc);
            //----------
            _uiHtmlBoxApp.LoadHtml(".", angleSharpTreeWalker);
            _appHost.StartApp(_uiHtmlBoxApp);
            _latestviewport.TopDownRecalculateContent();
            _latestviewport.PaintMe();
        }

        class AngleSharpTreeWalker : LayoutFarm.Composers.ExternalHtmlTreeWalker
        {
            AngleSharp.Html.Dom.IHtmlDocument _doc;
            public AngleSharpTreeWalker(AngleSharp.Html.Dom.IHtmlDocument doc)
            {
                _doc = doc;
            }
            public override IEnumerable<ExternalHtmlNode> GetHtmlNodeIter()
            {
                AngleSharpHtmlNodeCarrier nodeCarrier = new AngleSharpHtmlNodeCarrier();
                int currentLevel = 0;
                AngleSharp.Dom.INode currentNode = _doc;
                Stack<AngleSharp.Dom.INode> nodeStack = new Stack<AngleSharp.Dom.INode>();
            CHLD_LOOP:
                currentLevel = nodeStack.Count;
                if (currentNode != null)
                {
                    nodeCarrier.SetLevel(currentLevel);
                    nodeCarrier.SetHtmlNode(currentNode);
                    yield return nodeCarrier;

                    if (currentNode.NodeType == NodeType.Element)
                    {
                        AngleSharp.Html.Dom.IHtmlElement htmlElem = (AngleSharp.Html.Dom.IHtmlElement)currentNode;
                        foreach (AngleSharp.Dom.Attr attr in htmlElem.Attributes)
                        {
                            nodeCarrier.SetHtmlAttribute(attr);
                            yield return nodeCarrier;
                        }
                    }
                    if (currentNode.HasChildNodes)
                    {
                        //special 
                        nodeCarrier.SetStateEnterChildContext();
                        yield return nodeCarrier;
                        //------
                        nodeStack.Push(currentNode);
                        currentNode = currentNode.FirstChild;
                        goto CHLD_LOOP;
                    }
                    else
                    {
                        currentNode = currentNode.NextSibling;
                        goto CHLD_LOOP;
                    }
                }
                if (nodeStack.Count > 0)
                {
                    //special 
                    nodeCarrier.SetStateExitChildContext();
                    yield return nodeCarrier;

                    currentNode = nodeStack.Pop();
                    currentNode = currentNode.NextSibling;
                    goto CHLD_LOOP;
                }
            }

            class AngleSharpHtmlNodeCarrier : ExternalHtmlNode
            {
                string _currentElementName;
                AngleSharp.Dom.INode _currentNode;
                ExternalHtmlNodeKind _currentNodeKind;
                string _currentTextNodeContent;
                int _currentLevel;
                AngleSharp.Dom.Attr _currentAttr;
                public void SetLevel(int level)
                {
                    _currentLevel = level;
                }
                public void SetHtmlAttribute(AngleSharp.Dom.Attr attr)
                {
                    _currentNodeKind = ExternalHtmlNodeKind.Attribute;
                    _currentAttr = attr;
                }
                public override void GetAttributeNameAndValue(out string name, out string value)
                {
                    name = _currentAttr.Name.ToLower();
                    value = _currentAttr.Value;
                }
                public void SetStateEnterChildContext()
                {
                    _currentNodeKind = ExternalHtmlNodeKind.EnterChildContext;
                }
                public void SetStateExitChildContext()
                {
                    _currentNodeKind = ExternalHtmlNodeKind.ExitChildContext;

                }
                public void SetHtmlNode(AngleSharp.Dom.INode node)
                {
                    _currentAttr = null;
                    _currentNode = node;
                    switch (node.NodeType)
                    {
                        case NodeType.Text:
                            {
                                _currentElementName = null;
                                _currentNodeKind = ExternalHtmlNodeKind.TextNode;
                                AngleSharp.Dom.IText textnode = (AngleSharp.Dom.IText)_currentNode;
                                _currentTextNodeContent = textnode.TextContent;
                            }
                            break;
                        case NodeType.Document:
                            {
                                _currentElementName = null;
                                _currentNodeKind = ExternalHtmlNodeKind.Document;
                            }
                            break;
                        case NodeType.Element:
                            {
                                _currentNodeKind = ExternalHtmlNodeKind.Element;
                                AngleSharp.Html.Dom.IHtmlElement htmlElem = (AngleSharp.Html.Dom.IHtmlElement)_currentNode;
                                _currentElementName = htmlElem.NodeName.ToLower();
                                _currentTextNodeContent = null;
                            }
                            break;
                        default:
                            break;
                    }
                }
                public override object ActualHtmlNode => _currentNode;
                public override string HtmlElementName => _currentElementName;
                public override ExternalHtmlNodeKind HtmlNodeKind => _currentNodeKind;
                public override string CurrentTextNodeContent => _currentTextNodeContent;
                public override int Level => _currentLevel;
            }
        }


    }
}
