using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Services;
using AjaxControlToolkit;

namespace WebServiceAjaxFetchResources
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ServiceSlideShow : System.Web.Services.WebService
    {
        private AjaxControlToolkit.Slide[] _allPhotos;
        
        String _noImagesFoundWebLocation; //path and file of the image containing "No Images Found" text
        String _arrayOfImageExtensions; //string of valid image file extensions without "." in the value ex "jpg,gif"
        String[] _extensionList; //string array of valid image file extensions without "." and with comma removed 

        

        //public ServiceSlideShow()
        //{
        //    //_allPhotos = GetAllSlides();
        //}

        

        //[WebMethod]
        ////[System.Web.Script.Services.ScriptMethod]
        //public AjaxControlToolkit.Slide[] GetAllSlides()
        //{
        //    string[] fileNames = System.IO.Directory.GetFiles(Server.MapPath("photos"));
        //    AjaxControlToolkit.Slide[] photos = new AjaxControlToolkit.Slide[fileNames.Length];
        //    for (int i = 0; i < fileNames.Length; i++)
        //    {
        //        //string[] file = fileNames[i].Split('\\');
        //        string fileName = fileNames[i].Replace('\\', '/');
        //        //photos[i] = new AjaxControlToolkit.Slide("photos/" + file[file.Length - 1], "", "");
        //        photos[i] = new AjaxControlToolkit.Slide(fileName, "", "");
        //    }
        //    return photos;
        //} 

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetAllSlides(string contextKey)
        {
            // get valid extensions
            //GetImageExtensionsFromWebConfig();

            // contextKey not empty
            if (String.IsNullOrEmpty(contextKey))
                return null;

            // contextKey in scope
            if ((contextKey.IndexOf("..") >= 0))
                return null;

            // verify contextKey directory exists
            String mapPath = Server.MapPath(contextKey);
            if (!Directory.Exists(mapPath))
                throw new Exception("SlideService.asmx::GetSlides - mapPath does not exist - " + Server.MapPath(mapPath));

            return System.IO.Directory.GetFiles(mapPath);
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public AjaxControlToolkit.Slide[] GetSlides(int rowNum, int maxRow, 
                                                    int colNum, int maxCol)
        {
            int tnPerPanel = maxCol*maxRow;
            int offset = colNum + rowNum * maxCol - 1;
            AjaxControlToolkit.Slide[] returnPhotos = new Slide[_allPhotos.Length / tnPerPanel];
            
            int j = 0;
            for (int i = offset; i < _allPhotos.Length; i += tnPerPanel, j++ )
            {
                returnPhotos[j] = _allPhotos[i];
            }
            
            return returnPhotos;
        }
    }
}
