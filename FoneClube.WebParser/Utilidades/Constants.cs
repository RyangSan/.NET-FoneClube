using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace VideoUpload.Utilidades
{
    public class Constants
    {
        static public string apiAdress = WebConfigurationManager.AppSettings["apiAdress"].ToString();
        static public string VideoUploadSubfolder = WebConfigurationManager.AppSettings["folderBucketAmazon"].ToString();

        public const string extensao = ".mp4";

        public const string awsAccessKey = "AKIAIPY2ULPP4RN54NJQ";
        public const string awsSecretAccessKey = "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";
        public const string VideoUploadBucket = "iosstream";

       
        

    }
}