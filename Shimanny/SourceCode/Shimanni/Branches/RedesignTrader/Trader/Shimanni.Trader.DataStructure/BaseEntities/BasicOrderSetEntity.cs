using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shimanni.Trader.DataStructure.BaseEntities
{
    public abstract class BasicOrderSetEntety
    {

        #region Data Members
        private BasicQuoteEntity _MM;
        private BasicQuoteEntity _Opening;
        private BasicQuoteEntity _Closing;

        #endregion

        #region Constractors
        /// <summary>
        /// Initializes a new instance of the BasicOrderSetEntety class.
        /// </summary>
        /// <param name="mM"></param>
        /// <param name="opening"></param>
        /// <param name="closing"></param>
        public BasicOrderSetEntety(BasicQuoteEntity mM, BasicQuoteEntity opening, BasicQuoteEntity closing)
        {
            _MM = mM;
            _Opening = opening;
            _Closing = closing;
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
