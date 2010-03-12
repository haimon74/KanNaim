// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar
using System;
using System.Collections.Generic;
using System.Text;

namespace AJAXASMXHandler
{
    public class ETagMethodAttribute : Attribute
    {
        private bool _Enabled;

        public bool Enabled
        {
            get { return _Enabled; }
            set { _Enabled = value; }
        }

        public ETagMethodAttribute()
        {
            this._Enabled = true;
        }

        public ETagMethodAttribute(bool enabled)
        {
            this._Enabled = enabled;
        }
    }
}
