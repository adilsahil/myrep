/*
 * ----------------------------------------------------------------------
 * <copyright file="Common.cs" company="GI Infologics PVT Ltd">
 *      Copyright (c) Kameda Infologics Pvt Ltd. All rights reserved.
 * </copyright>
 * <author>Arun Rajan T.</author>
 * <Date>31-Dec-2009<Date>
 * <ModiDate>22-Apr-2010<Date>
 * 
 * <Modiauthor>Arun Rajan T.</Modiauthor>
 * <ModiDate>12-Jan-2011<Date>
 * <Purpose>Added three functions for GetAgeInString(),getAge</Purpose>
 * <Modiauthor>Arun Rajan T.</Modiauthor>
 * <ModiDate>26-Mar-2014<Date>
 * <Purpose>Given function comments,added function'RemoveInvalidFileNameCharacters',modified function 'GetFormattedName'</Purpose>
 * -----------------------------------------------------------------------
*/

namespace Infologics.Medilogics.General.Control
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Data;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Media.Imaging;
    using System.Windows.Documents;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Ink;
    using Infologics.Medilogics.General.Control.Classes;
    using System.Reflection;
    using System.Configuration;
    using Infologics.Medilogics.Enumerators.General;
    using Infologics.Medilogics.Enumerators.Investigations;
    using System.Windows.Controls;
    using System.ComponentModel;
    using Infologics.Medilogics.Enumerators.EMR;
    using System.Drawing;

    /// <summary>
    /// 
    /// </summary>
    public class Common
    {
        /// <summary>
        /// Converts the image fileto bytes.
        /// </summary>
        /// <param name="ImageFilePath">The image file path.</param>
        /// <returns></returns>
        public byte[] ConvertImageFiletoBytes(string ImageFilePath)
        {
            FileStream FStream = null;
            try
            {
                FStream = new FileStream(ImageFilePath, FileMode.Open, System.IO.FileAccess.Read);
                return this.ConvertImageFiletoBytes(FStream);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (FStream != null)
                {
                    FStream.Close();
                    FStream.Dispose();
                }
            }
        }

        /// <summary>
        /// This Function will helps to Convert the Given File Path to Bytes array.
        /// </summary>
        /// <param name="FStream">FileStream</param>
        /// <returns>
        /// byte[]
        /// </returns>
        public byte[] ConvertImageFiletoBytes(FileStream FStream)
        {
            Byte[] objByteArray = null;
            BinaryReader objBinaryReader = null;
            try
            {
                if (FStream != null)
                {
                    objBinaryReader = new BinaryReader(FStream);
                    long _NumBytes = FStream.Length;
                    objByteArray = objBinaryReader.ReadBytes(Convert.ToInt32(_NumBytes));
                    _NumBytes = 0;

                }
                return objByteArray;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (objBinaryReader != null)
                {
                    objBinaryReader.Close();
                }
            }
        }

        /// <summary>
        /// Determines whether the specified obj value is numeric.
        /// </summary>
        /// <param name="objValue">The obj value.</param>
        /// <returns>
        ///   <c>true</c> if the specified obj value is numeric; otherwise, <c>false</c>.
        /// </returns>
        public bool IsNumeric(object objValue)
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

        //public MemoryStream ConvertBytesToImageFile(Byte[] byteArryayData)
        //{
        //    try
        //    {
        //        return new MemoryStream(byteArryayData);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    finally
        //    {

        //    }
        //    //System.IO.MemoryStream ms = new System.IO.MemoryStream(byteArryayData);
        //    //Image empimage = Image.FromStream(ms);
        //    //pictureBox1.Image = empimage;
        //}
        /// <summary>
        /// Creates the bill number format for registation,consultation investigation etc.
        /// </summary>
        /// <param name="PrevBillNo">The PrevBillNo.</param>
        /// <param name="chrType">Type of the CHR.</param>
        /// <returns></returns>
        public string CreateBillNumberFormat(string PrevBillNo, char? chrType)
        {
            try
            {
                string BILLNO = string.Empty, YEAR = string.Empty;
                if (PrevBillNo.Equals(string.Empty) || PrevBillNo.Equals("0") || PrevBillNo.Length > 10 || PrevBillNo.Length < 10 ||
                       PrevBillNo.Substring(PrevBillNo.Length - 2, 2).ToString() != (DateTime.Now.ToString("yy").ToString()))
                {
                    BILLNO = chrType + "1".PadLeft(7, '0') + DateTime.Now.ToString("yy").ToString();
                }
                else
                {
                    if (PrevBillNo.Length >= 3 && PrevBillNo.Substring(PrevBillNo.Length - 2, 2).Equals(DateTime.Now.ToString("yy").ToString()))
                    {
                        PrevBillNo = PrevBillNo.Substring(1, PrevBillNo.Length - 3);
                        YEAR = DateTime.Now.ToString("yy").ToString();
                        BILLNO = chrType + (Convert.ToInt64(PrevBillNo) + 1).ToString().PadLeft(7, '0').ToString();
                        BILLNO = BILLNO + YEAR;
                    }
                }
                return BILLNO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Convert Month and Year to Days
        /// </summary>
        /// <param name="intAge">Age</param>
        /// <param name="AgeUnit">Age unit</param>
        /// <returns>
        /// Days
        /// </returns>
        public int ConvertToDays(int intAge, short AgeUnit)
        {
            Int32 Age;
            if (AgeUnit == 1) //month
            {
                Age = Convert.ToInt32(intAge * 30.4);
            }
            else if (AgeUnit == 2)//years
            {
                Age = Convert.ToInt32(intAge * 30.4 * 12);
            }
            else
            {
                Age = intAge;
            }
            return Age;
        }

        /// <summary>
        /// Gets the age in string.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="currentDate">The current date.</param>
        /// <returns></returns>
        public string GetAgeInString(DateTime dateOfBirth, DateTime currentDate)
        {
            try
            {
                int years = 0;
                int months = 0;
                int days = 0;
                // Find years
                years = currentDate.Year - dateOfBirth.Year;
                // Check if the last year was a full year. 
                if (currentDate < dateOfBirth.AddYears(years) && years != 0)
                {
                    --years;
                }
                dateOfBirth = dateOfBirth.AddYears(years);
                // check dateOfBirth <= endDate and the diff between them is < 1 year. 
                if (dateOfBirth.Year == currentDate.Year)
                {
                    months = currentDate.Month - dateOfBirth.Month;
                }
                else
                {
                    months = (12 - dateOfBirth.Month) + currentDate.Month;
                }
                // Check if the last month was a full month.

                if (currentDate < dateOfBirth.AddMonths(months) && months != 0)
                {
                    --months;
                }
                dateOfBirth = dateOfBirth.AddMonths(months);
                //  dateOfBirth < endDate and is within 1 month of each other.
                days = (currentDate - dateOfBirth).Days;
                return GetFormatedAgeInString(years, months, days);
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Gets the formated age in string.
        /// </summary>
        /// <param name="years">The years.</param>
        /// <param name="months">The months.</param>
        /// <param name="days">The days.</param>
        /// <returns></returns>
        private string GetFormatedAgeInString(int years, int months, int days)
        {
            try
            {
                ///<Summary> By : Krishanan Nambuthiri Suggestion.
                ///Age should be displayed as per age rule in print view 
                ///Age Rule: 
                ///============================================================= 
                ///Current Date -----> Today         
                ///1 to 30 days ------> Age in days (e.g. 5d) 
                ///31 days to 3 Months -----> Age in months and days  (Eg:1m 10d) 
                ///3 to 12 Months ------> Age should be in Months (6m) 
                ///1 to 3 years ------> Both Years and in Months  (2y 6m) 
                ///3+ Years ------> Years (Eg:25y)  
                ///</Summary>

                string AgeDay = string.Empty;
                string AgeMonth = string.Empty;
                string AgeYear = string.Empty;
                if (days > 0)
                {
                    AgeDay = days + "d";
                }
                if (months > 0)
                {
                    AgeMonth = months + "m ";
                }
                if (years > 0)
                {
                    AgeYear = years + "y ";
                }
                if (years == 0 && months == 0 && days == 0)
                {
                    ///Current Date -----> Today
                    AgeDay = "Today";
                }
                if (years == 0 && months >= 3 && months <= 12)
                {
                    ///3 to 12 Months ------> Age should be in Months (6m) 
                    AgeDay = string.Empty;
                }
                if (years >= 1 && years <= 3)
                {
                    ///1 to 3 years ------> Both Years and in Months  (2y 6m) 
                    AgeDay = string.Empty;
                }
                if (years >= 3)
                {
                    ///3+ Years ------> Years (Eg:25y) 
                    AgeDay = string.Empty;
                    AgeMonth = string.Empty;
                }
                return AgeYear + AgeMonth + AgeDay;
                //return years + " Years " + months + " Months " + days + " Days";
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the age.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="currentDate">The current date.</param>
        /// <returns>
        /// DataTable, Having Columns "Years","Months","Days","AgeInDays","AgeInString"
        /// </returns>
        public DataTable GetAge(DateTime dateOfBirth, DateTime currentDate)
        {
            try
            {
                int years = 0;
                int months = 0;
                int days = 0;
                GetAge(ref dateOfBirth, ref currentDate, ref years, ref months, ref days);
                DataTable dtAge = new DataTable();
                dtAge.Columns.Add("Years", typeof(int));
                dtAge.Columns.Add("Months", typeof(int));
                dtAge.Columns.Add("Days", typeof(int));
                dtAge.Columns.Add("AgeInDays", typeof(int));
                dtAge.Columns.Add("AgeInString", typeof(string));
                DataRow drRow = dtAge.NewRow();

                drRow["Years"] = years;
                drRow["Months"] = months;
                drRow["Days"] = days;
                drRow["AgeInDays"] = this.ConvertToDays((int)years, (short)Infologics.Medilogics.Enumerators.General.AgeUnit.Years) + this.ConvertToDays((int)months, (short)Infologics.Medilogics.Enumerators.General.AgeUnit.Months) + this.ConvertToDays((int)days, (short)Infologics.Medilogics.Enumerators.General.AgeUnit.Days);
                drRow["AgeInString"] = GetFormatedAgeInString(years, months, days);
                dtAge.Rows.Add(drRow);
                return dtAge;

            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Gets the age.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="years">The years.</param>
        /// <param name="months">The months.</param>
        /// <param name="days">The days.</param>
        public static void GetAge(ref DateTime dateOfBirth, ref DateTime endDate, ref int years, ref int months, ref int days)
        {
            // Find years
            years = endDate.Year - dateOfBirth.Year;
            // Check if the last year was a full year. 
            if (endDate < dateOfBirth.AddYears(years) && years != 0)
            {
                --years;
            }
            dateOfBirth = dateOfBirth.AddYears(years);
            // check dateOfBirth <= endDate and the diff between them is < 1 year. 
            if (dateOfBirth.Year == endDate.Year)
            {
                months = endDate.Month - dateOfBirth.Month;
            }
            else
            {
                months = (12 - dateOfBirth.Month) + endDate.Month;
            }
            // Check if the last month was a full month.

            if (endDate < dateOfBirth.AddMonths(months) && months != 0)
            {
                --months;
            }
            dateOfBirth = dateOfBirth.AddMonths(months);
            //  dateOfBirth < endDate and is within 1 month of each other.
            days = (endDate - dateOfBirth).Days;
        }

        /// <summary>
        /// This Function will check whether that column is already exist if so Gets the value of that field.
        /// </summary>
        /// <param name="drRowItem">The dr row item.</param>
        /// <param name="strColumnName">Name of the STR column.</param>
        /// <returns></returns>
        public object GetRowValue(DataRow drRowItem, string strColumnName)
        {
            try
            {
                object objReturnValue = DBNull.Value;
                if (drRowItem != null && drRowItem.Table != null && drRowItem.Table.Columns != null &&
                    drRowItem.Table.Columns.Contains(strColumnName))
                {
                    if (drRowItem.RowState != DataRowState.Deleted)
                    {
                        objReturnValue = drRowItem[strColumnName];
                    }
                    else
                    {
                        objReturnValue = drRowItem[strColumnName, DataRowVersion.Original];
                    }
                }
                return objReturnValue;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// An Extension Method to allow us t odo "The Title Of It".asTitleCase()
        /// which would return a TitleCased string.
        /// </summary>
        /// <param name="title">Title to work with.</param>
        /// <returns>
        /// Output title as TitleCase
        /// </returns>
        /// which would return a TitleCased string.
        public string asTitleCase(string title)
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
        /// Sets the parameter value.
        /// </summary>
        /// <param name="ParamArray">The param array.</param>
        /// <param name="ParameterName">Name of the parameter.</param>
        /// <param name="drRow">The dr row.</param>
        /// <param name="columnName">Name of the column.</param>
        public void SetIDbDataParameterValue(IDbDataParameter[] ParamArray, string ParameterName, DataRow drRow, string columnName)
        {
            if (ParamArray.Where(x => x.ParameterName == ParameterName).Count() > 0)
            {
                ParamArray.Where(x => x.ParameterName == ParameterName).First().Value = this.GetRowValue(drRow, columnName);
            }
        }

        /// <summary>
        /// Sets the parameter value.
        /// </summary>
        /// <param name="ParamArray">The param array.</param>
        /// <param name="ParameterName">Name of the parameter.</param>
        /// <param name="SettingValue">The setting value.</param>
        public void SetIDbDataParameterValue(IDbDataParameter[] ParamArray, string ParameterName, object SettingValue)
        {
            if (ParamArray.Where(x => x.ParameterName == ParameterName).Count() > 0)
            {
                ParamArray.Where(x => x.ParameterName == ParameterName).First().Value = SettingValue;
            }
        }

        /// <summary>
        /// Gets the parameter value.
        /// </summary>
        /// <param name="ParamArray">The param array.</param>
        /// <param name="ParameterName">Name of the parameter.</param>
        /// <returns></returns>
        public object GetIDbDataParameterValue(IDbDataParameter[] ParamArray, string ParameterName)
        {
            return ParamArray.Where(x => x.ParameterName == ParameterName).Count() > 0 ? ParamArray.Where(x => x.ParameterName == ParameterName).First().Value : DBNull.Value;
        }


        /// <summary>
        /// Gets the formated text.
        /// </summary>
        /// <param name="Value">The value.</param>
        /// <param name="FontName">Name of the font.</param>
        /// <param name="FontSize">Size of the font.</param>
        /// <param name="FontColor">Color of the font.</param>
        /// <param name="isBold">if set to <c>true</c> [is bold].</param>
        /// <param name="isItalic">if set to <c>true</c> [is italic].</param>
        /// <param name="isUnderLine">if set to <c>true</c> [is under line].</param>
        /// <returns></returns>
        public Span getFormatedText(string Value, string FontName, byte FontSize, string FontColor
                            , bool isBold, bool isItalic, bool isUnderLine)
        {

            Span line = new Span(new Run(Value));
            if (isUnderLine)
            {
                TextDecoration objUnderline = new TextDecoration();
                objUnderline.Pen = new System.Windows.Media.Pen(getBrush(FontColor), 2);
                objUnderline.PenThicknessUnit = TextDecorationUnit.FontRecommended;
                objUnderline.PenOffset = 2;
                objUnderline.PenOffsetUnit = TextDecorationUnit.FontRecommended;

                TextDecorationCollection objCollection = new TextDecorationCollection();
                objCollection.Add(objUnderline);

                line.TextDecorations = objCollection;
            }
            if (isItalic) line = new Italic(line);
            if (isBold) line.FontWeight = FontWeights.Bold;
            line.FontFamily = new System.Windows.Media.FontFamily(FontName);
            line.FontSize = FontSize;
            line.Foreground = getBrush(FontColor);
            return line;
        }

        /// <summary>
        /// Gets the brush.
        /// </summary>
        /// <param name="Color">The color.</param>
        /// <returns></returns>
        public System.Windows.Media.Brush getBrush(string Color)
        {
            try
            {
                BrushConverter bc = new BrushConverter();
                SolidColorBrush brush = bc.ConvertFromString(Color) as SolidColorBrush;
                return brush;
            }
            catch
            {
                BrushConverter bc = new BrushConverter();
                SolidColorBrush brush = bc.ConvertFromString("Black") as SolidColorBrush;
                return brush;
            }

        }

        /// <summary>
        /// LINQs to data table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="varlist">The varlist.</param>
        /// <returns>
        /// DataTable
        /// </returns>
        public DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();
            // column names 
            PropertyInfo[] oProps = null;
            if (varlist != null)
            {
                foreach (T rec in varlist)
                {
                    // Use reflection to get property names, to create table, Only first time, others 
                    if (oProps == null)
                    {
                        oProps = ((Type)rec.GetType()).GetProperties();
                        foreach (PropertyInfo pi in oProps)
                        {
                            Type colType = pi.PropertyType;

                            if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                            {
                                colType = colType.GetGenericArguments()[0];
                            }
                            dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                        }
                    }
                    DataRow dr = dtReturn.NewRow();
                    foreach (PropertyInfo pi in oProps)
                    {
                        dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                        (rec, null);
                    }
                    dtReturn.Rows.Add(dr);
                }
            }
            return dtReturn;
        }

        /// <summary>
        /// Converter Column values to a string using string seprator
        /// </summary>
        /// <param name="Data">The data.</param>
        /// <param name="ColumnName">Name of the column.</param>
        /// <param name="Separator">The separator.</param>
        /// <returns>
        /// DataTable
        /// </returns>
        public string GetColumnValuesWithSeparator(DataTable Data, string ColumnName, string Separator)
        {
            string Value = string.Empty;
            if (Data != null && Data.Rows.Count > 0)
            {
                Value = string.Join(Separator, (from row in Data.AsEnumerable()
                                                select row[ColumnName].ToString()).ToArray());
            }
            return Value;
        }

        /// <summary>
        /// Creates the object reflection wise
        /// </summary>
        /// <param name="ServiceName">Name of the service.</param>
        /// <returns></returns>
        public object CreateObject(string ServiceName)
        {
            try
            {
                //Create the control object using reflection
                string[] Str;
                Str = ServiceName.Split(new char[] { ',' });
                object obj;
                //Load the servicename object using reflection 
                obj = Activator.CreateInstance(Assembly.Load(Str[0]).GetType(Str[1]), true);
                return obj;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Reads the key and values in config
        /// </summary>
        /// <param name="Keys">The keys.</param>
        /// <returns></returns>
        public DataTable ReadKeyAndValues(string[] Keys)
        {
            DataTable dtSettings = new DataTable();
            dtSettings.Columns.Add("KEY");
            dtSettings.Columns.Add("VALUE");
            string Value;
            foreach (var item in Keys)
            {
                Value = ConfigurationSettings.AppSettings[item.ToString()];
                if (Value != null)
                {
                    dtSettings.Rows.Add(item.ToString(), Value);
                }
            }

            return dtSettings;
        }

        /// <summary>
        /// Sets the date.
        /// </summary>
        /// <param name="dtProDate">The dt pro date.</param>
        /// <returns>
        /// Formated Date.ie, Second Avoided DateTime
        /// </returns>
        public DateTime SetDate(DateTime dtProDate)
        {
            return Convert.ToDateTime(dtProDate.ToString("dd-MMM-yyyy HH:mm"));
        }

        /// <summary>
        /// Determines whether [has dates overlapping] [the specified x start date].
        /// </summary>
        /// <param name="xStartDate">The x start date.</param>
        /// <param name="xEndDate">The x end date.</param>
        /// <param name="yStartDate">The y start date.</param>
        /// <param name="yEndDate">The y end date.</param>
        /// <returns>
        ///   <c>true</c> if [has dates overlapping] [the specified x start date]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasDatesOverlapping(DateTime xStartDate, DateTime xEndDate, DateTime yStartDate, DateTime yEndDate)
        {
            return (xStartDate <= yEndDate) && (xEndDate >= yStartDate);
        }
        /// <summary>
        /// This Function will check the 'xDate' having value if value existing then return the Date else this will return the MaxDate
        /// </summary>
        /// <param name="xDate">The x date.</param>
        /// <returns>
        /// This Function will check the 'xDate' having value if value existing then return the Date else this will return the MaxDate
        /// </returns>
        public DateTime GetNullableDateToDate(DateTime? xDate)
        {
            return xDate.HasValue ? xDate.Value : DateTime.MaxValue;
        }


        /// <summary>
        /// Gets the exact decimal.
        /// </summary>
        /// <param name="decValue">The dec value.</param>
        /// <param name="numberOfDecimalPlaces">The number of decimal places.</param>
        /// <returns></returns>
        public decimal GetExactDecimal(decimal decValue, int numberOfDecimalPlaces)
        {
            decimal returnVal = 0;
            if (numberOfDecimalPlaces > 0)
            {
                //if 3 then 1000 and divide by 1000
                //if 2 then  100 and divide by 100
                //if 1 then   10 and divide by 10
                //if 0 then give truncate
                ////double multiVal = Math.Pow(10, Convert.ToDouble(numberOfDecimalPlaces));
                ////returnVal = Math.Truncate((decValue * 100 * Convert.ToDecimal(multiVal))) / Convert.ToDecimal(multiVal);
                int ind = decValue.ToString().IndexOf(".");
                int totalLen = decValue.ToString().Length;
                if (ind != -1 && (totalLen - (ind + 1)) > numberOfDecimalPlaces)
                {
                    returnVal = Convert.ToDecimal(decValue.ToString().Substring(0, ind + 1 + numberOfDecimalPlaces));
                }
                else
                {
                    returnVal = decValue;
                }

            }
            else if (numberOfDecimalPlaces == 0)
            {
                returnVal = Math.Truncate(decValue);
            }
            else
            {
                returnVal = decValue;
            }
            return returnVal;
        }

        /// <summary>
        /// Sets the normal variation setting.
        /// </summary>
        /// <param name="NormalRange">The normal range.</param>
        /// <param name="PanicRange">The panic range.</param>
        /// <param name="SelectedTestResultRow">The selected test result row.</param>
        public void SetNormalVariationSetting(DataRow NormalRange, DataRow PanicRange, DataRow SelectedTestResultRow)
        {
            try
            {
                DataTable dtCurrentNewTable = null;
                DataRow drCurrentTempRow = null;
                // bool isDataUpdatedInCurrentRow = false;
                if (SelectedTestResultRow != null)
                {
                    //We are doing the all Manipulation in "dtCurrentRow"
                    dtCurrentNewTable = SelectedTestResultRow.Table.Clone();

                    if (dtCurrentNewTable.Columns.Contains("ISNOTNORMAL") == false)
                    {
                        dtCurrentNewTable.Columns.Add("ISNOTNORMAL");
                    }
                    if (dtCurrentNewTable.Columns.Contains("ISPANIC") == false)
                    {
                        dtCurrentNewTable.Columns.Add("ISPANIC");
                        drCurrentTempRow = dtCurrentNewTable.Rows.Add(SelectedTestResultRow.ItemArray);
                    }
                    else
                    {
                        //isDataUpdatedInCurrentRow = true;
                        drCurrentTempRow = SelectedTestResultRow;
                    }
                    if (dtCurrentNewTable.Columns.Contains("WARNING_NOTE") == false)
                    {
                        dtCurrentNewTable.Columns.Add("WARNING_NOTE");
                        drCurrentTempRow["WARNING_NOTE"] = string.Empty;
                    }
                    drCurrentTempRow["ISNOTNORMAL"] = -1;
                    drCurrentTempRow["ISPANIC"] = -1;
                }
                //if (VariantsRangeTable != null)

                object value = SelectedTestResultRow["RESULT"];
                Double resultValue = 0;
                //COMPARISON_OPERATOR,VALUE,MIN_VALUE,MAX_VALUE
                if (SelectedTestResultRow != null && SelectedTestResultRow["RESULT_TYPE"] != DBNull.Value)
                {
                    if (Convert.ToInt16(SelectedTestResultRow["RESULT_TYPE"]) == Convert.ToInt16(ResultDataType.Integer)
                        || Convert.ToInt16(SelectedTestResultRow["RESULT_TYPE"]) == Convert.ToInt16(ResultDataType.Float))
                    {
                        drCurrentTempRow["ISNOTNORMAL"] = Convert.ToInt16(NormalVariation.NotSpecified); //-1;
                        drCurrentTempRow["ISPANIC"] = Convert.ToInt16(NormalVariation.NotSpecified); //-1;
                        if (NormalRange != null && NormalRange["COMPARISON_OPERATOR"] != DBNull.Value)
                        {
                            if (value != null && value.ToString().Trim() != string.Empty && Double.TryParse(Convert.ToString(value), out resultValue) == true)
                            {
                                string strResult = Convert.ToString(value);
                                if (Convert.ToInt32(NormalRange["COMPARISON_OPERATOR"])
                                    == Convert.ToInt32(ComparisonOperator.RangeOfValues))
                                {
                                    if (Convert.ToDouble(value) < Convert.ToDouble(NormalRange["MIN_VALUE"]) ||
                                            Convert.ToDouble(value) > Convert.ToDouble(NormalRange["MAX_VALUE"]))
                                    {
                                        drCurrentTempRow["ISNOTNORMAL"] = 1;
                                        if (Convert.ToDouble(value) < Convert.ToDouble(NormalRange["MIN_VALUE"]))
                                        {
                                            SelectedTestResultRow["NORMAL_VARIATION"] = Convert.ToInt16(NormalVariation.BelowNormalRange);
                                        }
                                        else if (Convert.ToDouble(value) > Convert.ToDouble(NormalRange["MAX_VALUE"]))
                                        {
                                            SelectedTestResultRow["NORMAL_VARIATION"] = Convert.ToInt16(NormalVariation.AboveNormalRange);
                                        }
                                    }
                                    else
                                    {
                                        drCurrentTempRow["ISNOTNORMAL"] = 0;
                                    }
                                }
                                else if (Convert.ToInt32(NormalRange["COMPARISON_OPERATOR"])
                                    == Convert.ToInt32(ComparisonOperator.Equal))
                                {
                                    if (Convert.ToDouble(value) != Convert.ToDouble(NormalRange["VALUE"]))
                                    {
                                        drCurrentTempRow["ISNOTNORMAL"] = 1;
                                        if (Convert.ToDouble(value) < Convert.ToDouble(NormalRange["VALUE"]))
                                        {
                                            SelectedTestResultRow["NORMAL_VARIATION"] = Convert.ToInt16(NormalVariation.BelowNormalRange);
                                        }
                                        else if (Convert.ToDouble(value) > Convert.ToDouble(NormalRange["VALUE"]))
                                        {
                                            SelectedTestResultRow["NORMAL_VARIATION"] = Convert.ToInt16(NormalVariation.AboveNormalRange);
                                        }
                                    }
                                    else
                                    {
                                        drCurrentTempRow["ISNOTNORMAL"] = 0;
                                    }
                                }
                                else if (Convert.ToInt32(NormalRange["COMPARISON_OPERATOR"])
                                    == Convert.ToInt32(ComparisonOperator.GreaterThan))
                                {
                                    if (Convert.ToDouble(value) <= Convert.ToDouble(NormalRange["VALUE"]))
                                    {
                                        drCurrentTempRow["ISNOTNORMAL"] = 1;
                                        SelectedTestResultRow["NORMAL_VARIATION"] = Convert.ToInt16(NormalVariation.BelowNormalRange);
                                    }
                                    else
                                    {
                                        drCurrentTempRow["ISNOTNORMAL"] = 0;
                                        SelectedTestResultRow["NORMAL_VARIATION"] = Convert.ToInt16(NormalVariation.NormalRange);
                                    }
                                }
                                else if (Convert.ToInt32(NormalRange["COMPARISON_OPERATOR"])
                                    == Convert.ToInt32(ComparisonOperator.GreaterThanAndEqual))
                                {
                                    if (Convert.ToDouble(value) < Convert.ToDouble(NormalRange["VALUE"]))
                                    {
                                        drCurrentTempRow["ISNOTNORMAL"] = 1;
                                        SelectedTestResultRow["NORMAL_VARIATION"] = Convert.ToInt16(NormalVariation.BelowNormalRange);
                                    }
                                    else
                                    {
                                        drCurrentTempRow["ISNOTNORMAL"] = 0;
                                    }
                                }
                                else if (Convert.ToInt32(NormalRange["COMPARISON_OPERATOR"])
                                    == Convert.ToInt32(ComparisonOperator.LessThan))
                                {
                                    if (Convert.ToDouble(value) >= Convert.ToDouble(NormalRange["VALUE"]))
                                    {
                                        drCurrentTempRow["ISNOTNORMAL"] = 1;
                                        SelectedTestResultRow["NORMAL_VARIATION"] = Convert.ToInt16(NormalVariation.AboveNormalRange);
                                    }
                                    else
                                    {
                                        drCurrentTempRow["ISNOTNORMAL"] = 0;
                                    }
                                }
                                else if (Convert.ToInt32(NormalRange["COMPARISON_OPERATOR"])
                                    == Convert.ToInt32(ComparisonOperator.LessThanAndEqual))
                                {
                                    if (Convert.ToDouble(value) > Convert.ToDouble(NormalRange["VALUE"]))
                                    {
                                        drCurrentTempRow["ISNOTNORMAL"] = 1;
                                        SelectedTestResultRow["NORMAL_VARIATION"] = Convert.ToInt16(NormalVariation.AboveNormalRange);
                                    }
                                    else
                                    {
                                        drCurrentTempRow["ISNOTNORMAL"] = 0;
                                    }
                                }
                            }
                        }
                        if (PanicRange != null && PanicRange["COMPARISON_OPERATOR"] != DBNull.Value)
                        {
                            if (value != null && value.ToString().Trim() != string.Empty && Double.TryParse(Convert.ToString(value), out resultValue) == true)
                            {
                                string strResult = Convert.ToString(value);
                                if (Convert.ToInt32(PanicRange["COMPARISON_OPERATOR"])
                                    == Convert.ToInt32(ComparisonOperator.RangeOfValues))
                                {
                                    if (Convert.ToDouble(value) < Convert.ToDouble(PanicRange["MIN_VALUE"]) ||
                                        Convert.ToDouble(value) > Convert.ToDouble(PanicRange["MAX_VALUE"]))
                                    {
                                        drCurrentTempRow["ISPANIC"] = 1;
                                        drCurrentTempRow["WARNING_NOTE"] = PanicRange["WARNING_NOTE"];
                                        if (Convert.ToDouble(value) < Convert.ToDouble(PanicRange["MIN_VALUE"]))
                                        {
                                            SelectedTestResultRow["NORMAL_VARIATION"] = Convert.ToInt16(NormalVariation.BelowPanic);
                                        }
                                        else if (Convert.ToDouble(value) > Convert.ToDouble(PanicRange["MAX_VALUE"]))
                                        {
                                            SelectedTestResultRow["NORMAL_VARIATION"] = Convert.ToInt16(NormalVariation.AbovePanic);

                                        }
                                    }
                                }
                                else if (Convert.ToInt32(PanicRange["COMPARISON_OPERATOR"])
                                    == Convert.ToInt32(ComparisonOperator.Equal))
                                {
                                    if (Convert.ToDouble(value) == Convert.ToDouble(PanicRange["VALUE"]))
                                    {
                                        drCurrentTempRow["ISPANIC"] = 1;
                                        drCurrentTempRow["WARNING_NOTE"] = PanicRange["WARNING_NOTE"];
                                        if (Convert.ToDouble(value) < Convert.ToDouble(PanicRange["VALUE"]))
                                        {
                                            SelectedTestResultRow["NORMAL_VARIATION"] = Convert.ToInt16(NormalVariation.BelowPanic);
                                        }
                                        else if (Convert.ToDouble(value) > Convert.ToDouble(PanicRange["VALUE"]))
                                        {
                                            SelectedTestResultRow["NORMAL_VARIATION"] = Convert.ToInt16(NormalVariation.AbovePanic);
                                        }
                                    }
                                }
                                else if (Convert.ToInt32(PanicRange["COMPARISON_OPERATOR"])
                                    == Convert.ToInt32(ComparisonOperator.GreaterThan))
                                {
                                    if (Convert.ToDouble(value) > Convert.ToDouble(PanicRange["VALUE"]))
                                    {
                                        drCurrentTempRow["ISPANIC"] = 1;
                                        drCurrentTempRow["WARNING_NOTE"] = PanicRange["WARNING_NOTE"];
                                        SelectedTestResultRow["NORMAL_VARIATION"] = Convert.ToInt16(NormalVariation.AbovePanic);
                                    }
                                }
                                else if (Convert.ToInt32(PanicRange["COMPARISON_OPERATOR"])
                                    == Convert.ToInt32(ComparisonOperator.GreaterThanAndEqual))
                                {
                                    if (Convert.ToDouble(value) >= Convert.ToDouble(PanicRange["VALUE"]))
                                    {
                                        drCurrentTempRow["ISPANIC"] = 1;
                                        drCurrentTempRow["WARNING_NOTE"] = PanicRange["WARNING_NOTE"];
                                        SelectedTestResultRow["NORMAL_VARIATION"] = Convert.ToInt16(NormalVariation.AbovePanic);
                                    }
                                }
                                else if (Convert.ToInt32(PanicRange["COMPARISON_OPERATOR"])
                                    == Convert.ToInt32(ComparisonOperator.LessThan))
                                {
                                    if (Convert.ToDouble(value) < Convert.ToDouble(PanicRange["VALUE"]))
                                    {
                                        drCurrentTempRow["ISPANIC"] = 1;
                                        drCurrentTempRow["WARNING_NOTE"] = PanicRange["WARNING_NOTE"];
                                        SelectedTestResultRow["NORMAL_VARIATION"] = Convert.ToInt16(NormalVariation.BelowPanic);
                                    }
                                }
                                else if (Convert.ToInt32(PanicRange["COMPARISON_OPERATOR"])
                                    == Convert.ToInt32(ComparisonOperator.LessThanAndEqual))
                                {
                                    if (Convert.ToDouble(value) <= Convert.ToDouble(PanicRange["VALUE"]))
                                    {
                                        drCurrentTempRow["ISPANIC"] = 1;
                                        drCurrentTempRow["WARNING_NOTE"] = PanicRange["WARNING_NOTE"];
                                        SelectedTestResultRow["NORMAL_VARIATION"] = Convert.ToInt16(NormalVariation.BelowPanic);
                                    }
                                }
                            }
                        }
                        if (Convert.ToInt16(drCurrentTempRow["ISNOTNORMAL"]) == 0)
                        {
                            SelectedTestResultRow["NORMAL_VARIATION"] = Convert.ToInt16(NormalVariation.NormalRange);
                        }
                        else if (Convert.ToInt16(drCurrentTempRow["ISNOTNORMAL"]) == -1 &&
                            Convert.ToInt16(drCurrentTempRow["ISPANIC"]) == -1)
                        {
                            SelectedTestResultRow["NORMAL_VARIATION"] = Convert.ToInt16(NormalVariation.NotSpecified);
                        }
                    }
                    else if (Convert.ToInt16(SelectedTestResultRow["RESULT_TYPE"]) == Convert.ToInt16(ResultDataType.String)
                        && (!SelectedTestResultRow.Table.Columns.Contains("STRING_RANGE_VALIDATION") || Convert.ToString(SelectedTestResultRow["STRING_RANGE_VALIDATION"]) != "1"))
                    {
                        if (NormalRange != null && NormalRange["COMPARISON_OPERATOR"] != DBNull.Value)
                        {
                            if (Convert.ToInt32(NormalRange["COMPARISON_OPERATOR"])
                                        == Convert.ToInt32(ComparisonOperator.StringValue))
                            {
                                if (Convert.ToString(SelectedTestResultRow["RESULT"]).Trim().ToUpper() != Convert.ToString(NormalRange["VALUE"]).Trim().ToUpper())
                                {
                                    drCurrentTempRow["ISNOTNORMAL"] = 1;
                                    SelectedTestResultRow["NORMAL_VARIATION"] = Convert.ToInt16(NormalVariation.BelowNormalRange);
                                }
                                else
                                {
                                    drCurrentTempRow["ISNOTNORMAL"] = 0;
                                    drCurrentTempRow["WARNING_NOTE"] = NormalRange["WARNING_NOTE"];
                                    SelectedTestResultRow["NORMAL_VARIATION"] = Convert.ToInt16(NormalVariation.NormalRange);
                                }
                            }
                            else
                            {
                                drCurrentTempRow["ISNOTNORMAL"] = -1;
                                //SelectedTestResultRow["WARNING_NOTE"] = "";
                                SelectedTestResultRow["NORMAL_VARIATION"] = Convert.ToInt16(NormalVariation.NotSpecified);
                            }
                        }
                        else
                        {
                            drCurrentTempRow["ISNOTNORMAL"] = -1;
                            //SelectedTestResultRow["WARNING_NOTE"] = "";
                            SelectedTestResultRow["NORMAL_VARIATION"] = Convert.ToInt16(NormalVariation.NotSpecified);
                        }
                    }
                    else
                    {
                        //SelectedTestResultRow["WARNING_NOTE"] = "";
                        drCurrentTempRow["ISNOTNORMAL"] = -1;
                        SelectedTestResultRow["NORMAL_VARIATION"] = Convert.ToInt16(NormalVariation.NotSpecified);
                    }
                    ///Set the Default Remarks based on the Normal Variation
                    if (!string.IsNullOrEmpty(Convert.ToString(SelectedTestResultRow["NORMAL_VARIATION"]))
                        && SelectedTestResultRow.Table.Columns.Contains("REMARKS") && SelectedTestResultRow.Table.Columns.Contains("INV_PAT_TEST_RESULT_ID")
                        && 
                        (string.IsNullOrEmpty(Convert.ToString(SelectedTestResultRow["INV_PAT_TEST_RESULT_ID"]))
                        ||
                        (!string.IsNullOrEmpty(Convert.ToString(SelectedTestResultRow["INV_PAT_TEST_RESULT_ID"])) 
                        && Convert.ToInt64(SelectedTestResultRow["INV_PAT_TEST_RESULT_ID"]) < 0))
                        )
                    {
                        switch ((NormalVariation)Convert.ToInt16(SelectedTestResultRow["NORMAL_VARIATION"]))
                        {
                            case NormalVariation.NormalRange:
                                if (NormalRange != null && NormalRange.Table.Columns.Contains("NORMAL_RESULT_REMARKS")
                                    && string.IsNullOrEmpty(Convert.ToString(SelectedTestResultRow["REMARKS"])))
                                {
                                    SelectedTestResultRow["REMARKS"] = NormalRange["NORMAL_RESULT_REMARKS"];
                                }                                
                                break;  
                            case NormalVariation.AboveNormalRange:
                            case NormalVariation.BelowNormalRange:
                                if (NormalRange != null && NormalRange.Table.Columns.Contains("ABNORMAL_RESULT_REMARKS")
                                    && string.IsNullOrEmpty(Convert.ToString(SelectedTestResultRow["REMARKS"])))                                        
                                {
                                    SelectedTestResultRow["REMARKS"] = NormalRange["ABNORMAL_RESULT_REMARKS"];
                                }
                                break;                          
                            case NormalVariation.AbovePanic:
                            case NormalVariation.BelowPanic:
                                if (PanicRange != null && PanicRange.Table.Columns.Contains("PANIC_RESULT_REMARKS")
                                    && string.IsNullOrEmpty(Convert.ToString(SelectedTestResultRow["REMARKS"])))
                                {
                                    SelectedTestResultRow["REMARKS"] = PanicRange["PANIC_RESULT_REMARKS"];
                                }
                                break;                                          
                            default:
                                break;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// It will be invoke the specified method in a particular type.
        /// </summary>
        /// <param name="DLLName">Name of dll</param>
        /// <param name="ControlName">Full name of control</param>
        /// <param name="MethodName">method to invoke</param>
        /// <param name="MethodParameters">Method parameters in a object array</param>
        /// <returns></returns>
        public object InvokeDLLMethod(string DLLName, string ControlName, string MethodName, object[] MethodParameters)
        {
            try
            {
                MethodInfo objminfo = null;
                object obj = new object();
                string dllPath = string.Empty;
                object objReturnValue = null;
                //dllPath = "C:\\Documents and Settings\\anish\\Desktop\\Test\\WpfApplication5\\ClassLibrary1\\bin\\Debug" + "\\" + dllName;
                Assembly asm = Assembly.LoadFile(DLLName);
                obj = asm.CreateInstance(ControlName);
                Type[] paramTypes = new Type[MethodParameters.Count()];
                for (int i = 0; i < MethodParameters.Count(); i++)
                {
                    paramTypes[i] = MethodParameters[i].GetType();
                }
                objminfo = obj.GetType().GetMethod(MethodName, paramTypes);
                if (objminfo != null)
                {
                    objReturnValue = objminfo.Invoke(obj, MethodParameters);
                }
                return objReturnValue;
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// Validate the URI
        /// </summary>
        /// <param name="Value">The value.</param>
        /// <returns>
        ///   <c>true</c> if [is valid URL] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValidUrl(string Value)
        {
            bool isValid = false;
            ////string pattern = @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";

            string pattern = @"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$";
            Regex reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            isValid = reg.IsMatch(Value);

            return isValid;
        }

        /// <summary>
        /// Set ListBox scroll position
        /// </summary>
        /// <param name="lboObj">The lbo obj.</param>
        /// <param name="position">The position.</param>
        public void SetListBoxScrollPosition(ref ListBox lboObj, int position)
        {
            try
            {
                //position -> 0   ToTop
                //position -> 1   ToBottom
                //position -> 2   ToLeft End
                //position -> 3   ToRight End
                if (lboObj.Items.Count > 1)
                {
                    if (VisualTreeHelper.GetChildrenCount(lboObj) > 0)
                    {
                        Decorator border = VisualTreeHelper.GetChild(lboObj, 0) as Decorator;
                        if (border != null)
                        {
                            // Get scrollviewer      
                            ScrollViewer scrollViewer = border.Child as ScrollViewer;
                            if (scrollViewer != null)
                            {
                                if (position == 0)
                                {
                                    scrollViewer.ScrollToTop();
                                }
                                else if (position == 1)
                                {
                                    scrollViewer.ScrollToBottom();
                                }
                                else if (position == 2)
                                {
                                    scrollViewer.ScrollToLeftEnd();
                                }
                                else if (position == 3)
                                {
                                    scrollViewer.ScrollToRightEnd();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Check given url is valid or not.  If http or https or ftp is missing it will be appended
        /// </summary>
        /// <param name="Value">The value.</param>
        /// <returns>
        ///   <c>true</c> if [is valid URL] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValidUrl(ref string Value)
        {
            System.Globalization.CompareInfo cmpUrl = System.Globalization.CultureInfo.InvariantCulture.CompareInfo;
            if ((cmpUrl.IsPrefix(Value, "http://") == false) && (cmpUrl.IsPrefix(Value, "https://") == false) && (cmpUrl.IsPrefix(Value, "ftp://") == false))
            {
                Value = "http://" + Value;
            }
            return IsValidUrl(Value);
        }

        /// <summary>
        /// Rounding the passed value
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="decimalPlace">The decimal place.</param>
        /// <returns></returns>
        public static decimal MathRound(decimal value, int decimalPlace)
        {
            return Math.Round(value, decimalPlace, MidpointRounding.AwayFromZero);
        }
        /// <summary>
        /// Check the valid email
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>
        ///   <c>true</c> if [is valid email] [the specified email]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidEmail(string email)
        {
            return new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Match(email).Success;
        }

        /// <summary>
        /// Returns the Description attribute of a Enumerator if exists
        /// If a description does not exist,the default enum value is returned
        /// </summary>
        /// <param name="currentEnum">The current enum.</param>
        /// <returns></returns>
        public string GetDescription(Enum currentEnum)
        {
            try
            {
                FieldInfo fi = currentEnum.GetType().
                            GetField(currentEnum.ToString());
                DescriptionAttribute da = (DescriptionAttribute)Attribute.GetCustomAttribute(fi,
                                                                                             typeof(DescriptionAttribute));
                string description = da != null ? da.Description : currentEnum.ToString();

                return description;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Generates the bitmap.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public BitmapImage GenerateBitmap(byte[] data)
        {
            System.Windows.Media.Imaging.BitmapImage bitmapImg = null;
            try
            {
                if (data != null)
                {
                    MemoryStream ms = new MemoryStream(data);
                    bitmapImg = new System.Windows.Media.Imaging.BitmapImage();
                    bitmapImg.BeginInit();
                    bitmapImg.StreamSource = new MemoryStream(ms.ToArray());
                    bitmapImg.EndInit();

                }
                return bitmapImg;
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Converts the UI control to string.
        /// </summary>
        /// <param name="oUiControl">The o UI control.</param>
        /// <returns></returns>
        public string ConvertUIControlToString(object oUiControl)
        {
            string sUiControlString = string.Empty;
            try
            {
                sUiControlString = System.Windows.Markup.XamlWriter.Save(oUiControl);
            }
            catch
            {
                throw;
            }
            return sUiControlString;

        }

        /// <summary>
        /// Converts the string to UI control.
        /// </summary>
        /// <param name="sUIString">The s UI string.</param>
        /// <returns></returns>
        public object ConvertStringToUIControl(string sUIString)
        {
            object oUIControl = null;
            try
            {
                oUIControl = System.Windows.Markup.XamlReader.Load(new MemoryStream(Encoding.UTF8.GetBytes(sUIString)));
            }
            catch
            {
                throw;
            }
            return oUIControl;
        }

        /// <summary>
        /// Creates the bl DLL object.
        /// </summary>
        /// <param name="sBlDll">The s bl DLL.</param>
        /// <returns></returns>
        public object CreateBlDllObject(string sBlDll)
        {
            object oBllDllInstance = null;
            try
            {

                string dllName = sBlDll.Substring(0, sBlDll.IndexOf(","));
                string controlName = sBlDll.ToString().Substring(sBlDll.IndexOf(",") + 1);
                //string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                //string sMode = System.Configuration.ConfigurationSettings.AppSettings["ModeOfCommunication"].ToString().ToUpper();
                //if (sMode.ToUpper() == "WCF")
                //{
                //    baseDir = baseDir + "bin\\";
                //}
                //string assemblyPath = baseDir + dllName;
                string baseDir = System.IO.Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
                string assemblyPath = baseDir + "\\" + dllName;
                string nameSpace = dllName.Substring(0, dllName.Length - 4);

                //Loading the UI Dll
                Assembly assembly = Assembly.LoadFile(assemblyPath);
                oBllDllInstance = assembly.CreateInstance(nameSpace + "." + controlName);
            }
            catch
            {
                throw;
            }
            return oBllDllInstance;
        }
        /// <summary>
        /// Get the base directory of the Application
        /// </summary>
        /// <returns></returns>
        public string GetBaseDirectory()
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string sMode = System.Configuration.ConfigurationSettings.AppSettings["ModeOfCommunication"].ToString().ToUpper();
                if (sMode.ToUpper() == "WCF")
                {
                    baseDir = baseDir + "bin\\";
                }
                //Added By Fayaz
                else if (sMode.ToUpper() == "LOCAL")
                {
                    if (System.Web.HttpRuntime.AppDomainAppId != null)
                    {
                        //is web app
                        baseDir = baseDir + "bin\\";
                    }
                    else
                    {
                        //is windows app
                        baseDir = baseDir;
                    }
                }
                //Commented by Arun
                //else
                //{
                //    baseDir = baseDir + "bin\\";// Added By Fayaz
                //}
                return baseDir;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Serializes the custom class object.
        /// </summary>
        /// <param name="templateClassObject">The template class object.</param>
        /// <param name="TypeParameters">The type parameters.</param>
        /// <returns></returns>
        public object SerializeCustomClassObject(object templateClassObject, params Type[] TypeParameters)
        {
            string strReturnData = string.Empty;
            try
            {
                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                using (System.IO.StreamReader reader = new System.IO.StreamReader(memoryStream))
                {
                    System.Runtime.Serialization.DataContractSerializer serializer =
                        new System.Runtime.Serialization.DataContractSerializer(templateClassObject.GetType()
                            , TypeParameters);
                    serializer.WriteObject(memoryStream, templateClassObject);
                    memoryStream.Position = 0;
                    strReturnData = reader.ReadToEnd();
                }
            }
            catch
            {
                throw;
            }
            return strReturnData;
        }

        /// <summary>
        /// Deserializes the custom class object.
        /// </summary>
        /// <param name="strSerializedData">The s STRXML.</param>
        ///  <param name="oType">Dezerialize to which Type . Eg: Deserialize the string To type: Infologics.Medilogics.General.Control.Classes.TemplateData()</param>
        /// <param name="TypeParameters">The type parameters.</param
        /// 
        /// <returns></returns>
        public object DeserializeCustomClassObject(string strSerializedData, Type oType, params Type[] TypeParameters)
        {
            //object oReturn= new Infologics.Medilogics.General.Control.Classes.TemplateData();
            object oReturn = null;
            try
            {
                using (System.IO.Stream stream = new System.IO.MemoryStream())
                {
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(strSerializedData);
                    stream.Write(data, 0, data.Length);
                    stream.Position = 0;
                    System.Runtime.Serialization.DataContractSerializer deserializer = new System.Runtime.Serialization.
                                                                                                DataContractSerializer(oType, TypeParameters);

                    if (deserializer.IsStartObject(new System.Xml.XmlTextReader(stream)))
                    {
                        stream.Position = 0;
                        oReturn = deserializer.ReadObject(stream);
                    }

                    // oReturn = deserializer.ReadObject(stream);
                }
            }
            catch
            {
                throw;
            }
            return oReturn;
        }
        /// <summary>
        /// get the Template Data object from string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public TemplateData GetTemplateDataObjectFromString(string obj)
        {
            try
            {
                Type[] oTypeArr = new Type[] { typeof(Infologics.Medilogics.CommonXSD.XSD.CommonShared.EMR_AUDITDataTable) };
                return DeserializeCustomClassObject(obj, typeof(TemplateData), oTypeArr) as TemplateData;
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Determines whether the specified expression is date.
        /// </summary>
        /// <param name="Expression">The expression.</param>
        /// <returns>
        /// 	<c>true</c> if the specified expression is date; otherwise, <c>false</c>.
        /// </returns>
        public bool IsDate(object Expression)
        {
            bool isDate = false;
            if (Expression != null)
            {
                if (Expression is DateTime)
                {
                    isDate = true;
                }
                if (Expression is string)
                {
                    DateTime date;
                    isDate = DateTime.TryParse(Expression.ToString(), out date);
                }
            }
            return isDate;
        }



        public byte[] ConvertStreamToByteArray(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
        /// Converts an enum to a DataTable.  
        /// /// </summary>        
        /// <param name="enumType">The enum to convert.</param>       
        /// /// <param name="keyFieldName">The desired name of the key column.</param>       
        /// /// <param name="valueFieldName">the desired name of the value column.</param>        
        /// <returns>A DataTable with the name/value pairs from the enum.</returns>
        public DataTable EnumToDataTable(Type enumType, string strKeyFieldName, string strValueFieldName)
        {            //Check inputs:  
            if (strKeyFieldName == String.Empty)
                strKeyFieldName = "KEY";
            if (strValueFieldName == String.Empty)
                strValueFieldName = "VALUE";
            if (strKeyFieldName == strValueFieldName)
                throw new Exception("Key and Value column names must be different.");
            //Create the DataTable with the desired columns:           
            DataTable table = new DataTable();
            table.Columns.Add(strValueFieldName, typeof(string));
            table.Columns.Add(strKeyFieldName, Enum.GetUnderlyingType(enumType));
            //Add the items from the enum:            
            foreach (string name in Enum.GetNames(enumType))
            {
                Enum currentEnum = System.Enum.Parse(enumType, name) as Enum;
                table.Rows.Add(this.GetDescription(currentEnum), currentEnum);
            }
            return table;
        }



        /// <summary>
        /// Get the Vital signs body mas index
        /// </summary>
        /// <param name="strHeight"></param>
        /// <param name="strWeight"></param>
        /// <returns></returns>
        public static string GetBmiValue(string strHeight, string strWeight, string strHeightUnit, string strWeightUnit)
        {
            try
            {
                string BMI = String.Empty;

                if (strWeight != String.Empty && strHeight != String.Empty)
                {
                    if ((strHeight == null ? 0 : strHeight.Length) > 0 && (strWeight == null ? 0 : strWeight.Length) > 0)
                    {
                        float sngHeight;
                        float sngWeight;
                        if (Single.TryParse(strWeight, out sngWeight) && Single.TryParse(strHeight, out sngHeight))
                        {
                            if (Convert.ToString(strHeightUnit).ToUpper() == "FEET" && Convert.ToString(strWeightUnit).ToUpper() == "LBS")
                            {
                                sngHeight = Convert.ToSingle(strHeight);
                                sngWeight = Convert.ToSingle(strWeight);

                                if (sngHeight != Single.MinValue && sngWeight != Single.MinValue)
                                {
                                    if (sngHeight > 0)
                                    {

                                        ////pounds and  inches
                                        //BMI = Convert.ToString(Math.Round(((sngWeight / ((sngHeight * 12) * (sngHeight * 12))) * 703), 3));

                                        ////pounds and Feet  

                                        string[] split = sngHeight.ToString().Split('.');
                                        if (split.Length == 2)
                                        {
                                            float firstValue = float.Parse(split[0]);
                                            float secondValue = float.Parse(split[1]);
                                            float Factor = (secondValue / 12);
                                            sngHeight = firstValue + Factor;
                                        }
                                        BMI = Convert.ToString(Math.Round(((sngWeight * 4.88) / (sngHeight * sngHeight)), 3));
                                    }
                                    else
                                    {
                                        BMI = "0";
                                    }
                                }
                                else
                                {
                                    BMI = String.Empty;
                                }
                            }
                            else
                            {
                                sngHeight = Convert.ToSingle(strHeight);
                                sngWeight = Convert.ToSingle(strWeight);

                                if (strHeight != String.Empty && sngHeight > 0)
                                {
                                    sngHeight = sngHeight / 100;
                                }
                                if (sngHeight != Single.MinValue && sngWeight != Single.MinValue)
                                {
                                    if (sngHeight > 0)
                                    {
                                        BMI = Convert.ToString(Math.Round(sngWeight / Math.Pow(sngHeight, 2), 3));
                                    }
                                    else
                                    {
                                        BMI = "0";
                                    }
                                }
                                else
                                {
                                    BMI = String.Empty;
                                }
                            }
                        }
                    }
                }
                return BMI;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Gets the bsa value.
        /// </summary>
        public static string GetBsaValue(string strHeight, string strWeight, string strHeightUnit, string strWeightUnit)
        {
            try
            {
                string BSA = String.Empty;

                if (strWeight != String.Empty && strHeight != String.Empty)
                {
                    if ((strHeight == null ? 0 : strHeight.Length) > 0 && (strWeight == null ? 0 : strWeight.Length) > 0)
                    {
                        if (Convert.ToString(strHeightUnit).ToUpper() == "FEET" && Convert.ToString(strWeightUnit).ToUpper() == "LBS")
                        {
                            float sngHeight;
                            float sngWeight;
                            if (Single.TryParse(strWeight, out sngWeight) && Single.TryParse(strHeight, out sngHeight))
                            {
                                sngHeight = Convert.ToSingle(strHeight);
                                sngWeight = Convert.ToSingle(strWeight);

                                if (sngHeight != Single.MinValue && sngWeight != Single.MinValue)
                                {
                                    //BSA = Convert.ToString(Math.Round(0.0235 * Math.Pow(sngHeight, 0.42246) * Math.Pow(sngWeight, 0.51456), 3));
                                    //BSA = Convert.ToString((((sngHeight * 12) * sngWeight) / 3131) / 2);
                                    BSA = Convert.ToString(Math.Round(((((sngHeight * 12) * sngWeight) / 3131) / 2), 3));
                                }
                                else
                                {
                                    BSA = String.Empty;
                                }
                            }
                        }
                        else
                        {
                            float sngHeight;
                            float sngWeight;
                            if (Single.TryParse(strWeight, out sngWeight) && Single.TryParse(strHeight, out sngHeight))
                            {
                                sngHeight = Convert.ToSingle(strHeight);
                                sngWeight = Convert.ToSingle(strWeight);

                                if (sngHeight != Single.MinValue && sngWeight != Single.MinValue)
                                {
                                    BSA = Convert.ToString(Math.Round(0.0235 * Math.Pow(sngHeight, 0.42246) * Math.Pow(sngWeight, 0.51456), 3));
                                }
                                else
                                {
                                    BSA = String.Empty;
                                }
                            }
                        }
                    }
                }
                return BSA;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the image fileto bytes with impersionation.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public byte[] GetImageFiletoBytesWithImpersionation(string filePath)
        {
            byte[] imageByte = null;
            if (Infologics.Medilogics.General.Control.Classes.Impersionation.Impersionation.GetImpersionation())
            {
                imageByte = ConvertImageFiletoBytes(filePath);
            }
            return imageByte;
        }

        /// <summary>
        /// Gets the name of the formatted.
        /// </summary>
        /// <param name="strTitle">The title.</param>
        /// <param name="strFirstName">The first name.</param>
        /// <param name="strMiddleName">Name of the middle.</param>
        /// <param name="strLastName">The last name.</param>
        /// <param name="strFamilyName">Name of the family.</param>
        /// <param name="intMode">The mode. if '0' then in Indian formate. if '1' then US Formate</param>
        /// <returns>Formated Name</returns>
        public string GetFormattedName(string strTitle, string strFirstName, string strMiddleName, string strLastName, string strFamilyName, int intMode)
        {
            try
            {
                StringBuilder strName = new StringBuilder();
                if (intMode == 0)
                {
                    updateStringData(strTitle, strName);
                    updateStringData(strFirstName, strName);
                    updateStringData(strMiddleName, strName);
                    updateStringData(strLastName, strName);
                    updateStringData(strFamilyName, strName);
                }
                else if (intMode == 1)
                {
                    updateStringData(strTitle, strName);
                    updateStringData(strLastName, strName);
                    updateStringData(strFirstName, strName);
                    updateStringData(strMiddleName, strName);
                    updateStringData(strFamilyName, strName);
                }
                return strName.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Updates the string data.
        /// </summary>
        /// <param name="strCurrentData">The STR current data.</param>
        /// <param name="strCommon">The STR common.</param>
        private void updateStringData(string strCurrentData, StringBuilder strCommon)
        {
            if (strCurrentData != null && strCurrentData != string.Empty)
            {
                if (strCommon.Length > 0)
                {
                    strCommon.Append(" ");
                }
                strCommon.Append(strCurrentData);
            }
        }


        /// <summary>
        /// Removes the invalid file path characters.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="replaceChar">The replace char.</param>
        /// <returns></returns>
        public string RemoveInvalidFileNameCharacters(string filename, string replaceChar)
        {
            //string regexSearch = new string(System.IO.Path.GetInvalidFileNameChars()) + new string(System.IO.Path.GetInvalidPathChars());
            //Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            //return r.Replace(filename, replaceChar);
            return (new string(System.IO.Path.GetInvalidFileNameChars()) + new string(System.IO.Path.GetInvalidPathChars())).Aggregate(filename, (current, c) => current.Replace(c.ToString(), replaceChar));
        }

        /// <summary>
        /// To return the datatable object of a entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DataTable EntityToDataTable<T>(T entity) where T : class
        {
            var properties = typeof(T).GetProperties();
            var table = new DataTable();

            foreach (var property in properties)
            {
                table.Columns.Add(property.Name, property.PropertyType);
            }

            table.Rows.Add(properties.Select(p => p.GetValue(entity, null)).ToArray());
            return table;
        }
        /// <summary>
        /// It will remove the End zero value of the converted string decimal value.
        /// Eg: if string is 1.0020. The actual Decimal value was 1.002. So we need to remove the end zero
        /// We can use this function in this knid of senario
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public string GetDecimalStringValueEndZeroExclusion(string Value)
        {
            try
            {
                string strConvertedValue = Value;
                decimal dOutValue = 0;
                decimal.TryParse(Value, out dOutValue);
                if (dOutValue != 0)
                {
                    //strConvertedValue = dOutValue.ToString().TrimEnd('0').TrimEnd('.');
                    string[] strarr = dOutValue.ToString().Split('.');
                    if (strarr.Length == 2 && strarr[1].EndsWith("0"))
                    {
                        if (Convert.ToDecimal(strarr[1]) == 0)
                        {
                            strConvertedValue = strarr[0];
                        }
                        else
                        {
                            strConvertedValue = strarr[0] + "." + strarr[1].TrimEnd('0');
                        }
                    }
                }
                return strConvertedValue;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// ModifiedParam 0 - MinDose, 1 - MaxDose, 2 - MinQTY, 3 - MaxQty
        /// dsData - MEDICATION_DATA, EMR_PH_RECOMMEND_DOSE
        /// </summary>
        /// <param name="dsData"></param>
        /// <param name="Mode"></param>
        /// <returns></returns>
        public DataTable MedicineDoseQtyCalculation(DataSet dsData, Int16 ModifiedParam)
        {
            try
            {
                string strIn = string.Empty;
                Int16 Mode = 0;//0 - Dose, 1- Qty
                DataTable dtMedData = dsData.Tables["MEDICATION_DATA"].Copy();
                Decimal dc = Decimal.MinValue;
                Decimal RecDose = Decimal.MinValue;
                Decimal RecDoseQty = Decimal.MinValue;
                if (dsData.Tables.Contains("MEDICATION_DATA") && dsData.Tables["MEDICATION_DATA"].Rows.Count > 0 &&
                    dsData.Tables.Contains("EMR_PH_RECOMMEND_DOSE") && dsData.Tables["EMR_PH_RECOMMEND_DOSE"].Rows.Count > 0)
                {
                    if (!dtMedData.Columns.Contains("NEW_MIN_DOSE"))
                    {
                        dtMedData.Columns.Add("NEW_MIN_DOSE", typeof(decimal));
                    }
                    if (!dtMedData.Columns.Contains("NEW_MAX_DOSE"))
                    {
                        dtMedData.Columns.Add("NEW_MAX_DOSE", typeof(decimal));
                    }
                    if (!dtMedData.Columns.Contains("NEW_MIN_QTY"))
                    {
                        dtMedData.Columns.Add("NEW_MIN_QTY", typeof(decimal));
                    }
                    if (!dtMedData.Columns.Contains("NEW_MAX_QTY"))
                    {
                        dtMedData.Columns.Add("NEW_MAX_QTY", typeof(decimal));
                    }

                    foreach (DataRow drItem in dtMedData.Rows)
                    {
                        var res = from sel in dsData.Tables["EMR_PH_RECOMMEND_DOSE"].AsEnumerable()
                                  where sel["GENERIC_ID"] != DBNull.Value && drItem["GENERIC_ID"] != DBNull.Value
                                  && Convert.ToDecimal(sel["GENERIC_ID"]) == Convert.ToDecimal(drItem["GENERIC_ID"])
                                  select sel;
                        if (res.Count() > 0)
                        {
                            if (ModifiedParam == 0 || ModifiedParam == 1)
                            {
                                Mode = 0;
                            }
                            else
                            {
                                Mode = 1;
                            }

                            switch (ModifiedParam)
                            {
                                case 0:
                                    RecDose = res.FirstOrDefault()["DOSE"] != DBNull.Value ? Convert.ToDecimal(res.FirstOrDefault()["DOSE"]) : Decimal.MinValue;
                                    RecDoseQty = res.FirstOrDefault()["QUANTITY"] != DBNull.Value ? Convert.ToDecimal(res.FirstOrDefault()["QUANTITY"]) : Decimal.MinValue;
                                    dc = drItem["DOSE"] != DBNull.Value ? Convert.ToDecimal(drItem["DOSE"]) : Decimal.MinValue;
                                    if (dc != Decimal.MinValue && RecDose != Decimal.MinValue && RecDoseQty != Decimal.MinValue)
                                    {
                                        drItem["NEW_MIN_QTY"] = Calculate(RecDose, RecDoseQty, dc, Mode);
                                    }
                                    break;

                                case 1:
                                    RecDose = res.FirstOrDefault()["MAX_DOSE"] != DBNull.Value ? Convert.ToDecimal(res.FirstOrDefault()["MAX_DOSE"]) : Decimal.MinValue;
                                    RecDoseQty = res.FirstOrDefault()["MAX_QUANTITY"] != DBNull.Value ? Convert.ToDecimal(res.FirstOrDefault()["MAX_QUANTITY"]) : Decimal.MinValue;
                                    dc = drItem["MAX_DOSE"] != DBNull.Value ? Convert.ToDecimal(drItem["MAX_DOSE"]) : Decimal.MinValue;
                                    if (dc != Decimal.MinValue && RecDose != Decimal.MinValue && RecDoseQty != Decimal.MinValue)
                                    {
                                        drItem["NEW_MAX_QTY"] = Calculate(RecDose, RecDoseQty, dc, Mode);
                                    }
                                    break;

                                case 2:
                                    RecDose = res.FirstOrDefault()["DOSE"] != DBNull.Value ? Convert.ToDecimal(res.FirstOrDefault()["DOSE"]) : Decimal.MinValue;
                                    RecDoseQty = res.FirstOrDefault()["QUANTITY"] != DBNull.Value ? Convert.ToDecimal(res.FirstOrDefault()["QUANTITY"]) : Decimal.MinValue;
                                    dc = drItem["QUANTITY"] != DBNull.Value ? Convert.ToDecimal(drItem["QUANTITY"]) : Decimal.MinValue;
                                    if (dc != Decimal.MinValue && RecDose != Decimal.MinValue && RecDoseQty != Decimal.MinValue)
                                    {
                                        drItem["NEW_MIN_DOSE"] = Calculate(RecDose, RecDoseQty, dc, Mode);
                                    }
                                    break;

                                case 3:
                                    RecDose = res.FirstOrDefault()["MAX_DOSE"] != DBNull.Value ? Convert.ToDecimal(res.FirstOrDefault()["MAX_DOSE"]) : Decimal.MinValue;
                                    RecDoseQty = res.FirstOrDefault()["MAX_QUANTITY"] != DBNull.Value ? Convert.ToDecimal(res.FirstOrDefault()["MAX_QUANTITY"]) : Decimal.MinValue;
                                    dc = drItem["MAX_QUANTITY"] != DBNull.Value ? Convert.ToDecimal(drItem["MAX_QUANTITY"]) : Decimal.MinValue;
                                    if (dc != Decimal.MinValue && RecDose != Decimal.MinValue && RecDoseQty != Decimal.MinValue)
                                        if (dc != Decimal.MinValue && RecDose != Decimal.MinValue && RecDoseQty != Decimal.MinValue)
                                        {
                                            drItem["NEW_MAX_DOSE"] = Calculate(RecDose, RecDoseQty, dc, Mode);
                                        }
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                }
                return dtMedData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Mode 0 - Calculate Dose
        /// Mode 1 - Calculate Qty        
        private Decimal Calculate(Decimal RecDoseDose, Decimal RecDoseQty, Decimal Input, Int16 Mode)
        {
            try
            {

                Decimal dcOut = 0;
                if (Mode == 0)
                {
                    //Calculate Dose                   
                    //dcOut = Math.Round((((Input) / (RecDoseDose)) * RecDoseQty), 2);                                        
                    //dcOut = FormatReturnVal(Common.MathRound((((Input) / (RecDoseDose)) * RecDoseQty), 2));
                    dcOut = Math.Truncate((((Input) / (RecDoseDose)) * RecDoseQty) * 100) / 100;
                }
                else
                {
                    //Calculate Qty
                    //dcOut = Math.Round((((Input) / (RecDoseQty)) * RecDoseDose), 2);
                    //dcOut = FormatReturnVal(Common.MathRound((((Input) / (RecDoseQty)) * RecDoseDose), 2));
                    dcOut = Math.Truncate((((Input) / (RecDoseQty)) * RecDoseDose) * 100) / 100;

                }
                return dcOut;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// ModifiedParam 0 - Frequency, 1 - Qty, 2 - Rate
        /// dsData - MEDICATION_DATA, FrequencyData
        /// </summary>
        /// <param name="dsData"></param>
        /// <param name="Mode"></param>
        /// <returns></returns>
        public DataTable IVMedicineFreqQtyRateCalculation(DataSet dsData, Int16 ModifiedParam)
        {
            try
            {
                DataTable dtMedData = dsData.Tables["MEDICATION_DATA"].Copy();
                if (!dtMedData.Columns.Contains("NEW_FREQUENCY_ID"))
                {
                    dtMedData.Columns.Add("NEW_FREQUENCY_ID", typeof(decimal));
                }
                if (!dtMedData.Columns.Contains("NEW_RATE"))
                {
                    dtMedData.Columns.Add("NEW_RATE", typeof(decimal));
                }
                if (!dtMedData.Columns.Contains("NEW_QTY"))
                {
                    dtMedData.Columns.Add("NEW_QTY", typeof(decimal));
                }
                if (!dtMedData.Columns.Contains("CALCULATED_FIELD"))
                {
                    dtMedData.Columns.Add("CALCULATED_FIELD", typeof(decimal));
                }
                dtMedData.Rows[0]["CALCULATED_FIELD"] = -1;
                switch (ModifiedParam)
                {
                    case 0: //frequency Modified then calculate Rate
                        if (dsData.Tables["MEDICATION_DATA"].Rows[0]["FREQUENCY_ID"].KIIsNotNullOrEmpty())
                        {
                            DataRow[] selectedFreRow = dsData.Tables["FREQUENCY_DATA"].Select("EMR_LOOKUP_ID=" + dsData.Tables["MEDICATION_DATA"].Rows[0]["FREQUENCY_ID"].ToString());
                            if (selectedFreRow.KIIsNotNullOrEmpty())
                            {
                                if (selectedFreRow.First()["FIELD13"].KIIsNotNullOrEmpty())
                                {
                                    if (dsData.Tables["MEDICATION_DATA"].Rows[0]["QUANTITY"].KIIsNotNullOrEmpty() && Convert.ToDecimal(dsData.Tables["MEDICATION_DATA"].Rows[0]["QUANTITY"]) > 0)
                                    {

                                        decimal Rate = Convert.ToDecimal(dsData.Tables["MEDICATION_DATA"].Rows[0]["QUANTITY"]) / Convert.ToDecimal(selectedFreRow.First()["FIELD13"]);
                                        //Rate = Convert.ToDecimal(Math.Round(Convert.ToDouble(Rate), 2));
                                        //Rate = FormatReturnVal(Common.MathRound(Rate, 2));
                                        Rate = Math.Truncate(Rate * 100) / 100;
                                        dtMedData.Rows[0]["NEW_RATE"] = Rate;
                                        dtMedData.Rows[0]["CALCULATED_FIELD"] = 2;
                                    }
                                    else if (dsData.Tables["MEDICATION_DATA"].Rows[0]["RATE"].KIIsNotNullOrEmpty() && Convert.ToDecimal(dsData.Tables["MEDICATION_DATA"].Rows[0]["RATE"]) > 0)
                                    {
                                        decimal Qty = Convert.ToDecimal(selectedFreRow.First()["FIELD13"]) * Convert.ToDecimal(dsData.Tables["MEDICATION_DATA"].Rows[0]["RATE"]);
                                        //Qty = Convert.ToDecimal(Math.Round(Convert.ToDouble(Qty), 2));
                                        //  Qty = FormatReturnVal(Common.MathRound(Qty, 2));
                                        Qty = Math.Truncate(Qty * 100) / 100;
                                        dtMedData.Rows[0]["NEW_QTY"] = Qty;
                                        dtMedData.Rows[0]["CALCULATED_FIELD"] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    case 1: //Qty Modified then calculate Rate
                        if (dsData.Tables["MEDICATION_DATA"].Rows[0]["QUANTITY"].KIIsNotNullOrEmpty() && Convert.ToDecimal(dsData.Tables["MEDICATION_DATA"].Rows[0]["QUANTITY"]) > 0)
                        {
                            if (dsData.Tables["MEDICATION_DATA"].Rows[0]["FREQUENCY_ID"].KIIsNotNullOrEmpty())
                            {
                                DataRow[] selectedFreRow = dsData.Tables["FREQUENCY_DATA"].Select("EMR_LOOKUP_ID=" + dsData.Tables["MEDICATION_DATA"].Rows[0]["FREQUENCY_ID"].ToString());
                                if (selectedFreRow.KIIsNotNullOrEmpty())
                                {
                                    if (selectedFreRow.First()["FIELD13"].KIIsNotNullOrEmpty())
                                    {
                                        decimal Rate = Convert.ToDecimal(dsData.Tables["MEDICATION_DATA"].Rows[0]["QUANTITY"]) / Convert.ToDecimal(selectedFreRow.First()["FIELD13"]);
                                        //Rate = Convert.ToDecimal(Math.Round(Convert.ToDouble(Rate), 2));                                        
                                        //Rate = FormatReturnVal(Common.MathRound(Rate, 2));
                                        Rate = Math.Truncate(Rate * 100) / 100;
                                        dtMedData.Rows[0]["NEW_RATE"] = Rate;
                                        dtMedData.Rows[0]["CALCULATED_FIELD"] = 2;
                                    }
                                }
                            }
                            else if (dsData.Tables["MEDICATION_DATA"].Rows[0]["RATE"].KIIsNotNullOrEmpty() && Convert.ToDecimal(dsData.Tables["MEDICATION_DATA"].Rows[0]["RATE"]) > 0)
                            {
                                var FreqWithDuration = (from FreqData in dsData.Tables["FREQUENCY_DATA"].AsEnumerable()
                                                        where FreqData["FIELD13"].KIIsNotNullOrEmpty()
                                                        select FreqData);
                                if (FreqWithDuration.Count() > 0)
                                {
                                    decimal FreqJustBelowDuration = decimal.MinValue;
                                    decimal FreqJustAboveDuration = decimal.MinValue;
                                    decimal duration = Convert.ToDecimal(dsData.Tables["MEDICATION_DATA"].Rows[0]["QUANTITY"]) / Convert.ToDecimal(dsData.Tables["MEDICATION_DATA"].Rows[0]["RATE"]);

                                    var FreqAboveDuration = (from FreqData in FreqWithDuration.AsEnumerable()
                                                             where FreqData["FIELD13"].KIIsNotNullOrEmpty() &&
                                                             Convert.ToDecimal(FreqData["FIELD13"]) > duration
                                                             orderby Convert.ToDecimal(FreqData["FIELD13"]) ascending
                                                             select FreqData);

                                    if (FreqAboveDuration.Count() > 0)
                                    {
                                        FreqJustAboveDuration = Convert.ToDecimal(FreqAboveDuration.First()["FIELD13"]);
                                    }

                                    var FreqBelowDuration = (from FreqData in FreqWithDuration.AsEnumerable()
                                                             where FreqData["FIELD13"].KIIsNotNullOrEmpty() &&
                                                             Convert.ToDecimal(FreqData["FIELD13"]) <= duration
                                                             orderby Convert.ToDecimal(FreqData["FIELD13"]) descending
                                                             select FreqData);
                                    if (FreqBelowDuration.Count() > 0)
                                    {
                                        FreqJustBelowDuration = Convert.ToDecimal(FreqBelowDuration.First()["FIELD13"]);
                                    }

                                    if (FreqJustBelowDuration != decimal.MinValue && FreqJustAboveDuration == decimal.MinValue)
                                    {
                                        dtMedData.Rows[0]["NEW_FREQUENCY_ID"] = FreqBelowDuration.First()["EMR_LOOKUP_ID"];
                                    }
                                    else if (FreqJustAboveDuration != decimal.MinValue && FreqJustBelowDuration == decimal.MinValue)
                                    {
                                        dtMedData.Rows[0]["NEW_FREQUENCY_ID"] = FreqAboveDuration.First()["EMR_LOOKUP_ID"];
                                    }
                                    else
                                    {
                                        decimal DifferenceBelow = duration - FreqJustBelowDuration;
                                        decimal DifferenceAbove = FreqJustAboveDuration - duration;
                                        if (DifferenceBelow <= DifferenceAbove) //taken below
                                        {
                                            dtMedData.Rows[0]["NEW_FREQUENCY_ID"] = FreqBelowDuration.First()["EMR_LOOKUP_ID"];
                                        }
                                        else
                                        {
                                            dtMedData.Rows[0]["NEW_FREQUENCY_ID"] = FreqAboveDuration.First()["EMR_LOOKUP_ID"];
                                        }
                                    }
                                    dtMedData.Rows[0]["CALCULATED_FIELD"] = 0;
                                }
                            }
                        }
                        break;

                    case 2: //Rate modified then if qty available then calculate Frequency  else if freq available then calculate qty
                        if (dsData.Tables["MEDICATION_DATA"].Rows[0]["RATE"].KIIsNotNullOrEmpty() && Convert.ToDecimal(dsData.Tables["MEDICATION_DATA"].Rows[0]["RATE"]) > 0)
                        {
                            if (dsData.Tables["MEDICATION_DATA"].Rows[0]["QUANTITY"].KIIsNotNullOrEmpty() && Convert.ToDecimal(dsData.Tables["MEDICATION_DATA"].Rows[0]["QUANTITY"]) > 0)
                            {
                                var FreqWithDuration = (from FreqData in dsData.Tables["FREQUENCY_DATA"].AsEnumerable()
                                                        where FreqData["FIELD13"].KIIsNotNullOrEmpty()
                                                        select FreqData);
                                if (FreqWithDuration.Count() > 0)
                                {
                                    decimal FreqJustBelowDuration = decimal.MinValue;
                                    decimal FreqJustAboveDuration = decimal.MinValue;
                                    decimal duration = Convert.ToDecimal(dsData.Tables["MEDICATION_DATA"].Rows[0]["QUANTITY"]) / Convert.ToDecimal(dsData.Tables["MEDICATION_DATA"].Rows[0]["RATE"]);

                                    var FreqAboveDuration = (from FreqData in FreqWithDuration.AsEnumerable()
                                                             where FreqData["FIELD13"].KIIsNotNullOrEmpty() &&
                                                             Convert.ToDecimal(FreqData["FIELD13"]) > duration
                                                             orderby Convert.ToDecimal(FreqData["FIELD13"]) ascending
                                                             select FreqData);

                                    if (FreqAboveDuration.Count() > 0)
                                    {
                                        FreqJustAboveDuration = Convert.ToDecimal(FreqAboveDuration.First()["FIELD13"]);
                                    }

                                    var FreqBelowDuration = (from FreqData in FreqWithDuration.AsEnumerable()
                                                             where FreqData["FIELD13"].KIIsNotNullOrEmpty() &&
                                                             Convert.ToDecimal(FreqData["FIELD13"]) <= duration
                                                             orderby Convert.ToDecimal(FreqData["FIELD13"]) descending
                                                             select FreqData);
                                    if (FreqBelowDuration.Count() > 0)
                                    {
                                        FreqJustBelowDuration = Convert.ToDecimal(FreqBelowDuration.First()["FIELD13"]);
                                    }

                                    if (FreqJustBelowDuration != decimal.MinValue && FreqJustAboveDuration == decimal.MinValue)
                                    {
                                        dtMedData.Rows[0]["NEW_FREQUENCY_ID"] = FreqBelowDuration.First()["EMR_LOOKUP_ID"];
                                    }
                                    else if (FreqJustAboveDuration != decimal.MinValue && FreqJustBelowDuration == decimal.MinValue)
                                    {
                                        dtMedData.Rows[0]["NEW_FREQUENCY_ID"] = FreqAboveDuration.First()["EMR_LOOKUP_ID"];
                                    }
                                    else
                                    {
                                        decimal DifferenceBelow = duration - FreqJustBelowDuration;
                                        decimal DifferenceAbove = FreqJustAboveDuration - duration;
                                        if (DifferenceBelow <= DifferenceAbove)
                                        {
                                            dtMedData.Rows[0]["NEW_FREQUENCY_ID"] = FreqBelowDuration.First()["EMR_LOOKUP_ID"];
                                        }
                                        else
                                        {
                                            dtMedData.Rows[0]["NEW_FREQUENCY_ID"] = FreqAboveDuration.First()["EMR_LOOKUP_ID"];
                                        }
                                    }
                                    dtMedData.Rows[0]["CALCULATED_FIELD"] = 0;
                                }
                            }
                            else if (dsData.Tables["MEDICATION_DATA"].Rows[0]["FREQUENCY_ID"].KIIsNotNullOrEmpty())
                            {
                                DataRow[] selectedFreRow = dsData.Tables["FREQUENCY_DATA"].Select("EMR_LOOKUP_ID=" + dsData.Tables["MEDICATION_DATA"].Rows[0]["FREQUENCY_ID"].ToString());
                                if (selectedFreRow.KIIsNotNullOrEmpty())
                                {
                                    if (selectedFreRow.First()["FIELD13"].KIIsNotNullOrEmpty())
                                    {
                                        decimal Qty = Convert.ToDecimal(selectedFreRow.First()["FIELD13"]) * Convert.ToDecimal(dsData.Tables["MEDICATION_DATA"].Rows[0]["RATE"]);
                                        //Qty = FormatReturnVal(Common.MathRound(Qty, 2));
                                        Qty = Math.Truncate(Qty * 100) / 100;
                                        dtMedData.Rows[0]["NEW_QTY"] = Qty;
                                        dtMedData.Rows[0]["CALCULATED_FIELD"] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
                return dtMedData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool IsMedicineScheduled { get; set; }
        public decimal CalculateMedicineTotalQuantity(DataSet dsData, int mode)
        {
            try
            {
                DataTable dtschedulefreq = new DataTable();
                DataTable dtdtls = new DataTable();
                bool IsScheduledataAvailable = false;
                bool IsScheduleavailableforFrequency = false;
                bool IsLifelong = false;
                decimal totalQty = 0;
                int count = 0;
                Int16 isGas = Int16.MinValue;
                Int16 isinfusion = Int16.MinValue;
                Int64 FrequencyID = Int64.MinValue;
                Int64 Frequency_value = Int64.MinValue;
                DateTime Start_Date = DateTime.MinValue;
                DateTime End_Date = DateTime.MinValue;
                TimeSpan StartTime = TimeSpan.Zero;
                TimeSpan EndTime = TimeSpan.Zero;
                decimal quantity = decimal.MinValue;
                string admin_time = string.Empty;
                DateTime dtEnd = DateTime.MaxValue;
                bool isfrequencyFreetext = false;
                //DateTime StrStartTime; 
                //DateTime StrEndTime;
                //DateTime dtEnd = DateTime.MaxValue;
                //DateTime dtStart = DateTime.MinValue;
                if (mode != int.MinValue && mode == 1) // For OrderSet
                {
                    if (dsData != null && dsData.Tables.Contains("EMR_PAT_DTLS_PH_ORDER_VALIDATION") && dsData.Tables["EMR_PAT_DTLS_PH_ORDER_VALIDATION"].KIIsNotNullAndRowCount())
                    {
                        dtdtls = dsData.Tables["EMR_PAT_DTLS_PH_ORDER_VALIDATION"].Copy();
                    }
                }
                else if (mode != int.MinValue && mode == 2)  //For Direct Billing
                {
                    if (dsData != null && dsData.Tables.Contains("PH_PAT_DTLS_ORDER") && dsData.Tables["PH_PAT_DTLS_ORDER"].KIIsNotNullAndRowCount())
                    {
                        dtdtls = dsData.Tables["PH_PAT_DTLS_ORDER"].Copy();
                    }
                }
                else  //CPOE Medication
                {
                    if (dsData.Tables.Contains("EMR_PAT_DTLS_PH_ORDER") && dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].KIIsNotNullAndRowCount())
                    {
                        dtdtls = dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Copy();
                    }
                }
                if (dtdtls.KIIsNotNullAndRowCount())
                {
                    if (dtdtls.Columns.Contains("ISLIFELONG") && dtdtls.Rows[0]["ISLIFELONG"].KIIsNotNullOrEmpty() && Convert.ToInt16(dtdtls.Rows[0]["ISLIFELONG"]) == 1)
                    {
                        IsLifelong = true;
                    }
                    if (dtdtls.Columns.Contains("ISGAS") && dtdtls.Rows[0]["ISGAS"].KIIsNotNullOrEmpty())
                    {
                        isGas = Convert.ToInt16(dtdtls.Rows[0]["ISGAS"]);
                    }

                    if (dtdtls.Columns.Contains("ISINFUSION") && dtdtls.Rows[0]["ISINFUSION"].KIIsNotNullOrEmpty())
                    {
                        isinfusion = Convert.ToInt16(dtdtls.Rows[0]["ISINFUSION"]);
                    }
                    if (dtdtls.Columns.Contains("ISFREQUENCY_FREETEXT") && dtdtls.Rows[0]["ISFREQUENCY_FREETEXT"].KIIsNotNullOrEmpty() && Convert.ToInt16(dtdtls.Rows[0]["ISFREQUENCY_FREETEXT"]) == 1)
                    {
                        isfrequencyFreetext = true;
                    }

                    if (dtdtls.Rows[0]["FREQUENCY"].KIIsNotNullOrEmpty())
                    {
                        FrequencyID = Convert.ToInt64(dtdtls.Rows[0]["FREQUENCY"]);
                    }

                    if (dtdtls.Rows[0]["START_DATE"].KIIsNotNullOrEmpty())
                    {
                        Start_Date = Convert.ToDateTime(dtdtls.Rows[0]["START_DATE"]);
                    }

                    if (dtdtls.Rows[0]["START_DATE"].KIIsNotNullOrEmpty())
                    {
                        StartTime = TimeSpan.Parse(Convert.ToDateTime(dtdtls.Rows[0]["START_DATE"]).ToString("HH:mm:ss"));
                    }

                    if (dtdtls.Rows[0]["END_DATE"].KIIsNotNullOrEmpty())
                    {
                        End_Date = Convert.ToDateTime(dtdtls.Rows[0]["END_DATE"]);
                    }
                    if (dtdtls.Rows[0]["END_DATE"].KIIsNotNullOrEmpty())
                    {
                        EndTime = TimeSpan.Parse(Convert.ToDateTime(dtdtls.Rows[0]["END_DATE"]).ToString("HH:mm:ss"));
                    }
                    //if (dtdtls.Rows[0]["FREQUENCY_VALUE"].KIIsNotNullOrEmpty())
                    if (dtdtls.Columns.Contains("FREQUENCY_VALUE") && dtdtls.Rows[0]["FREQUENCY_VALUE"].KIIsNotNullOrEmpty())
                    {
                        Frequency_value = Convert.ToInt64(dtdtls.Rows[0]["FREQUENCY_VALUE"]);
                    }

                    if (dtdtls.Columns.Contains("ADMIN_TIME") && dtdtls.Rows[0]["ADMIN_TIME"].KIIsNotNullOrEmpty())
                    {
                        admin_time = Convert.ToString(dtdtls.Rows[0]["ADMIN_TIME"]);
                    }

                    if (dtdtls.Columns.Contains("ISQUANTITY_RANGE") && dtdtls.Columns.Contains("MAX_QUANTITY") && dtdtls.Rows[0]["ISQUANTITY_RANGE"].KIIsNotNullOrEmpty() && Convert.ToInt16(dtdtls.Rows[0]["ISQUANTITY_RANGE"]) == 1)
                    {
                        if (dtdtls.Rows[0]["MAX_QUANTITY"].KIIsNotNullOrEmpty())
                        {
                            quantity = Convert.ToDecimal(dtdtls.Rows[0]["MAX_QUANTITY"]);
                        }
                    }
                    else
                    {
                        //For direct billing Based dose ---qty is calculated so quantity value not requried to check
                        if (mode == 2 && dtdtls.Columns.Contains("DOSE") && dtdtls.Rows[0]["DOSE"].KIIsNotNullOrEmpty())
                        {
                            quantity = Convert.ToDecimal(dtdtls.Rows[0]["DOSE"]);
                        }

                        if (mode != 2 && dtdtls.Rows[0]["QUANTITY"].KIIsNotNullOrEmpty())
                        {
                            quantity = Convert.ToDecimal(dtdtls.Rows[0]["QUANTITY"]);
                        }
                    }
                }
                //conn = DALHelper.GetConnection();
                //CommonServer obj = new CommonServer();
                //DataTable dtcriteria = new DataTable();
                //dtcriteria.Columns.Add("MODE");
                //dtcriteria.Columns.Add("LOOKUP_TYPE");
                //DataRow drCriteria = dtcriteria.NewRow();
                //drCriteria["MODE"] = 7;
                //drCriteria["LOOKUP_TYPE"]="FREQUENCY";
                //dtcriteria.Rows.Add(drCriteria);
                DataTable dtFrequency = dsData.Tables["EMR_LOOKUP"].Copy();
                // DataTable dtFrequency =  //fetch lookup data - 'FREQUENCY'
                //DataRow drdtls = new DataRow();
                //drdtls["FREQUENCY"] = Frequency;


                if (FrequencyID != Int64.MinValue && FrequencyID != 0)
                {
                    DataRow[] drSelectedFrq = dtFrequency.Select("EMR_LOOKUP_ID=" + FrequencyID);
                    if (drSelectedFrq.Length > 0)
                    {
                        //if (drSelectedFrq[0]["FIELD15"] != DBNull.Value && Convert.ToInt32(drSelectedFrq[0]["FIELD15"]) == 1)
                        //{
                        //    IsScheduleavailableforFrequency = true;
                        //}
                        //else
                        //{
                        //    IsScheduleavailableforFrequency = false;
                        //}
                    }
                    if (Start_Date.ToString().Trim() != string.Empty && End_Date.ToString().Trim() != string.Empty)
                    {
                        DateTime Medstartdate = Convert.ToDateTime(Start_Date.ToString("dd-MMM-yyyy"));
                        DateTime MedEnddate = Convert.ToDateTime(End_Date.ToString("dd-MMM-yyyy"));
                        if (IsScheduleavailableforFrequency == true && IsLifelong == false && mode != 2)  //not from billing and not lifelong
                        {
                            if (dtdtls.KIIsNotNullAndRowCount())
                            {
                              // dtschedulefreq = GetMedicineScheduledDays(dtdtls.Rows[0]);
                            }
                            //else //
                            //{ 
                            //}
                          //  IsScheduledataAvailable = dtschedulefreq.KIIsNotNullAndRowCount();

                        }

                        //  DataRow[] drSelectedFrq1 = dtFrequency.Select("EMR_LOOKUP_ID=" + FrequencyID);
                        if (drSelectedFrq.Length > 0)
                        {

                            if (drSelectedFrq[0]["FIELD5"].ToString() != string.Empty && drSelectedFrq[0]["FIELD5"].ToString() == "1")
                            {
                                DateTime dtStartPrn = Convert.ToDateTime(Start_Date);
                                DateTime dtEndPrn = Convert.ToDateTime(End_Date);
                                if (IsMedicineScheduled == true)
                                {
                                    //if (IsScheduledataAvailable == true)
                                    //{
                                    //    if (IsAdminDateinScheduledDays(dtschedulefreq, dtStartPrn.Date) == true)
                                    //    {
                                    //        count++;
                                    //    }
                                    //}
                                }
                                else
                                {
                                    count++;
                                }
                                dtStartPrn = dtStartPrn.AddDays(1);
                                dtStartPrn = Convert.ToDateTime(dtStartPrn.ToString("dd-MMM-yyyy"));
                                if (dtStartPrn != null && dtEndPrn != null && dtEndPrn.Date != DateTime.MinValue.Date && dtEndPrn.Date != DateTime.MaxValue.Date && dtStartPrn.Date != DateTime.MinValue.Date && dtStartPrn.Date != DateTime.MaxValue.Date)
                                {
                                    while (dtStartPrn <= dtEndPrn)
                                    {
                                        if (IsMedicineScheduled == true)
                                        {
                                            //if (IsScheduledataAvailable == true)
                                            //{
                                            //    if (IsAdminDateinScheduledDays(dtschedulefreq, dtStartPrn.Date) == true)
                                            //    {
                                            //        count++;
                                            //    }
                                            //}
                                        }
                                        else
                                        {
                                            count++;
                                        }
                                        dtStartPrn = dtStartPrn.AddDays(1);
                                    }
                                }
                            }
                            else
                            {
                                switch (Convert.ToInt16(drSelectedFrq[0]["FIELD6"]))
                                {
                                    case (int)Infologics.Medilogics.Enumerators.EMR.FrequencyTypes.prn:
                                        DateTime dtStartPrn = Convert.ToDateTime(Start_Date);
                                        DateTime dtEndPrn = Convert.ToDateTime(End_Date);
                                        if (IsMedicineScheduled == true)
                                        {
                                            //if (IsScheduledataAvailable == true)
                                            //{
                                            //    if (IsAdminDateinScheduledDays(dtschedulefreq, dtStartPrn.Date) == true)
                                            //    {
                                            //        count++;
                                            //    }
                                            //}
                                        }
                                        else
                                        {
                                            count++;
                                        }
                                        dtStartPrn = dtStartPrn.AddDays(1);
                                        dtStartPrn = Convert.ToDateTime(dtStartPrn.ToString("dd-MMM-yyyy"));
                                        if (dtStartPrn != null && dtEndPrn != null && dtEndPrn.Date != DateTime.MinValue.Date && dtEndPrn.Date != DateTime.MaxValue.Date && dtStartPrn.Date != DateTime.MinValue.Date && dtStartPrn.Date != DateTime.MaxValue.Date)
                                        {
                                            while (dtStartPrn <= dtEndPrn)
                                            {
                                                if (IsMedicineScheduled == true)
                                                {
                                                    //if (IsScheduledataAvailable == true)
                                                    //{
                                                    //    if (IsAdminDateinScheduledDays(dtschedulefreq, dtStartPrn.Date) == true)
                                                    //    {
                                                    //        count++;
                                                    //    }
                                                    //}
                                                }
                                                else
                                                {
                                                    count++;
                                                }
                                                dtStartPrn = dtStartPrn.AddDays(1);
                                            }
                                        }
                                        break;
                                    case (int)Infologics.Medilogics.Enumerators.EMR.FrequencyTypes.Hourly:
                                        DateTime dtStartHr = Convert.ToDateTime(Start_Date);
                                        DateTime dtEndHr = Convert.ToDateTime(End_Date);
                                        if (IsMedicineScheduled == true)
                                        {
                                            //if (IsScheduledataAvailable == true)
                                            //{
                                            //    if (IsAdminDateinScheduledDays(dtschedulefreq, dtStartHr.Date) == true)
                                            //    {
                                            //        count++;
                                            //    }
                                            //}
                                        }
                                        else
                                        {
                                            count++;
                                        }
                                        if (Frequency_value != Int64.MinValue && Frequency_value.ToString().Trim() != "0")
                                        {
                                            dtStartHr = dtStartHr.AddHours(Convert.ToInt64(Frequency_value));
                                            if (dtStartHr != null && dtEndHr != null && dtEndHr.Date != DateTime.MinValue.Date && dtEndHr.Date != DateTime.MaxValue.Date && dtStartHr.Date != DateTime.MinValue.Date && dtStartHr.Date != DateTime.MaxValue.Date)
                                            {
                                                while (dtStartHr <= dtEndHr)
                                                {
                                                    if (IsMedicineScheduled == true)
                                                    {
                                                        //if (IsScheduledataAvailable == true)
                                                        //{
                                                        //    if (IsAdminDateinScheduledDays(dtschedulefreq, dtStartHr.Date) == true)
                                                        //    {
                                                        //        count++;
                                                        //    }
                                                        //}
                                                    }
                                                    else
                                                    {
                                                        count++;
                                                    }
                                                    dtStartHr = dtStartHr.AddHours(Convert.ToInt64(Frequency_value));
                                                }
                                            }
                                        }
                                        break;
                                    case (int)Infologics.Medilogics.Enumerators.EMR.FrequencyTypes.Days:
                                        DateTime dtStartHrDays = Convert.ToDateTime(Start_Date);
                                        DateTime dtEndHrDays = Convert.ToDateTime(End_Date);
                                        if (IsMedicineScheduled == true)
                                        {
                                            //if (IsScheduledataAvailable == true)
                                            //{
                                            //    if (IsAdminDateinScheduledDays(dtschedulefreq, dtStartHrDays.Date) == true)
                                            //    {
                                            //        count++;
                                            //    }
                                            //}
                                        }
                                        else
                                        {
                                            count++;
                                        }
                                        if (Frequency_value != Int64.MinValue && Frequency_value.ToString().Trim() != "0")
                                        {
                                            dtStartHrDays = dtStartHrDays.AddDays(Convert.ToInt64(Frequency_value));
                                            if (dtStartHrDays != null && dtEndHrDays != null && dtEndHrDays.Date != DateTime.MinValue.Date && dtEndHrDays.Date != DateTime.MaxValue.Date && dtStartHrDays.Date != DateTime.MinValue.Date && dtStartHrDays.Date != DateTime.MaxValue.Date)
                                            {
                                                while (dtStartHrDays <= dtEndHrDays)
                                                {
                                                    if (IsMedicineScheduled == true)
                                                    {
                                                        //if (IsScheduledataAvailable == true)
                                                        //{
                                                        //    if (IsAdminDateinScheduledDays(dtschedulefreq, dtStartHrDays.Date) == true)
                                                        //    {
                                                        //        count++;
                                                        //    }
                                                        //}
                                                    }
                                                    else
                                                    {
                                                        count++;
                                                    }
                                                    dtStartHrDays = dtStartHrDays.AddDays(Convert.ToInt64(Frequency_value));
                                                }
                                            }
                                        }
                                        break;
                                    default:
                                        if (admin_time.ToString().Trim() != string.Empty)
                                        {
                                            string[] var = (admin_time.ToString()).Split(',');
                                            DateTime dtStart = Convert.ToDateTime(Start_Date).Date;
                                            //DateTime
                                            dtEnd = Convert.ToDateTime(End_Date).Date;
                                            if (IsMedicineScheduled == true)
                                            {
                                                //if (IsScheduledataAvailable == true)
                                                //{
                                                //    if (IsAdminDateinScheduledDays(dtschedulefreq, dtStart.Date) == true)
                                                //    {
                                                //        count++;
                                                //    }
                                                //}
                                            }
                                            else
                                            {
                                                count++;
                                            }
                                            TimeSpan ttmaxDuration = TimeSpan.Zero;
                                            if (drSelectedFrq[0]["FIELD3"].ToString() != String.Empty)
                                            {
                                                ttmaxDuration = TimeSpan.FromMinutes(Convert.ToDouble(drSelectedFrq[0]["FIELD3"]));
                                            }

                                            TimeSpan tTimeGap = StartTime + ttmaxDuration;
                                            bool canAdd = false;
                                            for (int i = 1; i < var.Length; i++)
                                            {
                                                if (canAdd == false)
                                                {
                                                    if (i == 1)
                                                    {
                                                        tTimeGap = StartTime + ttmaxDuration;
                                                    }
                                                    if (TimeSpan.Parse(var[i]) >= tTimeGap)
                                                    {
                                                        canAdd = true;
                                                    }
                                                    else
                                                    {
                                                        canAdd = false;
                                                    }
                                                }
                                                if (canAdd == true)
                                                {
                                                    if (IsMedicineScheduled == true)
                                                    {
                                                        //if (IsScheduledataAvailable == true)
                                                        //{
                                                        //    if (IsAdminDateinScheduledDays(dtschedulefreq, dtStart.Date) == true)
                                                        //    {
                                                        //        count++;
                                                        //    }
                                                        //}
                                                    }
                                                    else
                                                    {
                                                        count++;
                                                    }
                                                }
                                            }


                                            dtStart = dtStart.AddDays(1);
                                            if (dtStart != null && dtEnd != null && dtEnd.Date != DateTime.MinValue.Date && dtEnd.Date != DateTime.MaxValue.Date && dtStart.Date != DateTime.MinValue.Date && dtStart.Date != DateTime.MaxValue.Date)
                                            {
                                                while (dtStart <= dtEnd)
                                                {
                                                    for (int i = 0; i < var.Count(); i++)
                                                    {
                                                        if (dtStart == dtEnd)
                                                        {

                                                            if (EndTime >= TimeSpan.Parse(var[i]))
                                                            {
                                                                if (IsMedicineScheduled == true)
                                                                {
                                                                    //if (IsScheduledataAvailable == true)
                                                                    //{
                                                                    //    if (IsAdminDateinScheduledDays(dtschedulefreq, dtStart.Date) == true)
                                                                    //    {
                                                                    //        count++;
                                                                    //    }
                                                                    //}
                                                                }
                                                                else
                                                                {
                                                                    count++;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (IsMedicineScheduled == true)
                                                            {
                                                                //if (IsScheduledataAvailable == true)
                                                                //{
                                                                //    if (IsAdminDateinScheduledDays(dtschedulefreq, dtStart.Date) == true)
                                                                //    {
                                                                //        count++;
                                                                //    }
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                count++;
                                                            }
                                                        }
                                                    }
                                                    dtStart = dtStart.AddDays(1);
                                                }
                                            }

                                            break;


                                        }
                                        else  //if no admin time
                                        {
                                            DateTime dtStart = Start_Date;
                                            dtEnd = End_Date;
                                            if (dtStart != null && dtEnd != null && dtEnd.Date != DateTime.MinValue.Date && dtEnd.Date != DateTime.MaxValue.Date && dtStart.Date != DateTime.MinValue.Date && dtStart.Date != DateTime.MaxValue.Date)
                                            {
                                                while (dtStart <= dtEnd)
                                                {
                                                    if (IsMedicineScheduled == true)
                                                    {
                                                        //if (IsScheduledataAvailable == true)
                                                        //{
                                                        //    if (IsAdminDateinScheduledDays(dtschedulefreq, dtStart.Date) == true)
                                                        //    {
                                                        //        if (drSelectedFrq[0]["FIELD6"] == DBNull.Value || Convert.ToInt16(drSelectedFrq[0]["FIELD6"]) < 1)
                                                        //        {
                                                        //            count = count + 1;
                                                        //        }
                                                        //        else
                                                        //        {
                                                        //            count = count + Convert.ToInt16(drSelectedFrq[0]["FIELD6"]);
                                                        //        }
                                                        //    }
                                                        //}
                                                    }
                                                    else
                                                    {
                                                        if (drSelectedFrq[0]["FIELD6"] == DBNull.Value || Convert.ToInt16(drSelectedFrq[0]["FIELD6"]) < 1)
                                                        {
                                                            count = count + 1;
                                                        }
                                                        else
                                                        {
                                                            count = count + Convert.ToInt16(drSelectedFrq[0]["FIELD6"]);
                                                        }
                                                    }
                                                    dtStart = dtStart.AddDays(1);
                                                }
                                            }
                                            else
                                            {
                                                if (IsMedicineScheduled == true)
                                                {
                                                    if (IsScheduledataAvailable == true)
                                                    {
                                                        //if (IsAdminDateinScheduledDays(dtschedulefreq, dtStart.Date) == true)
                                                        //{
                                                        //    if (drSelectedFrq[0]["FIELD6"] == DBNull.Value || Convert.ToInt16(drSelectedFrq[0]["FIELD6"]) < 1)
                                                        //    {
                                                        //        count = count + 1;
                                                        //    }
                                                        //    else
                                                        //    {
                                                        //        count = count + Convert.ToInt16(drSelectedFrq[0]["FIELD6"]);
                                                        //    }
                                                        //}
                                                    }
                                                }
                                                else
                                                {
                                                    if (drSelectedFrq[0]["FIELD6"] == DBNull.Value || Convert.ToInt16(drSelectedFrq[0]["FIELD6"]) < 1)
                                                    {
                                                        count = count + 1;
                                                    }
                                                    else
                                                    {
                                                        count = count + Convert.ToInt16(drSelectedFrq[0]["FIELD6"]);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                }
                            }
                        }
                    }
                }
                else  //No Frequency ID
                {
                    if (isfrequencyFreetext == true && admin_time.ToString().Trim() != string.Empty)//freetext AND HAVE ADMIN TIME
                    {
                        string[] var = (admin_time.ToString()).Split(',');
                        DateTime dtStart = Convert.ToDateTime(Start_Date).Date;
                        //DateTime
                        dtEnd = Convert.ToDateTime(End_Date).Date;
                        if (IsMedicineScheduled == true)
                        {
                            //if (IsScheduledataAvailable == true)
                            //{
                            //    if (IsAdminDateinScheduledDays(dtschedulefreq, dtStart.Date) == true)
                            //    {
                            //        count++;
                            //    }
                            //}
                        }
                        else
                        {
                            count++;
                        }
                        TimeSpan ttmaxDuration = TimeSpan.Zero;
                        //if (drSelectedFrq[0]["FIELD3"].ToString() != String.Empty)
                        //{
                        //    ttmaxDuration = TimeSpan.FromMinutes(Convert.ToDouble(drSelectedFrq[0]["FIELD3"]));
                        //}

                        TimeSpan tTimeGap = StartTime + ttmaxDuration;
                        bool canAdd = false;
                        for (int i = 1; i < var.Length; i++)
                        {
                            //if (canAdd == false)
                            //{
                            //    if (i == 1)
                            //    {
                            //        tTimeGap = StartTime + ttmaxDuration;
                            //    }
                            //    if (TimeSpan.Parse(var[i]) >= tTimeGap)
                            //    {
                            //        canAdd = true;
                            //    }
                            //    else
                            //    {
                            //        canAdd = false;
                            //    }
                            //}
                            if (canAdd == true)
                            {
                                if (IsMedicineScheduled == true)
                                {
                                    //if (IsScheduledataAvailable == true)
                                    //{
                                    //    if (IsAdminDateinScheduledDays(dtschedulefreq, dtStart.Date) == true)
                                    //    {
                                    //        count++;
                                    //    }
                                    //}
                                }
                                else
                                {
                                    count++;
                                }
                            }
                        }


                        dtStart = dtStart.AddDays(1);
                        if (dtStart != null && dtEnd != null && dtEnd.Date != DateTime.MinValue.Date && dtEnd.Date != DateTime.MaxValue.Date && dtStart.Date != DateTime.MinValue.Date && dtStart.Date != DateTime.MaxValue.Date)
                        {
                            while (dtStart <= dtEnd)
                            {
                                for (int i = 0; i < var.Count(); i++)
                                {
                                    if (dtStart == dtEnd)
                                    {

                                        if (EndTime >= TimeSpan.Parse(var[i]))
                                        {
                                            if (IsMedicineScheduled == true)
                                            {
                                                //if (IsScheduledataAvailable == true)
                                                //{
                                                //    if (IsAdminDateinScheduledDays(dtschedulefreq, dtStart.Date) == true)
                                                //    {
                                                //        count++;
                                                //    }
                                                //}
                                            }
                                            else
                                            {
                                                count++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (IsMedicineScheduled == true)
                                        {
                                            //if (IsScheduledataAvailable == true)
                                            //{
                                            //    if (IsAdminDateinScheduledDays(dtschedulefreq, dtStart.Date) == true)
                                            //    {
                                            //        count++;
                                            //    }
                                            //}
                                        }
                                        else
                                        {
                                            count++;
                                        }
                                    }
                                }
                                dtStart = dtStart.AddDays(1);
                            }
                        }

                    }
                    else
                    {
                        if (isinfusion == 1)
                        {
                            count = 1;
                        }
                        else
                        {
                            DateTime dtStartDate = Convert.ToDateTime(Start_Date).Date;
                            DateTime dtEndDate = Convert.ToDateTime(End_Date).Date;
                            //if (IsScheduledataAvailable == true)
                            //{
                            //    if (IsAdminDateinScheduledDays(dtschedulefreq, dtStartDate.Date) == true)
                            //    {
                            //    }
                            //}
                            if (dtStartDate <= dtEndDate)
                            {
                                TimeSpan TimeDiff = dtEndDate.Subtract(dtStartDate);
                                count = TimeDiff.Days + 1;
                            }
                        }
                    }
                }
                if (count != 0)
                {
                    if (quantity != decimal.MinValue)
                    {
                        totalQty = (count * Convert.ToDecimal(quantity));
                    }
                    else
                    {
                        totalQty = count;
                    }

                }
                return totalQty;
            }
            catch (Exception)
            {
                throw;
            }
        }
   
        /// <summary>
        /// Gets the custom age[YY/MM/DD].
        /// </summary>
        /// <param name="Date">The date.</param>
        /// <returns></returns>
        public static string GetCustomAge(DateTime Date)
        {
            try
            {
                string Age;
                int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
                DateTime fromDate;
                DateTime toDate;
                int year;
                int month;
                int day;
                fromDate = DateTime.Parse(Date.ToString("dd/MM/yyyy"));
                toDate = DateTime.Parse(DateTime.Today.ToString("dd/MM/yyyy"));
                int increment = 0;
                //day calculation
                if (fromDate.Day > toDate.Day)
                {
                    increment = monthDay[fromDate.Month - 1];
                }
                if (increment == -1)
                {
                    if (DateTime.IsLeapYear(fromDate.Year))
                    {
                        increment = 29;
                    }
                    else
                    {
                        increment = 28;
                    }
                }
                if (increment != 0)
                {
                    day = (toDate.Day + increment) - fromDate.Day;
                    increment = 1;
                }
                else
                {
                    day = toDate.Day - fromDate.Day;
                }

                //month calculation
                if ((fromDate.Month + increment) > toDate.Month)
                {
                    month = (toDate.Month + 12) - (fromDate.Month + increment);
                    increment = 1;
                }
                else
                {
                    month = (toDate.Month) - (fromDate.Month + increment);
                    increment = 0;
                }
                //year calculation
                year = toDate.Year - (fromDate.Year + increment);

                Age = Convert.ToString(year) + "/" + Convert.ToString(month) + "/" + Convert.ToString(day) + "[YY/MM/DD]";
                return Age;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="Patient"></param>
        /// <param name="dtVitalVisibility"></param>
        /// <returns></returns>
        public static DataTable FilterVitalSignBasedOnMasterSetting(DataTable dtVitalLookup, PatientInformation Patient,
            DataTable dtVitalVisibility)
        {

            //DataTable dtVitalVisibility = EMRData.GetEMRLookupData("VITAL SIGNS VISIBILITY CONDITIONS");

            int AgeInMonths = 0;
            //int CONST_AgeLimit;
            Infologics.Medilogics.General.Control.Common objCommon = new Common();
            DataTable Age = objCommon.GetAge(Patient.DOB, DateTime.Today);
            if (Age != null && Age.Columns.Contains("Months") && Age.Rows[0]["Months"] != DBNull.Value)
            {
                AgeInMonths = Convert.ToInt32(Age.Rows[0]["Months"]) + Convert.ToInt32(Age.Rows[0]["Years"]) * 12;
            }

            DataTable dtReturn = dtVitalLookup.Clone();

            foreach (DataRow item in dtVitalLookup.Rows)
            {
                string GenderFemale;
                string GenderMale;
                string VsVisitType;

                GenderMale = item["FIELD7"] == DBNull.Value ? string.Empty : item["FIELD7"].ToString();
                GenderFemale = item["FIELD8"] == DBNull.Value ? string.Empty : item["FIELD8"].ToString();
                VsVisitType = item["FIELD5"] == DBNull.Value ? string.Empty : item["FIELD5"].ToString();

                bool IsVisibleVitals = true;

                if (dtVitalVisibility != null && dtVitalVisibility.Rows.Count > 0)
                {
                    DataRow[] drCondition = dtVitalVisibility.Select(
                        string.Format("LOOKUP_VALUE ='{0}'", Convert.ToString(item["EMR_LOOKUP_ID"])));
                    if (drCondition.Count() > 0)
                    {
                        int MinValue, MaxValue;
                        int.TryParse(Convert.ToString(drCondition[0]["FIELD1"]), out MinValue);
                        int.TryParse(Convert.ToString(drCondition[0]["FIELD2"]), out MaxValue);
                        if (MinValue >= 0)
                        {
                            if (MaxValue > 0)
                            {
                                if (AgeInMonths >= MinValue && AgeInMonths <= MaxValue)
                                {
                                    IsVisibleVitals = true;
                                }
                                else
                                {
                                    IsVisibleVitals = false;
                                }
                            }
                            else
                            {
                                if (AgeInMonths >= MinValue)
                                {
                                    IsVisibleVitals = true;
                                }
                            }
                        }

                    }
                    else
                    {
                        IsVisibleVitals = true;
                    }
                }
                else
                {
                    IsVisibleVitals = true;
                }

                if (IsVisibleVitals)
                {
                    if ((Patient.Gender == Gender.Female && GenderFemale == "1") ||
                              (Patient.Gender == Gender.Male && GenderMale == "1"))
                    {
                        IsVisibleVitals = true;
                    }
                    else
                    {
                        IsVisibleVitals = false;
                    }
                }

                switch (Patient.VisitStatus)
                {
                    case (int)Enumerators.Visit.EpisodeStatus.IP:
                    case (int)Enumerators.Visit.EpisodeStatus.Emergency:
                        {
                            int _visitType;
                            int.TryParse(VsVisitType, out _visitType);

                            if (!Patient.IsOTEMR && (_visitType == 1 || _visitType == 2 || _visitType == 5 || _visitType == 4))
                            {
                                if (IsVisibleVitals)
                                {
                                    IsVisibleVitals = true;
                                }
                            }
                            else if (Patient.IsOTEMR && (_visitType == 3 || _visitType == 4 || _visitType == 5))
                            {
                                if (IsVisibleVitals)
                                {
                                    IsVisibleVitals = true;
                                }
                            }
                            else
                            {
                                IsVisibleVitals = false;
                            }
                        }
                        break;
                    case (int)Enumerators.Visit.EpisodeStatus.OP:
                        {
                            int _visitType;
                            int.TryParse(VsVisitType, out _visitType);
                            if (!Patient.IsOTEMR && (_visitType == 0 || _visitType == 2 || _visitType == 5))
                            {
                                if (IsVisibleVitals)
                                {
                                    IsVisibleVitals = true;
                                }
                            }
                            else if (Patient.IsOTEMR && (_visitType == 3 || _visitType == 4 || _visitType == 5))
                            {
                                if (IsVisibleVitals)
                                {
                                    IsVisibleVitals = true;
                                }
                            }
                            else
                            {
                                IsVisibleVitals = false;
                            }
                        }
                        break;
                }

                if (IsVisibleVitals)
                {
                    dtReturn.ImportRow(item);
                }
            }
            return dtReturn;
        }
        // added by Muhammed Haris on 03FEB2017
        public bool IsMedicineAdminStatusPostive(DataRow drAdmin, int Mode)
        {
            bool IsStatus = false;
            try
            {
                if (drAdmin != null && drAdmin.Table.Columns.Contains("STATUS") && drAdmin["STATUS"].ToString().Trim().Length > 0)
                {
                    if (Convert.ToInt16(drAdmin["STATUS"]) == Convert.ToInt16(MedicineAdministrationStatus.Administrated)
                        || Convert.ToInt16(drAdmin["STATUS"]) == Convert.ToInt16(MedicineAdministrationStatus.Discontinued)
                        || Convert.ToInt16(drAdmin["STATUS"]) == Convert.ToInt16(MedicineAdministrationStatus.GivenLate)
                        || Convert.ToInt16(drAdmin["STATUS"]) == Convert.ToInt16(MedicineAdministrationStatus.GivenEarly)
                        || Convert.ToInt16(drAdmin["STATUS"]) == Convert.ToInt16(MedicineAdministrationStatus.SelfAdministration)
                        || Convert.ToInt16(drAdmin["STATUS"]) == Convert.ToInt16(MedicineAdministrationStatus.Completed)
                        || Convert.ToInt16(drAdmin["STATUS"]) == Convert.ToInt16(MedicineAdministrationStatus.Started)
                        || Convert.ToInt16(drAdmin["STATUS"]) == Convert.ToInt16(MedicineAdministrationStatus.AdministrationEdited)
                        || Convert.ToInt16(drAdmin["STATUS"]) == Convert.ToInt16(MedicineAdministrationStatus.AdministrationRestart)
                        || Convert.ToInt16(drAdmin["STATUS"]) == Convert.ToInt16(MedicineAdministrationStatus.AdministrationContinue)
                        || Convert.ToInt16(drAdmin["STATUS"]) == Convert.ToInt16(MedicineAdministrationStatus.AdministrationStarted)
                        || Convert.ToInt16(drAdmin["STATUS"]) == Convert.ToInt16(MedicineAdministrationStatus.AdministrationStop)
                        || Convert.ToInt16(drAdmin["STATUS"]) == Convert.ToInt16(MedicineAdministrationStatus.Verified))
                    {
                        IsStatus = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return IsStatus;
        }

        // added by Muhammed Haris on 18MARCH2017
        public ImageSource CreateBarcodeImage(string code)
        {
            if (IsFontInstalled("3 of 9 Barcode"))
            {
                var myBitmap = new Bitmap(300, 50);
                var g = Graphics.FromImage(myBitmap);
                var jgpEncoder = GetEncoder(System.Drawing.Imaging.ImageFormat.Jpeg);

                g.Clear(System.Drawing.Color.White);

                var strFormat = new StringFormat { Alignment = StringAlignment.Center };
                g.DrawString(code, new Font("3 of 9 Barcode", 50), System.Drawing.Brushes.Black, new RectangleF(0, 0, 0, 0), strFormat);

                var myEncoder = System.Drawing.Imaging.Encoder.Quality;
                var myEncoderParameters = new System.Drawing.Imaging.EncoderParameters(1);

                var myEncoderParameter = new System.Drawing.Imaging.EncoderParameter(myEncoder, 100L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                var handle = myBitmap.GetHbitmap();
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            else
            {
                return null;
            }
        }

        private static System.Drawing.Imaging.ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
        {

            var codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders();

            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        private static bool IsFontInstalled(string fontName)
        {
            using (var testFont = new Font(fontName, 8))
                return fontName.Equals(testFont.Name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
