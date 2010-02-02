using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shimanni.Trader.BL.Entites;
using Shimanni.Trader.BL;
using Shimanni.Trader.DataStructure.BaseEntities;
using Shimanni.Trader.DataStructure;

namespace Shimanni.Trader.BL.Entites
{
    class BasicOrder : BasicQuoteEntity
    {

        #region Constructores

        /// <summary>
        /// Initializes a new instance of the BasicOrder class.
        /// </summary>
        public BasicOrder(): base()
        {
            _Price = double.MaxValue;
            _Size = double.MaxValue;
            _Side = eSide.Null;
        }
        
        
        /// <summary>
        /// Initializes a new instance of the BasicOrder class.
        /// </summary>
        /// <summary>
        /// Initializes a new instance of the BasicOrder class.
        /// </summary>
        /// <param name="price"></param>
        /// <param name="size"></param>
        /// <param name="side"></param>
        public BasicOrder(double price, double size, eSide side) :base (price,size,side)
        {

        }
        #endregion



    }
}
