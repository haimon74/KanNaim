using System;
using System.Windows.Forms;

namespace Kan_Naim_Main
{
    public partial class FormArticleRichTextBoxEditor : Form
    {
        public delegate void ReturnHtmlTextCallbackType(string htmlContent);
        private static ReturnHtmlTextCallbackType _funcName;

        public FormArticleRichTextBoxEditor(string rtfText, ReturnHtmlTextCallbackType funcName)
        {
            InitializeComponent();
            
            if (rtfText != "")
                userControlRichTextBoxEditor1.RichTextBox1.Rtf = rtfText;

            _funcName = new ReturnHtmlTextCallbackType(funcName);
        }
    
        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            userControlRichTextBoxEditor1.RichTextBox1.Text =
                "מוקדם יותר דווח כי ישראל דורשת לגרש 120 מתוך מאות האסירים האמורים להשתחרר לעסקה, ולא לאפשר להם לחזור לבתיהם בגדה המערבית - וזאת בניגוד לעמדות שהביע בעבר צוות המשא ומתן של חמאס. עם זאת, מקורב לשר הביטחון העריך אתמול כי \"ההצעה שעברה לחמאס טובה\"." +
                "אנשי חמאס והמתווך נועדו במשך שלוש שעות וחצי\n" +
                "פגישתו של המתווך הגרמני וחברי צוות המשא ומתן הפלסטיני, שבראשו בכיר הארגון מחמוד א-זהאר, נערכה הבוקר ונמשכה שלוש שעות וחצי. מקורות בחמאס מסרו כי במהלך הפגישה העביר המתווך לחברי צוות המשא ומתן את תשובתה של ישראל בנוגע לעסקה ודן עמם בתכניה.\n\n" +
                "ההצעה הישראלית הועברה למתווך הגרמני אתמול בלילה, עם סיום דיוני פורום \"השביעייה\". גורמים החברים בפורום העידו כי הצליחו להתגבר על רוב חילוקי הדעות ביניהם, צמצמו פערים מול תביעת חמאס וקיבלו את רוב הסתייגויותיו של ראש השב\"כ יובל דיסקין. \"בסופו של דבר\", אמר גורם מדיני בכיר, \"ההצעה" +
                "שגובשה הייתה על דעת רוב חברי השביעייה\".\n\n" + "\n\n  " + HaimDLL.Constants.WordSeperatorCharactersAsString + "   \n\n" +
                "המחלוקת כעת מול חמאס היא על שמות האסירים שישראל פסלה את שחרורם\", אמר גורם מדיני בכיר. \"באשר לאלו שהיא הסכימה לשחרר, הוויכוח הוא מי ישוחרר לעזה, מי לחו\"ל ומי לשטחי יהודה ושומרון\". עוד הוסיף הגורם כי הכדור עבר למעשה לידי חמאס. \"העברנו את הקווים האדומים, ועתה חמאס צריך להחליט אם זה מקובל עליו\", אמר. \"זה ייקח לפחות עוד סבב אחד של דיונים\". במילים אחרות, העסקה לשחרור שליט לא תיחתם בימים הקרובים והמצב הנוכחי עלול להימשך לפחות עד אמצע ינואר.\n\n" +
                "בשיחה שקיים אתמול עם nrg מעריב סירב בכיר חמאס מחמוד א-זהאר להתייחס לדיווחים לפיהם ישראל הציגה מספר הסתייגויות מדרישות החמאס. כמו כן הבהיר א-זהאר כי מבחינת חמאס ההצעה שהועברה לישראל היא ההצעה האחרונה.\n\n "; 
            */
        }

        private void userControlRichTextBoxEditor1_Load(object sender, EventArgs e)
        {
            if (userControlRichTextBoxEditor1.RichTextBox1.Text == "")
            {
                userControlRichTextBoxEditor1.RichTextBox1.Rtf =
@"{\rtf1\fbidis\ansi\ansicpg1252\deff0\deflang1033\deflangfe1033
{
\fonttbl
{\f0\fswiss\fprq2\fcharset0 Arial;}
{\f1\fswiss\fprq2\fcharset0 Microsoft Sans Serif;}
{\f2\fswiss\fprq2\fcharset0 Tahoma;}
{\f3\froman\fprq2\fcharset0 Times New Roman;}
{\f4\fswiss\fprq2\fcharset0 Verdana;}}
{
\colortbl ;\red0\green0\blue128;\red255\green255\blue0;
\red0\green255\blue255;\red255\green0\blue0;\red0\green255\blue0;
\red0\green0\blue255;\red128\green128\blue128;
}
\viewkind4\uc1\pard\rtlpar\nowidctlpar\qr\f0\fs20\par\pard\ltrpar\nowidctlpar\cf1\b\f1 Microsoft \cf2 Sans \cf3 Serif\cf0\b0\par
\f2  Tahoma\par
\cf4\f3 Times \cf5 New \cf6 Roman\cf0\par
\cf7\f4 Verdana\cf0\par
\par
}";
            }
        }

        private void FormArticleRichTextBoxEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            _funcName(userControlRichTextBoxEditor1.RichTextBox1.Rtf);
        }
    }
}
