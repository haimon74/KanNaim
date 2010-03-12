using System;
using System.Collections.ObjectModel;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Business.Metadata
{
	/// <summary>
	/// A collection of <see cref="IGalleryObjectMetadataItem" /> objects.
	/// </summary>
	[Serializable]
	class GalleryObjectMetadataItemCollection : Collection<IGalleryObjectMetadataItem>, IGalleryObjectMetadataItemCollection
	{
		private bool? _regenerateAllOnSaveEmptyCollection;

		/// <summary>
		/// Initializes a new instance of the <see cref="GalleryObjectMetadataItemCollection"/> class.
		/// </summary>
		public GalleryObjectMetadataItemCollection() : base(new System.Collections.Generic.List<IGalleryObjectMetadataItem>())
		{
		}

		/// <summary>
		/// Determines whether the <paramref name="galleryObjectMetadataItem"/> is already a member of the collection. An object is considered a member
		/// of the collection if the value of its <see cref="IGalleryObjectMetadataItem.MetadataItemName"/> property matches one in the existing collection.
		/// </summary>
		/// <param name="galleryObjectMetadataItem">An <see cref="IGalleryObjectMetadataItem"/> to determine whether it is a member of the current collection.</param>
		/// <returns>
		/// Returns <c>true</c> if <paramref name="galleryObjectMetadataItem"/> is a member of the current collection;
		/// otherwise returns <c>false</c>.
		/// </returns>
		public new bool Contains(IGalleryObjectMetadataItem galleryObjectMetadataItem)
		{
			foreach (IGalleryObjectMetadataItem metadataItemIterator in (System.Collections.Generic.List<IGalleryObjectMetadataItem>)Items)
			{
				if (galleryObjectMetadataItem.MetadataItemName == metadataItemIterator.MetadataItemName)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Create a new <see cref="IGalleryObjectMetadataItem"/> item from the specified parameters and add it to the collection. Return a
		/// reference to the new item.
		/// </summary>
		/// <param name="mediaObjectMetadataId">A value that uniquely indentifies this metadata item.</param>
		/// <param name="metadataItemName">The name of this metadata item.</param>
		/// <param name="description">The description of the metadata item (e.g. "Exposure time", "Camera model")</param>
		/// <param name="value">The value of the metadata item (e.g. "F5.7", "1/500 sec.").</param>
		/// <param name="hasChanges">A value indicating whether this metadata item has changes that have not been persisted to the database.</param>
		/// <returns>Returns a reference to the new item.</returns>
		public IGalleryObjectMetadataItem AddNew(int mediaObjectMetadataId, FormattedMetadataItemName metadataItemName, string description, string value, bool hasChanges)
		{
			IGalleryObjectMetadataItem metadataItem = new GalleryObjectMetadataItem(mediaObjectMetadataId, metadataItemName, description, value, hasChanges);
			Items.Add(metadataItem);

			return metadataItem;
		}

		/// <summary>
		/// Adds the metadata items to the current collection.
		/// </summary>
		/// <param name="galleryObjectMetadataItems">The metadata items to add to the collection.</param>
		public void AddRange(IGalleryObjectMetadataItemCollection galleryObjectMetadataItems)
		{
			foreach (IGalleryObjectMetadataItem metadataItem in galleryObjectMetadataItems)
			{
				Items.Add(metadataItem);
			}
		}

		/// <summary>
		/// Sort the objects in this collection based on the Description property.
		/// </summary>
		public void Sort()
		{
			// We know galleryObjectMetadataItems is actually a List<IGalleryObjectMetadataItem> because we passed it to the constructor.
			System.Collections.Generic.List<IGalleryObjectMetadataItem> galleryObjectMetadataItems = (System.Collections.Generic.List<IGalleryObjectMetadataItem>) Items;

			galleryObjectMetadataItems.Sort();
		}

		/// <summary>
		/// Gets the <see cref="IGalleryObjectMetadataItem"/> object that matches the specified
		/// <see cref="GalleryServerPro.Business.Metadata.FormattedMetadataItemName"/>. The <paramref name="metadataItem"/>
		/// parameter remains null if no matching object is in the collection.
		/// </summary>
		/// <param name="metadataName">The <see cref="GalleryServerPro.Business.Metadata.FormattedMetadataItemName"/> of the
		/// <see cref="IGalleryObjectMetadataItem"/> to get.</param>
		/// <param name="metadataItem">When this method returns, contains the <see cref="IGalleryObjectMetadataItem"/> associated with the
		/// specified <see cref="GalleryServerPro.Business.Metadata.FormattedMetadataItemName"/>, if the key is found; otherwise, the
		/// parameter remains null. This parameter is passed uninitialized.</param>
		/// <returns>
		/// Returns true if the <see cref="IGalleryObjectMetadataItemCollection"/> contains an element with the specified
		/// <see cref="GalleryServerPro.Business.Metadata.FormattedMetadataItemName"/>; otherwise, false.
		/// </returns>
		public bool TryGetMetadataItem(FormattedMetadataItemName metadataName, out IGalleryObjectMetadataItem metadataItem)
		{
			// We know galleryObjectMetadataItems is actually a List<IGalleryObjectMetadataItem> because we passed it to the constructor.
			System.Collections.Generic.List<IGalleryObjectMetadataItem> galleryObjectMetadataItems = (System.Collections.Generic.List<IGalleryObjectMetadataItem>) Items;

			metadataItem = galleryObjectMetadataItems.Find(delegate(IGalleryObjectMetadataItem metaItem)
			{
				return (metaItem.MetadataItemName == metadataName);
			});

			return (metadataItem != null);
		}

		/// <summary>
		/// Get a list of items whose metadata must be updated with the metadata currently in the media object's file. All
		/// IGalleryObjectMetadataItem whose ExtractFromFileOnSave property are true are returned. This is called during a
		/// save operation to indicate which metadata items must be updated. Guaranteed to not return null. If no items
		/// are found, an empty collection is returned.
		/// </summary>
		/// <returns>
		/// Returns a list of items whose metadata must be updated with the metadata currently in the media object's file.
		/// </returns>
		public IGalleryObjectMetadataItemCollection GetItemsToUpdate()
		{
			// We know galleryObjectMetadataItems is actually a List<IGalleryObjectMetadataItem> because we passed it to the constructor.
			System.Collections.Generic.List<IGalleryObjectMetadataItem> galleryObjectMetadataItems = (System.Collections.Generic.List<IGalleryObjectMetadataItem>) Items;
			IGalleryObjectMetadataItemCollection metadataItemsCollection = new GalleryObjectMetadataItemCollection();

			galleryObjectMetadataItems.ForEach(delegate(IGalleryObjectMetadataItem metaItem)
			{
				if (metaItem.ExtractFromFileOnSave)
				{
					metadataItemsCollection.Add(metaItem);
				}
			});

			return metadataItemsCollection;
		}

		/// <summary>
		/// Get a list of items whose metadata must be persisted to the data store, either because it has been added or because
		/// it has been modified. All IGalleryObjectMetadataItem whose HasChanges property are true are returned. This is called during a
		/// save operation to indicate which metadata items must be saved. Guaranteed to not return null. If no items
		/// are found, an empty collection is returned.
		/// </summary>
		/// <returns>
		/// Returns a list of items whose metadata must be updated with the metadata currently in the media object's file.
		/// </returns>
		public IGalleryObjectMetadataItemCollection GetItemsToSave()
		{
			// We know galleryObjectMetadataItems is actually a List<IGalleryObjectMetadataItem> because we passed it to the constructor.
			System.Collections.Generic.List<IGalleryObjectMetadataItem> galleryObjectMetadataItems = (System.Collections.Generic.List<IGalleryObjectMetadataItem>) Items;
			IGalleryObjectMetadataItemCollection metadataItemsCollection = new GalleryObjectMetadataItemCollection();

			galleryObjectMetadataItems.ForEach(delegate(IGalleryObjectMetadataItem metaItem)
			{
				if (metaItem.HasChanges)
				{
					metadataItemsCollection.Add(metaItem);
				}
			});

			return metadataItemsCollection;
		}

		/// <summary>
		/// Perform a deep copy of this metadata collection.
		/// </summary>
		/// <returns>
		/// Returns a deep copy of this metadata collection.
		/// </returns>
		public IGalleryObjectMetadataItemCollection Copy()
		{
			IGalleryObjectMetadataItemCollection metaDataItemCollectionCopy = new GalleryObjectMetadataItemCollection();

			foreach (IGalleryObjectMetadataItem metaDataItem in this.Items)
			{
				metaDataItemCollectionCopy.Add(metaDataItem.Copy());
			}

			return metaDataItemCollectionCopy;
		}

		/// <summary>
		/// Gets or sets a value indicating whether all metadata items in the collection should be replaced with the current
		/// metadata in the image file. This property is calculated based on the <see cref="IGalleryObjectMetadataItem.ExtractFromFileOnSave"/>
		/// property on each metadata item in the collection. Setting this property causes the
		/// <see cref="IGalleryObjectMetadataItem.ExtractFromFileOnSave"/>  property to be set to the specified value for *every* metadata item in the collection.
		/// If the collection is empty, then the value is stored in a private class field. Note that since new items added to
		/// the collection have their <see cref="IGalleryObjectMetadataItem.ExtractFromFileOnSave"/> property set to false, if you set <see cref="RegenerateAllOnSave"/> = "true" on
		/// an empty collection, then add one or more items, this property will subsequently return false. This property is
		/// ignored for <see cref="IAlbum"/> objects.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if <see cref="IGalleryObjectMetadataItem.ExtractFromFileOnSave"/>  =  true for *every* metadata item in
		/// the collection; otherwise, <c>false</c>.
		/// </value>
		public bool RegenerateAllOnSave
		{
			get
			{
				// We know galleryObjectMetadataItems is actually a List<IGalleryObjectMetadataItem> because we passed it to the constructor.
				// Return true if all metadata items have the ExtractFromFileOnSave property set to true. If we have an empty collection,
				// return the value of a private field if it was previously set; otherwise return false.
				System.Collections.Generic.List<IGalleryObjectMetadataItem> galleryObjectMetadataItems = (System.Collections.Generic.List<IGalleryObjectMetadataItem>) Items;

				if (galleryObjectMetadataItems.Count > 0)
				{
					return galleryObjectMetadataItems.TrueForAll(delegate(IGalleryObjectMetadataItem metaItem)
					{
						return metaItem.ExtractFromFileOnSave;
					});
				}
				else if (this._regenerateAllOnSaveEmptyCollection.HasValue)
				{
					return this._regenerateAllOnSaveEmptyCollection.Value;
				}
				else
				{
					return false;
				}
			}
			set
			{
				// We know galleryObjectMetadataItems is actually a List<IGalleryObjectMetadataItem> because we passed it to the constructor.
				// Store value in ExtractFromFileOnSave property on each item. If we have an empty collection, store in a private field.
				System.Collections.Generic.List<IGalleryObjectMetadataItem> galleryObjectMetadataItems = (System.Collections.Generic.List<IGalleryObjectMetadataItem>) Items;

				if (galleryObjectMetadataItems.Count > 0)
				{
					galleryObjectMetadataItems.ForEach(delegate(IGalleryObjectMetadataItem metaItem)
					{
						metaItem.ExtractFromFileOnSave = value;
					});
				}
				else
				{
					this._regenerateAllOnSaveEmptyCollection = value;
				}
			}
		}
	}
}
