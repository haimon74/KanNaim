using System;

namespace GalleryServerPro.Web.Entity
{
	/// <summary>
	/// Represents a user in the current application.
	/// </summary>
	public class UserEntity
	{
		#region Private Fields

		private string _comment;
		private readonly DateTime _creationDate;
		private string _email;
		private bool _isApproved;
		private readonly bool _isLockedOut;
		private readonly bool _isOnline;
		private DateTime _lastActivityDate;
		private readonly DateTime _lastLockoutDate;
		private DateTime _lastLoginDate;
		private readonly DateTime _lastPasswordChangedDate;
		private readonly string _passwordQuestion;
		private readonly string _providerName;
		private readonly object _providerUserKey;
		private readonly string _userName;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="UserEntity"/> class.
		/// </summary>
		/// <param name="comment">Application-specific information for the membership user.</param>
		/// <param name="creationDate">The date and time when the user was added to the membership data store.</param>
		/// <param name="email">The e-mail address for the membership user.</param>
		/// <param name="isApproved">Indicates whether the membership user can be authenticated.</param>
		/// <param name="isLockedOut">Indicates whether the membership user is locked out.</param>
		/// <param name="isOnline">Indicates whether the membership user is online.</param>
		/// <param name="lastActivityDate">The date and time when the membership user was last authenticated or accessed the application.</param>
		/// <param name="lastLockoutDate">The most recent date and time that the membership user was locked out.</param>
		/// <param name="lastLoginDate">The date and time when the user was last authenticated.</param>
		/// <param name="lastPasswordChangedDate">The date and time when the membership user's password was last updated.</param>
		/// <param name="passwordQuestion">The password question for the membership user.</param>
		/// <param name="providerName">The name of the membership provider that stores and retrieves user information for the membership user.</param>
		/// <param name="providerUserKey">The user identifier from the membership data source for the user.</param>
		/// <param name="userName">The logon name of the membership user.</param>
		public UserEntity(string comment, DateTime creationDate, string email, bool isApproved, bool isLockedOut, bool isOnline, DateTime lastActivityDate, DateTime lastLockoutDate, DateTime lastLoginDate, DateTime lastPasswordChangedDate, string passwordQuestion, string providerName, object providerUserKey, string userName)
		{
			_comment = comment;
			_creationDate = creationDate;
			_email = email;
			_isApproved = isApproved;
			_isLockedOut = isLockedOut;
			_isOnline = isOnline;
			_lastActivityDate = lastActivityDate;
			_lastLockoutDate = lastLockoutDate;
			_lastLoginDate = lastLoginDate;
			_lastPasswordChangedDate = lastPasswordChangedDate;
			_passwordQuestion = passwordQuestion;
			_providerName = providerName;
			_providerUserKey = providerUserKey;
			_userName = userName;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets application-specific information for the membership user. 
		/// </summary>
		/// <value>Application-specific information for the membership user.</value>
		public string Comment
		{
			get { return this._comment; }
			set { this._comment = value; }
		}

		/// <summary>
		/// Gets the date and time when the user was added to the membership data store.
		/// </summary>
		/// <value>The date and time when the user was added to the membership data store.</value>
		public DateTime CreationDate
		{
			get { return this._creationDate; }
		}

		/// <summary>
		/// Gets or sets the e-mail address for the membership user.
		/// </summary>
		/// <value>The e-mail address for the membership user.</value>
		public string Email
		{
			get { return this._email; }
			set { this._email = value; }
		}

		/// <summary>
		/// Gets or sets whether the membership user can be authenticated.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if user can be authenticated; otherwise, <c>false</c>.
		/// </value>
		public bool IsApproved
		{
			get { return this._isApproved; }
			set { this._isApproved = value; }
		}

		/// <summary>
		/// Gets a value indicating whether the membership user is locked out and unable to be validated.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the membership user is locked out and unable to be validated; otherwise, <c>false</c>.
		/// </value>
		public bool IsLockedOut
		{
			get { return this._isLockedOut; }
		}

		/// <summary>
		/// Gets whether the user is currently online.
		/// </summary>
		/// <value><c>true</c> if the user is online; otherwise, <c>false</c>.</value>
		public bool IsOnline
		{
			get { return this._isOnline; }
		}

		/// <summary>
		/// Gets or sets the date and time when the membership user was last authenticated or accessed the application.
		/// </summary>
		/// <value>The date and time when the membership user was last authenticated or accessed the application.</value>
		public DateTime LastActivityDate
		{
			get { return this._lastActivityDate; }
			set { this._lastActivityDate = value; }
		}

		/// <summary>
		/// Gets the most recent date and time that the membership user was locked out.
		/// </summary>
		/// <value>The most recent date and time that the membership user was locked out.</value>
		public DateTime LastLockoutDate
		{
			get { return this._lastLockoutDate; }
		}

		/// <summary>
		/// Gets or sets the date and time when the user was last authenticated.
		/// </summary>
		/// <value>The date and time when the user was last authenticated.</value>
		public DateTime LastLoginDate
		{
			get { return this._lastLoginDate; }
			set { this._lastLoginDate = value; }
		}

		/// <summary>
		/// Gets the date and time when the membership user's password was last updated.
		/// </summary>
		/// <value>The date and time when the membership user's password was last updated.</value>
		public DateTime LastPasswordChangedDate
		{
			get { return this._lastPasswordChangedDate; }
		}

		/// <summary>
		/// Gets the password question for the membership user.
		/// </summary>
		/// <value>The password question for the membership user.</value>
		public string PasswordQuestion
		{
			get { return this._passwordQuestion; }
		}

		/// <summary>
		/// Gets the name of the membership provider that stores and retrieves user information for the membership user.
		/// </summary>
		/// <value>The name of the membership provider that stores and retrieves user information for the membership user.</value>
		public string ProviderName
		{
			get { return this._providerName; }
		}

		/// <summary>
		/// Gets the user identifier from the membership data source for the user.
		/// </summary>
		/// <value>The user identifier from the membership data source for the user.</value>
		public object ProviderUserKey
		{
			get { return this._providerUserKey; }
		}

		/// <summary>
		/// Gets the logon name of the membership user.
		/// </summary>
		/// <value>The logon name of the membership user.</value>
		public string UserName
		{
			get { return this._userName; }
		}

		#endregion
	}
}
