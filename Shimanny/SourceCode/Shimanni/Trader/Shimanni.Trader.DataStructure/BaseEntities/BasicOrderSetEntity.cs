using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shimanni.Trader.DataStructure.BaseEntities
{
    public abstract class BasicOrderSetEntety
    {

        #region Data Members
        protected BasicQuoteEntity _MM;
        protected BasicQuoteEntity _Opening;
        protected BasicQuoteEntity _Closing;

        #endregion

        #region Constractors
        /// <summary>
        /// Initializes a new instance of the BasicOrderSetEntety class.
        /// </summary>
        /// <param name="pMM"></param>
        /// <param name="pOpening"></param>
        /// <param name="pClosing"></param>
        public BasicOrderSetEntety(BasicQuoteEntity pMM, BasicQuoteEntity pOpening, BasicQuoteEntity pClosing)
        {
            _MM = pMM;
            _Opening = pOpening;
            _Closing = pClosing;
        }
        /// <summary>
        /// Initializes a new instance of the BasicOrderSetEntety class.
        /// </summary>
        public BasicOrderSetEntety()
        {
        }


        #endregion

        #region Propeties
        public BasicQuoteEntity MM
        {
            get { return _MM; }
            set { _MM = value; }
        }
        public BasicQuoteEntity Opening
        {
            get { return Opening; }
            set { _Opening = value; }
        }
        public BasicQuoteEntity Closing
        {
            get { return _Closing; }
            set { _Closing = value; }
        }
        
        
        
        #endregion

    }
}
