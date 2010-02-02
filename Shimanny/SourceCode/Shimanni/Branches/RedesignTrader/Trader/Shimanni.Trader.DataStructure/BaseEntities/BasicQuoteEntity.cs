using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shimanni.Trader.DataStructure.BaseEntities
{
    public abstract class BasicQuoteEntity
    {

        #region Data Members
        private double _Price;
        private double _Size;
        private eSide _Side;

        #endregion

        #region Constractors
        /// <summary>
        /// Initializes a new instance of the BasicOrderEntety class.
        /// </summary>
        public BasicQuoteEntity()
        {
        }

        /// <summary>
        /// Initializes a new instance of the BasicOrder class.
        /// </summary>
        /// <param name="price"></param>
        /// <param name="size"></param>
        /// <param name="side"></param>
        public BasicQuoteEntity(double price, double size, eSide side)
        {
            _Price = price;
            _Size = size;
            _Side = side;
        }


        #endregion

        #region Propeties
        public double Price
        {
            get { return _Price; }
            set { _Price = value; }
        }

        public double Size
        {
            get { return _Size; }
            set { _Size = value; }
        }

        public eSide Side
        {
            get { return _Side; }
            set { _Side = value; }
        }
        #endregion
        
    }
}
