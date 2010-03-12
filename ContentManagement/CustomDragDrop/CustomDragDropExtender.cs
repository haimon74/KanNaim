// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.ComponentModel.Design;
using AjaxControlToolkit;


[assembly: System.Web.UI.WebResource("CustomDragDrop.CustomDragDropBehavior.js", "text/javascript")]

namespace CustomDragDrop
{
    [Designer(typeof(CustomDragDropDesigner))]
    [ClientScriptResource("CustomDragDrop.CustomDragDropBehavior", "CustomDragDrop.CustomDragDropBehavior.js")]
    [TargetControlType(typeof(WebControl))]
    [RequiredScript(typeof(CustomFloatingBehaviorScript))]
    [RequiredScript(typeof(DragDropScripts))]
    public class CustomDragDropExtender : ExtenderControlBase
    {
        // TODO: Add your property accessors here.
        //
        [ExtenderControlProperty]
        public string DragItemClass
        {
            get
            {
                return GetPropertyValue<String>("DragItemClass", string.Empty);
            }
            set
            {
                SetPropertyValue<String>("DragItemClass", value);
            }
        }

        [ExtenderControlProperty]
        public string DragItemHandleClass
        {
            get
            {
                return GetPropertyValue<String>("DragItemHandleClass", string.Empty);
            }
            set
            {
                SetPropertyValue<String>("DragItemHandleClass", value);
            }
        }

        [ExtenderControlProperty]
        [IDReferenceProperty(typeof(WebControl))]
        public string DropCueID
        {
            get
            {
                return GetPropertyValue<String>("DropCueID", string.Empty);
            }
            set
            {
                SetPropertyValue<String>("DropCueID", value);
            }
        }

        [ExtenderControlProperty()]
        [DefaultValue("")]
        [ClientPropertyName("onDrop")]
        public string OnClientDrop
        {
            get
            {
                return GetPropertyValue<String>("OnClientDrop", string.Empty);
            }
            set
            {
                SetPropertyValue<String>("OnClientDrop", value);
            }
        }

    }
}
