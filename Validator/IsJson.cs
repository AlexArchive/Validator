using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Validator
{
    public partial class Validator
    {
        public static bool IsJson(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return false;
            }
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.Deserialize<Dictionary<string, object>>(input);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }
    }
}
