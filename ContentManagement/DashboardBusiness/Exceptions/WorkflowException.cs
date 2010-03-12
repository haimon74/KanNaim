// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using System.Collections.Generic;
using System.Text;

namespace DashboardBusiness.Exceptions
{
	class WorkflowException : ApplicationException
	{
        public WorkflowException(Exception actualException)
            : base(actualException.Message, actualException)
        {
            
        }
	}
}
