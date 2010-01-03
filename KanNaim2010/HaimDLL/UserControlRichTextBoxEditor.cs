using System;       
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections;
using Kan_Naim_Main;

namespace HaimDLL
{
    public partial class UserControlRichTextBoxEditor : UserControl
    {
        
        class RtfStyle
        {
            public class Colors
            {
                public Color ForeColor;
                public Color BackgroundColor;
            }

            private HorizontalAlignment _itsAlignment;
            private int _itsIndent;
            private Font _itsFont;
            private Color _itsColor;
            private bool _itsBullet;
            private Color _itsBackColor;
            private readonly Colors _itsFontColors;
            
            public RtfStyle() { _itsFontColors = new Colors(); }

            public RtfStyle(HorizontalAlignment itsAlignment, int itsIndent, 
                Font itsFont, Color itsColor, bool itsBullet, Color itsBackColor)
            {
                ItsAlignment = itsAlignment;
                ItsIndent = itsIndent;
                ItsFont = itsFont;
                ItsColor = itsColor;
                ItsBullet = itsBullet;
                ItsBackColor = itsBackColor;
                _itsFontColors = new Colors();
                ItsFontColors.ForeColor = ItsColor;
                ItsFontColors.BackgroundColor = itsBackColor;
            }

            public HorizontalAlignment ItsAlignment
            {
                get { return _itsAlignment; }
                set { _itsAlignment = value; }
            }
            public int ItsIndent
            {
                get { return _itsIndent; }
                set { _itsIndent = value; }
            }
            public Font ItsFont
            {
                get { return _itsFont; }
                set { _itsFont = value; }
            }
            public Color ItsColor
            {
                get { return _itsColor; }
                set { _itsColor = value; }
            }
            public bool ItsBullet
            {
                get { return _itsBullet; }
                set { _itsBullet = value; }
            }
            public Color ItsBackColor
            {
                get { return _itsBackColor; }
                set { _itsBackColor = value; }
            }
            public Colors ItsFontColors
            {
                get { return _itsFontColors; }
            }
                       

            public bool IsEqual(RtfStyle checkedObject)
            {
                return ( //TODO - fix all below
                           (ItsAlignment == checkedObject.ItsAlignment) &&
                           (ItsBackColor == checkedObject.ItsBackColor) &&
                           (ItsBullet == checkedObject.ItsBullet) &&
                           (ItsColor == checkedObject.ItsColor) &&
                           //(ItsFont == checkedObject.ItsFont) &&
                           (ItsFont.Bold == checkedObject.ItsFont.Bold) &&
                           (ItsFont.FontFamily.Name == checkedObject.ItsFont.FontFamily.Name) &&
                           (ItsFont.Italic == checkedObject.ItsFont.Italic) &&
                           (ItsFont.Size == checkedObject.ItsFont.Size) &&
                           (ItsFont.Underline == checkedObject.ItsFont.Underline) &&
                           //(ItsFontColors == checkedObject.ItsFontColors) &&
                           (ItsIndent == checkedObject.ItsIndent) 
                       );
            }

/*
            private static string FontPropertiesToString(Font font)
            {
                return String.Format("{0}:{1};{2}:{3};{4}:{5};{6}:{7};{8}:{9}",
                    Constants.EnumCssProperties.FontSize, font.Size,
                    Constants.EnumCssProperties.FontFamily, font.FontFamily,
                    Constants.EnumHtmlTags.Bold, font.Bold,
                    Constants.EnumHtmlTags.Italic, font.Italic,
                    Constants.EnumHtmlTags.Underlined, font.Underline);
            }
*/

            public static string ConvertAttributeValueToString(object theAttribute)
            {
                Type type = theAttribute.GetType();
                string returnValue = "";

                switch (type.Name)
                {
                    case "HorizontalAlignment":
                        returnValue = ((HorizontalAlignment)theAttribute).ToString();
                        break;
                    case "int":
                        returnValue = ((int)theAttribute).ToString();
                        break;
                    case "Color":
                        returnValue = ((Color)theAttribute).Name;
                        break;
                    case "bool":
                        returnValue = ((bool)theAttribute).ToString();
                        break;
                    case "FontFamily":
                        returnValue = ((FontFamily)theAttribute).Name;
                        break;
                    case "float":
                        returnValue = ((float)theAttribute).ToString();
                        break;
                    case "Colors":
                        var colors = new Colors
                                         {
                                             ForeColor = ((Colors) theAttribute).ForeColor,
                                             BackgroundColor = ((Colors) theAttribute).BackgroundColor
                                         };
                        returnValue = String.Format("{0}:{1};{2}:{3};", 
                            Constants.EnumCssProperties.FontColor, colors.ForeColor, 
                            Constants.EnumCssProperties.BackgroundColor, colors.BackgroundColor);
                        break;
                }
                return returnValue;
            }

