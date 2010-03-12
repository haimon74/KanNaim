
using System;
using System.Configuration;
using System.Web.Profile;

namespace Dropthings.Web.Framework
{
    /// <summary>
    /// Summary description for UserProfile
    /// </summary>
    [Serializable]
    public class UserProfile : System.Web.Profile.ProfileBase
    {
        public UserProfile()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        [SettingsAllowAnonymousAttribute(true)]
        [DefaultSettingValue("true")]
        public virtual bool IsFirstVisit
        {
            get
            {
                return ((bool)(this.GetPropertyValue("IsFirstVisit")));
            }
            set
            {
                this.SetPropertyValue("IsFirstVisit", value);
            }
        }

        [SettingsAllowAnonymousAttribute(true)]
        public virtual string Fullname
        {
            get
            {
                return ((string)(this.GetPropertyValue("Fullname")));
            }
            set
            {
                this.SetPropertyValue("Fullname", value);
            }
        }

    }
}