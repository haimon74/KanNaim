
using System.Collections.Generic;

namespace HaimDLL
{
    public class Constants
    {
        public const string My10InfoConnectionString =
            @"Data Source=sql02.intervision.co.il;Initial Catalog=10info;Persist Security Info=True;User ID=10info;Password=4a7RxLszj";

        public const string MyLocalMsSqlKanNaimConnectionString = 
            @"Data Source=PRIVATE-DCB1672;Initial Catalog=Kan-Naim;Integrated Security=True";

        public const string RemoteMySqlKanNaimConnectionString =
            @"Provider=SQLOLEDB;Data Source=PRIVATE-DCB1672;Integrated Security=SSPI;Initial Catalog=KanNaimOld";

        public const string WordSeperatorCharactersAsString = " ,.;:\t\n";


        public enum EnumRtfTags
        {

        }
        public enum EnumHtmlTags
        {
            // Basic Tags
            Html = 0,
            Head = 1,
            // Text Formatting Tags
            Bold,
            Underlined,
            Italic,
            Big,
            Small,
            SuperScript,
            SubScript,
            ShortQuotation,
            Definition,
            // Paragraphs Tags
            Preformatted, 
            Paragraph,
            OrderedList,
            UnorderedList,
            ListItem,
            LineBreak,
            HorizontalLine,
            BlockQuote,
            // Special defined styles
            Body,
            Header1,
            Header2,
            Header3,
            ComputerCode,
            Keyboard,
            Teletype,
            Address
        }
        public enum EnumHtmlTagsExtension
        {
            Title ,
            Meta ,
            Link ,
            Ancor , //hyperlink
            Image ,
            Embed ,
            Table ,
            TableHeader,
            TableRow,
            TableData // = table cell
        }
        public enum EnumCssProperties
        {
            FontSize,
            FontColor,
            BackgroundColor,
            FontFamily,
            TextAlign, // right, center, (justify)
            TextDirection,
            Position,
            FontWeight,
            FontStyle, //Italic / normal
            TextDecoration, //underline
            VerticalAlign,  // sup or sub
            
        }
        /*
        public static readonly IDictionary<string, string> HtmlStyleTagsCollection = 
            new Dictionary<string, string>()
                {
                    {EnumHtmlTags.Bold.ToString(), "b"},
                    {EnumHtmlTags.Underlined.ToString(), "u"},
                    {EnumHtmlTags.Italic.ToString(), "i"},
                    {EnumHtmlTags.Paragraph.ToString(), "p"},
                    {EnumCssProperties.FontSize.ToString(), "font-size"},
                    {EnumCssProperties.FontColor.ToString(), "font-color"},
                    {EnumCssProperties.BackgroundColor.ToString(), "background-color"},
                    {EnumCssProperties.FontFamily.ToString(), "font-family"},
                    {EnumCssProperties.TextAlign.ToString(), "text-align"},
                    {EnumCssProperties.TextDirection.ToString(), "text-direction"},
                    {EnumCssProperties.Position.ToString(), "position"},
                    {EnumCssProperties.FontWeight.ToString(), "font-weight"},
                    {EnumCssProperties.FontStyle.ToString(), "font-style"}, // italic
                    {EnumCssProperties.TextDecoration.ToString(), "text-decoration"}, // underline
                    {EnumCssProperties.VerticalAlign.ToString(), "vertical-align"},
                    {EnumHtmlTags.SubScript.ToString(), "sub"},
                    {EnumHtmlTags.SuperScript.ToString(), "sup"},
                    {EnumCssProperties.Position.ToString(), "position-right"}, 
                    {EnumHtmlTags.UnorderedList.ToString(), "ul"},
                    {EnumHtmlTags.OrderedList.ToString(), "ol"},
                    {EnumHtmlTags.ListItem.ToString(), "li"},
                    {EnumHtmlTagsExtension.Ancor.ToString(), "a"},
                    {EnumHtmlTagsExtension.Image.ToString(), "img"},
                    {EnumHtmlTagsExtension.Table.ToString(), "table"},
                    {EnumHtmlTagsExtension.TableRow.ToString(), "tr"},
                    {EnumHtmlTagsExtension.TableData.ToString(), "td"},
                    {EnumHtmlTagsExtension.Embed.ToString(), "embed"}
                };
        */
        public static readonly IDictionary<string, string> HtmlStyleCollection =
            new Dictionary<string, string>()
                {
                    
                    {EnumHtmlTags.Paragraph.ToString(), "<span style='text-align:right; text-indent:50px'>"}, 
                    {EnumCssProperties.FontSize.ToString(), "<span style='font-size:{0}pt;'>"},
                    {EnumCssProperties.FontColor.ToString(), "<span style='color:{0};'>"},
                    {EnumCssProperties.BackgroundColor.ToString(), "<span style='background-color:{0};'>"},
                    {EnumCssProperties.FontFamily.ToString(), "<span style='font-family:{0};'>"},
                    {EnumCssProperties.TextAlign.ToString(), "<span style='text-align:justify;'>"},
                    {EnumCssProperties.TextDirection.ToString(), "<span style='text-direction;'>"},
                    {EnumCssProperties.Position.ToString(), "<span style='position:relative; right:{0}px;'>"},
                    {EnumCssProperties.FontWeight.ToString(), "<span style='font-weight:bold;'>"},
                    {EnumCssProperties.FontStyle.ToString(), "<span style='font-style:italic;'>"},
                    {EnumCssProperties.TextDecoration.ToString(), "<span style='text-decoration:underline;'>"},
                    {EnumCssProperties.VerticalAlign.ToString(), "<span style='vertical-align:{0};'>"},
                    {EnumHtmlTags.SubScript.ToString(), "sub"},
                    {EnumHtmlTags.SuperScript.ToString(), "sup"},
                    {EnumHtmlTags.UnorderedList.ToString(), "ul"},
                    {EnumHtmlTags.OrderedList.ToString(), "ol"},
                    {EnumHtmlTags.ListItem.ToString(), "li"},
                    {EnumHtmlTagsExtension.Ancor.ToString(), "<a charset='UTF-8' lang='he' href='{0}' target='_blank'>"},
                    {EnumHtmlTagsExtension.Image.ToString(), 
                        "<img style='position:static; text-align:justify; float:{0};' id='{1}' height='{2}' width='{3}' src='{4}' title='{5}' alt='{6}' />"},
                    // where the position from the top is selected by the number of paragraph chosing !!! - TODO
                    {EnumHtmlTagsExtension.Table.ToString(), "<table rules='all' cellpadding='3px' cellspacing='2px' width='auto' >"},
                    {EnumHtmlTagsExtension.TableRow.ToString(), "<tr>"},
                    {EnumHtmlTagsExtension.TableData.ToString(), "<td><pre>"},
                    {EnumHtmlTagsExtension.Embed.ToString(), "<embed>"}
                     
                    /*
                    {EnumHtmlTags.Paragraph.ToString(), "<span style='text-align:right; text-indent:50px'>"}, 
                    {EnumCssProperties.FontSize.ToString(), "<span style='font-size:12pt;'>"},
                    {EnumCssProperties.FontColor.ToString(), "<span style='color:blue;'>"},
                    {EnumCssProperties.BackgroundColor.ToString(), "<span style='background-color:yellow;'>"},
                    {EnumCssProperties.FontFamily.ToString(), "<span style='font-family:Arial';>"},
                    {EnumCssProperties.TextAlign.ToString(), "<span style='text-align:justify;'>"},
                    {EnumCssProperties.TextDirection.ToString(), "<span style='text-direction;'>"},
                    {EnumCssProperties.Position.ToString(), "<span style='position:relative; right:12px;'>"},
                    {EnumCssProperties.FontWeight.ToString(), "<span style='font-weight:bold;'>"},
                    {EnumCssProperties.FontStyle.ToString(), "<span style='font-style:italic;'>"},
                    {EnumCssProperties.TextDecoration.ToString(), "<span style='text-decoration:underline;'>"},
                    {EnumCssProperties.VerticalAlign.ToString(), "<span style='vertical-align:center;'>"},
                    {EnumHtmlTags.SubScript.ToString(), "sub"},
                    {EnumHtmlTags.SuperScript.ToString(), "sup"},
                    {EnumHtmlTags.UnorderedList.ToString(), "ul"},
                    {EnumHtmlTags.OrderedList.ToString(), "ol"},
                    {EnumHtmlTags.ListItem.ToString(), "li"},
                    {EnumHtmlTagsExtension.Ancor.ToString(), "<a charset='UTF-8' lang='he' href='{0}' target='_blank'>"},
                    {EnumHtmlTagsExtension.Image.ToString(), 
                        "<img style='position:static; text-align:justify; float:{0};' id='{1}' height='{2}' width='{3}' src='{4}' title='{5}' alt='{6}' />"},
                    // where the position from the top is selected by the number of paragraph chosing !!! - TODO
                    {EnumHtmlTagsExtension.Table.ToString(), "<table rules='all' cellpadding='3px' cellspacing='2px' width='auto' >"},
                    {EnumHtmlTagsExtension.TableRow.ToString(), "<tr>"},
                    {EnumHtmlTagsExtension.TableData.ToString(), "<td><pre>"},
                    {EnumHtmlTagsExtension.Embed.ToString(), "<embed>"}
                     */
                };
        /*
        public static readonly string[] HtmlTagKeys = {
                                                      "b", //0
                                                      "u", //1
                                                      "i", //2
                                                      "p", //3
                                                      "font_size", //4
                                                      "font_color", //5
                                                      "background_color", //6
                                                      "font_family", //7
                                                      "text_alignment", //8
                                                      "sub", //9
                                                      "sup", //10
                                                      "identation", //11
                                                      "a", //12
                                                      "text_direction", //13
                                                      "img", //14
                                                      "embed", //15
                                                      "table", //16
                                                      "ul", //17
                                                      "ol", //18
                                                      "li", //19                                                 
                                                      "th", //20
                                                      "tr", //21
                                                      "td"  //22
                                                  };
         */
        public static readonly string[] RtfTagsUntags = {
                                                      "\\b \\b0",
                                                      "\\ul ul0",
                                                      "\\i \\i0",
                                                      "\\sub \\sub0",
                                                      "\\super \\super0"
                                                  };
        
        public static readonly string[] RtfTags = {
                                                      "", 
                                                      "" , 
                                                      ""
                                                  };

        public static readonly string[] HtmlTags = {
                                                      "", 
                                                      "" , 
                                                      ""
                                                  };

    }
}
