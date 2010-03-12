using System;

namespace GalleryServerPro.Business.Interfaces
{
	/// <summary>
	/// A collection of <see cref="IAppError" /> objects.
	/// </summary>
	public interface IAppErrorCollection : System.Collections.Generic.ICollection<IAppError>
	{
		/// <summary>
		/// Finds the application error with the specified <paramref name="appErrorId"/>.
		/// </summary>
		/// <param name="appErrorId">The value that uniquely identifies the application error (<see cref="IAppError.AppErrorId"/>).</param>
		/// <returns>Returns an IAppError.</returns>
		IAppError FindById(int appErrorId);
		
		/// <summary>
		/// Sort the objects in this collection based on the <see cref="IAppError.TimeStamp" /> property,
		/// with the most recent timestamp first.
		/// </summary>
		void Sort();

		/// <summary>
		/// Gets a reference to the <see cref="IAppError" /> object at the specified index position.
		/// </summary>
		/// <param name="indexPosition">An integer specifying the position of the object within this collection to
		/// return. Zero returns the first item.</param>
		/// <returns>Returns a reference to the <see cref="IAppError" /> object at the specified index position.</returns>
		IAppError this[Int32 indexPosition]
		{
			get;
			set;
		}

		/// <summary>
		/// Searches for the specified object and returns the zero-based index of the first occurrence within the collection.  
		/// </summary>
		/// <param name="appError">The application error to locate in the collection. The value can be a null 
		/// reference (Nothing in Visual Basic).</param>
		/// <returns>The zero-based index of the first occurrence of appError within the collection, if found; 
		/// otherwise, –1. </returns>
		Int32 IndexOf(IAppError appError);
	}
}
