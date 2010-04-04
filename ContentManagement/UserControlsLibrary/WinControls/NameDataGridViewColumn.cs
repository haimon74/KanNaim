using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UserControlsLibrary.WinControls
{
    class NameDataGridViewColumn : SimpleDataGridViewColumn
    {
        public enum TextFieldEnum
        {
            MiddleName = 7,
            OtherName = 8,
            Password = 9,
            UserName = 11,
            CountryName = 12,
            ContinentName = 13,
            Fax = 15,
            Phone = 16,
            CategoryName = 17,
            FirstName = 18,
            LastName = 19,
            CityName = 20,
            Email = 25,
            ProductName = 27,
            ItemName = 28,
            CompanyName = 30,
            Address = 31,
            Notes = 35,
            DomainUrl = 40,
            LinkUrl = 45,
            ImageUrl = 48,
            ResourceUrl = 50,
            EmbedUrl = 60,
            Description = 100,
        } ;

        private const int WidthFactor = 5;

        static private readonly Dictionary<TextFieldEnum, string> HebrewHeaderTextDic = new Dictionary<TextFieldEnum, string>();
        static private readonly Dictionary<TextFieldEnum, string> EnglishHeaderTextDic = new Dictionary<TextFieldEnum, string>();

        private static Dictionary<TextFieldEnum, string> _activeHeaderTextDic;



        private static void BuildStaticData(bool isHeb)
        {
            HebrewHeaderTextDic[TextFieldEnum.FirstName] = "שם פרטי";
            HebrewHeaderTextDic[TextFieldEnum.LastName] = "שם משפחה";
            HebrewHeaderTextDic[TextFieldEnum.MiddleName] = "שם נוסף";
            HebrewHeaderTextDic[TextFieldEnum.CompanyName] = "שם חברה";
            HebrewHeaderTextDic[TextFieldEnum.ProductName] = "שם מוצר";
            HebrewHeaderTextDic[TextFieldEnum.CategoryName] = "שם קטגוריה";
            HebrewHeaderTextDic[TextFieldEnum.ItemName] = "שם פריט";
            HebrewHeaderTextDic[TextFieldEnum.CityName] = "שם עיר";
            HebrewHeaderTextDic[TextFieldEnum.CountryName] = "שם מדינה";
            HebrewHeaderTextDic[TextFieldEnum.ContinentName] = "שם יבשת";

            EnglishHeaderTextDic[TextFieldEnum.FirstName] = "First Name";
            EnglishHeaderTextDic[TextFieldEnum.LastName] = "Last Name";
            EnglishHeaderTextDic[TextFieldEnum.MiddleName] = "Other Name";
            EnglishHeaderTextDic[TextFieldEnum.CompanyName] = "Company";
            EnglishHeaderTextDic[TextFieldEnum.ProductName] = "Product";
            EnglishHeaderTextDic[TextFieldEnum.CategoryName] = "Category";
            EnglishHeaderTextDic[TextFieldEnum.ItemName] = "Item";
            EnglishHeaderTextDic[TextFieldEnum.CityName] = "City";
            EnglishHeaderTextDic[TextFieldEnum.CountryName] = "Country";
            EnglishHeaderTextDic[TextFieldEnum.ContinentName] = "Continent";

            _activeHeaderTextDic = (isHeb)
                                       ? HebrewHeaderTextDic
                                       : EnglishHeaderTextDic;
        }

        public NameDataGridViewColumn(
            bool            isHeb,
            TextFieldEnum   dataPropertyName, 
            int             displayIndex, 
            string          name)
        {
            BuildStaticData(isHeb);

            this.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.DataPropertyName = String.Format("{0}", dataPropertyName);
            this.DividerWidth = 2;
            this.DisplayIndex = displayIndex;
            this.HeaderCell = new DataGridViewColumnHeaderCell();
            this.HeaderText = _activeHeaderTextDic[dataPropertyName];
            this.Width = (int)dataPropertyName * WidthFactor;
            this.MinimumWidth = this.Width / 3;
            this.FillWeight = this.Width;
            this.Name = name ?? this.DataPropertyName;
            this.SortMode = DataGridViewColumnSortMode.Programmatic;
            this.DefaultHeaderCellType = Type.GetType("string");
            this.ValueType = Type.GetType("string");
        }

        public void InitializeVirtualProperties(
            DataGridViewCellStyle defaultCellStyle, bool isHeb,
            bool? frozen, bool? readOnly, bool? visible)
        {
            this.CellTemplate = new SimpleDataGridViewTextBoxCell(new SimpleDataGridViewCellStyle(isHeb, null, null));
            this.Frozen = frozen ?? true;
            this.DefaultCellStyle = defaultCellStyle ?? new SimpleDataGridViewCellStyle(isHeb, null, null);
            this.Resizable = DataGridViewTriState.True;
            this.ReadOnly = readOnly ?? false;
            this.Visible = visible ?? true;
        }
    }
}
