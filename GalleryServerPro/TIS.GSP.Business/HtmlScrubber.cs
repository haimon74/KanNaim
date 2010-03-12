using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace GalleryServerPro.Business
{
  /// <summary>
  /// Validates that HTML does not contain unauthorized elements, attributes, and javascript.
  /// </summary>
  public class HtmlScrubber
  {
    #region Static Default

		/// <summary>
		/// Gets the allowed attributes that apply to all HTML tags.
		/// </summary>
		/// <returns>Gets the allowed attributes that apply to all HTML tags.</returns>
		static string DefaultAttributes()
		{
			return "class,style,id";
		}

		/// <summary>
		/// Gets the allowed HTML tags and their respective allowed attributes.
		/// </summary>
		/// <returns>Gets the allowed HTML tags and their respective allowed attributes.</returns>
    static NameValueCollection DefaultTags()
    {
      NameValueCollection defaultTags = new NameValueCollection();
      defaultTags.Add("h1", "align");
      defaultTags.Add("h2", "align");
      defaultTags.Add("h3", "align");
      defaultTags.Add("h4", "align");
      defaultTags.Add("h5", "align");
      defaultTags.Add("h6", "align");
      defaultTags.Add("strong", "");
      defaultTags.Add("em", "");
      defaultTags.Add("u", "");
      defaultTags.Add("b", "");
      defaultTags.Add("i", "");
      defaultTags.Add("strike", "");
      defaultTags.Add("sup", "");
      defaultTags.Add("sub", "");
      defaultTags.Add("font", "color,size,face");
      defaultTags.Add("blockquote", "dir");
      defaultTags.Add("ul", "");
      defaultTags.Add("ol", "");
      defaultTags.Add("li", "");
      defaultTags.Add("p", "align,dir");
      defaultTags.Add("address", "");
      defaultTags.Add("pre", "");
      defaultTags.Add("div", "align");
      defaultTags.Add("hr", "");
      defaultTags.Add("br", "");
      defaultTags.Add("a", "href,target,name,title");
      defaultTags.Add("span", "align");
      defaultTags.Add("img", "src,alt,title");

			// Add the default attributes that apply to all HTML tags.
			string defaultAttributes = DefaultAttributes();
			foreach (string key in defaultTags.AllKeys)
			{
				defaultTags.Add(key, defaultAttributes);
			}

      return defaultTags;
    }
    private NameValueCollection allowedTags = null;
    static Regex regex = new Regex("<[^>]+>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
    static Regex jsAttributeRegex = new Regex("javascript:", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
    static Regex xmlLineBreak = new Regex("&#x([DA9]|20|85|2028|0A|0D)(;)?", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
    static Regex filterdCharacters = new Regex("\\=|\\\"|\\'|\\s|\"'", RegexOptions.Compiled);
    static Regex validProtocols = new Regex("^((http(s)?|mailto|mms):|/)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    static Regex bannedChars = new Regex("\\s", RegexOptions.Compiled);
    #endregion

    #region Public Statics

		/// <summary>
		/// Clean potentially dangerous HTML and optionally remove javascript from the specified string. All
		/// tags and attributes are compared to a list of acceptable HTML tags and attributes. Attributes not
		/// in the white list are removed. If encodeExceptions=true, unrecognized tags are HTML encoded. If false,
		/// tags are removed (but not the inner text of the tag).
		/// </summary>
		/// <param name="stringToClean">The string to clean.</param>
		/// <param name="encodeExceptions">A value indicating whether to HTML-encode any HTML tags not present
		/// in the list of acceptable HTML tags. If false, unrecognized tags are removed, but the contents of
		/// the tag are preserved.</param>
		/// <param name="filterScripts">A value indicating whether to remove any embedded javascript. A true
		/// value indicates javacript is removed.</param>
		/// <returns>
		/// Returns a string with potentially dangerous HTML made safe.
		/// </returns>
		public static string Clean(string stringToClean, bool encodeExceptions, bool filterScripts)
    {
      if (string.IsNullOrEmpty(stringToClean))
        return stringToClean;

      HtmlScrubber f = new HtmlScrubber(stringToClean, encodeExceptions, filterScripts);
      return f.Clean();
    }

		/// <summary>
		/// Clean the potentially dangerous HTML so that unauthorized HTML and Javascript is removed. If the configuration
		/// setting allowHtmlInTitlesAndCaptions is true, then the input is "cleaned" so that all HTML tags that are not in
		/// a predefined list of acceptable HTML tags are HTML-encoded, and all attributes not found in the white list
		/// are removed (e.g. onclick, onmouseover). If allowHtmlInTitlesAndCaptions = "false", then all HTML tags are
		/// removed. Regardless of the configuration setting, all &lt;script&gt; tags are escaped and all instances of 
		/// "javascript:" are removed. 
		/// </summary>
		/// <param name="html">The string containing the potentially dangerous HTML tags.</param>
		/// <returns>Returns a string with potentially dangerous HTML tags HTML-encoded or removed.</returns>
		public static string SmartClean(string html)
		{
			string cleanHtml = String.Empty;

			if (GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.AllowHtmlInTitlesAndCaptions)
			{
				cleanHtml = HtmlScrubber.Clean(html, true, true);
			}
			else
			{
				cleanHtml = HtmlScrubber.RemoveAllTags(html);
			}

			return cleanHtml;
		}

    /// <summary>
    /// Remove all HTML tags from the specified string. If the overload with the escapeQuotes parameter is true, then all 
		/// apostrophes and quotation marks are replaced with &quot; and &apos; so that the string can be specified in HTML 
		/// attributes such as title tags. If the escapeQuotes parameter is not specified, no replacement is performed.
    /// </summary>
    /// <param name="html">The string containing HTML tags to remove.</param>
    /// <returns>Returns a string with all HTML tags removed, including the brackets.</returns>
    public static string RemoveAllTags(string html)
    {
			return RemoveAllTags(html, false);
    }

    /// <summary>
		/// Remove all HTML tags from the specified string. If the overload with the escapeQuotes parameter is true, then all 
		/// apostrophes and quotation marks are replaced with &quot; and &apos; so that the string can be specified in HTML 
		/// attributes such as title tags. If the escapeQuotes parameter is not specified, no replacement is performed.
		/// </summary>
    /// <param name="html">The string containing HTML tags to remove.</param>
		/// <param name="escapeQuotes">When true, all apostrophes and quotation marks are replaced with &quot; and &apos;.</param>
    /// <returns>Returns a string with all HTML tags removed, including the brackets.</returns>
    public static string RemoveAllTags(string html, bool escapeQuotes)
    {
      if (html == null || html.Trim() == string.Empty)
        return html;

      HtmlScrubber f = new HtmlScrubber(html);
      f.allowedTags.Clear();

			string cleanHtml = f.Clean();

			if (escapeQuotes)
			{
				cleanHtml = cleanHtml.Replace("\"", "&quot;");
				cleanHtml = cleanHtml.Replace("'", "&apos;");
			}

			return cleanHtml;
    }

    #endregion

    #region Private statics

		//private static string StripScriptTags(string text)
		//{
		//  return jsAttributeRegex.Replace(text, string.Empty);
		//}

    #endregion

    #region Private members
    string input = null;
    StringBuilder output = new StringBuilder();
    bool cleanJS;
    bool isFormatted;
    bool encodeExceptions;
    #endregion

    #region cnstr

    /// <summary>
    /// Filters unknown markup. Will not encode exceptions
    /// </summary>
    /// <param name="html">Markup to filter</param>
    public HtmlScrubber(string html)
      : this(html, false, true)
    {
    }

    /// <summary>
    /// Filters unknown markup
    /// </summary>
    /// <param name="html">Markup to filter</param>
    /// <param name="encodeRuleExceptions">Should unknown elements be encoded or removed?</param>
    public HtmlScrubber(string html, bool encodeRuleExceptions)
      : this(html, encodeRuleExceptions, true)
    {

    }

    /// <summary>
    /// Filters unknown markup
    /// </summary>
    /// <param name="html">Markup to filter</param>
    /// <param name="encodeRuleExceptions">Should unknown elements be encoded or removed?</param>
    /// <param name="removeScripts">Check for javascript: attributes</param>
    public HtmlScrubber(string html, bool encodeRuleExceptions, bool removeScripts)
    {
      input = html;
      cleanJS = removeScripts;
      encodeExceptions = encodeRuleExceptions;
      allowedTags = DefaultTags();
    }

    #endregion

    #region Cleaners
		/// <summary>
		/// Returns the results of a cleaning.
		/// </summary>
		/// <returns></returns>
    public string Clean()
    {
      if (!isFormatted)
      {
        Format();
        isFormatted = true;
      }
      return output.ToString();
    }

    #endregion

    #region Format / Walk

    /// <summary>
    /// Walks one time through the HTML. All elements/tags are validated.
    /// The rest of the text is simply added to the internal queue
    /// </summary>
    protected virtual void Format()
    {
      //Lets look for elements/tags
      Match mx = regex.Match(input, 0, input.Length);

      //Never seems to be null
      while (!String.IsNullOrEmpty(mx.Value))
      {
        //find the first occurence of this elment
        int index = input.IndexOf(mx.Value, StringComparison.Ordinal);

        //add the begining to this tag
        output.Append(input.Substring(0, index));

        //remove this from the supplied text
        input = input.Remove(0, index);

        //validate the element
        output.Append(Validate(mx.Value));

        //remove this element from the supplied text
        input = input.Remove(0, mx.Length);

        //Get the next match
        mx = regex.Match(input, 0, input.Length);
      }

      //If not Html is found, we should just place all the input into the output container
      if (input != null && input.Trim().Length > 0)
        output.Append(input);
    }

    #endregion

    #region Validators

		/// <summary>
		/// Main method for starting element validation
		/// </summary>
		/// <param name="tag">A string representing an HTML element tag. Examples: &lt;b&gt;, &lt;br /&gt;, &lt;p class="header"&lt;</param>
		/// <returns>Returns the validated tag.</returns>
    protected string Validate(string tag)
    {
      if (tag.StartsWith("</", StringComparison.Ordinal))
        return ValidateEndTag(tag);

			if (tag.EndsWith("/>", StringComparison.Ordinal))
        return ValidateSingleTag(tag);


      return ValidateStartTag(tag);

    }

		/// <summary>
		/// Validates single element tags such as <br/> and <hr class="X"/>
		/// </summary>
		/// <param name="tag">A string representing a self-enclosed HTML element tag. Example: &lt;br /&gt;</param>
		/// <returns>Returns the validated tag.</returns>
		private string ValidateSingleTag(string tag)
    {
      string strip = tag.Substring(1, tag.Length - 3).Trim();

      int index = strip.IndexOfAny(new char[] { ' ', '\r', '\n' });
      if (index == -1)
        index = strip.Length;

      string tagName = strip.Substring(0, index);

      int colonIndex = tagName.IndexOf(":", StringComparison.Ordinal) + 1;

      string safeTagName = tagName.Substring(colonIndex, tagName.Length - colonIndex);

      string allowedAttributes = allowedTags[safeTagName] as string;
      if (allowedAttributes == null)
        return encodeExceptions ? Uri.EscapeUriString(tag) : string.Empty;

      string atts = strip.Remove(0, tagName.Length).Trim();

      return ValidateAttributes(allowedAttributes, atts, tagName, "<{0}{1} />");



    }

    /// <summary>
    /// Validates a start tag
    /// </summary>
		/// <param name="tag">A string representing a starting HTML element tag. Examples: &lt;b&gt;, &lt;p class="header"&lt;</param>
		/// <returns>Returns the tag and any valid attributes.</returns>
    protected virtual string ValidateStartTag(string tag)
    {
      //Check for potential attributes
      int endIndex = tag.IndexOfAny(new char[] { ' ', '\r', '\n' });

      //simple tag <tag>
      if (endIndex == -1)
        endIndex = tag.Length - 1;

      //Grab the tag name
      string tagName = tag.Substring(1, endIndex - 1);

      //watch for html pasted from Office and messy namespaces
      int colonIndex = tagName.IndexOf(":", StringComparison.Ordinal);
      string safeTagName = tagName;
      if (colonIndex != -1)
        safeTagName = tagName.Substring(colonIndex + 1);


      //Use safe incase a : is present
      string allowedAttributes = allowedTags[safeTagName] as string;

      //If we do not find a record in the Hashtable, this tag is not valid
      if (allowedAttributes == null)
				return encodeExceptions ? Uri.EscapeUriString(tag) : string.Empty; //remove element and all attributes if not valid

      //remove the tag name and find all of the current element's attributes
      int start = (colonIndex == -1) ? tagName.Length : safeTagName.Length + colonIndex + 1;

      string attributes = tag.Substring(start + 1, (tag.Length - (start + 2)));

      //if we have attributes, make sure there is no extra padding in the way
      if (attributes != null)
        attributes = attributes.Trim();

      //Validate the attributes
      return ValidateAttributes(allowedAttributes, attributes, tagName, "<{0}{1}>");


    }

		/// <summary>
		/// Validates the element's attribute collection
		/// </summary>
		/// <param name="allowedAttributes">The allowed attributes. Example: "href,target,name,title"</param>
		/// <param name="tagAttributes">The tag attributes. Example: "src='mypic.jpg' alt='My photo'"</param>
		/// <param name="tagName">Name of the tag. Examples: p, br, a</param>
		/// <param name="tagFormat">The tag format. Examples: "&lt;{0}{1}&gt;" for a start tag such as &lt;p class="header"&gt;; "&lt;{0}{1} /&gt;"
		/// for a complete tag such as &lt;br /&gt;</param>
		/// <returns>Returns the tag and any valid attributes.</returns>
		protected virtual string ValidateAttributes(string allowedAttributes, string tagAttributes, string tagName, string tagFormat)
    {
      string atts = "";
      // Are there any attributes to validate?
      if (allowedAttributes.Length > 0)
      {
        tagAttributes = xmlLineBreak.Replace(tagAttributes, string.Empty);

        for (int start = 0, end = 0;
          start < tagAttributes.Length;
          start = end)
        {
          //Put the end index at the end of the attribute name.
          end = tagAttributes.IndexOf('=', start);
          if (end < 0)
            end = tagAttributes.Length;
          //Get the attribute name and see if it's allowed.
          string att = tagAttributes.Substring(start, end - start).Trim();

          bool allowed = Regex.IsMatch(allowedAttributes, string.Format(CultureInfo.CurrentCulture, "({0},|{0}$)", att), RegexOptions.IgnoreCase);
          //Now advance the end index to include the attribute value.
          if (end < tagAttributes.Length)
          {
            //Skip any blanks after the '='.
            for (++end;
              end < tagAttributes.Length && (tagAttributes[end] == ' ' || tagAttributes[end] == '\r' || tagAttributes[end] == '\n');
              ++end) ;
            if (end < tagAttributes.Length)
            {
              //Find the end of the value.
              end = tagAttributes[end] == '"' //Quoted with double quotes?
                ? tagAttributes.IndexOf('"', end + 1)
                : tagAttributes[end] == '\'' //Quoted with single quotes?
                ? tagAttributes.IndexOf('\'', end + 1)
                : tagAttributes.IndexOfAny(new char[] { ' ', '\r', '\n' } , end); //Otherwise, assume not quoted.
              //If we didn't find the terminating character, just go to the end of the string.
              //Otherwise, advance the end index past the terminating character.
              end = end < 0 ? tagAttributes.Length : end + 1;
            }
          }
          //If the attribute is allowed, copy it.
          if (allowed)
          {
            //Special actions on these attributes. IE will render just about anything that looks like the word javascript:
            //this includes line breaks, special characters codes, etc.
						if (att.ToUpperInvariant() == "SRC" || att.ToUpperInvariant() == "HREF")
            {
              //File the value of the attribute
              //string attValue  = tagAttributes.Substring(start + att.Length, end - (start+att.Length)).Trim();
              string attValue = tagAttributes.Substring(start, end - start).Trim();

              attValue = attValue.Substring(att.Length);

              //temporarily remove some characters - mainly =, ", ', and white spaces
              attValue = filterdCharacters.Replace(attValue, string.Empty);

              //validate only http, https, mailto, and / (relative) requests are made
              if (validProtocols.IsMatch(attValue))
              {
                atts += ' ' + bannedChars.Replace(tagAttributes.Substring(start, end - start).Trim(), string.Empty);
              }

              //If the "if" above fails, we do not render the attribute!

            }
            else
            {
              atts += ' ' + tagAttributes.Substring(start, end - start).Trim();
            }


          }
        }
        //Are we filtering for Javascript?
        if (cleanJS)
          atts = jsAttributeRegex.Replace(atts, string.Empty);
      }
      return string.Format(CultureInfo.InvariantCulture, tagFormat, tagName, atts);
    }


		/// <summary>
		/// Validate end/closing tag
		/// </summary>
		/// <param name="tag">A string representing an HTML element end tag. Example: &lt;/p&gt;</param>
		/// <returns></returns>
    protected virtual string ValidateEndTag(string tag)
    {
      string tagName = tag.Substring(2, tag.Length - 3);

      int index = tag.IndexOf(":", StringComparison.Ordinal) - 1;
      if (index == -2)
      {
        index = 0;
      }
      tagName = tagName.Substring(index);
      string allowed = allowedTags[tagName] as string;

      if (allowed == null)
				return encodeExceptions ? Uri.EscapeUriString(tag) : string.Empty;

      return tag;

    }

    #endregion
  }
}
