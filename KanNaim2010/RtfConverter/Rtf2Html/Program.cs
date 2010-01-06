// -- FILE ------------------------------------------------------------------
// name       : Program.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2008.05.30
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2009 by Itenso GmbH, Switzerland
// --------------------------------------------------------------------------
using System;
using System.IO;
using System.Diagnostics;
using Itenso.Sys.Application;
using Itenso.Rtf;
using Itenso.Rtf.Support;
using Itenso.Rtf.Parser;
using Itenso.Rtf.Interpreter;
using Itenso.Rtf.Converter.Image;
using Itenso.Rtf.Converter.Html;

namespace Itenso.Solutions.Community.Rtf2Html
{

	// ------------------------------------------------------------------------
	enum ProgramExitCode
	{
		Successfully = 0,
		InvalidSettings = -1,
		ParseRtf = -2,
		DestinationDirectory = -3,
		InterpretRtf = -4,
		ConvertHtml = -5,
		SaveHtml = -6,
	} // enum ProgramExitCode

	// ------------------------------------------------------------------------
	class Program
	{

		// ----------------------------------------------------------------------
		public Program()
		{
			this.settings = new ProgramSettings();
		} // Program

		// ----------------------------------------------------------------------
		private static ProgramExitCode ExitCode
		{
			get { return (ProgramExitCode)Environment.ExitCode; }
			set { Environment.ExitCode = (int)value; }
		} // ExitCode

		// ----------------------------------------------------------------------
		public void Execute()
		{
			Console.WriteLine( string.Concat(
				ApplicationInfo.ShortCaption,
				", ",
				ApplicationInfo.Copyright ) );

			// program settings
			if ( ValidateProgramSettings() == false )
			{
				return;
			}

			// parse rtf
			IRtfGroup rtfStructure = ParseRtf();
			if ( ExitCode != ProgramExitCode.Successfully )
			{
				return;
			}

			// destination directory
			EnsureDestinationDirectory();
			if ( ExitCode != ProgramExitCode.Successfully )
			{
				return;
			}

			// image handling
			RtfVisualImageAdapter imageAdapter = new RtfVisualImageAdapter(
				this.settings.ImageFileNamePattern,
				this.settings.ImageFormat );

			// interpret rtf
			IRtfDocument rtfDocument = InterpretRtf( rtfStructure, imageAdapter );
			if ( ExitCode != ProgramExitCode.Successfully )
			{
				return;
			}

			// convert to hmtl
			string html = ConvertHmtl( rtfDocument, imageAdapter );
			if ( ExitCode != ProgramExitCode.Successfully )
			{
				return;
			}

			// save html
			string fileName = SaveHmtl( html );
			if ( ExitCode != ProgramExitCode.Successfully )
			{
				return;
			}

			// open html file
			OpenHtmlFile( fileName );
			if ( ExitCode != ProgramExitCode.Successfully )
			{
				return;
			}

			// display html text
			DisplayHtmlText( html );
			if ( ExitCode != ProgramExitCode.Successfully )
			{
				return;
			}

			Console.WriteLine( "successfully converted RTF to HTML in " + this.settings.DestinationDirectory );
		} // Execute

		// ----------------------------------------------------------------------
		private bool ValidateProgramSettings()
		{
			if ( this.settings.IsHelpMode )
			{
				ShowHelp();
				return false;
			}

			if ( !this.settings.IsValid )
			{
				ShowHelp();
				ExitCode = ProgramExitCode.InvalidSettings;
				return false;
			}

			return true;
		} // ValidateProgramSettings

		// ----------------------------------------------------------------------
		private IRtfGroup ParseRtf()
		{
			IRtfGroup rtfStructure;
			RtfParserListenerFileLogger parserLogger = null;
			try
			{
				// logger
				if ( this.settings.LogParser )
				{
					string logFileName = this.settings.BuildDestinationFileName( 
						this.settings.LogDirectory,
						RtfParserListenerFileLogger.DefaultLogFileExtension );
					parserLogger = new RtfParserListenerFileLogger( logFileName );
				}

				// rtf parser
				// open readonly - in case of dominant locks...
				using ( FileStream stream = File.Open( this.settings.SourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite ) )
				{
					// parse the rtf structure
					RtfParserListenerStructureBuilder structureBuilder = new RtfParserListenerStructureBuilder();
					RtfParser parser = new RtfParser( structureBuilder );
					parser.IgnoreContentAfterRootGroup = true; // support WordPad documents
					if ( parserLogger != null )
					{
						parser.AddParserListener( parserLogger );
					}
					parser.Parse( new RtfSource( stream ) );
					rtfStructure = structureBuilder.StructureRoot;
				}
			}
			catch ( Exception e )
			{
				if ( parserLogger != null )
				{
					parserLogger.Dispose();
				}

				Console.WriteLine( "error while parsing rtf: " + e.Message );
				ExitCode = ProgramExitCode.ParseRtf;
				return null;
			}

			return rtfStructure;
		} // ParseRtf