            public static bool AttributeIsEqual(object theCurrentAttribute, RtfStyle theNextRtfCharStyle)
            {
                bool returnValue = false;

                try
                {

                    if (theCurrentAttribute == null) throw new ArgumentNullException("theCurrentAttribute");
                    if (theNextRtfCharStyle == null) throw new ArgumentNullException("theNextRtfCharStyle");

                    Type type = theCurrentAttribute.GetType();

                    string currentValue;
                    string nextValue;

                    switch (type.Name)
                    {
                        case "HorizontalAlignment":
                            returnValue = (theNextRtfCharStyle.ItsAlignment.CompareTo(theCurrentAttribute) == 0);
                            break;
                        case "int":
                            returnValue = (theNextRtfCharStyle.ItsIndent.CompareTo(theCurrentAttribute) == 0);
                            break;
                        case "Font":
                            currentValue = ConvertAttributeValueToString(theCurrentAttribute);
                            nextValue = ConvertAttributeValueToString(theNextRtfCharStyle.ItsFont);
                            returnValue = (currentValue == nextValue);
                            break;
                        case "FontFamily":
                            currentValue = ConvertAttributeValueToString(theCurrentAttribute);
                            nextValue = ConvertAttributeValueToString(theNextRtfCharStyle.ItsFont.FontFamily.Name);
                            returnValue = (currentValue == nextValue);
                            break;
                        case "Color":
                            currentValue = ((Color)theCurrentAttribute).ToString();
                            nextValue = theNextRtfCharStyle.ItsColor.ToString();
                            returnValue = (currentValue == nextValue);
                            break;
                        case "bool":
                            returnValue = (theNextRtfCharStyle.ItsFont.Bold == (bool)theCurrentAttribute);
                            break;
                        case "Colors":
                            currentValue = ConvertAttributeValueToString(theCurrentAttribute);
                            nextValue = ConvertAttributeValueToString(theNextRtfCharStyle.ItsFontColors);
                            returnValue = (currentValue == nextValue);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception exception)
                {

                    Messages.ExceptionMessage(exception);
                }

                return returnValue;
            }
        }

        private static readonly IDictionary RtfTagsUntagsCollection = new Dictionary<string,string>();
        private static readonly IDictionary FontFamilyByToolStripKeyCollection = new Dictionary<string, string>();

        private static readonly IDictionary<string, IDictionary<string ,string>> HtmlTagsByLocations = new Dictionary<string, IDictionary<string, string>>();

        public UserControlRichTextBoxEditor()
        {
            InitializeComponent();
            
            RtfTagsUntagsCollection[_toolStripMenuItemFontBold.Name] = Constants.RtfTagsUntags[0];
            RtfTagsUntagsCollection[_toolStripMenuItemFontUnderlined.Name] = Constants.RtfTagsUntags[1];
            RtfTagsUntagsCollection[_toolStripMenuItemFontItalic.Name] = Constants.RtfTagsUntags[2];
            RtfTagsUntagsCollection[_toolStripMenuItemFontSub.Name] = Constants.RtfTagsUntags[3];
            RtfTagsUntagsCollection[_toolStripMenuItemFontSup.Name] = Constants.RtfTagsUntags[4];

            FontFamilyByToolStripKeyCollection[_fontArialToolStripMenuItem.Name] = "Arial";
            FontFamilyByToolStripKeyCollection[_fontTahomaToolStripMenuItem.Name] = "Tahoma";
            FontFamilyByToolStripKeyCollection[_fontVerdanaToolStripMenuItem.Name] = "Verdana";
            FontFamilyByToolStripKeyCollection[_fontMiriamToolStripMenuItem.Name] = "Miriam";
            FontFamilyByToolStripKeyCollection[_fontLevenimToolStripMenuItem.Name] = "Levenim";
            FontFamilyByToolStripKeyCollection[_fontLucidaToolStripMenuItem.Name] = "Lucida";
            FontFamilyByToolStripKeyCollection[_fontImpactToolStripMenuItem.Name] = "Impact";
            FontFamilyByToolStripKeyCollection[_fontDavidToolStripMenuItem.Name] = "David";
            FontFamilyByToolStripKeyCollection[_fontWingdingsToolStripMenuItem.Name] = "Wingdings";
            FontFamilyByToolStripKeyCollection[_fontCourierToolStripMenuItem.Name] = "Courier New";
            FontFamilyByToolStripKeyCollection[_fontTimesNewRomanToolStripMenuItem.Name] = "Times New Roman";

            //foreach (string key in Constants.HtmlStyleTagsCollection.Keys)
            const string key = "tag";
            {
                HtmlTagsByLocations[key + "_start"] = new Dictionary<string, string>();
                HtmlTagsByLocations[key + "_end"] = new Dictionary<string, string>();
            }
            
        }

        private void ResolveStartEndTags4FontFormat(object sender, bool state, out string startTag, out string endTag)
        {
            var tagUntag = (string)RtfTagsUntagsCollection[((ToolStripMenuItem)sender).Name];
            string[] tagUntagArray = tagUntag.Split(' ');
            string tag = tagUntagArray[0].TrimEnd();
            string untag = tagUntagArray[1];
            
            int savedOriginalSelectionLength = RichTextBox1.SelectionLength;

            RichTextBox1.SelectionLength = 1;

            if (state)
            {
                startTag = untag;
                endTag = tag;
            }
            else
            {
                startTag = tag;
                endTag = untag;
            }

            RichTextBox1.SelectionLength = savedOriginalSelectionLength;
        }

        private int GetRtfSelectionLength()
        {
            int savedOriginalSelectionStart = RichTextBox1.SelectionStart;
            int savedOriginalSelectionLength = RichTextBox1.SelectionLength;

            RichTextBox1.SelectionStart = 0;
            RichTextBox1.SelectionLength += (savedOriginalSelectionStart + 1);
            int length1 = RichTextBox1.SelectedRtf.Length;

            RichTextBox1.SelectionStart = 0;
            RichTextBox1.SelectionLength = savedOriginalSelectionStart + 1;
            int length2 = RichTextBox1.SelectedRtf.Length;

            int returnValue = length1 - length2;

            RichTextBox1.SelectionStart = savedOriginalSelectionStart;
            RichTextBox1.SelectionLength = savedOriginalSelectionLength;

            return returnValue;
        }

