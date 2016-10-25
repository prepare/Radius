using System;
using System.Windows.Forms;
using AngleSharp.Dom.Html;
using AngleSharp.Parser;
using AngleSharp.Parser.Html;

namespace TestSubCore2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string simpleHtml = @"
                <html>
                 <head></head>
                 <body>hello1</body>
                </html>            ";

            HtmlParser parser = new HtmlParser();
            IHtmlDocument htmldoc = parser.Parse(simpleHtml);


        }
    }
}
