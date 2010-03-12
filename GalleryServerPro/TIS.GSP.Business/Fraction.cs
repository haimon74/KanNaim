using System;
using System.Globalization;

/* This file, along with ImagePropertyTag.cs and Fraction.cs, are modified versions of the project posted 
 * at http://www.codeproject.com/dotnet/ImageInfo.asp, which was inspired by the earlier project 
 * http://www.codeproject.com/cs/media/photoproperties.asp.
 * Thanks to George Mamaladze and Jeffrey S. Gangel for their hard work. */

namespace GalleryServerPro.Business
{
  ///<summary>
  /// Represents a fractional number by storing a distinct numerator and denominator. Contains static methods
  /// to provide functionality for basic arithmetic operations on the fraction.</summary>
	public class Fraction
  {
    #region Private Fields

		private System.Int64 _numerator;
		private System.Int64 _denominator;

    #endregion

    #region Constructors

    ///<summary>
    /// Creates a Fraction Number having only a numerator and assuming denumerator = 1.
    /// </summary>
		///<param name="numerator">A System.Int64 representing the numerator of the fraction.</param>
		public Fraction(System.Int64 numerator) : this(numerator, 1) { }

    ///<summary>
    /// Creates a Fraction Number having a numerator and denumerator.
    /// </summary>
		///<param name="numerator">A System.Int64 representing the numerator of the fraction.</param>
		///<param name="denominator">A System.Int64 representing the denominator of the fraction.</param>
		public Fraction(System.Int64 numerator, System.Int64 denominator)
		{
		  this._numerator = numerator;
		  this._denominator = denominator;
		}
    
    #endregion

    /// <summary>
    /// Provides a string representation of the fraction in the format "numerator/denominator" (i.e. "1/200").
    /// Whole numbers where the denominator is 1 are shown without the slash or 
    /// denominator (i.e. "12" instead of "12/1").
    /// </summary>
    public override string ToString()
    {
      if (_denominator == 1)
        return String.Format(CultureInfo.CurrentCulture, "{0}", _numerator);
      else
        return String.Format(CultureInfo.CurrentCulture, "{0}/{1}", _numerator, _denominator);
    }

		/// <summary>
		/// Returns this fraction in its decimal form.
		/// </summary>
		/// <returns>Returns this fraction in its decimal form.</returns>
		public float ToSingle()
		{
			if (_denominator == 0)
				return 0;

			return (float) _numerator / (float) _denominator;
		}

		/// <summary>
		/// Gets or sets the numerator of this fraction.
		/// </summary>
		/// <value>The numerator of this fraction.</value>
		public System.Int64 Numerator
    {
      get { return _numerator; }
      set { _numerator = value; }
    }

		/// <summary>
		/// Gets or sets the denominator of this fraction.
		/// </summary>
		/// <value>The denominator of this fraction.</value>
		public System.Int64 Denominator
    {
      get { return _denominator; }
      set { _denominator = value; }
    }

		/// <summary>
		/// Overloads the plus (+) operator.
		/// </summary>
		/// <param name="fracA">A Fraction to add to fracB.</param>
		/// <param name="fracB">A Fraction to add to fracA.</param>
		/// <returns>The result of the addition of fracA and fracB.</returns>
    public static Fraction operator +(Fraction fracA, Fraction fracB)
    {
      return new Fraction(fracA._numerator * fracB._denominator + fracB._numerator * fracA._denominator, fracA._denominator * fracB._denominator);
    }

		/// <summary>
		/// Overloads the multiplication (*) operator.
		/// </summary>
		/// <param name="fracA">A Fraction to multiply with fracB.</param>
		/// <param name="fracB">A Fraction to multiply with fracA.</param>
		/// <returns>The result of the multiplication of fracA and fracB.</returns>
		public static Fraction operator *(Fraction fracA, Fraction fracB)
    {
      return new Fraction(fracA._numerator * fracB._numerator, fracA._denominator * fracB._denominator);
    }

		/// <summary>
		/// Overloads the casting of this fraction to a <see cref="double" />. This is performed by carrying out the implicit
		/// division of the fraction. For example, the fraction 1/2 is cast to 0.50.
		/// </summary>
		/// <param name="frac">The Fraction to cast to a <see cref="double" />.</param>
		/// <returns>The result of the casting of this instance to a <see cref="double" />.</returns>
		public static implicit operator double(Fraction frac)
		{
			if (frac._denominator == 0)
				return 0;

			return ((double)frac._numerator) / ((double)frac._denominator);
		}
  }
}