		// ----------------------------------------------------------------------
		private void EnsureDestinationDirectory()
		{
			if ( !this.settings.HasDestinationOutput )
			{
				return;
			}

			try
			{
				if ( !Directory.Exists( this.settings.DestinationDirectory ) )
				{
					Directory.CreateDirectory( this.settings.DestinationDirectory );
				}
			}
			catch ( Exception e )
			{
				Console.WriteLine( "error while creating destination directory: " + e.Message );
				ExitCode = ProgramExitCode.DestinationDirectory;
				return;
			}
		} // EnsureDestinationDirectory

		// ----------------------------------------------------------------------
		private IRtfDocument InterpretRtf( IRtfGroup rtfStructure, IRtfVisualImageAdapter imageAdapter )
		{
			IRtfDocument rtfDocument;
			RtfInterpreterListenerFileLogger interpreterLogger = null;
			try
			{
				// logger
				if ( this.settings.LogInterpreter )
				{
					string logFileName = this.settings.BuildDestinationFileName(
						this.settings.LogDirectory,
						RtfInterpreterListenerFileLogger.DefaultLogFileExtension );
					interpreterLogger = new RtfInterpreterListenerFileLogger( logFileName );
				}

				// image converter
				RtfImageConverter imageConverter = null;
				if ( this.settings.SaveImage )
				{
					RtfImageConvertSettings imageConvertSettings = new RtfImageConvertSettings( imageAdapter );
					imageConvertSettings.ImagesPath = this.settings.DestinationDirectory;
					imageConvertSettings.BackgroundColor = this.settings.ImageBackgroundColor;
					if ( this.settings.ExtendedImageScale )
					{
						imageConvertSettings.ScaleExtension = 0.5f;
					}
					imageConverter = new RtfImageConverter( imageConvertSettings );
				}

				// rtf parser
				// interpret the rtf structure using the extractors
				rtfDocument = RtfInterpreterTool.BuildDoc( rtfStructure, interpreterLogger, imageConverter );

			}
			catch ( Exception e )
			{
				if ( interpreterLogger != null )
				{
					interpreterLogger.Dispose();
				}

				Console.WriteLine( "error while interpreting rtf: " + e.Message );
				ExitCode = ProgramExitCode.InterpretRtf;
				return null;
			}

			return rtfDocument;
		} // InterpretRtf

		// ----------------------------------------------------------------------
		private string ConvertHmtl( IRtfDocument rtfDocument, IRtfVisualImageAdapter imageAdapter )
		{
			string html;

			try
			{
				RtfHtmlConvertSettings htmlConvertSettings = new RtfHtmlConvertSettings( imageAdapter );
				if ( this.settings.CharacterSet != null )
				{
					htmlConvertSettings.CharacterSet = this.settings.CharacterSet;
				}
				htmlConvertSettings.Title = this.settings.SourceFileNameWithoutExtension;
				htmlConvertSettings.ImagesPath = this.settings.ImagesPath;
				htmlConvertSettings.IsShowHiddenText = this.settings.ShowHiddenText;
				htmlConvertSettings.UseNonBreakingSpaces = this.settings.UseNonBreakingSpaces;
				if ( this.settings.ConvertScope != RtfHtmlConvertScope.None )
				{
					htmlConvertSettings.ConvertScope = this.settings.ConvertScope;
				}
				if ( !string.IsNullOrEmpty( this.settings.StyleSheets ) )
				{
					string[] styleSheets = this.settings.StyleSheets.Split( ',' );
					htmlConvertSettings.StyleSheetLinks.AddRange( styleSheets );
				}
				htmlConvertSettings.ConvertVisualHyperlinks = this.settings.ConvertVisualHyperlinks;
				if ( !string.IsNullOrEmpty( this.settings.VisualHyperlinkPattern ) )
				{
					htmlConvertSettings.VisualHyperlinkPattern = this.settings.VisualHyperlinkPattern;
				}

				RtfHtmlConverter htmlConverter = new RtfHtmlConverter( rtfDocument, htmlConvertSettings );
				html = htmlConverter.Convert();
			}
			catch ( Exception e )
			{
				Console.WriteLine( "error while converting to html: " + e.Message );
				ExitCode = ProgramExitCode.ConvertHtml;
				return null;
			}

			return html;
		} // ConvertHmtl

		// ----------------------------------------------------------------------
		private string SaveHmtl( string text )
		{
			if ( !this.settings.SaveHtml )
			{
				return null;
			}

			string fileName = this.settings.BuildDestinationFileName( null, RtfHtmlConverter.DefaultHtmlFileExtension );
			try
			{
				using ( TextWriter writer = new StreamWriter( fileName, false, this.settings.Encoding ) )
				{
					writer.Write( text );
				}
			}
			catch ( Exception e )
			{
				Console.WriteLine( "error while saving html: " + e.Message );
				ExitCode = ProgramExitCode.SaveHtml;
				return null;
			}

			return fileName;
		} // SaveHmtl

