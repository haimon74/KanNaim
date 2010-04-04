using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms.Design;


namespace UserControlsLibrary.WinControls
{
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")] 
    public class CustomDataGridViewDesigner : ControlDesigner
    {
        // The PreFilterProperties method is where you can add or remove
        // properties from the component being designed. 
        //
        // In this implementation, the Visible property is removed,
        // the BackColor property is shadowed by the designer, and
        // the a new property, called Locked, is added.
        protected override void PreFilterProperties(IDictionary properties)
        {
            //// Always call the base PreFilterProperties implementation 
            //// before you modify the properties collection.
            //base.PreFilterProperties(properties);

            //// Remove the visible property.
            //properties.Remove("Visible");

            //// Shadow the BackColor property.
            //PropertyDescriptor propertyDesc = TypeDescriptor.CreateProperty(
            //    typeof(CustomDataGridViewDesigner),
            //    (PropertyDescriptor)properties["BackColor"],
            //    new Attribute[0]);
            //properties["BackColor"] = propertyDesc;

            //// Create the Locked property.
            //properties["Locked"] = TypeDescriptor.CreateProperty(
            //         typeof(CustomDataGridViewDesigner),
            //         "Locked",
            //         typeof(bool),
            //         CategoryAttribute.Design,
            //         DesignOnlyAttribute.Yes);



            base.PreFilterProperties(properties);
            string[] propertiesToHide = {
                                            "AccessibleDescription", 
                                            "AccessibleName",
                                            "AccessibleRole",
                                            "AllowDrop",
                                            "AllowUserToOrderColumns",
                                            "CausesValidation",
                                            "ColumnHeadersVisible",
                                            "Cursor",
                                            "EditMode",
                                            "Enabled",
                                            "EnableHeadersVisualStyles",
                                            "ImeMode",
                                            "Locked",
                                            //"Modifiers", - don't remove it !!
                                            "RowHeadersVisible",
                                            "RowHeadersWidth",
                                            "ScrollBars",
                                            "ShowCellErrors",
                                            "ShowEditingIcon",
                                            "ShowRowErrors",
                                            "Tag",
                                            "UseWaitCursor",
                                            "ColumnHeadersBorderStyle",
                                            "RowHeadersBorderStyle",
                                            "StandardTab",
                                            "TabStop",
                                            "AutoSizeColumnsMode",
                                            "BackgroundColor",
                                            "DefaultCellStyle",
                                            //"Dock",
                                            "GridColor",
                                            "Location",
                                            "MinimumSize",
                                            "MultiSelect",
                                            "RightToLeft",
                                            "RowHeadersWidthSizeMode",
                                            "SelectionMode",
                                            "ShowCellToolTips",
                                            "Size",
                                            "AutoSizeRowsMode",
                                            "BorderStyle",
                                            "CellBorderStyle",
                                            "ColumnHeadersHeightSizeMode",
                                            "ClipboardCopyMode",
                                            "Columns",
                                            "Anchor",
                                            "DataSource",
                                            "DataMember"
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
