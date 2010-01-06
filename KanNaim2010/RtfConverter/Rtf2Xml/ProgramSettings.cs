// -- FILE ------------------------------------------------------------------
// name       : ProgramSettings.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2008.06.10
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2009 by Itenso GmbH, Switzerland
// --------------------------------------------------------------------------
using System.IO;
using System.Text;
using Itenso.Sys.Application;

namespace Itenso.Solutions.Community.Rtf2Xml
{


	// ------------------------------------------------------------------------
	class ProgramSettings
	{

		// ----------------------------------------------------------------------
		public ProgramSettings()
		{
			LoadApplicationArguments();
		} // ProgramSettings

		// ----------------------------------------------------------------------
		public bool IsHelpMode
		{
			get { return this.applicationArguments.IsHelpMode; }
		} // IsHelpMode

		// ----------------------------------------------------------------------
		public bool IsValid
		{
			get 
			{
				if ( !this.applicationArguments.IsValid )
				{
					return false;
				}

				if ( !string.IsNullOrEmpty( XmlPrefix ) && string.IsNullOrEmpty( XmlNamespace ) )
				{
					return false;
				}

				return true;
			}
		} // IsValid

		// ----------------------------------------------------------------------
		public bool IsValidSourceFile
		{
			get
			{
				string sourceFile = SourceFile;
				return !string.IsNullOrEmpty( sourceFile ) && File.Exists( sourceFile );
			}
		} // IsValidSourceFile

		// ----------------------------------------------------------------------
		public string SourceFile
		{
			get { return this.sourceFileArgument.Value; }
		} // SourceFile

		// ----------------------------------------------------------------------
		public string SourceFileNameWithoutExtension
		{
			get
			{
				string sourceFile = SourceFile;
				if ( sourceFile == null )
				{
					return null;
				}
				FileInfo fi = new FileInfo( sourceFile );
				return fi.Name.Replace( fi.Extension, string.Empty );
			}
		} // SourceFileNameWithoutExtension

		// ----------------------------------------------------------------------
		public string DestinationDirectory
		{
			get
			{
				string destinationDirectory = destinationDirectoryArgument.Value;
				if ( string.IsNullOrEmpty( destinationDirectory ) && IsValidSourceFile )
				{
					FileInfo fi = new FileInfo( SourceFile );
					return fi.DirectoryName;
				}
				return destinationDirectory;
			}
		} // DestinationDirectory

		// ----------------------------------------------------------------------
		public string CharacterEncoding
		{
			get { return this.characterEncodingArgument.Value; }
		} // CharacterEncoding	

		// ----------------------------------------------------------------------
		public Encoding Encoding
		{
			get
			{
				Encoding encoding = Encoding.UTF8;

				if ( !string.IsNullOrEmpty( CharacterEncoding ) )
				{
					switch ( CharacterEncoding.ToLower() )
					{
						case "ascii":
							encoding = Encoding.ASCII;
							break;
						case "utf7":
							encoding = Encoding.UTF7;
							break;
						case "utf8":
							encoding = Encoding.UTF8;
							break;
						case "unicode":
							encoding = Encoding.Unicode;
							break;
						case "bigendianunicode":
							encoding = Encoding.BigEndianUnicode;
							break;
						case "utf32":
							encoding = Encoding.UTF32;
							break;
						case "operatingsystem":
							encoding = Encoding.Default;
							break;
					}
				}

				return encoding;
			}
		} // Encoding

		// ----------------------------------------------------------------------
		public string XmlPrefix
		{
			get { return this.xmlPrefixArgument.Value; }
		} // XmlPrefix

		// ----------------------------------------------------------------------
		public string XmlNamespace
		{
			get { return this.xmlNamespaceArgument.Value; }
		} // XmlNamespace	
		
		// ----------------------------------------------------------------------
		public string LogDirectory
		{
			get { return this.logDirectoryArgument.Value; }
		} // LogDirectory

		// ----------------------------------------------------------------------
		public bool LogParser
		{
			get { return this.logParserArgument.Value; }
		} // LogParser

		// ----------------------------------------------------------------------
		public bool LogInterpreter
		{
			get { return this.logInterpreterArgument.Value; }
		} // LogInterpreter

		// ----------------------------------------------------------------------
		public bool ShowHiddenText
		{
			get { return this.showHiddenTextArgument.Value; }
		} // ShowHiddenText

		// ----------------------------------------------------------------------
		public string BuildDestinationFileName( string path, string extension )
		{
			string sourceFileNameWithoutExtension = SourceFileNameWithoutExtension;
			if ( sourceFileNameWithoutExtension == null )
			{
				return null;
			}

			return Path.Combine( 
				string.IsNullOrEmpty( path ) ? DestinationDirectory : path,
				sourceFileNameWithoutExtension + extension );
		} // BuildDestinationFileName

		// ----------------------------------------------------------------------
		private void LoadApplicationArguments()
		{
			this.applicationArguments.Arguments.Add( new HelpModeArgument() );
			this.applicationArguments.Arguments.Add( this.sourceFileArgument );
			this.applicationArguments.Arguments.Add( this.destinationDirectoryArgument );
			this.applicationArguments.Arguments.Add( this.characterEncodingArgument );
			this.applicationArguments.Arguments.Add( this.xmlPrefixArgument );
			this.applicationArguments.Arguments.Add( this.xmlNamespaceArgument );
			this.applicationArguments.Arguments.Add( this.logDirectoryArgument );
			this.applicationArguments.Arguments.Add( this.logParserArgument );
			this.applicationArguments.Arguments.Add( this.logInterpreterArgument );
			this.applicationArguments.Arguments.Add( this.showHiddenTextArgument );

			this.applicationArguments.Load();
		} // LoadApplicationArguments

		// ----------------------------------------------------------------------
		// members
		private readonly ApplicationArguments applicationArguments = new ApplicationArguments();
		private readonly ValueArgument sourceFileArgument = new ValueArgument( ArgumentType.Mandatory );
		private readonly ValueArgument destinationDirectoryArgument = new ValueArgument();
		private readonly NamedValueArgument characterEncodingArgument = new NamedValueArgument( "CE" );
		private readonly NamedValueArgument xmlPrefixArgument = new NamedValueArgument( "P" );
		private readonly NamedValueArgument xmlNamespaceArgument = new NamedValueArgument( "NS" );
		private readonly NamedValueArgument logDirectoryArgument = new NamedValueArgument( "LD" );
		private readonly ToggleArgument logParserArgument = new ToggleArgument( "LP", false );
		private readonly ToggleArgument logInterpreterArgument = new ToggleArgument( "LI", false );
		private readonly ToggleArgument showHiddenTextArgument = new ToggleArgument( "HT", false );

	} // class ProgramSettings

} // namespace Itenso.Solutions.Community.Rtf2Xml
// -- EOF -------------------------------------------------------------------
