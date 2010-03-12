using System;
using System.Globalization;
using System.Windows.Media.Imaging;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Business.Metadata;
using GalleryServerPro.Business.Wpf.Properties;
using System.IO;

namespace GalleryServerPro.Business.Wpf
{
	/// <summary>
	/// Contains functionality for extracting image metadata using the .NET 3.0 Windows Presentation Foundation (WPF) classes.
	/// </summary>
	public static class WpfMetadataExtractor
	{
		/// <summary>
		/// Add items to the specified collection from metadata accessed through the Windows Presentation Foundation (WPF)
		/// classes. This includes the following items: Title, Author, Data taken, Camera model, Camera manufacturer, Keywords,
		/// Rating, Comment, Copyright, Subject. If any of these items are null, they are not added.
		/// </summary>
		/// <param name="imageFilePath">The image file path.</param>
		/// <param name="metadataItems">The collection of <see cref="IGalleryObjectMetadataItem" /> objects to add to.</param>
		/// <exception cref="System.Security.SecurityException">This function requires running under Full Trust, and will
		/// throw a security exception if it doesn't have it.</exception>
		public static void AddWpfBitmapMetadata(string imageFilePath, IGalleryObjectMetadataItemCollection metadataItems)
		{
			//return;
			BitmapMetadata bmpMetadata = GetBitmapMetadata(imageFilePath);

			if (bmpMetadata != null)
			{
				try
				{
					if (bmpMetadata.Title != null)
						metadataItems.AddNew(int.MinValue, FormattedMetadataItemName.Title, Resources.Metadata_Title, bmpMetadata.Title, true);

					if (bmpMetadata.Author != null)
						metadataItems.AddNew(int.MinValue, FormattedMetadataItemName.Author, Resources.Metadata_Author, ConvertStringCollectionToDelimitedString(bmpMetadata.Author), true);

					if (bmpMetadata.DateTaken != null)
					{
						DateTime dateTaken;
						if (DateTime.TryParse(bmpMetadata.DateTaken, out dateTaken))
						{
							metadataItems.AddNew(int.MinValue, FormattedMetadataItemName.DatePictureTaken, Resources.Metadata_DatePictureTaken, dateTaken.ToString("R", CultureInfo.InvariantCulture), true);
						}
					}

					if (bmpMetadata.CameraModel != null)
						metadataItems.AddNew(int.MinValue, FormattedMetadataItemName.CameraModel, Resources.Metadata_CameraModel, bmpMetadata.CameraModel, true);

					if (bmpMetadata.CameraManufacturer != null)
						metadataItems.AddNew(int.MinValue, FormattedMetadataItemName.EquipmentManufacturer, Resources.Metadata_EquipmentManufacturer, bmpMetadata.CameraManufacturer, true);

					if (bmpMetadata.Keywords != null)
						metadataItems.AddNew(int.MinValue, FormattedMetadataItemName.Keywords, Resources.Metadata_Keywords, ConvertStringCollectionToDelimitedString(bmpMetadata.Keywords), true);

					// Rating is an int, so we can't check for null.
					metadataItems.AddNew(int.MinValue, FormattedMetadataItemName.Rating, Resources.Metadata_Rating, bmpMetadata.Rating.ToString(CultureInfo.InvariantCulture), true);

					if (bmpMetadata.Comment != null)
						metadataItems.AddNew(int.MinValue, FormattedMetadataItemName.Comment, Resources.Metadata_Comment, bmpMetadata.Comment, true);

					if (bmpMetadata.Copyright != null)
						metadataItems.AddNew(int.MinValue, FormattedMetadataItemName.Copyright, Resources.Metadata_Copyright, bmpMetadata.Copyright, true);

					if (bmpMetadata.Subject != null)
						metadataItems.AddNew(int.MinValue, FormattedMetadataItemName.Subject, Resources.Metadata_Subject, bmpMetadata.Subject, true);
				}
				catch (NotSupportedException) { } // Some image types, such as png, throw a NotSupportedException. Let's swallow them and move on.
			}

		}

