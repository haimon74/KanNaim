// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace AJAXASMXHandler
{
    public class AsyncWebMethodState : IDisposable  
    {
        public HttpContext Context;
        
        internal string MethodName;
        internal IDisposable Target;
        internal WebMethodDef MethodDef;
        internal WebServiceDef ServiceDef;
        internal object ExtraData;

        public AsyncWebMethodState(object s)
            : this((AsyncWebMethodState)s)
        {
        }

        public AsyncWebMethodState(AsyncWebMethodState s) 
            : this(s.MethodName, s.Target, s.ServiceDef, s.MethodDef, s.Context, s.ExtraData)
        {

        }

        internal AsyncWebMethodState(string methodName,
            IDisposable target, WebServiceDef wsDef, WebMethodDef wmDef, HttpContext context,
            object extraData)
        {
            this.MethodName = methodName;
            this.Target = target;
            this.ServiceDef = wsDef;
            this.MethodDef = wmDef;
            this.Context = context;
            this.ExtraData = extraData;
        }

        public void Dispose()
        {
            this.MethodName = null;
            this.Target = null;
            this.ServiceDef = null;
            this.MethodDef = null;
            this.Context = null;
            this.ExtraData = null;
        }
    }

}
