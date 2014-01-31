using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace SNL.GIS.Services.WebQueue.Models
{
    public static class BranchesRepository
    {
        private static string branchesJSON = string.Empty;
        private static List<dynamic> Branches;

        public static string JSONString()
        {
            if (string.IsNullOrEmpty(BranchesRepository.branchesJSON))
            {
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/App_Data/branches.json")))
                {
                    string json = reader.ReadToEnd();
                    return json;
                }
            }
            return BranchesRepository.branchesJSON;
        }

        public static List<dynamic> AllBranches()
        {
            if (Branches == null)
            {
                var jsonObject = JArray.Parse(BranchesRepository.JSONString());
                BranchesRepository.Branches = jsonObject.ToObject<List<dynamic>>();
            }
            return Branches;
        }
    }
}