		/// <summary>
		/// Get a reference to the BitmapMetadata object for this image file that contains the metadata such as title, keywords, etc.
		/// Returns null if the metadata is not accessible.
		/// </summary>
		/// <returns> Returns a reference to the BitmapMetadata object for this image file that contains the metadata such as title, keywords, etc.</returns>
		/// <remarks>A BitmapDecoder object is created from the absolute filepath passed into the constructor. Through trial and
		/// error, the relevant metadata appears to be stored in the first frame in the BitmapDecoder property of the first frame
		/// of the root-level BitmapDecoder object. One might expect the Metadata property of the root-level BitmapDecoder object to
		/// contain the metadata, but it seems to always be null.</remarks>
		private static BitmapMetadata GetBitmapMetadata(string imageFilePath)
		{
			// Do not use the BitmapCacheOption.Default or None option, as it will hold a lock on the file until garbage collection. I discovered
			// this problem and it has been submitted to MS as a bug. See thread in the managed newsgroup:
			// http://www.microsoft.com/communities/newsgroups/en-us/default.aspx?dg=microsoft.public.dotnet.framework&tid=b694ada2-10c4-4999-81f8-97295eb024a9&cat=en_US_a4ab6128-1a11-4169-8005-1d640f3bd725&lang=en&cr=US&sloc=en-us&m=1&p=1
			// Also do not use BitmapCacheOption.OnLoad as suggested in the thread, as it causes the memory to not be released until 
			// eventually IIS crashes when you do things like synchronize 100 images.
			// BitmapCacheOption.OnDemand seems to be the only option that doesn't lock the file or crash IIS.
			// Update 2007-07-29: OnDemand seems to also lock the file. There is no good solution! Acckkk
			// Update 2007-08-04: After installing VS 2008 beta 2, which also installs .NET 2.0 SP1, I discovered that OnLoad no longer crashes IIS.
			// Update 2008-05-19: The Create method doesn't release the file lock when an exception occurs, such as when the file is a WMF. See:
			// http://www.microsoft.com/communities/newsgroups/en-us/default.aspx?dg=microsoft.public.dotnet.framework&tid=fe3fb82f-0191-40a3-b789-0602cc4445d3&cat=&lang=&cr=&sloc=&p=1
			// Bug submission: https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=344914
			// The workaround is to use a different overload of Create that takes a FileStream.

			if (String.IsNullOrEmpty(imageFilePath))
				return null;

			BitmapDecoder fileBitmapDecoder;
			using (Stream stream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				try
				{
					fileBitmapDecoder = BitmapDecoder.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
					// DO NOT USE: fileBitmapDecoder = BitmapDecoder.Create(new Uri(imageFilePath, UriKind.Absolute), BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
				}
				catch (NotSupportedException)
				{
					// Some image types, such as wmf, throw an exception. Let's skip trying to get the metadata.
					return null;
				}
			}

			if ((fileBitmapDecoder == null) || (fileBitmapDecoder.Frames.Count == 0))
				return null;

			BitmapFrame fileFirstFrame = fileBitmapDecoder.Frames[0];

			if (fileFirstFrame == null)
				return null;

			BitmapDecoder firstFrameBitmapDecoder = fileFirstFrame.Decoder;

			if ((firstFrameBitmapDecoder == null) || (firstFrameBitmapDecoder.Frames.Count == 0))
				return null;

			BitmapFrame firstFrameInDecoderInFirstFrameOfFile = firstFrameBitmapDecoder.Frames[0];

			// The Metadata property is of type ImageMetadata, so we must cast it to BitmapMetadata.
			return firstFrameInDecoderInFirstFrameOfFile.Metadata as BitmapMetadata;
		}

		private static string ConvertStringCollectionToDelimitedString(System.Collections.ObjectModel.ReadOnlyCollection<string> stringCollection)
		{
			string delimiter = "; ";
			string[] strings = new string[stringCollection.Count];
			stringCollection.CopyTo(strings, 0);
			return String.Join(delimiter, strings);
		}

	}
}