		// ----------------------------------------------------------------------
		private void OpenHtmlFile( string fileName )
		{
			if ( !this.settings.SaveHtml || !this.settings.OpenHtml )
			{
				return;
			}
			Process.Start( fileName );
		} // OpenHtmlFile

		// ----------------------------------------------------------------------
		private void DisplayHtmlText( string htmlText )
		{
			if ( !this.settings.DisplayHtml )
			{
				return;
			}
			Console.WriteLine( htmlText );
		} // DisplayHtmlText

		// ----------------------------------------------------------------------
		private static void ShowHelp()
		{
			Console.WriteLine();
			Console.WriteLine( "Convert RTF to HTML" );
			Console.WriteLine();
			Console.WriteLine( "Rtf2Html source-file [destination] [/CSS:names] [/ID:path] [/IT:format] [/BC:color] [/XS] [/CE:encoding] [/CS:charset]" );
			Console.WriteLine( "                     [/DS:scope] [/SH] [/SI] [/LD:path] [/LP] [/LI] [/D] [/O] [/HT] [/NBS] [/CH] [/HP:pattern] [/?]" );
			Console.WriteLine();
			Console.WriteLine( "   source-file       source rtf file" );
			Console.WriteLine( "   destination       destination directory (default=source-file directory)" );
			Console.WriteLine( "   /CSS:name1,name2  style sheet names (default=none)" );
			Console.WriteLine( "   /ID:path          images directory (default=destination directory)" );
			Console.WriteLine( "   /IT:format        images type format:" );
			Console.WriteLine( "                       jpg, gif or png (default=jpg)" );
			Console.WriteLine( "   /BC:color         image background color name (default=none)" );
			Console.WriteLine( "   /ID:path          images directory (default=destination directory)" );
			Console.WriteLine( "   /CE:encoding      character encoding:" );
			Console.WriteLine( "                       ASCII, UTF7, UTF8, Unicode, BigEndianUnicode, UTF32, OperatingSystem (default=UTF8)" );
			Console.WriteLine( "   /CS:charset       document character set used for the HTML header meta-tag 'content' (default=UTF-8)" );
			Console.WriteLine( "   /DS:scope         document scope, comma separated list of document sections:" );
			Console.WriteLine( "                       doc, html, head, body, content, all (default=all)" );
			Console.WriteLine( "   /SH               don't save HTML to the destination (default=on)" );
			Console.WriteLine( "   /SI               don't save images to the destination (default=on)" );
			Console.WriteLine( "   /LD:path          log file directory (default=destination directory)" );
			Console.WriteLine( "   /LP               write rtf parser log file (default=off)" );
			Console.WriteLine( "   /LI               write rtf interpreter log file (default=off)" );
			Console.WriteLine( "   /D                display HTML text on screen (default=off)" );
			Console.WriteLine( "   /O                open HTML in associated application (default=off)" );
			Console.WriteLine( "   /HT               show hidden text (default=off)" );
			Console.WriteLine( "   /NBS              use non-breaking spaces (default=off)" );
			Console.WriteLine( "   /CH               convert visual hyperlinks (default=off)" );
			Console.WriteLine( "   /HP:pattern       regular expression pattern to recognize visual hyperlinks, default:" );
			Console.WriteLine( "                       " + RtfHtmlConvertSettings.DefaultVisualHyperlinkPattern );
			Console.WriteLine( "   /?                this help" );
			Console.WriteLine();
			Console.WriteLine( "Samples:" );
			Console.WriteLine( "  Rtf2Html MyText.rtf" );
			Console.WriteLine( "  Rtf2Html MyText.rtf /DS:body,content" );
			Console.WriteLine( "  Rtf2Html MyText.rtf c:\\temp" );
			Console.WriteLine( "  Rtf2Html MyText.rtf c:\\temp /CSS:MyCompany.css" );
			Console.WriteLine( "  Rtf2Html MyText.rtf c:\\temp /CSS:MyCompany.css,ThisProject.css" );
			Console.WriteLine( "  Rtf2Html MyText.rtf c:\\temp /CSS:MyCompany.css,ThisProject.css /ID:images /IT:png /BC:white" );
			Console.WriteLine( "  Rtf2Html MyText.rtf c:\\temp /CSS:MyCompany.css,ThisProject.css /ID:images /IT:png /BC:white /LD:log /LP /LI" );
			Console.WriteLine();
		} // ShowHelp

		// ----------------------------------------------------------------------
		static void Main()
		{
			new Program().Execute();
		} // Main

		// ----------------------------------------------------------------------
		// members
		private readonly ProgramSettings settings;

	} // class Program

} // namespace Itenso.Solutions.Community.Rtf2Html
// -- EOF -------------------------------------------------------------------
