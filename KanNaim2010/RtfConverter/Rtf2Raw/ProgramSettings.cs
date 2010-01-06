// -- FILE ------------------------------------------------------------------
// name       : ProgramSettings.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2008.05.30
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2009 by Itenso GmbH, Switzerland
// --------------------------------------------------------------------------
using System.Drawing;
using System.IO;
using System.Text;
using System.Drawing.Imaging;
using Itenso.Sys.Application;

namespace Itenso.Solutions.Community.Rtf2Raw
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
			get { return this.applicationArguments.IsValid; }
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
		public string ImageFileNamePattern
		{
			get { return SourceFileNameWithoutExtension + "{0}{1}"; }
		} // ImageFileNamePattern	

		// ----------------------------------------------------------------------
		public string ImageType
		{
			get { return this.imageTypeArgument.Value; }
		} // ImageType	

		// ----------------------------------------------------------------------
		public bool ScaleImage
		{
			get { return this.imageScaleArgument.Value; }
		} // ScaleImage

		// ----------------------------------------------------------------------
		public ImageFormat ImageFormat
		{
			get
			{
				ImageFormat imageFormat = null;
				if ( !string.IsNullOrEmpty( ImageType ) )
				{
					switch ( ImageType.ToLower() )
					{
						case "bmp":
							imageFormat = ImageFormat.Bmp;
							break;
						case "emf":
							imageFormat = ImageFormat.Emf;
							break;
						case "exif":
							imageFormat = ImageFormat.Exif;
							break;
						case "gif":
							imageFormat = ImageFormat.Gif;
							break;
						case "ico":
							imageFormat = ImageFormat.Icon;
							break;
						case "jpg":
							imageFormat = ImageFormat.Jpeg;
							break;
						case "png":
							imageFormat = ImageFormat.Png;
							break;
						case "tiff":
							imageFormat = ImageFormat.Tiff;
							break;
						case "wmf":
							imageFormat = ImageFormat.Wmf;
							break;
					}
				}
				return imageFormat;
			}
		} // ImageFormat	

		// ----------------------------------------------------------------------
		public Color? ImageBackgroundColor
		{
			get
			{
				string backgroundColorName = this.imageBackgroundColorNameArgument.Value;
				if ( string.IsNullOrEmpty( backgroundColorName ) )
				{
					return null;
				}

				Color backgroundColor = Color.FromName( backgroundColorName );
				if ( !backgroundColor.IsKnownColor )
				{
					return null;
				}
				return backgroundColor;
			}
		} // ImageBackgroundColor

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
		public bool HasDestinationOutput
		{
			get { return SaveText || SaveImage; }
		} // HasDestinationOutput	

		// ----------------------------------------------------------------------
		public bool SaveText
		{
			get { return this.saveTextArgument.Value; }
		} // SaveText	

		// ----------------------------------------------------------------------
		public bool SaveImage
		{
			get { return this.saveImageArgument.Value; }
		} // SaveImage	

		// ----------------------------------------------------------------------
		public bool DisplayRawText
		{
			get { return this.displayRawTextArgument.Value; }
		} // DisplayRawText

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
		public bool OpenTextFile
		{
			get { return this.openTextFileArgument.Value; }
		} // OpenTextFile

		// ----------------------------------------------------------------------
		public bool ShowHiddenText
		{
			get { return this.showHiddenTextArgument.Value; }
		} // ShowHiddenText

		// ----------------------------------------------------------------------
		public bool ExtendedImageScale
		{
			get { return this.extendedImageScaleArgument.Value; }
		} // ExtendedImageScale

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
			this.applicationArguments.Arguments.Add( this.imageTypeArgument );
			this.applicationArguments.Arguments.Add( this.imageScaleArgument );
			this.applicationArguments.Arguments.Add( this.imageBackgroundColorNameArgument );
			this.applicationArguments.Arguments.Add( this.characterEncodingArgument );
			this.applicationArguments.Arguments.Add( this.saveTextArgument );
			this.applicationArguments.Arguments.Add( this.saveImageArgument );
			this.applicationArguments.Arguments.Add( this.logDirectoryArgument );
			this.applicationArguments.Arguments.Add( this.logParserArgument );
			this.applicationArguments.Arguments.Add( this.logInterpreterArgument );
			this.applicationArguments.Arguments.Add( this.displayRawTextArgument );
			this.applicationArguments.Arguments.Add( this.openTextFileArgument );
			this.applicationArguments.Arguments.Add( this.showHiddenTextArgument );
			this.applicationArguments.Arguments.Add( this.extendedImageScaleArgument );

			this.applicationArguments.Load();
		} // LoadApplicationArguments

		// ----------------------------------------------------------------------
		// members
		private readonly ApplicationArguments applicationArguments = new ApplicationArguments();
		private readonly ValueArgument sourceFileArgument = new ValueArgument( ArgumentType.Mandatory );
		private readonly ValueArgument destinationDirectoryArgument = new ValueArgument();
		private readonly NamedValueArgument imageTypeArgument = new NamedValueArgument( "IT" );
		private readonly ToggleArgument imageScaleArgument = new ToggleArgument( "IS", false );
		private readonly NamedValueArgument imageBackgroundColorNameArgument = new NamedValueArgument( "BC" );
		private readonly NamedValueArgument characterEncodingArgument = new NamedValueArgument( "CE" );
		private readonly ToggleArgument saveTextArgument = new ToggleArgument( "ST", true );
		private readonly ToggleArgument saveImageArgument = new ToggleArgument( "SI", true );
		private readonly NamedValueArgument logDirectoryArgument = new NamedValueArgument( "LD" );
		private readonly ToggleArgument logParserArgument = new ToggleArgument( "LP", false );
		private readonly ToggleArgument logInterpreterArgument = new ToggleArgument( "LI", false );
		private readonly ToggleArgument displayRawTextArgument = new ToggleArgument( "D", false );
		private readonly ToggleArgument openTextFileArgument = new ToggleArgument( "O", false );
		private readonly ToggleArgument showHiddenTextArgument = new ToggleArgument( "HT", false );
		private readonly ToggleArgument extendedImageScaleArgument = new ToggleArgument( "XS", false );

	} // class ProgramSettings

} // namespace Itenso.Solutions.Community.Rtf2Raw
// -- EOF -------------------------------------------------------------------
