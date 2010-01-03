using System;
using System.Windows.Forms;


namespace Kan_Naim_Main
{
    public partial class FormEditRichTextBoxArticle : Form
    {
        public FormEditRichTextBoxArticle()
        {
            InitializeComponent();
        }

        private void FormEditRichTextBoxArticle_Load(object sender, EventArgs e)
        {
            richTextBox1.Text =
                "מוקדם יותר דווח כי ישראל דורשת לגרש 120 מתוך מאות האסירים האמורים להשתחרר לעסקה, ולא לאפשר להם לחזור לבתיהם בגדה המערבית - וזאת בניגוד לעמדות שהביע בעבר צוות המשא ומתן של חמאס. עם זאת, מקורב לשר הביטחון העריך אתמול כי \"ההצעה שעברה לחמאס טובה\"." +
                "אנשי חמאס והמתווך נועדו במשך שלוש שעות וחצי\n" +
                "פגישתו של המתווך הגרמני וחברי צוות המשא ומתן הפלסטיני, שבראשו בכיר הארגון מחמוד א-זהאר, נערכה הבוקר ונמשכה שלוש שעות וחצי. מקורות בחמאס מסרו כי במהלך הפגישה העביר המתווך לחברי צוות המשא ומתן את תשובתה של ישראל בנוגע לעסקה ודן עמם בתכניה.\n\n" +
                "ההצעה הישראלית הועברה למתווך הגרמני אתמול בלילה, עם סיום דיוני פורום \"השביעייה\". גורמים החברים בפורום העידו כי הצליחו להתגבר על רוב חילוקי הדעות ביניהם, צמצמו פערים מול תביעת חמאס וקיבלו את רוב הסתייגויותיו של ראש השב\"כ יובל דיסקין. \"בסופו של דבר\", אמר גורם מדיני בכיר, \"ההצעה" +
                "שגובשה הייתה על דעת רוב חברי השביעייה\".\n\n" + "\n\n  "+ HaimDLL.Constants.WordSeperatorCharactersAsString + "   \n\n" +
                "המחלוקת כעת מול חמאס היא על שמות האסירים שישראל פסלה את שחרורם\", אמר גורם מדיני בכיר. \"באשר לאלו שהיא הסכימה לשחרר, הוויכוח הוא מי ישוחרר לעזה, מי לחו\"ל ומי לשטחי יהודה ושומרון\". עוד הוסיף הגורם כי הכדור עבר למעשה לידי חמאס. \"העברנו את הקווים האדומים, ועתה חמאס צריך להחליט אם זה מקובל עליו\", אמר. \"זה ייקח לפחות עוד סבב אחד של דיונים\". במילים אחרות, העסקה לשחרור שליט לא תיחתם בימים הקרובים והמצב הנוכחי עלול להימשך לפחות עד אמצע ינואר.\n\n" +
                "בשיחה שקיים אתמול עם nrg מעריב סירב בכיר חמאס מחמוד א-זהאר להתייחס לדיווחים לפיהם ישראל הציגה מספר הסתייגויות מדרישות החמאס. כמו כן הבהיר א-זהאר כי מבחינת חמאס ההצעה שהועברה לישראל היא ההצעה האחרונה.\n\n ";
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void ptToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItemFontBold_Click(object sender, EventArgs e)
        {
            var msg = String.Format("SelectedText: {0}\nSelectionAlignment: {1}\nSelectionBackColor: {2}\nSelectionColor: {3}\nSelectionBullet: {4}\n",
                richTextBox1.SelectedText, richTextBox1.SelectionAlignment, richTextBox1.SelectionBackColor, richTextBox1.SelectionColor, richTextBox1.SelectionBullet);
            msg += String.Format("SelectionFont: {0}\nSelectionRightIndent: {1}\nSelectionStart: {2}\nSelectionLength: {3}\nSelectionCharOffset: {4}\n\n",
                richTextBox1.SelectionFont, richTextBox1.SelectionRightIndent, richTextBox1.SelectionStart, richTextBox1.SelectionLength, richTextBox1.SelectionCharOffset);
            msg += String.Format("RTF: {0}\n\nSelectedRTF:{1}",richTextBox1.Rtf, richTextBox1.SelectedRtf);

            ReselectingAsFullWordsBeforeAndAfterSelection();
            
            richTextBox1.SelectedRtf.Insert(0, "\\b");
            //richTextBox1.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
            //richTextBox1.AutoWordSelection = false;
            //richTextBox1.AcceptsTab = true;            
            //richTextBox1.WordWrap = false;
            //richTextBox1.ZoomFactor = 1;
            richTextBox1.Text += msg;
            //MessageBox.Show(msg);
        }

        private void ReselectingAsFullWordsBeforeAndAfterSelection()
        {
            var wordSeperatorChars = HaimDLL.Constants.WordSeperatorCharactersAsString.ToCharArray();
            var length = richTextBox1.SelectionLength;
            var idxSelectionStart = richTextBox1.SelectionStart;
            richTextBox1.SelectionStart -= 10;
            richTextBox1.SelectionLength = 10;
            var words = richTextBox1.SelectedRtf.Split(wordSeperatorChars, StringSplitOptions.RemoveEmptyEntries);
            var lastWord = words[words.Length - 1];
            var offsetBefore = lastWord.Length;
            richTextBox1.SelectionStart = idxSelectionStart + length;
            richTextBox1.SelectionLength = 10;
            words = richTextBox1.SelectedRtf.Split(wordSeperatorChars, StringSplitOptions.RemoveEmptyEntries);
            var firstWord = words[0];
            var offsetAfter = firstWord.Length;
            richTextBox1.SelectionStart = idxSelectionStart - offsetBefore;
            richTextBox1.SelectionLength = length + offsetBefore + offsetAfter;
        }

        ////////////////////////////////////////
            //        MS-Word Processing
            ////////////////////////////////////////

            //Word.ApplicationClass wordApp = new Word.ApplicationClass();

        private void Func1()
        {
            Microsoft.Office.Interop.Word.ApplicationClass oWordApp = new Microsoft.Office.Interop.Word.ApplicationClass();
            Microsoft.Office.Interop.Word.Document oWordDoc = new Microsoft.Office.Interop.Word.Document();

            object fileName = (object)@"D:\Documents and Settings\Haim\Desktop\Kan-Naim\test.doc";
            //object fileName = (object)@"D:\Documents and Settings\Haim\Desktop\Kan-Naim\ArticleEditor.doc";
            object oMissing = System.Reflection.Missing.Value;

            oWordApp.Visible = true;
            oWordDoc = oWordApp.Documents.Open(ref fileName, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            oWordDoc.ActiveWindow.Selection.WholeStory();
            oWordDoc.ActiveWindow.Selection.Copy();

            IDataObject data = Clipboard.GetDataObject();

            if (data != null) richTextBox1.Text = data.GetData(DataFormats.Text).ToString();

            object oTrue = true;
            object oFalse = false;

            Object oTemplatePath = (object)@"D:\Documents and Settings\Haim\Desktop\Kan-Naim\ArticleTemplate.dotx";
            oWordDoc = oWordApp.Documents.Add(ref oTemplatePath, ref oMissing, ref oMissing, ref oTrue);

            Microsoft.Office.Tools.Word.RichTextContentControl richTextContentControl1;
            Microsoft.Office.Tools.Word.Document oWordTools = new Microsoft.Office.Tools.Word.Document();
            //oWordTools.Application.ActiveDocument.ContentControls = oWordApp;
            oWordTools.Paragraphs[1].Range.InsertParagraphBefore();
            oWordTools.Paragraphs[1].Range.Select();
            richTextContentControl1 = oWordTools.Controls.AddRichTextContentControl("richTextControl1");
            richTextContentControl1.PlaceholderText = "Enter your first name";

            //oWordDoc.ContentControls.get_Item(

        }

        private void ToolStripMenuItemFontItalic_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItemFontUnderlined_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItemFontSup_Click(object sender, EventArgs e)
        {

        }

        private void fontBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void browserPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