        private void ReselectingAsFullWordsBeforeAndAfterSelection()
        { // TODO: fix bug - not all reselections working well
            var wordSeperatorChars = Constants.WordSeperatorCharactersAsString.ToCharArray();
            var length = RichTextBox1.SelectionLength;
            var idxSelectionStart = RichTextBox1.SelectionStart;

            int offsetBefore = 0;
            int offsetAfter = 0;

            RichTextBox1.SelectionStart = idxSelectionStart - 1;
            RichTextBox1.SelectionLength = 2;

            int testIdxBefore = Constants.WordSeperatorCharactersAsString
                .IndexOfAny(RichTextBox1.SelectedText.ToCharArray());

            if (testIdxBefore < 0)
            {
                RichTextBox1.SelectionStart -= 9;
                RichTextBox1.SelectionLength += 9;
                string beforeSelectedText = RichTextBox1.SelectedText;
                var words = beforeSelectedText.Split(wordSeperatorChars, StringSplitOptions.None);
                var lastWord = words[words.Length - 1];
                offsetBefore = lastWord.Length;
            }

            RichTextBox1.SelectionStart = idxSelectionStart + length - 1;
            RichTextBox1.SelectionLength = 2;

            int testIdxAfter = Constants.WordSeperatorCharactersAsString
                .IndexOfAny(RichTextBox1.SelectedText.ToCharArray());

            if (testIdxAfter < 0)
            {
                //RichTextBox1.SelectionStart = idxSelectionStart + length;
                RichTextBox1.SelectionLength = 11;
                string afterSelectedText = RichTextBox1.SelectedText;
                var words = afterSelectedText.Split(wordSeperatorChars, StringSplitOptions.None);
                var firstWord = words[0];
                offsetAfter = firstWord.Length;
            }
            RichTextBox1.SelectionStart = idxSelectionStart - offsetBefore;
            RichTextBox1.SelectionLength = length + offsetBefore + offsetAfter;
        }

/*
        private bool StringStartWithRtfTag(string rtf)
        {
            // return RtfTags.Any(rtfTag => rtf.StartsWith(rtfTag)); -- equal too
            return RtfTags.Any(rtf.StartsWith);
        }
*/

        private int GetRtfSelectionStartFromTextSelectionStart()
        {
            int savedOriginalSelectionStart = RichTextBox1.SelectionStart;
            int savedOriginalSelectionLength = RichTextBox1.SelectionLength;
            string savedOriginalSelectedText = RichTextBox1.SelectedText;

            RichTextBox1.SelectionStart = 0;
            RichTextBox1.SelectionLength = savedOriginalSelectionStart + 1;

            int rtfLength = RichTextBox1.SelectedRtf.Length;

            int returnValue = RichTextBox1.Rtf.IndexOf(savedOriginalSelectedText, rtfLength);

            RichTextBox1.SelectionStart = savedOriginalSelectionStart;
            RichTextBox1.SelectionLength = savedOriginalSelectionLength;
            
            return returnValue;
        }

/*
        private int GetSelectedStartOnRtfText()
        {
            int returnValue = RichTextBox1.SelectionStart;
            var rtfSplitOnSpaces = RichTextBox1.Rtf.Split(' ', '}');


            if (rtfSplitOnSpaces.Length < 2)
                return returnValue;

            var args = new[] {RichTextBox1.SelectedRtf};
            var stringLocations = RichTextBox1.Rtf.Split(args, StringSplitOptions.None);

            if (stringLocations.Length <= 2)
                return stringLocations[0].Length - RichTextBox1.SelectedRtf.Length;

            var optionalStringArrayList = FilterArrayListAfterSelectedStartIndex(stringLocations);

            var count = optionalStringArrayList.Count;

            if (count == 1)
                returnValue = ((string) optionalStringArrayList[0]).Length - RichTextBox1.SelectedRtf.Length;
            else if (count == 0)
                returnValue = RichTextBox1.SelectionStart;
            else
            {
                int lastOffset = 0;
                int testValue = 0;
                int length = RichTextBox1.SelectedRtf.Length;
                foreach (string testedString in optionalStringArrayList)
                {
                    var convertedText = RemoveRtfTags(testedString.Substring(0, testedString.Length - length));

                    testValue += convertedText.Length - length;

                    if (testValue >= RichTextBox1.SelectionStart)
                    {
                        lastOffset = testedString.Length - length;
                        break;
                    }
                    //else
                    testValue += (RemoveRtfTags(testedString)).Length;
                    returnValue += testedString.Length;
                }
                returnValue += lastOffset;
            }
            return returnValue;
        }
*/

/*
        private static string[] SplitStringWhereStartWithChar(string theString, char[] args)
        {
            string[] tempValue = theString.Split(args);

            if (tempValue.Length <= 1)
                return null;

            var returnValue = new string[tempValue.Length];

            returnValue[0] = tempValue[0].Substring(0, tempValue[0].Length - 1);
            string shiftedCharAsString = tempValue[0].Substring(tempValue[0].Length - 2);

            for (int index = 1 ; index < tempValue.Length; index++)
            {
                returnValue[index] = shiftedCharAsString + tempValue[index].Substring(0, tempValue[0].Length - 1);
                shiftedCharAsString = tempValue[index].Substring(tempValue[index].Length - 2);
            }

            return returnValue;
        }
*/
        
/*
        private static string NormalizeRtfHexadecimalCodes(string hexStr)
        {
            string returnValue = hexStr;
            
            while (returnValue.IndexOf("\'") > 0)
            {
                var idx = returnValue.IndexOf("\'");
                returnValue.Insert(idx, " ");
                idx++;
                string toConvert = "";
                
                while (returnValue.Substring(idx).StartsWith("\'"))
                {
                    toConvert += returnValue.Substring(idx, 4);
                    idx += 4;
                }
                string convertedString = HexCharactersToString(toConvert);
                returnValue = returnValue.Replace(toConvert, convertedString);
            }
            return returnValue;
        }
*/

/*
        {
            const string hexChars = "0123456789abcdef";
        private static string HexCharactersToString(string hexString)
            hexString = hexString.Replace("\'", "");
            int idx = 0;
            string returnValue = "";

            while (idx < hexString.Length)
            {
                string hexMsb = hexString.Substring(idx++, 1);
                string hexLsb = hexString.Substring(idx++, 1);

                int intValue = 16*hexChars.IndexOf(hexMsb) + hexChars.IndexOf(hexLsb);
                returnValue += Convert.ToChar(intValue).ToString();
            }
            return returnValue;
        }
*/

/*
        private static string RemoveRtfTags(string rtfString)
        {
            string returnValue = "";

            rtfString = NormalizeRtfHexadecimalCodes(rtfString);

            rtfString.Replace("{", "");
            rtfString.Replace("}", "");

            char[] args = {' ','\t'};
            string[] substrings = SplitStringWhereStartWithChar(rtfString, args);

            if (substrings[0].StartsWith(" "))
                returnValue += substrings[0];

            for (int i = 1; i < substrings.Length; i++)
            {
                if (substrings[i - 1].Contains("\\"))
                {
                    returnValue += substrings[i].Substring(1);
                }
                else
                {
                    returnValue += substrings[i];
                }
            }

            return returnValue;
        }
*/

/*
        private ArrayList FilterArrayListAfterSelectedStartIndex(string[] strings)
        {
            var arrayList = new ArrayList();

            int length = 0;

            for (int index = 0; index < strings.Length; index++)
            {
                if (length < RichTextBox1.SelectionStart)
                {
                    length += strings[index].Length;
                }
                else
                {
                    arrayList.Add(strings[index]);
                }
            }
            return arrayList;
        }
*/

