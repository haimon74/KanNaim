using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.ErrorHandler
{
	public class AppErrorCollection : Collection<IAppError>, IAppErrorCollection
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AppErrorCollection"/> class.
		/// </summary>
		public AppErrorCollection() : base(new List<IAppError>())
		{
		}

		/// <summary>
		/// Finds the application error with the specified <paramref name="appErrorId"/>.
		/// </summary>
		/// <param name="appErrorId">The value that uniquely identifies the application error (<see cref="IAppError.AppErrorId"/>).</param>
		/// <returns>Returns an IAppError.</returns>
		public IAppError FindById(int appErrorId)
		{
			// We know appErrors is actually a List<IAppError> because we specified it in the constructor.
			System.Collections.Generic.List<IAppError> appErrors = (System.Collections.Generic.List<IAppError>)Items;

			return appErrors.Find(delegate(IAppError appError)
			{
				return (appError.AppErrorId == appErrorId);
			});
		}

		/// <summary>
		/// Sort the objects in this collection based on the <see cref="IAppError.TimeStamp"/> property,
		/// with the most recent timestamp first.
		/// </summary>
		public void Sort()
		{
			// We know appErrors is actually a List<IAppError> because we passed it to the constructor.
			List<IAppError> appErrors = (List<IAppError>)Items;

			appErrors.Sort();
		}
	}
}
