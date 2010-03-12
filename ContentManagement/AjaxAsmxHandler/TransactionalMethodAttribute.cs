// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar
using System;
using System.Transactions;
using System.Collections.Generic;
using System.Text;

namespace AJAXASMXHandler
{    
    public class TransactionalMethodAttribute : Attribute
    {
        private TransactionScopeOption _TransactionOption = TransactionScopeOption.Required;

        public TransactionScopeOption TransactionOption
        {
            get { return _TransactionOption; }
            set { _TransactionOption = value; }
        }

        private IsolationLevel _IsolationLevel = IsolationLevel.ReadCommitted;

        public IsolationLevel IsolationLevel
        {
            get { return _IsolationLevel; }
            set { _IsolationLevel = value; }
        }

        private int _Timeout = 30;

        public int Timeout
        {
            get { return _Timeout; }
            set { _Timeout = value; }
        }

        public TransactionalMethodAttribute(TransactionScopeOption option, IsolationLevel isolationLevel, int timeout)
        {
            this.Timeout = timeout;
            this.IsolationLevel = isolationLevel;
            this.TransactionOption = option;
        }
        public TransactionalMethodAttribute(int timeout)
        {
            this.Timeout = timeout;            
        }
        public TransactionalMethodAttribute(TransactionScopeOption option)
        {
            this.TransactionOption = option;
        }
        public TransactionalMethodAttribute(TransactionScopeOption option, IsolationLevel isolationLevel)
        {
            this.TransactionOption = option;
            this.IsolationLevel = isolationLevel;
        }
        public TransactionalMethodAttribute(IsolationLevel isolationLevel)
        {
            this.IsolationLevel = isolationLevel;            
        }
        public TransactionalMethodAttribute()
        {            
        }
    }
}