        private void InsertTableTemplateText(int columns, int rows)
        {
            try
            {
                if (RichTextBox1.SelectionLength == 0)
                {
                    throw new ArgumentNullException();
                }

                string rowsStr = "";
                string cellsStr = "";
                for (int i = 0; i < columns; i++)
                {
                    cellsStr += String.Format("| [  ...............  ] |");
                }
                for (int i = 0; i < rows; i++)
                {
                    rowsStr += String.Format(
                        "[LINE #{0}] ---------------------------------------------------------------------------------------------\\par" +
                        "                     [ {1} ]\\par",
                        i + 1,
                        cellsStr);
                }

                const string tableStartTag = "[TABLE START]";
                const string tableEndTag = "[TABLE END]";
                string templateStr = String.Format("\\pard\\par\\rtlch\\rtlpar\\qr {0}\\par\\par{1}\\par{2}\\par", tableStartTag, rowsStr, tableEndTag);
                const string lineStr = "\\par ====================================================================\\par";
                templateStr = lineStr + templateStr + lineStr + "\\pard";
                int startIdx = GetRtfSelectionStartFromTextSelectionStart();
                string content = RichTextBox1.Rtf;
                RichTextBox1.Rtf = content.Insert(startIdx, templateStr);

                string txt = RichTextBox1.Text;
                string[] protectedTexts = { "[", "|", "]", "LINE", tableStartTag, tableEndTag};

                foreach (string protectedText in protectedTexts)
                {
                    RichTextBox1.SelectionStart = 0;

                    while (txt.Substring(++RichTextBox1.SelectionStart).Contains(protectedText))
                    {
                        RichTextBox1.SelectionStart = RichTextBox1.Text.IndexOf(protectedText, RichTextBox1.SelectionStart);
                        RichTextBox1.SelectionLength = protectedText.Length;
                        RichTextBox1.SelectionProtected = true;
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                Messages.ExceptionMessage(ex, "על מנת להכניס טבלה עליך לבחור מקום על ידי העכבר", " ");
            }
            
        }
        private void InsertTableToolStripMenuItemClick(object sender, EventArgs e)
        {
            InsertTableTemplateText(4 , 15);
        }

        private void _toolStripMenuItemFontAttrib_Click(object sender, EventArgs e)
        {
        
        }

        //public delegate void Del<T1,T2,T3,T4>(T1 t1, T2 t2, T3 t3, T4 t4);
        //public delegate void Del<T1,T2>(T1 item1, T2 item2);
        //public void Notify2(int i, int j) { }
        //var d1 = new Del<int>(Notify1);
        //var d2 = new Del<int, int>(Notify2);

        private void RtfTagUntagEvent(string startTag, string endTag)
        {    
            ReselectingAsFullWordsBeforeAndAfterSelection();

            int rtfSelectionStart = GetRtfSelectionStartFromTextSelectionStart();
            int rtfSelectionLength = GetRtfSelectionLength();
            
            string newRtfContent = RichTextBox1.Rtf;
            newRtfContent = newRtfContent.Insert(rtfSelectionStart + rtfSelectionLength, endTag);
            newRtfContent = newRtfContent.Insert(rtfSelectionStart - 1, startTag);
            RichTextBox1.Rtf = newRtfContent;
        }
        
        private void ToolStripMenuItemFontBold_Click(object sender, EventArgs e)
        {
            //var msg = String.Format("SelectedText: {0}\nSelectionAlignment: {1}\nSelectionBackColor: {2}\nSelectionColor: {3}\nSelectionBullet: {4}\n",
            //    RichTextBox1.SelectedText, RichTextBox1.SelectionAlignment, RichTextBox1.SelectionBackColor, RichTextBox1.SelectionColor, RichTextBox1.SelectionBullet);
            //msg += String.Format("SelectionFont: {0}\nSelectionRightIndent: {1}\nSelectionStart: {2}\nSelectionLength: {3}\nSelectionCharOffset: {4}\n\n",
            //    RichTextBox1.SelectionFont, RichTextBox1.SelectionRightIndent, RichTextBox1.SelectionStart, RichTextBox1.SelectionLength, RichTextBox1.SelectionCharOffset);
            //msg += String.Format("RTF: {0}\n\nSelectedRTF:{1}", RichTextBox1.Rtf, RichTextBox1.SelectedRtf);

            bool state = RichTextBox1.SelectionFont.Bold;
            string startTag, endTag;
            ResolveStartEndTags4FontFormat(sender, state, out startTag, out endTag);
            RtfTagUntagEvent(startTag, endTag);
            
            //RichTextBox1.Text += msg;

            //MessageBox.Show(msg);
        }

        private void _toolStripMenuItemFontItalic_Click(object sender, EventArgs e)
        {
            bool state = RichTextBox1.SelectionFont.Italic;

            string startTag, endTag;
            ResolveStartEndTags4FontFormat(sender, state, out startTag, out endTag);
            RtfTagUntagEvent(startTag, endTag);
        }

        private void _toolStripMenuItemFontUnderlined_Click(object sender, EventArgs e)
        {
            bool state = RichTextBox1.SelectionFont.Underline;
            string startTag, endTag;
            ResolveStartEndTags4FontFormat(sender, state, out startTag, out endTag);
            RtfTagUntagEvent(startTag, endTag);
        }

        private void _toolStripMenuItemFontSup_Click(object sender, EventArgs e)
        {
            bool state = RichTextBox1.SelectionFont.Underline && RichTextBox1.SelectionFont.Italic && RichTextBox1.SelectionFont.Bold;
            string startTag, endTag;
            ResolveStartEndTags4FontFormat(sender, state, out startTag, out endTag);
            RtfTagUntagEvent(startTag, endTag);
        }

        private void _toolStripMenuItemFontSub_Click(object sender, EventArgs e)
        {
            bool state = RichTextBox1.SelectionFont.Underline && RichTextBox1.SelectionFont.Italic && RichTextBox1.SelectionFont.Bold;
            string startTag, endTag;
            ResolveStartEndTags4FontFormat(sender, state, out startTag, out endTag);
            RtfTagUntagEvent(startTag, endTag);
        }

        private void _toolStripMenuItemFontSize_Click(object sender, EventArgs e)
        {
            ReselectingAsFullWordsBeforeAndAfterSelection();

            Font font = GetSelectedFont();
            FontFamily fontFamily = font.FontFamily;
            
            int newSize;

            string name = ((ToolStripMenuItem)sender).Name;
            if (!int.TryParse(name.Substring(name.Length - 4, 2), out newSize))
            {
                if (!int.TryParse(name.Substring(name.Length - 3, 1), out newSize))
                {
                    Messages.ExceptionMessage(new ArgumentException(), " ", "function name structure incorrect");
                }
            }
            RichTextBox1.SelectionFont = new Font(fontFamily, newSize, font.Unit);

            //var size = font.Size;
            //var unit = font.Unit;
            //string startTag, endTag;
            //ResolveStartEndTags4FontSize(sender, size, out startTag, out endTag);
            //RtfTagUntagEvent(startTag, endTag);
        }

/*
        private static void ResolveStartEndTags4FontSize(object sender, float size, out string startTag, out string endTag)
        {
            string name = ((ToolStripMenuItem) sender).Name;
            int newSize;
            if (!int.TryParse(name.Substring(name.Length - 4 , 2) , out newSize))
            {
                if (!int.TryParse(name.Substring(name.Length - 3 , 1) , out newSize))
                { 
                    Messages.ExceptionMessage(new ArgumentException(), " " , "function name structure incorrect");
                }
            }

            if (size == newSize)
            {
                startTag = String.Format("\\fs{0}", 2 * size);
                endTag = startTag;
            }
            else
            {
                startTag = String.Format("\\fs{0}", 2 * newSize);
                endTag = String.Format("\\fs{0}", 2 * size);
            }
            
        }
*/

        private void _colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var senderName = ((ToolStripMenuItem)sender).Name;

            bool isBackgroundColor = senderName.Contains("Bg");
            bool isForeColor = senderName.Contains("Color");

            if (isBackgroundColor)
                senderName = senderName.Replace("Bg", "");
            
            if (isForeColor)
                senderName = senderName.Replace("Color", "");

            const string startStr = "_font";
            const string endStr = "ToolStripMenuItem";

            if (!((senderName.StartsWith(startStr)) && (senderName.EndsWith(endStr))))
            {
                Messages.ExceptionMessage(new AccessViolationException(),
                                          "couldnot retrieve the color name due to mismatch function name structure",
                                          " ");
                return;
            }

            string colorName = senderName.Replace(startStr, "").Replace(endStr, "");


            if (isBackgroundColor)
                RichTextBox1.SelectionBackColor = Color.FromName(colorName);
            
            if (isForeColor)
                RichTextBox1.SelectionColor = Color.FromName(colorName);

        }

        private Font GetSelectedFont()
        {
            int length = RichTextBox1.SelectionLength;
            RichTextBox1.SelectionLength = 1;
            Font font = RichTextBox1.SelectionFont;
            RichTextBox1.SelectionLength = length;
            return font;
        }

        private void _fontFamilyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReselectingAsFullWordsBeforeAndAfterSelection();
            var fontFamily = (string)FontFamilyByToolStripKeyCollection[((ToolStripMenuItem) sender).Name];
            Font font = GetSelectedFont();
            RichTextBox1.SelectionFont = new Font(fontFamily, font.Size, font.Unit);
        }

