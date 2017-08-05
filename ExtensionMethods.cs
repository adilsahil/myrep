namespace Infologics.Medilogics.General.Control
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data;
    using System.Text.RegularExpressions;
    using System.Globalization;
    using System.Threading;
    using Infologics.Medilogics.General.Control.Classes;

    public static class ExtensionMethods
    {
        /// <summary>
        /// Determines whether the specified column in the datarow is null,
        /// if so then returns the specified  value, else return the value in the coulumn
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="colName">Name of the column</param>
        /// <param name="defaultVal">The value to return if specified column is null.</param>
        /// <returns>The value in the specified column or defaultvalue</returns>
        public static T IsNull<T>(this DataRow drSrc, string colName, T defaultVal)
        {
            T value = defaultVal;
            if (drSrc != null && drSrc.Table.Columns.Contains(colName))
            {
                value = drSrc.RowState == DataRowState.Deleted
                        ? drSrc[colName, DataRowVersion.Original] != DBNull.Value
                            ? (T)Convert.ChangeType(drSrc[colName, DataRowVersion.Original], typeof(T))
                            : defaultVal
                        : drSrc[colName] != DBNull.Value
                            ? (T)Convert.ChangeType(drSrc[colName], typeof(T))
                            : defaultVal;
            }
            return value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringData"></param>
        /// <returns></returns>
        public static bool KIIsNotNullOrEmpty(this object stringData)
        {
            return (stringData != null && string.IsNullOrEmpty(Convert.ToString(stringData)) == false &&   string.IsNullOrEmpty(Convert.ToString(stringData).Trim()) == false);
        }

        /// <summary>
        /// Determines whether the specified column in the DataRowVersion is null,
        /// if so then returns the specified  value, else returns the value in the coulumn
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="colName">Name of the column</param>
        /// <param name="drVersion">The datarow version.</param>
        /// <param name="defaultVal">The value to return if specified column is null.</param>
        /// <returns>The value in the specified column or defaultvalue</returns>
        public static T IsNull<T>(this DataRow drSrc, string colName, DataRowVersion drVersion, T defaultVal)
        {
            T value = defaultVal;
            if (drSrc != null && drSrc.Table.Columns.Contains(colName))
            {
                value = drSrc[colName, drVersion] != DBNull.Value
                            ? (T)Convert.ChangeType(drSrc[colName, drVersion], typeof(T))
                            : defaultVal;
            }
            return value;
        }
        /// <summary>
        /// Determines whether the specified obj value is numeric.
        /// </summary>
        /// <param name="objValue">The obj value.</param>
        /// <returns>
        ///   <c>true</c> if the specified obj value is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(this object objValue)
        {
            try
            {
                bool blnResult = false;
                if (objValue != null)
                {
                    //Regex All_Numeric_Regex = new Regex("[^0-9.]"); // "(^[0-9]*$)" 
                    //blnResult = !All_Numeric_Regex.IsMatch(objValue.ToString());
                    //bool variable to hold the return value
                    bool match;
                    //regula expression to match numeric values
                    string pattern = "(^[-+]?\\d+(,?\\d*)*\\.?\\d*([Ee][-+]\\d*)?$)|(^[-+]?\\d?(,?\\d*)*\\.\\d+([Ee][-+]\\d*)?$)";
                    //generate new Regulsr Exoression eith the pattern and a couple RegExOptions
                    Regex regEx = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                    //tereny expresson to see if we have a match or not
                    match = regEx.Match(objValue.ToString()).Success ? true : false;
                    //return the match value (true or false)
                    blnResult = match;
                }
                return blnResult;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Determines whether the specified obj value is numeric.
        /// </summary>
        /// <param name="objValue">The obj value.</param>
        /// <returns>
        ///   <c>true</c> if the specified obj value is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(this string objValue)
        {
            try
            {
                bool blnResult = false;
                if (objValue != null)
                {
                    //Regex All_Numeric_Regex = new Regex("[^0-9.]"); // "(^[0-9]*$)" 
                    //blnResult = !All_Numeric_Regex.IsMatch(objValue.ToString());
                    //bool variable to hold the return value
                    bool match;
                    //regula expression to match numeric values
                    string pattern = "(^[-+]?\\d+(,?\\d*)*\\.?\\d*([Ee][-+]\\d*)?$)|(^[-+]?\\d?(,?\\d*)*\\.\\d+([Ee][-+]\\d*)?$)";
                    //generate new Regulsr Exoression eith the pattern and a couple RegExOptions
                    Regex regEx = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                    //tereny expresson to see if we have a match or not
                    match = regEx.Match(objValue).Success ? true : false;
                    //return the match value (true or false)
                    blnResult = match;
                }
                return blnResult;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// An Extension Method to allow us t odo "The Title Of It".asTitleCase()
        /// which would return a TitleCased string. ///
        /// </summary>
        /// <param name="title">Title to work with.</param>
        /// <returns>Output title as TitleCase</returns>
        /// which would return a TitleCased string.
        public static string asTitleCase(this string title)
        {
            string WorkingTitle = title;
            if (string.IsNullOrEmpty(WorkingTitle) == false)
            {
                char[] space = new char[] { ' ' };
                List<string> artsAndPreps = new List<string>() { "a", "an", "and", "any", "at", "from", "into", "of", "on", "or", "some", "the", "to", };           //Get the culture property of the thread.   
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;         //Create TextInfo object.   
                TextInfo textInfo = cultureInfo.TextInfo;           //Convert to title case.         
                WorkingTitle = textInfo.ToTitleCase(title.ToLower());
                List<string> tokens = title.Split(space, StringSplitOptions.RemoveEmptyEntries).ToList();
                WorkingTitle = tokens[0];
                tokens.RemoveAt(0);
                WorkingTitle += tokens.Aggregate<String, String>(String.Empty,
                    (String prev, String input) => prev
                    + (artsAndPreps.Contains(input.ToLower()) // If True 
                    ? " " + input.ToLower()              // Return the prep/art lowercase    
                    : " " + input));                   // Otherwise return the valid word.           // Handle an "Out Of" but not in the start of the sentance    
                WorkingTitle = Regex.Replace(WorkingTitle, @"(?!^Out)(Out\s+Of)", "out of");
            }
            return WorkingTitle;
        }
         

        /// <summary>
        /// Assigning table name if the table name is Blank
        /// </summary>
        /// <param name="Table"></param>
        /// <returns></returns>
        public static void AssignTableName(this DataTable Table)
        {
            Table.TableName = (Table != null && Table.TableName == string.Empty) ? "CRITERIA" : Table.TableName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Table"></param>
        /// <returns></returns>
        public static bool KIIsNotNullAndRowCount(this DataTable Table)
        {
            return Table == null || Table.Rows.Count == 0 ? false : true;
        }

        /// <summary>
        /// Determines whether [is not null and row count] [the specified table].
        /// </summary>
        /// <param name="Table">The table.</param>
        /// <returns>
        ///   <c>true</c> if [is not null and row count] [the specified table]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotNullAndRowCount(this DataTable Table)
        {
            return Table == null || Table.Rows.Count == 0 ? false : true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="joinChar">","</param>
        /// <param name="joinColumnName"></param>
        /// <returns></returns>
        public static string KIAppendCharacter(this DataTable Table, string joinChar, string joinColumnName)
        {
            string Data = string.Empty;
            if (Table.KIIsNotNullAndRowCount() && Table.Columns.Contains(joinColumnName))
            {
                Data = String.Join(joinChar, (from row in Table.AsEnumerable()
                                              select row[joinColumnName].ToString()).ToArray());
            }
            return Data;
        }

        /// <summary>
        /// Check the valid email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool KIIsValidEmail(this string email)
        {   
            return new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Match(email.Trim()).Success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string KIasTitleCase(this string title)
        {
            string WorkingTitle = title;
            if (string.IsNullOrEmpty(WorkingTitle) == false)
            {
                char[] space = new char[] { ' ' };
                List<string> artsAndPreps = new List<string>() { "a", "an", "and", "any", "at", "from", "into", "of", "on", "or", "some", "the", "to", };           //Get the culture property of the thread.   
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;         //Create TextInfo object.   
                TextInfo textInfo = cultureInfo.TextInfo;           //Convert to title case.         
                WorkingTitle = textInfo.ToTitleCase(title.ToLower());
                List<string> tokens = title.Split(space, StringSplitOptions.RemoveEmptyEntries).ToList();
                WorkingTitle = tokens[0];
                tokens.RemoveAt(0);
                WorkingTitle += tokens.Aggregate<String, String>(String.Empty,
                    (String prev, String input) => prev
                    + (artsAndPreps.Contains(input.ToLower()) // If True 
                    ? " " + input.ToLower()              // Return the prep/art lowercase    
                    : " " + input));                   // Otherwise return the valid word.           // Handle an "Out Of" but not in the start of the sentance    
                WorkingTitle = Regex.Replace(WorkingTitle, @"(?!^Out)(Out\s+Of)", "out of");
            }
            return WorkingTitle;
        }
        /// <summary>
        /// Format the Decimal Places based on the setting value.eg:1 to 1.000 (3 decimal places) or 1.00(2 decimal places) etc
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string KIFormatDecimalPlace(this string value, int DecimalPlace)
        {
            System.Globalization.NumberFormatInfo numFormat = new System.Globalization.CultureInfo(System.Globalization.CultureInfo.CurrentCulture.Name, false).NumberFormat;
            numFormat.NumberDecimalDigits = DecimalPlace;
            return string.IsNullOrEmpty(System.Convert.ToString(value)) ? Math.Round(0.00, DecimalPlace).ToString("F", numFormat) :
                Math.Round(System.Convert.ToDouble(value), DecimalPlace).ToString("F", numFormat);
        }
        /// <summary>
        /// To sum the specified column values in a source
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static T KISum<T>(this DataTable dt, string columnName)
        {
            T Sum = default(T);
            var Data = dt.AsEnumerable().Where(dr => dr[columnName] != DBNull.Value);
            if (Data.Count() > 0)
            {
                if (typeof(T).ToString().Equals("System.Decimal"))
                {
                    Sum = (T)Convert.ChangeType(Data.Sum(data => Convert.ToDecimal(data[columnName])), typeof(T));
                }
                else if (typeof(T).ToString().Equals("System.Double"))
                {
                    Sum = (T)Convert.ChangeType(Data.Sum(data => Convert.ToDouble(data[columnName])), typeof(T));
                }
                else
                {
                    Sum = (T)Convert.ChangeType(Data.Sum(data => Convert.ToDecimal(data[columnName])), typeof(T));
                }
            }
            return Sum;
        }

        /// <summary>
        /// Get the value of the specified column from a Row
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static T KIGetDataRowValue<T>(this DataRow dr, string columnName)
        {
            return (dr.Table.Columns.Contains(columnName) && dr[columnName] != DBNull.Value ?
                (T)Convert.ChangeType(dr[columnName], typeof(T)) : default(T));
        }

        /// <summary>
        /// KIs the get updated date.
        /// </summary>
        /// <param name="dtCurrentDateTime">The dt current date time.</param>
        /// <param name="minuteChangeingFactor">The minute changeing factor.</param>
        /// <returns></returns>
        public static DateTime KIGetUpdatedDate(this DateTime dtCurrentDateTime, int minuteChangeingFactor)
        {
            decimal dc = (Math.Ceiling(Convert.ToDecimal(dtCurrentDateTime.Minute / minuteChangeingFactor)) * minuteChangeingFactor) - dtCurrentDateTime.Minute;
            if (dc != 0)
            {
                dtCurrentDateTime = dtCurrentDateTime.AddMinutes(Convert.ToDouble(dc));
            }
            return dtCurrentDateTime;
        }

        public static string KiGetImagePath(this string sPhysicalPath)
        {
            string sReturnPath = string.Empty;
            try
            {
                string sMode = System.Configuration.ConfigurationSettings.AppSettings["ModeOfCommunication"].ToString().ToUpper();
                if (sMode == "WCF")
                {
                    string sVitrualPath = System.Configuration.ConfigurationSettings.AppSettings["ImageVirtualPath"].ToString().ToUpper();
                    string[] sPhyPath = sPhysicalPath.Split('\\');
                    sPhyPath[2] = sVitrualPath;
                    foreach (string strPath in sPhyPath)
                    {
                        if (!string.IsNullOrEmpty(strPath))
                        {
                            sReturnPath += strPath + "/";
                        }
                    }
                    sReturnPath = sReturnPath.Substring(0, sReturnPath.Length - 1);

                }
                else
                {
                    sReturnPath = sPhysicalPath;

                }
            }
            catch
            {
                throw;
            }

            return sReturnPath;


        }
        /// <summary>
        /// Serialiaze Template Data object to String object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string KISerializeToString(this TemplateData obj)
        {
            try
            {
                string str = string.Empty;
                if (obj != null)
                {
                    Type[] oTypeArr = new Type[] { typeof(Infologics.Medilogics.CommonXSD.XSD.CommonShared.EMR_AUDITDataTable) };
                    str = (string)new Infologics.Medilogics.General.Control.Common().SerializeCustomClassObject(obj, oTypeArr);
                }
                return str;
            }
            catch (Exception)
            {
                
                throw;
            }
           
        }

        /// <summary>
        /// To Convert Object to Decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Decimal KIToDecimal(this object value)
        {
            try
            {
                decimal output = 0;
                if (Convert.ToString(value) != string.Empty)
                {
                    decimal.TryParse(Convert.ToString(value), out output);
                }
                return output;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        ///It will iterate through the Enumerable Types and Do the iteration passed as parameter.
        /// </summary>
        public static IEnumerable<T> ForEach<T>(
                this IEnumerable<T> source,
                    Action<T> act)
        {
            if (source != null)
            {
                foreach (T element in source) act(element);                
            }
            return source;
        }

        /// <summary>
        /// KIs the remove invalid file path characters.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="replaceChar">The replace char.</param>
        /// <returns></returns>
        public static string KIRemoveInvalidFileNameCharacters(this string filename, string replaceChar)
        {
            //string regexSearch = new string(System.IO.Path.GetInvalidFileNameChars()) + new string(System.IO.Path.GetInvalidPathChars());
            //Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            //return r.Replace(filename, replaceChar);
            return (new string(System.IO.Path.GetInvalidFileNameChars()) + new string(System.IO.Path.GetInvalidPathChars())).Aggregate(filename, (current, c) => current.Replace(c.ToString(), replaceChar));
        }


    }
}
