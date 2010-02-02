using System;

namespace Krs.Ats.IBNet
{
    /// <summary>
    /// Contract Details Event Arguments
    /// </summary>
    [Serializable()]
    public class ContractDetailsEventArgs : EventArgs
    {
        private readonly ContractDetails contractDetails;

        /// <summary>
        /// Full Constructor
        /// </summary>
        /// <param name="contractDetails">This structure contains a full description of the contract being looked up.</param>
        public ContractDetailsEventArgs(ContractDetails contractDetails)
        {
            this.contractDetails = contractDetails;
        }

        /// <summary>
        /// This structure contains a full description of the contract being looked up.
        /// </summary>
        public ContractDetails ContractDetails
        {
            get { return contractDetails; }
        }
    }
}