        private void _justifyRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox1.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void _justifyCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox1.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void _unOrderedListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool currentState = RichTextBox1.SelectionBullet;
            RichTextBox1.SelectionBullet = !currentState;
        }

        private void _increaseIdentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int currentValue = RichTextBox1.SelectionIndent;
            if (currentValue < 200)
            RichTextBox1.SelectionIndent += 50;
        }

        private void _decreaseIdentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int currentValue = RichTextBox1.SelectionIndent;
            if (currentValue > 0)
                RichTextBox1.SelectionIndent -= 50;
        }

        private void RetrieveCharAttributesByIndex(int currentIndex,
            ref RtfStyle previousRtfCharStyle,
            ref RtfStyle currentRtfCharStyle)
        {
            if (currentIndex > 0)
                previousRtfCharStyle = currentRtfCharStyle;

            RichTextBox1.SelectionStart = currentIndex;
            RichTextBox1.SelectionLength = 1;

            currentRtfCharStyle = new RtfStyle(
                RichTextBox1.SelectionAlignment,
                RichTextBox1.SelectionIndent,
                RichTextBox1.SelectionFont,
                RichTextBox1.SelectionColor,
                RichTextBox1.SelectionBullet,
                RichTextBox1.SelectionBackColor); 
        }

        public string GeneratBaseHtmlString(string content)
        {
            string returnValue = String.Format(
                "<html><head><basefont color=\"black\" size=\"12\" font=\"Arial\" />" + 
                "<base target=\"_blank\" /></head><body><bdo dir=\"rtl\"><pre>" +
                "<!------------------------------------------------------------------------> \n\n\n\n\n\n" +

                "{0}" +

                "<!------------------------------------------------------------------------> \n\n\n\n\n\n" +
                "</pre></bdo></body></html>", content);
            
            return returnValue;
        }


        public string ConvertRtfTextToHtmlText()
        {
            string content = RichTextBox1.Text;
            int currentIndex = 0;
            var currentRtfCharAttributes = new RtfStyle();
            var previousRtfCharAttributes = new RtfStyle();
            
            RetrieveCharAttributesByIndex(currentIndex, ref previousRtfCharAttributes, ref currentRtfCharAttributes);

            string returnValue = content;

            while (++currentIndex < content.Length)
            {
                RetrieveCharAttributesByIndex(currentIndex, ref previousRtfCharAttributes, ref currentRtfCharAttributes);

                bool styleChanged = (!currentRtfCharAttributes.IsEqual(previousRtfCharAttributes));
                
                if (styleChanged)
                {
                    InsertAllChangedStylesIntoCollection(currentRtfCharAttributes, previousRtfCharAttributes, currentIndex);
                }
            }

            int length = HtmlTagsByLocations["tag_start"].Keys.Count;
            
            while (length > 0)
            {
                returnValue = InsertTagToHtmlTextAndUpdateKey(returnValue);
                length = HtmlTagsByLocations["tag_start"].Keys.Count + HtmlTagsByLocations["tag_end"].Keys.Count;
            }

            returnValue = GeneratBaseHtmlString(returnValue);
            return returnValue;
        }

        private static string  InsertTagToHtmlTextAndUpdateKey(string htmlText)
        {
            var startKeyList = HtmlTagsByLocations["tag_start"].Keys.Select(keyStart => int.Parse(keyStart)).ToList();
            startKeyList.Sort();
            var endKeyList = HtmlTagsByLocations["tag_end"].Keys.Select(keyEnd => int.Parse(keyEnd)).ToList();
            endKeyList.Sort();

            string key, htmlStr;
            int idx;
            int startKey = int.MaxValue, endKey = int.MaxValue;

            if (startKeyList.Count > 0)
                startKey = startKeyList[0];

            if (endKeyList.Count > 0)
                endKey = endKeyList[0];

            if (startKey < endKey)
            {
                key = startKeyList[0].ToString();
                htmlStr = HtmlTagsByLocations["tag_start"][key];
                HtmlTagsByLocations["tag_start"].Remove(key);
                startKeyList.Remove(startKeyList[0]);
                idx = int.Parse(key);
            }
            else
            {
                key = endKeyList[0].ToString();
                htmlStr = HtmlTagsByLocations["tag_end"][key];
                HtmlTagsByLocations["tag_end"].Remove(key);
                endKeyList.Remove(endKeyList[0]);
                idx = int.Parse(key);
            }            
            string returnValue = htmlText.Insert(idx, htmlStr);

            int offset = htmlStr.Length;

            for (int i = (startKeyList.Count - 1) ; i >= 0; i--)
            {
                int oldKey = startKeyList[i];
                string keyStr = oldKey.ToString();
                string keepValue = HtmlTagsByLocations["tag_start"][keyStr];
                HtmlTagsByLocations["tag_start"].Remove(keyStr);
                string newKeyStr = (oldKey + offset).ToString();
                HtmlTagsByLocations["tag_start"][newKeyStr] = keepValue;
            }
            for (int i = (endKeyList.Count - 1) ; i >= 0; i--)
            {
                int oldKey = endKeyList[i];
                string keyStr = oldKey.ToString();
                string keepValue = HtmlTagsByLocations["tag_end"][keyStr];
                HtmlTagsByLocations["tag_end"].Remove(keyStr);
                string newKeyStr = (oldKey + offset).ToString();
                HtmlTagsByLocations["tag_end"][newKeyStr] = keepValue;
            }

            return returnValue;
        }

        private void InsertAllChangedStylesIntoCollection(RtfStyle currentRtfCharAttributes, RtfStyle previousRtfCharAttributes, int currentIndex)
        {
            if (currentRtfCharAttributes.ItsAlignment != previousRtfCharAttributes.ItsAlignment)
            {
                InsertSpanTagStyledToCollection(
                    Constants.EnumCssProperties.TextAlign,
                    currentIndex,
                    currentRtfCharAttributes.ItsAlignment);
            }
            if (currentRtfCharAttributes.ItsBackColor != previousRtfCharAttributes.ItsBackColor)
            {
                InsertSpanTagStyledToCollection(
                    Constants.EnumCssProperties.BackgroundColor,
                    currentIndex,
                    currentRtfCharAttributes.ItsAlignment);
            }
            if (currentRtfCharAttributes.ItsBullet != previousRtfCharAttributes.ItsBullet)
            {
                InsertSpanTagStyledToCollection(
                    Constants.EnumHtmlTags.UnorderedList,
                    currentIndex,
                    currentRtfCharAttributes.ItsBullet);
            }
            if (currentRtfCharAttributes.ItsColor != previousRtfCharAttributes.ItsColor)
            {
                InsertSpanTagStyledToCollection(
                    Constants.EnumCssProperties.FontColor,
                    currentIndex,
                    currentRtfCharAttributes.ItsColor);
            }
            if (currentRtfCharAttributes.ItsIndent != previousRtfCharAttributes.ItsIndent)
            {
                InsertSpanTagStyledToCollection(
                    Constants.EnumCssProperties.Position,
                    currentIndex,
                    currentRtfCharAttributes.ItsIndent);
            }
            if (currentRtfCharAttributes.ItsFont.Bold != previousRtfCharAttributes.ItsFont.Bold)
            {
                InsertSpanTagStyledToCollection(
                    Constants.EnumCssProperties.FontWeight,
                    currentIndex,
                    currentRtfCharAttributes.ItsFont.Bold);
            }
            if (currentRtfCharAttributes.ItsFont.Italic != previousRtfCharAttributes.ItsFont.Italic)
            {
                InsertSpanTagStyledToCollection(
                    Constants.EnumCssProperties.FontStyle,
                    currentIndex,
                    currentRtfCharAttributes.ItsFont.Italic);
            }
            if (currentRtfCharAttributes.ItsFont.Underline != previousRtfCharAttributes.ItsFont.Underline)
            {
                InsertSpanTagStyledToCollection(
                    Constants.EnumCssProperties.TextDecoration,
                    currentIndex,
                    currentRtfCharAttributes.ItsFont.Underline);
            }
            if (currentRtfCharAttributes.ItsFont.Size != previousRtfCharAttributes.ItsFont.Size)
            {
                InsertSpanTagStyledToCollection(
                    Constants.EnumCssProperties.FontSize,
                    currentIndex,
                    currentRtfCharAttributes.ItsFont.Size);
            }
            if (currentRtfCharAttributes.ItsFont.FontFamily != previousRtfCharAttributes.ItsFont.FontFamily)
            {
                InsertSpanTagStyledToCollection(
                    Constants.EnumCssProperties.FontFamily,
                    currentIndex,
                    currentRtfCharAttributes.ItsFont.FontFamily);
            }
        }

        private void InsertSpanTagStyledToCollection(Enum enumKey, int currentIndex, object theAttribute)
        {
            string theCurrentValue = RtfStyle.ConvertAttributeValueToString(theAttribute);
            const string tagName = "tag";
            //string startTagStr = String.Format("<span class='{0}_{1}'>", tagName, theCurrentValue);
            string startTagStr;
            if (Constants.HtmlStyleCollection[enumKey.ToString()].Contains("{0}"))
            {
                startTagStr = String.Format(Constants.HtmlStyleCollection[enumKey.ToString()], theCurrentValue);
            }
            else
            {
                startTagStr = Constants.HtmlStyleCollection[enumKey.ToString()];
            }
            InsertIndexAndHtmlTagToCollection(tagName + "_start", currentIndex.ToString(), startTagStr);

            int endTagIdx = currentIndex;
            endTagIdx++;
            while ((NextCharAttributeValueIsEqual(endTagIdx, theAttribute)) && (endTagIdx < RichTextBox1.Text.Length))
            {
                endTagIdx++;
            }
            
            string endTagStr = String.Format("<!--class='{0}_{1}'--></span>", tagName, theCurrentValue);
            InsertIndexAndHtmlTagToCollection(tagName + "_end", endTagIdx.ToString(), endTagStr);
        }

        private bool NextCharAttributeValueIsEqual(int nextTagIdx, object theCurrentAttribute)
        {
            RichTextBox1.SelectionStart = nextTagIdx;
            RichTextBox1.SelectionLength = 1;

            var nextRtfCharAttributes = new RtfStyle();
            var currentRtfCharAttributes = new RtfStyle();
            RetrieveCharAttributesByIndex(nextTagIdx, ref currentRtfCharAttributes, ref nextRtfCharAttributes);

            return RtfStyle.AttributeIsEqual(theCurrentAttribute, nextRtfCharAttributes);
        }

        private static void InsertIndexAndHtmlTagToCollection(string tagTypeName, string tagLocation, string tagString)
        {
            if ( ! HtmlTagsByLocations.ContainsKey(tagTypeName))
                return;

            if (HtmlTagsByLocations[tagTypeName].ContainsKey(tagLocation))
            {
                if (tagTypeName.EndsWith("_start"))
                {
                    HtmlTagsByLocations[tagTypeName][tagLocation] =
                        String.Format("{0} {1}", HtmlTagsByLocations[tagTypeName][tagLocation], tagString);
                }
                else
                {
                    HtmlTagsByLocations[tagTypeName][tagLocation] =
                        String.Format("{0} {1}", tagString, HtmlTagsByLocations[tagTypeName][tagLocation]);
                }
            }
            else
            {
                HtmlTagsByLocations[tagTypeName].Add(tagLocation, tagString);
            }
        }

        private void ToolStripMenuItem3Col10Row_Click(object sender, EventArgs e)
        {
            InsertTableTemplateText(3, 10);
        }

        private void ToolStripMenuItem5Col20Row_Click(object sender, EventArgs e)
        {
            InsertTableTemplateText(5, 20);
        }

        private void BrowserPreviewToolStripMenuItemClick(object sender, EventArgs e)
        {
            
            var browserAsForm = new FormPreviewArticleOnWebBrowser();
            var browser = browserAsForm.WebBrowser1;

            string htmlText = ConvertRtfTextToHtmlText();

            browser.DocumentText = htmlText;
            browserAsForm.Show();
        }

        private void RemoveImageOrVideoFromRtf(object sender)
        {
            string name = ((ToolStripMenuItem)sender).Name.ToLower();
            bool contains = ((name.Contains("image")) || name.Contains("img"));
            string objName = (contains) ? "image" : "video";

            string objLeftStr = String.Format("[{0}:left]", objName);
            string objRightStr = String.Format("[{0}:right]", objName); 
            string objStr = "";

            if ((RichTextBox1.Text.Contains(objLeftStr)))
            {
                objStr = objLeftStr;
            }
            if ((RichTextBox1.Text.Contains(objRightStr)))
            {
                objStr = objRightStr;
            }
            if (objStr.Length > 0)
            {
                RichTextBox1.SelectionStart = RichTextBox1.Text.IndexOf(objStr);
                RichTextBox1.SelectionLength = objStr.Length;
                RichTextBox1.SelectionProtected = false;
                RichTextBox1.Rtf = RichTextBox1.Rtf.Replace(objStr, "");
            }
        }

        private void AddImageOrVideoToRtf(object sender)
        {
            int idx = GetRtfSelectionStartFromTextSelectionStart();

            RemoveImageOrVideoFromRtf(sender);

            string name = ((ToolStripMenuItem) sender).Name.ToLower();
            bool contains = ((name.Contains("image")) || name.Contains("img"));
            string objName = (contains) ? "image" : "video";

            var floating = ((ToolStripMenuItem)sender).Name.Contains("Left") ? "left" : "right";
            string strToInsert = String.Format("\\par [{0}:{1}]", objName, floating);

            string rtf = RichTextBox1.Rtf;
            idx = rtf.IndexOf("\\par", idx);
            RichTextBox1.Rtf = rtf.Insert(idx, strToInsert);

            RichTextBox1.SelectionStart = RichTextBox1.Text.IndexOf("[" + objName + ":");
            RichTextBox1.SelectionLength = strToInsert.Length - 4;
            RichTextBox1.SelectionProtected = true;
        }

        private void _imageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddImageOrVideoToRtf(sender);
        }

        private void _imgFloatLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _imageToolStripMenuItem_Click(sender, e);
        }

        private void _imgFloatRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _imageToolStripMenuItem_Click(sender, e);
        }

        private void RemoveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveImageOrVideoFromRtf(sender);
        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void _videoFloatLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _videoToolStripMenuItem_Click(sender, e);
        }

        private void _videoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddImageOrVideoToRtf(sender);
        }

        private void _videoFloatRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _videoToolStripMenuItem_Click(sender, e);
        }

        private void RemoveVideoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveImageOrVideoFromRtf(sender);
        }

        private void RemoveAllProtectedTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox1.SelectionStart = 0;
            RichTextBox1.SelectionLength = RichTextBox1.Text.Length;
            RichTextBox1.SelectionProtected = false;

            string tableStart = "TABLE START";
            string tableEnd = "TABLE END";

            string rtf = RichTextBox1.Rtf;
            int idx1 = rtf.IndexOf(tableStart);
            int idx2 = rtf.IndexOf(tableEnd);

            if ((idx1 > 0) && (idx2 > 0))
            {
                RichTextBox1.Rtf = rtf.Substring(0, idx1 + 1) +
                                   rtf.Substring(idx2-1);
            }

            string[] removeStrArray = {"image:left", "image:right", "video:left", "video:right","TABLE", "START", "END", "[", "]", "=="};
            string txt = RichTextBox1.Text;

            foreach (string removeStr in removeStrArray)
            {
                RichTextBox1.Rtf = RichTextBox1.Rtf.Replace(removeStr, "");
            }
        }
    }
}
