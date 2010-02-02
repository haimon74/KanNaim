using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shimanni.Trader.DataStructure.BaseEntities;

namespace Shimanni.Trader.BL.Entites
{
    class BasicOrderSet : BasicOrderSetEntety
    {
        

        #region Constractors
        /// <summary>
        /// Initializes a new instance of the BasicOrderSetEntety class.
        /// </summary>
        /// <param name="pMM"></param>
        /// <param name="pOpening"></param>
        /// <param name="pClosing"></param>
        public BasicOrderSet(BasicQuoteEntity pMM, BasicQuoteEntity pOpening, BasicQuoteEntity pClosing) :
                base(pMM, pOpening, pClosing)
        {
        }
        /// <summary>
        /// Initializes a new instance of the BasicOrderSetEntety class.
        /// </summary>
        public BasicOrderSet() :
                base()
        {
            _MM = new BasicOrder();
            _Closing = new BasicOrder();
            _Opening = new BasicOrder();
        }


        #endregion
        
                

    }
}
