using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms.Design;


namespace UserControlsLibrary.WinControls
{
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class FormRtlLayoutDesigner : ControlDesigner
    {
        // The PreFilterProperties method is where you can add or remove
        // properties from the component being designed. 
        //
        // In this implementation, the Visible property is removed,
        // the BackColor property is shadowed by the designer, and
        // the a new property, called Locked, is added.
        protected override void PreFilterProperties(IDictionary properties)
        {
            
            base.PreFilterProperties(properties);
            string[] propertiesToHide = {
                                            "AcceptButton",
                                            "AccessibleDescription", 
                                            "AccessibleName",
                                            "AccessibleRole",
                                            "AutoScaleMode",
                                            "AllowDrop",
                                            "AutoScroll",
                                            "AutoScrollMargin",
                                            "AutoScrollMinSize",
                                            "AutoValidate",
                                            "CancelButton",
                                            "CausesValidation",
                                            "DoubleBuffered",
                                            "Cursor",
                                            "IsMidContainer",
                                            "Enabled",
                                            "RightToLeft",
                                            "ImeMode",
                                            "ShowIcon",
                                            "Tag",
                                            "TopMost",
                                            "TransparencyKey",
                                            "UseWaitCursor",
                                            "WindowState"
                                        };

            foreach (string propname in propertiesToHide)
            {
                PropertyDescriptor prop =
                  (PropertyDescriptor)properties[propname];
                if (prop != null)
                {
                    AttributeCollection runtimeAttributes =
                                               prop.Attributes;
                    // make a copy of the original attributes 

                    // but make room for one extra attribute

                    Attribute[] attrs =
                       new Attribute[runtimeAttributes.Count + 1];
                    runtimeAttributes.CopyTo(attrs, 0);
                    attrs[runtimeAttributes.Count] =
                                    new BrowsableAttribute(false);
                    prop =
                     TypeDescriptor.CreateProperty(this.GetType(),
                                 propname, prop.PropertyType, attrs);
                    properties[propname] = prop;
                }
            }

        }

    }
}
