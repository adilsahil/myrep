/*
 * ----------------------------------------------------------------------
 * <copyright file="CommonFunctions.cs" company="GI Infologics PVT Ltd">
 *      Copyright (c) GI Infologics Pvt Ltd. All rights reserved.
 * </copyright>
 * <author>Sreeroop C M</author>
 * <Date><Date>
 * -----------------------------------------------------------------------
*/

namespace Infologics.Medilogics.CommonClient.Controls.CommonFunctions
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Data;
    using System.ComponentModel;
    using System.Reflection;
    using System.Windows.Media.Imaging;
    using Infologics.Medilogics.CommonServer.Main;
    using System.IO;
    using Infologics.Medilogics.CommonClient.Controls.StaticData;
    using Infologics.Medilogics.General.Control.Classes.FileTransfer;
    using Infologics.Medilogics.CommonClient.Controls.FileHandler;
    using System.Text.RegularExpressions;
    using Infologics.Medilogics.HospitalRouter.Common;
    using Infologics.Medilogics.CommonShared.EMRMain;
    using Infologics.Medilogics.General.Control;
    using Infologics.Medilogics.Enumerators.EMR;
    using System.Configuration;
    using Infologics.Medilogics.Resources.MessageBoxLib;
    using Infologics.Medilogics.CommonClient.Controls.Concrete_Class;
    /// <summary>
    /// 
    /// </summary>
    public class CommonFunctions
    {
        /// <summary>
        /// Enums to data table.
        /// </summary>
        /// <param name="enumObject">The enum object.</param>
        /// <returns>DataTable</returns>
        public DataTable EnumToDataTable(Type enumObject)
        {
            try
            {
                return this.EnumToDataTable(enumObject, String.Empty, String.Empty);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// Converts an enum to a DataTable.  
        /// /// </summary>        
        /// <param name="enumType">The enum to convert.</param>       
        /// /// <param name="keyFieldName">The desired name of the key column.</param>       
        /// /// <param name="valueFieldName">the desired name of the value column.</param>        
        /// <returns>A DataTable with the name/value pairs from the enum.</returns>
        public DataTable EnumToDataTable(Type enumType, string keyFieldName, string valueFieldName)
        {            //Check inputs:  
            if (keyFieldName == String.Empty)
                keyFieldName = "KEY";
            if (valueFieldName == String.Empty)
                valueFieldName = "VALUE";
            if (keyFieldName == valueFieldName)
                throw new Exception("Key and Value column names must be different.");
            //Create the DataTable with the desired columns:           
            DataTable table = new DataTable();
            table.Columns.Add(valueFieldName, typeof(string));
            table.Columns.Add(keyFieldName, Enum.GetUnderlyingType(enumType));
            //Add the items from the enum:            
            foreach (string name in Enum.GetNames(enumType))
            {
                Enum currentEnum = System.Enum.Parse(enumType, name) as Enum;
                table.Rows.Add(this.GetDescription(currentEnum), currentEnum);
            }
            return table;
        }

        /// <summary>
        /// Returns the Description attribute of a Enumerator if exists
        /// If a description does not exist,the default enum value is returned
        /// </summary>
        /// <param name="currentEnum"></param>
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
        ///  Convert Number to word
        /// </summary>
        /// <param name="dcAmount"> Accept Amount for converting to Words</param>
        /// <returns> String value with correspoding words</returns>
        public string ToWords(double dcAmount)
        {
            //old (HIS 2)
            //string RupeeString;
            //string PaiseString;
            //string PaisePart;
            //int DecimalPosition;
            //string StringAmount;
            //string result;
            //PaisePart = "0";
            //PaiseString = "";
            //RupeeString = "";

            //if ((Amount <= 0) || (Amount > 999999999.999))
            //    result = "";
            //else
            //{
            //    StringAmount = Amount.ToString("0.000");
            //    DecimalPosition = StringAmount.IndexOf(".", 1);
            //    RupeeString = ToWordsData((int)Amount);
            //    if (DecimalPosition > 0)
            //    {
            //        PaisePart = StringAmount.Substring(DecimalPosition + 1, 3);
            //        PaisePart = Convert.ToInt32(PaisePart).ToString("000");
            //        PaisePart = PaisePart.Substring(0, 3);
            //        if (Convert.ToInt32(PaisePart) > 0)
            //            PaiseString = ToWordsData(Convert.ToInt32(PaisePart));
            //    }

            //    if ((((int)Amount) != 0) && (Convert.ToInt32(PaisePart) != 0))
            //        result = "BD " + RupeeString.Trim() + " And Fil " + PaiseString.Trim() + " Only.";
            //    else if ((((int)Amount) != 0) && (Convert.ToInt32(PaisePart) == 0))
            //        result = "BD " + RupeeString.Trim() + " Only.";
            //    else if ((((int)Amount) == 0) && (Convert.ToInt32(PaisePart) != 0))
            //        result = "Fil " + PaiseString + " Only.";
            //    else
            //        result = "BD " + RupeeString.Trim() + " Fil " + PaiseString.Trim() + " Only.";
            //}
            //return result;

            //To take the currency details from app settings

            DataTable dtSettings = null;
            MainCommon objComon = new MainCommon();
            dtSettings = objComon.FetchGenApplication(0, string.Empty);

            string CurrencyType = dtSettings.Select("GEN_APPLICATION_SETTING_ID=" + 98)[0]["VALUE"].ToString();
            string SubUnit = dtSettings.Select("GEN_APPLICATION_SETTING_ID=" + 99)[0]["VALUE"].ToString();
            int PaiseDigits = Convert.ToInt16(dtSettings.Select("GEN_APPLICATION_SETTING_ID=" + 97)[0]["VALUE"].ToString());
            StringBuilder sbResultData = new StringBuilder();


            double InfiniteAmount = 999999999 + (Math.Pow(10, Convert.ToDouble(PaiseDigits)) - 1) / Math.Pow(10, Convert.ToDouble(PaiseDigits));
            StringBuilder sbFormatStrings = new StringBuilder();

            string RupeeString;
            string PaiseString;
            string PaisePart;
            int DecimalPosition;
            string StringAmount;
            string result;
            PaisePart = "0";
            PaiseString = "";
            RupeeString = "";

            //if ((Amount <= 0) || (Amount > 999999999.999))
            //    result = "";
            if ((dcAmount <= 0) || (dcAmount > InfiniteAmount))
                sbResultData.Append(string.Empty);
            else
            {
                sbFormatStrings.Append("0.");
                sbFormatStrings.Append('0', Convert.ToInt16(PaiseDigits));

                // StringAmount = Amount.ToString("0.000");
                StringAmount = dcAmount.ToString(sbFormatStrings.ToString());

                DecimalPosition = StringAmount.IndexOf(".", 1);
                RupeeString = ToWordsData((int)dcAmount);
                if (DecimalPosition > 0)
                {
                    //PaisePart = StringAmount.Substring(DecimalPosition + 1, 3);
                    //PaisePart = Convert.ToInt32(PaisePart).ToString("000");
                    //PaisePart = PaisePart.Substring(0, 3);
                    sbFormatStrings = new StringBuilder();
                    sbFormatStrings.Append('0', PaiseDigits);

                    PaisePart = StringAmount.Substring(DecimalPosition + 1, PaiseDigits);
                    PaisePart = Convert.ToInt32(PaisePart).ToString(sbFormatStrings.ToString());
                    PaisePart = PaisePart.Substring(0, PaiseDigits);
                    if (Convert.ToInt32(PaisePart) > 0)
                        PaiseString = ToWordsData(Convert.ToInt32(PaisePart));
                }

                //if ((((int)Amount) != 0) && (Convert.ToInt32(PaisePart) != 0))
                //    result = "BD " + RupeeString.Trim() + " And Fil " + PaiseString.Trim() + " Only.";
                //else if ((((int)Amount) != 0) && (Convert.ToInt32(PaisePart) == 0))
                //    result = "BD " + RupeeString.Trim() + " Only.";
                //else if ((((int)Amount) == 0) && (Convert.ToInt32(PaisePart) != 0))
                //    result = "Fil " + PaiseString + " Only.";
                //else
                //    result = "BD " + RupeeString.Trim() + " Fil " + PaiseString.Trim() + " Only.";


                StringBuilder sbPaiseString = new StringBuilder();
                StringBuilder sbRupeeString = new StringBuilder();

                if ((int)dcAmount != 0)
                {
                    sbRupeeString.Append(CurrencyType);
                    sbRupeeString.Append(" ");
                    sbRupeeString.Append(RupeeString.Trim());
                    sbRupeeString.Append(" And ");
                }
                if (Convert.ToInt32(PaisePart) != 0)
                {


                    sbPaiseString.Append(PaiseString.Trim());
                    sbPaiseString.Append(" ");
                    sbPaiseString.Append(SubUnit);
                }
                else
                {
                    sbRupeeString.Replace(" And ", " ");
                }
                sbResultData.Append(sbRupeeString);
                if (SubUnit.ToUpper() == "BAISA" || SubUnit.ToUpper() == "BAIZA")
                {
                    sbResultData.Append("Baiza " + PaisePart + "/1000");
                }
                else
                {
                    sbResultData.Append(sbPaiseString);
                }
                sbResultData.ToString().Trim();
                sbResultData.Append(" Only ");

            }

            return sbResultData.ToString();

        }

        private string ToWordsData(long dcAmount)
        {
            string AmountString;
            string[] Values = new string[11];
            string[] Names = new string[11];
            bool[] ABCTrue = new bool[11];
            string[] ABCAnd = new string[11];
            int X;
            int j;
            int aBCOCCURS;
            string AANAME;
            AmountString = dcAmount.ToString("000000000");
            Values[7] = "0";
            Names[7] = "";

            for (j = 1; j <= 10; ++j)
            {
                ABCAnd[j] = "";
                X = j;
                if (j != 7)
                {
                    if (j < 8)
                        X = j;
                    else
                        X = j - 1;
                    Values[j] = AmountString.Substring(X - 1, 1);
                    if (Values[j] == " ")
                        Values[j] = "0";
                }
            }

            for (j = 1; j <= 9; j = j + 2)
            {
                if (Values[j] == "0")
                    Names[j] = "";
                else
                {
                    if (Values[j] == "1")
                    {
                        ABCTrue[j] = true;
                        Names[j] = "";
                    }
                    else
                    {
                        if (Values[j] == "2")
                            Names[j] = "Twenty ";
                        if (Values[j] == "3")
                            Names[j] = "Thirty ";
                        if (Values[j] == "4")
                            Names[j] = "Forty ";
                        if (Values[j] == "5")
                            Names[j] = "Fifty ";
                        if (Values[j] == "6")
                            Names[j] = "Sixty ";
                        if (Values[j] == "7")
                            Names[j] = "Seventy ";
                        if (Values[j] == "8")
                            Names[j] = "Eighty ";
                        if (Values[j] == "9")
                            Names[j] = "Ninety ";
                    }
                }
            }

            //'**----------------------EVEN GROUP------------------------**
            for (j = 2; j <= 10; j = j + 2)
            {
                if (ABCTrue[j - 1] == true)
                {
                    if (Values[j] == "0")
                        Names[j] = "Ten ";
                    if (Values[j] == "1")
                        Names[j] = "Eleven ";
                    if (Values[j] == "2")
                        Names[j] = "Twelve ";
                    if (Values[j] == "3")
                        Names[j] = "Thirteen ";
                    if (Values[j] == "4")
                        Names[j] = "Fourteen ";
                    if (Values[j] == "5")
                        Names[j] = "Fifteen ";
                    if (Values[j] == "6")
                        Names[j] = "Sixteen ";
                    if (Values[j] == "7")
                        Names[j] = "Seventeen ";
                    if (Values[j] == "8")
                        Names[j] = "Eighteen ";
                    if (Values[j] == "9")
                        Names[j] = "Nineteen ";
                }
                else
                {
                    if (Values[j] == "0")
                        Names[j] = "";
                    if (Values[j] == "1")
                        Names[j] = "One ";
                    if (Values[j] == "2")
                        Names[j] = "Two ";
                    if (Values[j] == "3")
                        Names[j] = "Three ";
                    if (Values[j] == "4")
                        Names[j] = "Four ";
                    if (Values[j] == "5")
                        Names[j] = "Five ";
                    if (Values[j] == "6")
                        Names[j] = "Six ";
                    if (Values[j] == "7")
                        Names[j] = "Seven ";
                    if (Values[j] == "8")
                        Names[j] = "Eight ";
                    if (Values[j] == "9")
                        Names[j] = "Nine ";
                }
            }
            aBCOCCURS = 0;

            for (j = 2; j <= 10; j = j + 2)
            {
                if ((Values[j] != "0") || (Values[j - 1] != "0"))
                {
                    aBCOCCURS = aBCOCCURS + 1;
                    if (j == 2)
                    {
                        ABCAnd[j] = "Crore ";
                        ABCTrue[j] = true;
                    }
                    if (j == 4)
                    {
                        ABCAnd[j] = "Lakh ";
                        ABCTrue[j] = true;
                    }
                    if (j == 6)
                    {
                        ABCAnd[j] = "Thousand ";
                        ABCTrue[j] = true;
                    }
                    if (j == 8)
                    {
                        ABCTrue[j] = true;
                        ABCAnd[j] = "Hundred ";
                    }
                    if (j == 10)
                        ABCTrue[j] = true;

                }
            }

            if (aBCOCCURS > 1)
            {
                if ((ABCAnd[8] == "Hundred ") && (ABCTrue[10] == true))
                    ABCAnd[9] = "And ";
                else
                    if ((ABCAnd[6] == "Thousand ") && ((ABCTrue[8] == true) || (ABCTrue[10] == true)))
                        ABCAnd[7] = "And ";
                    else
                        if ((ABCAnd[4] == "LAKH ") && ((ABCTrue[6] == true) || (ABCTrue[8] == true) || (ABCTrue[10] == true)))
                            ABCAnd[5] = "And ";
                        else
                            if ((ABCAnd[2] == "Crore ") && ((ABCTrue[4] == true) || (ABCTrue[6] == true) || (ABCTrue[8] == true) || (ABCTrue[10] == true)))
                                ABCAnd[3] = "And ";

            }

            AANAME = " ";
            for (j = 1; j <= 10; ++j)
            {
                if (ABCAnd[j] == "And ")
                    AANAME = AANAME + ABCAnd[j] + Names[j];
                else
                    AANAME = AANAME + Names[j] + ABCAnd[j];
            }
            return AANAME;
        }

        /// <summary>
        /// Export HL7 Message
        /// </summary>
        /// <param name="strFileName"></param>
        /// <param name="strFilePath"></param>
        /// <param name="strMessage"></param>
        /// <returns></returns>
        public bool ExportHL7Message(string strFileName, string strFilePath, string strMessage)
        {
            try
            {
                bool isSuccess = false;
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                dlg.FileName = strFileName;
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    FileStream objFS = new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write, FileShare.Write);
                    StreamWriter objWriter = new StreamWriter(objFS);
                    objWriter.Write(strMessage);
                    objWriter.Close();
                    objFS.Close();

                    string strSetting = CommonData.GetDefaultSettings("IS_EXPORTFILE_SAVED");
                    if (strSetting == "1")
                    {
                        // Write the message to a file in the specified path
                        Directory.CreateDirectory(@strFilePath);
                        string path = @strFilePath + strFileName + ".txt";
                        System.IO.StreamWriter file = new System.IO.StreamWriter(path);
                        file.WriteLine(strMessage);
                        file.Close();
                    }
                    isSuccess = true;
                }
                return isSuccess;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public BitmapImage DownloadClientLogoFromServer(string sPath)
        {
            System.Windows.Media.Imaging.BitmapImage bitmapImg = null;
            try
            {
                RemoteFileInfo oFileData = null;
                FileDetails oFileDetails = null;

                FiletransferMain oFiletransferMain = new FiletransferMain();
                DownloadRequest oDataRequest = new DownloadRequest();
                oDataRequest.FileName = sPath;
                oFileData = oFiletransferMain.GetSream(oDataRequest,false);
                if (null != oFileData)
                {
                    oFileDetails = new FileDetails();
                    oFileDetails.FileStream = oFileData.FileByteStream;
                    oFileDetails.FileName = oFileData.FileName;
                    oFileDetails.FileExtension = oFileData.FileExtension;
                }
                if (oFileDetails != null && oFileDetails.FileStream != null)
                {
                    Stream oStream = oFileDetails.FileStream;
                    Byte[] imgArray = new General.Control.Common().ConvertStreamToByteArray(oStream);
                    if (imgArray != null)
                    {
                        MemoryStream ms = new MemoryStream(imgArray);
                        bitmapImg = new System.Windows.Media.Imaging.BitmapImage();
                        bitmapImg.BeginInit();
                        bitmapImg.StreamSource = new MemoryStream(ms.ToArray());
                        bitmapImg.EndInit();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return bitmapImg;
        }

        /// <summary>
        /// This Function Retuns An Entity class Contains Stream,Filename ,Extension
        /// </summary>
        /// <param name="sFileFullPath"> Full server path of the File including extension </param>
        /// <returns></returns>
        public string DownLoadFileFromServer(String sFileFullPath)
        {
            string sFilePath = string.Empty;
            try
            {
                FileDetails oFileData = null;
                using (ImpersonationFromUI objImpersonate = new ImpersonationFromUI())
                {
                    if (objImpersonate.GetImpersonation())
                    {
                        oFileData = DownLoadStreamFromServer(sFileFullPath);
                    }
                }
                if (oFileData != null && oFileData.FileStream != null)
                {
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory + "Temp_Files";
                    if (!System.IO.Directory.Exists(baseDir))
                        System.IO.Directory.CreateDirectory(baseDir);
                    Stream oStream = oFileData.FileStream;
                    sFilePath = baseDir + "\\" + oFileData.FileName + oFileData.FileExtension;
                    FileStream targetStream = null;
                    if (oStream != null)
                    {
                        // Create a FileStream object to write a stream to a file
                        using (targetStream = new FileStream(sFilePath, FileMode.Create,
                                      FileAccess.Write, FileShare.None))
                        {
                            //read from the input stream in 65000 byte chunks
                            const int bufferLen = 65000;
                            byte[] buffer = new byte[bufferLen];
                            int count = 0;
                            while ((count = oStream.Read(buffer, 0, bufferLen)) > 0)
                            {
                                // save to output stream
                                targetStream.Write(buffer, 0, count);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return sFilePath;
        }


        public FileDetails DownLoadStreamFromServer(String sFileFullPath)
        {
            RemoteFileInfo oFileData = null;
            FileDetails oFileDetails = null;
            try
            {
                #region Commented for Multi - Hospitatl Implementation
                //FiletransferMain oFiletransferMain = new FiletransferMain();
                //DownloadRequest oDataRequest = new DownloadRequest();
                //oDataRequest.FileName = sFileFullPath;
                //RemoteFileInfo oFileData = oFiletransferMain.GetSream(oDataRequest);
                #endregion
                //Added by Alex Jose fo Multi Hospital Implementation                
                //FiletransferMain oFiletransferMain = new FiletransferMain();                
                DownloadRequest oDataRequest = new DownloadRequest();
                oDataRequest.FileName = sFileFullPath;
                realParams = new object[1];
                realParams[0] = oDataRequest;
                CreateParametersForMultiHospitalFetch("GetStream", (int)Infologics.Medilogics.Enumerators.General.KIProjects.FileTransfer);
                Infologics.Medilogics.HospitalRouter.Main.MainMHFileTranfer oMainFileTransfr = new Infologics.Medilogics.HospitalRouter.Main.MainMHFileTranfer();
                HrFileTransferParams oHrFileTransferParams = new HrFileTransferParams();
                oHrFileTransferParams.FunctionParams = oDs;
                oHrFileTransferParams.AssemblyParameters = realParams;
                RemoteFileInfo oRemoteFileInfo = oMainFileTransfr.GetFileData(oHrFileTransferParams); ;
                if (oRemoteFileInfo != null)
                {
                    // oFileData  = oFiletransferMain.GetSream(oDataRequest);
                    oFileData = oRemoteFileInfo;
                    if (null != oFileData)
                    {
                        oFileDetails = new FileDetails();
                        oFileDetails.FileStream = oFileData.FileByteStream;
                        oFileDetails.FileName = oFileData.FileName;
                        oFileDetails.FileExtension = oFileData.FileExtension;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return oFileDetails;
        }

        public FileDetails DownLoadStreamFromHospitalServer(String sFileFullPath, Decimal hospitalId, DataTable SelectedHospitalList)
        {
            RemoteFileInfo oFileData = null;
            FileDetails oFileDetails = null;
            try
            {
                #region Commented for Multi - Hospitatl Implementation
                //FiletransferMain oFiletransferMain = new FiletransferMain();
                //DownloadRequest oDataRequest = new DownloadRequest();
                //oDataRequest.FileName = sFileFullPath;
                //RemoteFileInfo oFileData = oFiletransferMain.GetSream(oDataRequest);
                #endregion
                //Added by Alex Jose fo Multi Hospital Implementation                
                //FiletransferMain oFiletransferMain = new FiletransferMain();                
                // oFileData  = oFiletransferMain.GetSream(oDataRequest);
                DownloadRequest oDataRequest = new DownloadRequest();
                oDataRequest.FileName = sFileFullPath;
                realParams = new object[1];
                realParams[0] = oDataRequest;
                CreateParametersForFetchWithHospitalId("GetStream", (int)Infologics.Medilogics.Enumerators.General.KIProjects.FileTransfer, hospitalId, SelectedHospitalList);
                Infologics.Medilogics.HospitalRouter.Main.MainMHFileTranfer oMainFileTransfr = new Infologics.Medilogics.HospitalRouter.Main.MainMHFileTranfer();
                HrFileTransferParams oHrFileTransferParams = new HrFileTransferParams();
                oHrFileTransferParams.FunctionParams = oDs;
                oHrFileTransferParams.AssemblyParameters = realParams;
                RemoteFileInfo oRemoteFileInfo = oMainFileTransfr.GetFileData(oHrFileTransferParams); ;
                if (oRemoteFileInfo != null)
                {
                    oFileData = oRemoteFileInfo;
                    if (null != oFileData)
                    {
                        oFileDetails = new FileDetails();
                        oFileDetails.FileStream = oFileData.FileByteStream;
                        oFileDetails.FileName = oFileData.FileName;
                        oFileDetails.FileExtension = oFileData.FileExtension;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return oFileDetails;
        }

        object[] realParams = null;
        DataSet oDs = new DataSet();
        public DataSet CreateParametersForMultiHospitalFetch(string strFnctnName, int intProject)
        {
            try
            {
                oDs.Tables.Clear();
                DataTable _oDt = new Infologics.Medilogics.CommonClient.Controls.XSD.Client.HOSPITAL_ROUTER_CRITERIADataTable().Clone();
                _oDt.Rows.Add(strFnctnName, intProject);
                DataTable _oDtEndpoint = CommonData.SelectedHospitalList;
                oDs.Tables.Add(_oDt.Copy());
                oDs.Tables.Add(_oDtEndpoint.Copy());
                return oDs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet CreateParametersForFetchWithHospitalId(string strFnctnName, int intProject, decimal decHospitalId, DataTable SelectedHospitalList)
        {
            try
            {
                oDs.Tables.Clear();
                DataTable _oDt = new Infologics.Medilogics.CommonClient.Controls.XSD.Client.HOSPITAL_ROUTER_CRITERIADataTable().Clone();
                _oDt.Rows.Add(strFnctnName, intProject);
                DataTable _oDtEndpoint = null;
                if (SelectedHospitalList != null)
                {
                    if (SelectedHospitalList.Select("HOSPITAL_ID =" + decHospitalId).Count() > 0)
                    {
                        _oDtEndpoint = SelectedHospitalList.Select("HOSPITAL_ID =" + decHospitalId).CopyToDataTable();
                    }
                    else
                    {
                        _oDtEndpoint = SelectedHospitalList.Clone();
                    }
                }
                else
                {
                    if (CommonData.SelectedHospitalList.Select("HOSPITAL_ID =" + decHospitalId).Count() > 0)
                    {
                        _oDtEndpoint = CommonData.SelectedHospitalList.Select("HOSPITAL_ID =" + decHospitalId).CopyToDataTable();
                    }
                    else
                    {
                        _oDtEndpoint = CommonData.SelectedHospitalList.Clone();
                    }
                }
                _oDtEndpoint.TableName = CommonData.SelectedHospitalList.TableName;
                oDs.Tables.Add(_oDt.Copy());
                oDs.Tables.Add(_oDtEndpoint.Copy());
                return oDs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string UpLoadFileToServer(string sLocalFileFullPath, string sDestinationFolderPath)
        {
            string sFileSavedPath = string.Empty;
            try
            {
                FiletransferMain oFiletransferMain = new FiletransferMain();
                if (!string.IsNullOrEmpty(sLocalFileFullPath))
                {
                    if (File.Exists(sLocalFileFullPath))
                    {
                        FileInfo oFileInfo = new FileInfo(sLocalFileFullPath);
                        RemoteFileInfo oDataRequest = new RemoteFileInfo();
                        oDataRequest.FileExtension = oFileInfo.Extension;
                        oDataRequest.FileName = System.IO.Path.GetFileNameWithoutExtension(sLocalFileFullPath);
                        oDataRequest.FileByteStream = System.IO.File.OpenRead(sLocalFileFullPath) as Stream;
                        oDataRequest.DestinationFolderPath = sDestinationFolderPath;

                        RemoteFileInfo oFileSavedData = oFiletransferMain.SaveFile(oDataRequest,false);
                        sFileSavedPath = oFileSavedData.FileName;
                        oDataRequest.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return sFileSavedPath;
        }


        public string UpLoadFileToServerWithAppKey(string sLocalFileFullPath, string sAppSettingKey)
        {
            string sFileSavedPath = string.Empty;
            try
            {
                FiletransferMain oFiletransferMain = new FiletransferMain();
                if (!string.IsNullOrEmpty(sLocalFileFullPath))
                {
                    if (File.Exists(sLocalFileFullPath))
                    {
                        FileInfo oFileInfo = new FileInfo(sLocalFileFullPath);
                        RemoteFileInfo oDataRequest = new RemoteFileInfo();
                        oDataRequest.FileExtension = oFileInfo.Extension;
                        oDataRequest.FileName = System.IO.Path.GetFileNameWithoutExtension(sLocalFileFullPath);
                        oDataRequest.FileByteStream = System.IO.File.OpenRead(sLocalFileFullPath) as Stream;
                        oDataRequest.AppSettingKey = sAppSettingKey;

                        RemoteFileInfo oFileSavedData = oFiletransferMain.SaveFile(oDataRequest,CommonData.IsImpersonation);
                        sFileSavedPath = oFileSavedData.FileName;
                        oDataRequest.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return sFileSavedPath;
        }


        public string UpLoadFileToServerWithAppKeySharedFile(string sLocalFileFullPath, string sAppSettingKey, bool isImpersonateAllMode)//isImpersonateSharedFile file attachement only with new impersonation method
        {
            string sFileSavedPath = string.Empty;
            try
            {
                FiletransferMain oFiletransferMain = new FiletransferMain();
                if (!string.IsNullOrEmpty(sLocalFileFullPath))
                {
                    if (File.Exists(sLocalFileFullPath))
                    {
                        FileInfo oFileInfo = new FileInfo(sLocalFileFullPath);
                        RemoteFileInfo oDataRequest = new RemoteFileInfo();
                        oDataRequest.FileExtension = oFileInfo.Extension;
                        oDataRequest.FileName = System.IO.Path.GetFileNameWithoutExtension(sLocalFileFullPath);
                        oDataRequest.FileByteStream = System.IO.File.OpenRead(sLocalFileFullPath) as Stream;
                        oDataRequest.AppSettingKey = sAppSettingKey;

                        RemoteFileInfo oFileSavedData = oFiletransferMain.SaveFile(oDataRequest, isImpersonateAllMode);
                        sFileSavedPath = oFileSavedData.FileName;
                        oDataRequest.Dispose();

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return sFileSavedPath;
        }

        public object CreateUiDllObject(string sUiDll)
        {
            object oUiDllInstance = null;
            try
            {
                string dllName = sUiDll.Substring(0, sUiDll.IndexOf(","));
                string controlName = sUiDll.ToString().Substring(sUiDll.IndexOf(",") + 1);
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string assemblyPath = baseDir + dllName;
                string nameSpace = dllName.Substring(0, dllName.Length - 4);

                //Loading the UI Dll
                Assembly assembly = Assembly.LoadFile(assemblyPath);
                oUiDllInstance = assembly.CreateInstance(nameSpace + "." + controlName);
            }
            catch
            {
                throw;
            }
            return oUiDllInstance;
        }


        public string FormatMRNO(string strMrno)
        {
            try
            {
                if (strMrno != null && strMrno != string.Empty && strMrno.Trim().Length > 0)
                {
                    DataTable dtGenBillSetting = CommonData.GenBillSetting;
                    if (dtGenBillSetting != null && dtGenBillSetting.Rows.Count > 0)
                    {
                        var query = from dr in dtGenBillSetting.AsEnumerable()
                                    where Convert.ToString(dr["SETTING"]).Trim() == "MRNO"
                                    select dr;
                        if (query.Count() > 0)
                        {
                            String strPrefix = "";//, AppendYear = "", strSuffix = "";
                            int Mrnolength = 0;
                            strPrefix = query.First()["PREFIX"] != DBNull.Value ? Convert.ToString(query.First()["PREFIX"]) : "";
                            //strSuffix = query.First()["POSTFIX"] != DBNull.Value ? Convert.ToString(query.First()["POSTFIX"]) : "";
                            //AppendYear = query.First()["APPEND_YEAR"] != DBNull.Value ? Convert.ToString(query.First()["APPEND_YEAR"]) : "";
                            Mrnolength = query.First()["TOTAL_LENGTH"] != DBNull.Value ? Convert.ToInt16(query.First()["TOTAL_LENGTH"]) : 0;
                            int CntMrno = Mrnolength - (strPrefix.Length);
                            if (strMrno.Length <= CntMrno && strMrno != "0" && Mrnolength > 0)
                            {
                                strMrno = strMrno.PadLeft(CntMrno, '0').ToString();
                                if (!string.IsNullOrEmpty(strPrefix))
                                {
                                    strMrno = strPrefix + strMrno;
                                }
                            }
                            //if (!string.IsNullOrEmpty(strSuffix))
                            //{
                            //    strMrno = strMrno + strSuffix;
                            //}
                        }
                    }
                }
                return strMrno;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public string FormatMRNO(string strMrno)
        //{
        //    try
        //    {
        //        if (strMrno != null && strMrno != string.Empty && strMrno.Trim().Length > 0)
        //        {
        //            DataTable dtGenBillSetting = CommonData.GenBillSetting;
        //            if (dtGenBillSetting != null && dtGenBillSetting.Rows.Count > 0)
        //            {
        //                var query = from dr in dtGenBillSetting.AsEnumerable()
        //                            where Convert.ToString(dr["SETTING"]).Trim() == "MRNO"
        //                            select dr;
        //                if (query.Count() > 0)
        //                {                            
        //                    String strPrefix = "", AppendYear = "", strSuffix = "";
        //                    int Mrnolength = 0;
        //                    strPrefix = query.First()["PREFIX"] != DBNull.Value ? Convert.ToString(query.First()["PREFIX"]) : "";
        //                    strSuffix = query.First()["POSTFIX"] != DBNull.Value ? Convert.ToString(query.First()["POSTFIX"]) : "";
        //                    AppendYear = query.First()["APPEND_YEAR"] != DBNull.Value ? Convert.ToString(query.First()["APPEND_YEAR"]) : "";
        //                    Mrnolength = query.First()["TOTAL_LENGTH"] != DBNull.Value ? Convert.ToInt16(query.First()["TOTAL_LENGTH"]) : 0;                         
        //                    int CntMrno = Mrnolength - (strPrefix.Length + strSuffix.Length);
        //                    string Prefix = strMrno.Substring(0, strPrefix.Length);
        //                    string Suffix = strMrno.Substring(strMrno.Length - strSuffix.Length, strSuffix.Length);
        //                    if (strMrno.Length >= (strPrefix.Length + strSuffix.Length))
        //                    {                               
        //                        //if (!string.IsNullOrEmpty(strPrefix) && strPrefix != "" && Prefix.ToUpper().Equals(strPrefix.ToUpper()))
        //                        //{
        //                        //    MRNO = MRNO.ToUpper().TrimStart(Prefix.ToCharArray());
        //                        //}
        //                        //else if (IsNotNumeric(Prefix) && strPrefix != "")//other suffix
        //                        //{
        //                        //    MRNO = MRNO.ToUpper().TrimStart(Prefix.ToUpper().ToCharArray());
        //                        //}
        //                        if (!string.IsNullOrEmpty(Prefix) && strPrefix != "")
        //                        {
        //                            strMrno = strMrno.ToUpper().Substring(0, strPrefix.Length);
        //                        }

        //                        //if (!string.IsNullOrEmpty(strSuffix) && strSuffix != "" && Suffix.ToUpper().Equals(strSuffix.ToUpper()))
        //                        //{
        //                        //   MRNO = MRNO.ToUpper().TrimEnd(Suffix.ToCharArray());                                   
        //                        //}
        //                        //else if (!string.IsNullOrEmpty(Suffix) && strSuffix != "")//other suffix
        //                        //{
        //                        //   MRNO = MRNO.ToUpper().TrimEnd(Suffix.ToUpper().ToCharArray());                                   
        //                        //}
        //                        if (!string.IsNullOrEmpty(Suffix) && strSuffix != "")//other suffix
        //                        {
        //                            strMrno = strMrno.ToUpper().Substring(strMrno.Length - Suffix.Length, Suffix.Length);
        //                        }
        //                    }
        //                    if (strMrno.Length <= CntMrno && strMrno != "0" && Mrnolength > 0)
        //                    {
        //                        strMrno = strMrno.PadLeft(CntMrno, '0').ToString();
        //                    }
        //                    if (!string.IsNullOrEmpty(strPrefix))
        //                    {
        //                        strMrno = strPrefix + strMrno;
        //                    }
        //                    if (!string.IsNullOrEmpty(strSuffix))
        //                    {
        //                        strMrno = strMrno + strSuffix;
        //                    }
        //                }
        //            }
        //        }
        //        return strMrno;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        #region FormatMRNO (Old format) - Commented by vasim 25-sept-2013
        //public string FormatMRNO(string MRNO)
        //{
        //    try
        //    {
        //        string MRNOSuffix = CommonData.MRNOSuffix;
        //        if (MRNO.Length >= 2)
        //        {
        //            if (!string.IsNullOrEmpty(MRNOSuffix) &&
        //            MRNOSuffix.Equals("0") == false && MRNO.Substring(MRNO.Length - 2, 2).ToUpper().Equals(MRNOSuffix.ToUpper()))
        //            {
        //                MRNO = MRNO.ToUpper().TrimEnd(MRNOSuffix.ToCharArray());
        //            }
        //            else if (IsNotNumeric(MRNO.Substring(MRNO.Length - 2, 2)))//other suffix
        //            {
        //                MRNOSuffix = MRNO.Substring(MRNO.Length - 2, 2).ToUpper();
        //                MRNO = MRNO.ToUpper().TrimEnd(MRNOSuffix.ToCharArray());
        //            }
        //        }
        //        if (MRNO.Length <= 8 && MRNO != "0")
        //        {
        //            // YEAR = DateTime.Now.ToString("yy").ToString();
        //            MRNO = MRNO.PadLeft(9, '0').ToString();
        //            //MRNO = MRNO + YEAR;
        //        }
        //        if (!string.IsNullOrEmpty(MRNOSuffix))
        //        {
        //            MRNO = MRNO + MRNOSuffix;
        //        }

        //        return MRNO;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        #endregion

        public bool IsMRNONumeric(string text)
        {
            try
            {
                //if (text.Length >= 2)
                //{
                //    string MRNOSuffix = text.Substring(text.Length - 2, 2);
                ////    MRNOSuffix = Regex.Replace(MRNOSuffix.ToUpper(), @"[^A-Z]+", String.Empty).ToUpper();
                //    text = text.ToUpper().TrimEnd(MRNOSuffix.ToCharArray());///Dt include this in below Regex
                //}
                Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
                //return regex.IsMatch(text);
                return regex.IsMatch(text.Length >= 2 ? text.Substring(0, text.Length - 2) : text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //bool IsNotNumeric(string text)
        //{
        //    try
        //    {
        //        //if (text.Length >= 2)
        //        //{
        //        //    string MRNOSuffix = text.Substring(text.Length - 2, 2);

        //        ////    MRNOSuffix = Regex.Replace(MRNOSuffix.ToUpper(), @"[^A-Z]+", String.Empty).ToUpper();
        //        //    text = text.ToUpper().TrimEnd(MRNOSuffix.ToCharArray());///Dt include this in below Regex
        //        //}

        //        Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
        //        //return regex.IsMatch(text);
        //        return regex.IsMatch(text) ? false : true;

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        /// <summary>
        /// To Specify the rendering preference.(Default/SoftwareOnly)
        /// SoftwareOnly-it will effect to media..so change to default
        /// </summary>
        public void WPFApplicationRenderOptions()
        {
            string strAccelerationSetting = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["IsHardwareAccelerationRequired"]);
            if (string.IsNullOrEmpty(strAccelerationSetting) || strAccelerationSetting.ToUpper() == "FALSE")
            {
                System.Windows.Media.RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.SoftwareOnly;
            }
            else
            {
                System.Windows.Media.RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.Default;
            }
        }

        public DateTime SetDate(DateTime dtDate)
        {
            return Convert.ToDateTime(dtDate.ToString("dd-MMM-yyyy HH:mm"));
        }

        public DataTable GenerateMedicineAdminTimeData(DataSet dsData, Infologics.Medilogics.Enumerators.Visit.EpisodeStatus enumEpisodeStatus)
        {
            try
            {
                DataTable dtschedulefreq = new DataTable();
                bool IsScheduledataAvailable = false;
                //bool IsScheduleavailableforFrequency = true;// property to set whether frequency applicable for Schedule
                bool canRemove = false;
                Int32 SelectedMedicationType = Int32.MinValue;
                Int32 SelectedPastOrderType = Int32.MinValue;
                DataTable dtEMR_PAT_DTLS_PH_ORDER_TIME = new DataTable();
                dtEMR_PAT_DTLS_PH_ORDER_TIME = new Infologics.Medilogics.CommonXSD.XSD.CPOEMedicine.EMR_PAT_DTLS_PH_ORDER_TIMEDataTable().Clone();

                //General.Control.Common obj = new Common();
                int CPOEMaxAdminDays = int.MinValue;
                bool IsFrequencyScheduled = false;
                decimal Emr_PatDtlsOrderTimeID = -1;
                TimeSpan StartTime = TimeSpan.Zero;
                TimeSpan EndTime = TimeSpan.MaxValue;
                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MinValue;
                bool IsFrequencyRequiredForIV = false;
                String strMaxAdminDays = CommonData.GetDefaultSettings("CPOE MEDICINE MAX ADMIN DAYS");
                if (CommonData.GetDefaultSettings("CPOE_MEDICINE_IS_FREQUENCY_REQUIRED_FOR_IV") == "1")
                {
                    IsFrequencyRequiredForIV = true;
                }
                if (strMaxAdminDays.KIIsNotNullOrEmpty())
                {
                    CPOEMaxAdminDays = Convert.ToInt32(strMaxAdminDays);
                }


                //------------------Frequency for IV medicine (time table entry) ends-----------------
                bool hasAdminTime = false;
                //DataTable dtcriteria = new DataTable();
                //dtcriteria.Columns.Add("MODE");
                //dtcriteria.Columns.Add("LOOKUP_TYPE");
                //DataRow drCriteria = dtcriteria.NewRow();
                //drCriteria["MODE"] = 7;
                //drCriteria["LOOKUP_TYPE"] = "FREQUENCY";
                // dtcriteria.Rows.Add(drCriteria);
                DataTable dtfreqCriteria = new DataTable("CRITERIA");
                dtfreqCriteria.Columns.Add("LOOKUP_TYPE", typeof(string));
                dtfreqCriteria.Columns.Add("ISVALID", typeof(Int16));
                dtfreqCriteria.Rows.Add("FREQUENCY", 1);
                MainEMRShared objMainEMRShared = new MainEMRShared();
                object dtFrequency = objMainEMRShared.FetchEmrLookUp(dtfreqCriteria);//FetchChemoLookUP

                if (dsData != null && dsData.Tables.Contains("EMR_PAT_DTLS_PH_ORDER") && dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].KIIsNotNullAndRowCount())
                {
                    if (dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Columns.Contains("ISFREQUENCY_FREETEXT") && dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["ISFREQUENCY_FREETEXT"] != DBNull.Value && Convert.ToInt16(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["ISFREQUENCY_FREETEXT"]) == 1 &&
                        dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Columns.Contains("FREQUENCY_FREETEXT") && dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["FREQUENCY_FREETEXT"] != DBNull.Value)
                    {
                        if (dtFrequency != null)
                        {
                            DataTable dtTempFreq = (DataTable)dtFrequency;
                            DataRow[] drFreq = dtTempFreq.Select("EMR_LOOKUP_ID=0");
                            if (drFreq.Length == 0)
                            {
                                DataRow drr = dtTempFreq.NewRow();
                                drr["EMR_LOOKUP_ID"] = 0;
                                drr["LOOKUP_TYPE"] = "FREQUENCY";
                                drr["LOOKUP_VALUE"] = dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["FREQUENCY_FREETEXT"];
                                drr["FIELD1"] = "Free Text";
                                drr["FIELD2"] = "0";
                                drr["FIELD5"] = "0";
                                drr["FIELD6"] = "-4";
                                drr["ISVALID"] = 1;
                                drr["HOSPITAL_ID"] = CommonData.HospitalID;
                                dtTempFreq.Rows.Add(drr);
                                dtFrequency = (object)dtTempFreq;
                            }
                        }
                    }
                    if (Convert.ToInt16(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["ISLIFELONG"]) == 1)
                    {
                        dtEMR_PAT_DTLS_PH_ORDER_TIME = null;
                        return dtEMR_PAT_DTLS_PH_ORDER_TIME;
                    }
                    if ((Convert.ToInt16(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["ISGAS"]) == 0) && (IsFrequencyRequiredForIV == true || (IsFrequencyRequiredForIV == false && Convert.ToInt16(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["ISINFUSION"]) == 0)))
                    {
                        switch (Convert.ToInt32(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["MEDICATION_STATUS"]))
                        {
                            case (int)Infologics.Medilogics.Enumerators.EMR.MedicationStatus.NewOrders:
                                hasAdminTime = true;
                                break;
                            case (int)Infologics.Medilogics.Enumerators.EMR.MedicationStatus.Ongoing:
                                if (dsData != null && dsData.Tables.Contains("EMR_PAT_DTLS_PH_ORDER") && dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].KIIsNotNullAndRowCount() && dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["FREQUENCY"].ToString() != string.Empty)
                                {
                                    hasAdminTime = ((CommonData.GetDefaultSettings("CPOE MEDICINE ONGOING HOME MEDICATION ADMINISTRATION REQUIRED") == "1") && dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["FREQUENCY"].ToString() != string.Empty
                                    && dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["MEDICATION_STATUS"].KIIsNotNullOrEmpty()
                                       && (Convert.ToInt16(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["MEDICATION_STATUS"]) == (int)Infologics.Medilogics.Enumerators.EMR.MedicationStatus.Ongoing)
                                       && dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["ISHOMEMEDICATION"].KIIsNotNullOrEmpty() && Convert.ToInt32(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["ISHOMEMEDICATION"]) == Convert.ToInt32(Infologics.Medilogics.Enumerators.EMR.PastOrderType.HomePrescription)) ? true : false;
                                }
                                break;
                            case (int)Infologics.Medilogics.Enumerators.EMR.MedicationStatus.Past:
                                if ((CommonData.GetDefaultSettings("CPOE_MEDICINE_IMMUNIZATION_PAST_ORDER_ADMINISTRATION_ENABLE") == "1") && dsData != null && dsData.Tables.Contains("EMR_PAT_DTLS_PH_ORDER")
                                    && dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].KIIsNotNullAndRowCount()
                                    && dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["FREQUENCY"].ToString() != string.Empty
                                    && dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["MEDICATION_STATUS"].KIIsNotNullOrEmpty()
                                       && (Convert.ToInt16(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["MEDICATION_STATUS"]) == (int)Infologics.Medilogics.Enumerators.EMR.MedicationStatus.Past)
                                    && (dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["PRESCRIPTION_MODE"].KIIsNotNullOrEmpty()
                                    && Convert.ToInt16(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["PRESCRIPTION_MODE"]) == Convert.ToInt16(PrescriptionMode.Immunization))
                                     && dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["ISHOMEMEDICATION"].KIIsNotNullOrEmpty()

                                    )
                                {
                                    hasAdminTime = true;
                                }
                                else
                                {
                                    hasAdminTime = false;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (SelectedMedicationType == (int)Infologics.Medilogics.Enumerators.EMR.MedicationStatus.Ongoing)
                        {
                            //	hasAdminTime = ( ( CommonData.GetDefaultSettings("CPOE MEDICINE ONGOING HOME MEDICATION ADMINISTRATION REQUIRED") == "1" ) && SelectedMedicationType != Int32.MinValue && SelectedPastOrderType == Convert.ToInt32(Infologics.Medilogics.Enumerators.EMR.PastOrderType.HomePrescription) ) ? true : false;
                            hasAdminTime = ((CommonData.GetDefaultSettings("CPOE MEDICINE ONGOING HOME MEDICATION ADMINISTRATION REQUIRED") == "1") && dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["FREQUENCY"].ToString() != string.Empty
                                && dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["MEDICATION_STATUS"].KIIsNotNullOrEmpty()
                                   && (Convert.ToInt16(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["MEDICATION_STATUS"]) == (int)Infologics.Medilogics.Enumerators.EMR.MedicationStatus.Ongoing)
                                   && dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["ISHOMEMEDICATION"].KIIsNotNullOrEmpty() && Convert.ToInt32(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["ISHOMEMEDICATION"]) == Convert.ToInt32(Infologics.Medilogics.Enumerators.EMR.PastOrderType.HomePrescription)) ? true : false;

                        }
                        else if (SelectedMedicationType == (int)Infologics.Medilogics.Enumerators.EMR.MedicationStatus.Past)
                        {
                            if ((CommonData.GetDefaultSettings("CPOE_MEDICINE_IMMUNIZATION_PAST_ORDER_ADMINISTRATION_ENABLE") == "1") && dsData != null && dsData.Tables.Contains("EMR_PAT_DTLS_PH_ORDER")
                                   && dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].KIIsNotNullAndRowCount()
                                   && dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["FREQUENCY"].ToString() != string.Empty
                                   && dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["MEDICATION_STATUS"].KIIsNotNullOrEmpty()
                                      && (Convert.ToInt16(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["MEDICATION_STATUS"]) == (int)Infologics.Medilogics.Enumerators.EMR.MedicationStatus.Past)
                                   && (dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["PRESCRIPTION_MODE"].KIIsNotNullOrEmpty()
                                   && Convert.ToInt16(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["PRESCRIPTION_MODE"]) == Convert.ToInt16(PrescriptionMode.Immunization))
                                    && dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["ISHOMEMEDICATION"].KIIsNotNullOrEmpty()
                                //&& (Convert.ToInt32(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["ISHOMEMEDICATION"]) == Convert.ToInt32(Infologics.Medilogics.Enumerators.EMR.PastOrderType.Immu_sourceunspecified) ||
                                //   Convert.ToInt32(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["ISHOMEMEDICATION"]) == Convert.ToInt32(Infologics.Medilogics.Enumerators.EMR.PastOrderType.Immu_ParentsWrittenRecord) ||
                                //   Convert.ToInt32(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["ISHOMEMEDICATION"]) == Convert.ToInt32(Infologics.Medilogics.Enumerators.EMR.PastOrderType.Immu_ParentsRecall) ||
                                //   Convert.ToInt32(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["ISHOMEMEDICATION"]) == Convert.ToInt32(Infologics.Medilogics.Enumerators.EMR.PastOrderType.Immu_otherprovider) ||
                                //   Convert.ToInt32(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["ISHOMEMEDICATION"]) == Convert.ToInt32(Infologics.Medilogics.Enumerators.EMR.PastOrderType.Immu_NotAdministered) ||
                                //   Convert.ToInt32(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["ISHOMEMEDICATION"]) == Convert.ToInt32(Infologics.Medilogics.Enumerators.EMR.PastOrderType.Immu_fromschoolrecord) ||
                                //   Convert.ToInt32(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["ISHOMEMEDICATION"]) == Convert.ToInt32(Infologics.Medilogics.Enumerators.EMR.PastOrderType.Immu_frompublicagency) ||
                                //   Convert.ToInt32(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["ISHOMEMEDICATION"]) == Convert.ToInt32(Infologics.Medilogics.Enumerators.EMR.PastOrderType.Immu_fromotherregistry) ||
                                //   Convert.ToInt32(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["ISHOMEMEDICATION"]) == Convert.ToInt32(Infologics.Medilogics.Enumerators.EMR.PastOrderType.Immu_frombirthcertificate))
                                   )
                            {
                                hasAdminTime = true;
                            }
                            else
                            {
                                hasAdminTime = false;
                            }
                        }
                        else
                        {
                            hasAdminTime = false;
                        }
                    }

                    if (hasAdminTime == true)
                    {
                        bool canadd = true;
                        if (Convert.ToString(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["FREQUENCY"]).Trim() != string.Empty)
                        {
                            //DataRow[] drFrq = dtFrequency.Select("EMR_LOOKUP_ID=" + Convert.ToDecimal(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["FREQUENCY"]));
                            DataRow[] drFrq = ((DataTable)dtFrequency).Select("EMR_LOOKUP_ID=" + Convert.ToDecimal(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["FREQUENCY"]));
                            if (drFrq.Length > 0)
                            {
                                //if( drFrq[0]["FIELD15"] != DBNull.Value && Convert.ToInt32(drFrq[0]["FIELD15"]) == 1 )
                                //    {
                                //    IsScheduleavailableforFrequency = true;
                                //    }
                                //else
                                //    {
                                //    IsScheduleavailableforFrequency = false;
                                //    }
                            }
                        }
                        StartTime = TimeSpan.Parse(Convert.ToDateTime(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["START_DATE"]).ToString("HH:mm:ss"));
                        DateTime tempstartDate = Convert.ToDateTime(Convert.ToDateTime(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["START_DATE"]).ToString("dd-MMM-yyyy"));

                        if (CPOEMaxAdminDays == 0 || CPOEMaxAdminDays == int.MinValue)
                        {
                            CPOEMaxAdminDays = 0;
                        }
                        else
                        {
                            CPOEMaxAdminDays = CPOEMaxAdminDays - 1;
                        }
                        DateTime MaxAdminDays = tempstartDate.AddDays(CPOEMaxAdminDays);
                        canadd = true;

                        IsFrequencyScheduled = false;
                        //IsFrequencyScheduled = IsScheduled(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"]);

                        if (canadd)
                        {
                            DateTime Medstartdate = Convert.ToDateTime(Convert.ToDateTime(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["START_DATE"]).ToString("dd-MMM-yyyy"));
                            DateTime MedEnddate = Convert.ToDateTime(Convert.ToDateTime(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["END_DATE"]).ToString("dd-MMM-yyyy"));
                            DateTime MedEndDateToCompare = (dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["STOP_DATE"] != DBNull.Value ? Convert.ToDateTime(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["STOP_DATE"]) : (dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["END_DATE"] != DBNull.Value ? Convert.ToDateTime(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["END_DATE"]) : DateTime.MinValue));
                            //if( IsFrequencyScheduled == true && IsScheduleavailableforFrequency == true )
                            //    {
                            //    dtschedulefreq = obj.GetMedicineScheduledDays(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]);
                            //    IsScheduledataAvailable = dtschedulefreq.KIIsNotNullAndRowCount();

                            //    }
                            DateTime dttime = DateTime.MinValue;
                            if (dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["FREQUENCY"] != DBNull.Value
                                && dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["FREQUENCY"].ToString().Trim().Length > 0)
                            {
                                DataRow[] drSelectedFrq = ((DataTable)dtFrequency).Select("EMR_LOOKUP_ID=" + Convert.ToDecimal(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["FREQUENCY"]));
                                if (drSelectedFrq.Length > 0)
                                {

                                    DataRow dr = null;
                                    if (drSelectedFrq[0]["FIELD5"].ToString() != string.Empty && drSelectedFrq[0]["FIELD5"].ToString() == "1")
                                    {
                                        #region Exceptional
                                        DateTime dtStartPrn = Convert.ToDateTime(Convert.ToDateTime(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["START_DATE"]).ToString("dd-MMM-yyyy"));
                                        DateTime dtEndPrn = Convert.ToDateTime(Convert.ToDateTime(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["END_DATE"]).ToString("dd-MMM-yyyy"));
                                        dttime = Convert.ToDateTime(dtStartPrn.ToString("dd-MMM-yyyy HH:mm"));

                                        //if( IsScheduledataAvailable == true )
                                        //    {
                                        //    if( obj.IsAdminDateinScheduledDays(dtschedulefreq , dttime.Date) == false )
                                        //        {
                                        //        dttime = DateTime.MinValue;
                                        //        }
                                        //    }

                                        if (dttime != null && dttime != DateTime.MinValue && dttime.Date <= MaxAdminDays && MedEndDateToCompare != DateTime.MinValue && SetDate(dttime) <= SetDate(MedEndDateToCompare))
                                        {
                                            dr = dtEMR_PAT_DTLS_PH_ORDER_TIME.NewRow();
                                            dr["EMR_PAT_DTLS_PH_ORDER_TIME_ID"] = Emr_PatDtlsOrderTimeID;
                                            //dtStartHr = Convert.ToDateTime(EMRPatMedicinesTemp.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["START_DATE"]);
                                            //dr["TIME"] = dtStartPrn;
                                            dr["TIME"] = Convert.ToDateTime(Convert.ToDateTime(dttime.ToString("dd-MMM-yyyy HH:mm")));//Convert.ToDateTime(dtStartPrn.ToString("dd-MMM-yyyy HH:mm"));
                                            dr["EMR_PAT_DTLS_PH_ORDER_ID"] = dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["EMR_PAT_DTLS_PH_ORDER_ID"];
                                            dtEMR_PAT_DTLS_PH_ORDER_TIME.Rows.Add(dr);
                                            Emr_PatDtlsOrderTimeID = Emr_PatDtlsOrderTimeID - 1;
                                        }
                                        dtStartPrn = dtStartPrn.AddDays(1);
                                        dtStartPrn = Convert.ToDateTime(dtStartPrn.ToString("dd-MMM-yyyy"));
                                        if (dtStartPrn != null && dtEndPrn != null && dtEndPrn.Date != DateTime.MinValue.Date && dtEndPrn.Date != DateTime.MaxValue.Date && dtStartPrn.Date != DateTime.MinValue.Date && dtStartPrn.Date != DateTime.MaxValue.Date)
                                        {
                                            while (dtStartPrn <= dtEndPrn)
                                            {
                                                if (Convert.ToDateTime(dtStartPrn.ToString("dd-MMM-yyyy")) == Convert.ToDateTime(dtEndPrn.ToString("dd-MMM-yyyy"))
                                                        && EndTime != TimeSpan.Parse("23:59:59"))
                                                {
                                                    dttime = Convert.ToDateTime(dtEndPrn.ToString("dd-MMM-yyyy HH:mm"));
                                                }
                                                else
                                                {
                                                    dttime = Convert.ToDateTime(dtStartPrn.ToString("dd-MMM-yyyy HH:mm"));
                                                }
                                                //if( IsScheduledataAvailable == true )
                                                //    {
                                                //    if( obj.IsAdminDateinScheduledDays(dtschedulefreq , dttime.Date) == false )
                                                //        {
                                                //        dttime = DateTime.MinValue;
                                                //        }
                                                //    }
                                                if (dttime != null && dttime != DateTime.MinValue && dttime.Date <= MaxAdminDays && MedEndDateToCompare != DateTime.MinValue && SetDate(dttime) <= SetDate(MedEndDateToCompare))
                                                {
                                                    dr = dtEMR_PAT_DTLS_PH_ORDER_TIME.NewRow();
                                                    dr["EMR_PAT_DTLS_PH_ORDER_TIME_ID"] = Emr_PatDtlsOrderTimeID;
                                                    dr["TIME"] = Convert.ToDateTime(dttime.ToString("dd-MMM-yyyy HH:mm"));
                                                    dr["EMR_PAT_DTLS_PH_ORDER_ID"] = dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["EMR_PAT_DTLS_PH_ORDER_ID"];
                                                    dtEMR_PAT_DTLS_PH_ORDER_TIME.Rows.Add(dr);
                                                    Emr_PatDtlsOrderTimeID = Emr_PatDtlsOrderTimeID - 1;
                                                }
                                                dtStartPrn = dtStartPrn.AddDays(1);
                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Normal
                                        switch (Convert.ToInt16(drSelectedFrq[0]["FIELD6"]))
                                        {
                                            case (int)Infologics.Medilogics.Enumerators.EMR.FrequencyTypes.prn:
                                                DateTime dtStartPrn = Convert.ToDateTime(Convert.ToDateTime(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["START_DATE"]).ToString("dd-MMM-yyyy"));
                                                DateTime dtEndPrn = Convert.ToDateTime(Convert.ToDateTime(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["END_DATE"]).ToString("dd-MMM-yyyy"));
                                                dttime = Convert.ToDateTime(dtStartPrn.ToString("dd-MMM-yyyy HH:mm"));
                                                //if( IsScheduledataAvailable == true )
                                                //    {
                                                //    if( obj.IsAdminDateinScheduledDays(dtschedulefreq , dttime.Date) == false )
                                                //        {
                                                //        dttime = DateTime.MinValue;
                                                //        }
                                                //    }
                                                if (dttime != null && dttime != DateTime.MinValue && dttime.Date <= MaxAdminDays && MedEndDateToCompare != DateTime.MinValue && SetDate(dttime) <= SetDate(MedEndDateToCompare))
                                                {
                                                    dr = dtEMR_PAT_DTLS_PH_ORDER_TIME.NewRow();
                                                    dr["EMR_PAT_DTLS_PH_ORDER_TIME_ID"] = Emr_PatDtlsOrderTimeID;
                                                    dr["TIME"] = Convert.ToDateTime(dttime.ToString("dd-MMM-yyyy HH:mm"));
                                                    dr["EMR_PAT_DTLS_PH_ORDER_ID"] = dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["EMR_PAT_DTLS_PH_ORDER_ID"];
                                                    dtEMR_PAT_DTLS_PH_ORDER_TIME.Rows.Add(dr);
                                                    Emr_PatDtlsOrderTimeID = Emr_PatDtlsOrderTimeID - 1;
                                                }
                                                dtStartPrn = dtStartPrn.AddDays(1);
                                                dtStartPrn = Convert.ToDateTime(dtStartPrn.ToString("dd-MMM-yyyy"));
                                                if (dtStartPrn != null && dtEndPrn != null && dtEndPrn.Date != DateTime.MinValue.Date && dtEndPrn.Date != DateTime.MaxValue.Date && dtStartPrn.Date != DateTime.MinValue.Date && dtStartPrn.Date != DateTime.MaxValue.Date)
                                                {
                                                    while (dtStartPrn <= dtEndPrn)
                                                    {
                                                        if (Convert.ToDateTime(dtStartPrn.ToString("dd-MMM-yyyy")) == Convert.ToDateTime(dtEndPrn.ToString("dd-MMM-yyyy"))
                                                            && EndTime != TimeSpan.Parse("23:59:59"))
                                                        {
                                                            dttime = Convert.ToDateTime(dtEndPrn.ToString("dd-MMM-yyyy HH:mm"));
                                                        }
                                                        else
                                                        {
                                                            dttime = Convert.ToDateTime(dtStartPrn.ToString("dd-MMM-yyyy HH:mm"));
                                                        }
                                                        //if( IsScheduledataAvailable == true )
                                                        //    {
                                                        //    if( obj.IsAdminDateinScheduledDays(dtschedulefreq , dttime.Date) == false )
                                                        //        {
                                                        //        dttime = DateTime.MinValue;
                                                        //        }
                                                        //    }
                                                        if (dttime != null && dttime != DateTime.MinValue && dttime.Date <= MaxAdminDays && MedEndDateToCompare != DateTime.MinValue && SetDate(dttime) <= SetDate(MedEndDateToCompare))
                                                        {

                                                            dr = dtEMR_PAT_DTLS_PH_ORDER_TIME.NewRow();
                                                            dr["EMR_PAT_DTLS_PH_ORDER_TIME_ID"] = Emr_PatDtlsOrderTimeID;
                                                            dr["TIME"] = Convert.ToDateTime(dttime.ToString("dd-MMM-yyyy HH:mm"));
                                                            dr["EMR_PAT_DTLS_PH_ORDER_ID"] = dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["EMR_PAT_DTLS_PH_ORDER_ID"];

                                                            dtEMR_PAT_DTLS_PH_ORDER_TIME.Rows.Add(dr);
                                                            Emr_PatDtlsOrderTimeID = Emr_PatDtlsOrderTimeID - 1;
                                                        }
                                                        dtStartPrn = dtStartPrn.AddDays(1);
                                                    }
                                                }
                                                break;
                                            case (int)Infologics.Medilogics.Enumerators.EMR.FrequencyTypes.Hourly:
                                                if (dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["FREQUENCY_VALUE"] != DBNull.Value
                                                    && dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["FREQUENCY_VALUE"].ToString().Trim().Length > 0 &&
                                                    dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["FREQUENCY_VALUE"].ToString().Trim() != "0")
                                                {
                                                    DateTime dtStartHr = Convert.ToDateTime(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["START_DATE"]);
                                                    DateTime dtEndHr = Convert.ToDateTime(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["END_DATE"]);

                                                    dttime = Convert.ToDateTime(dtStartHr.ToString("dd-MMM-yyyy HH:mm"));
                                                    //if( IsScheduledataAvailable == true )
                                                    //    {
                                                    //    if( obj.IsAdminDateinScheduledDays(dtschedulefreq , dttime.Date) == false )
                                                    //        {
                                                    //        dttime = DateTime.MinValue;
                                                    //        }
                                                    //    }
                                                    if (dttime != null && dttime != DateTime.MinValue && dttime.Date <= MaxAdminDays && MedEndDateToCompare != DateTime.MinValue && SetDate(dttime) <= SetDate(MedEndDateToCompare))
                                                    {
                                                        dr = dtEMR_PAT_DTLS_PH_ORDER_TIME.NewRow();
                                                        dr["EMR_PAT_DTLS_PH_ORDER_TIME_ID"] = Emr_PatDtlsOrderTimeID;
                                                        dr["TIME"] = Convert.ToDateTime(dttime.ToString("dd-MMM-yyyy HH:mm"));//Convert.ToDateTime(dtStartHr.ToString("dd-MMM-yyyy HH:mm"));
                                                        dr["EMR_PAT_DTLS_PH_ORDER_ID"] = dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["EMR_PAT_DTLS_PH_ORDER_ID"];
                                                        dtEMR_PAT_DTLS_PH_ORDER_TIME.Rows.Add(dr);
                                                        Emr_PatDtlsOrderTimeID = Emr_PatDtlsOrderTimeID - 1;
                                                    }
                                                    dtStartHr = dtStartHr.AddHours(Convert.ToInt32(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["FREQUENCY_VALUE"]));

                                                    if (dtStartHr != null && dtEndHr != null && dtEndHr.Date != DateTime.MinValue.Date && dtEndHr.Date != DateTime.MaxValue.Date && dtStartHr.Date != DateTime.MinValue.Date && dtStartHr.Date != DateTime.MaxValue.Date)
                                                    {
                                                        while (dtStartHr <= dtEndHr)
                                                        {

                                                            dttime = Convert.ToDateTime(dtStartHr.ToString("dd-MMM-yyyy HH:mm"));
                                                            //if( IsScheduledataAvailable == true )
                                                            //    {
                                                            //    if( obj.IsAdminDateinScheduledDays(dtschedulefreq , dttime.Date) == false )
                                                            //        {
                                                            //        dttime = DateTime.MinValue;
                                                            //        }
                                                            //    }
                                                            if (dttime != null && dttime != DateTime.MinValue && dttime.Date <= MaxAdminDays && MedEndDateToCompare != DateTime.MinValue && SetDate(dttime) <= SetDate(MedEndDateToCompare))
                                                            {
                                                                dr = dtEMR_PAT_DTLS_PH_ORDER_TIME.NewRow();
                                                                dr["EMR_PAT_DTLS_PH_ORDER_TIME_ID"] = Emr_PatDtlsOrderTimeID;
                                                                dr["TIME"] = Convert.ToDateTime(dttime.ToString("dd-MMM-yyyy HH:mm"));//Convert.ToDateTime(dtStartHr.ToString("dd-MMM-yyyy HH:mm"));
                                                                dr["EMR_PAT_DTLS_PH_ORDER_ID"] = dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["EMR_PAT_DTLS_PH_ORDER_ID"];
                                                                dtEMR_PAT_DTLS_PH_ORDER_TIME.Rows.Add(dr);
                                                                Emr_PatDtlsOrderTimeID = Emr_PatDtlsOrderTimeID - 1;
                                                            }
                                                            dtStartHr = dtStartHr.AddHours(Convert.ToInt32(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["FREQUENCY_VALUE"]));
                                                        }
                                                    }
                                                }
                                                break;
                                            case (int)Infologics.Medilogics.Enumerators.EMR.FrequencyTypes.Days:
                                                if (dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["FREQUENCY_VALUE"] != DBNull.Value
                                                    && dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["FREQUENCY_VALUE"].ToString().Trim().Length > 0 &&
                                                    dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["FREQUENCY_VALUE"].ToString().Trim() != "0")
                                                {
                                                    DateTime dtStartHrDays = Convert.ToDateTime(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["START_DATE"]);
                                                    DateTime dtEndHrDays = Convert.ToDateTime(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["END_DATE"]);

                                                    dttime = Convert.ToDateTime(dtStartHrDays.ToString("dd-MMM-yyyy HH:mm"));
                                                    //if( IsScheduledataAvailable == true )
                                                    //    {
                                                    //    if( obj.IsAdminDateinScheduledDays(dtschedulefreq , dttime.Date) == false )
                                                    //        {
                                                    //        dttime = DateTime.MinValue;
                                                    //        }
                                                    //    }

                                                    if (dttime != null && dttime != DateTime.MinValue && dttime.Date <= MaxAdminDays && MedEndDateToCompare != DateTime.MinValue && SetDate(dttime) <= SetDate(MedEndDateToCompare))
                                                    {
                                                        dr = dtEMR_PAT_DTLS_PH_ORDER_TIME.NewRow();
                                                        dr["EMR_PAT_DTLS_PH_ORDER_TIME_ID"] = Emr_PatDtlsOrderTimeID;
                                                        dr["TIME"] = Convert.ToDateTime(dttime.ToString("dd-MMM-yyyy HH:mm"));//Convert.ToDateTime(dtStartHrDays.ToString("dd-MMM-yyyy HH:mm"));
                                                        dr["EMR_PAT_DTLS_PH_ORDER_ID"] = dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["EMR_PAT_DTLS_PH_ORDER_ID"];
                                                        dtEMR_PAT_DTLS_PH_ORDER_TIME.Rows.Add(dr);
                                                        Emr_PatDtlsOrderTimeID = Emr_PatDtlsOrderTimeID - 1;
                                                    }
                                                    dtStartHrDays = dtStartHrDays.AddDays(Convert.ToInt32(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["FREQUENCY_VALUE"]));

                                                    if (dtStartHrDays != null && dtEndHrDays != null && dtEndHrDays.Date != DateTime.MinValue.Date && dtEndHrDays.Date != DateTime.MaxValue.Date && dtStartHrDays.Date != DateTime.MinValue.Date && dtStartHrDays.Date != DateTime.MaxValue.Date)
                                                    {
                                                        while (dtStartHrDays <= dtEndHrDays)
                                                        {

                                                            dttime = Convert.ToDateTime(dtStartHrDays.ToString("dd-MMM-yyyy HH:mm"));
                                                            //if( IsScheduledataAvailable == true )
                                                            //    {
                                                            //    if( obj.IsAdminDateinScheduledDays(dtschedulefreq , dttime.Date) == false )
                                                            //        {
                                                            //        dttime = DateTime.MinValue;
                                                            //        }
                                                            //    }
                                                            if (dttime != null && dttime != DateTime.MinValue && dttime.Date <= MaxAdminDays && MedEndDateToCompare != DateTime.MinValue && SetDate(dttime) <= SetDate(MedEndDateToCompare))
                                                            {
                                                                dr = dtEMR_PAT_DTLS_PH_ORDER_TIME.NewRow();
                                                                dr["EMR_PAT_DTLS_PH_ORDER_TIME_ID"] = Emr_PatDtlsOrderTimeID;
                                                                dr["TIME"] = Convert.ToDateTime(dttime.ToString("dd-MMM-yyyy HH:mm"));//Convert.ToDateTime(dtStartHrDays.ToString("dd-MMM-yyyy HH:mm"));
                                                                dr["EMR_PAT_DTLS_PH_ORDER_ID"] = dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["EMR_PAT_DTLS_PH_ORDER_ID"];
                                                                dtEMR_PAT_DTLS_PH_ORDER_TIME.Rows.Add(dr);
                                                                Emr_PatDtlsOrderTimeID = Emr_PatDtlsOrderTimeID - 1;
                                                            }
                                                            dtStartHrDays = dtStartHrDays.AddDays(Convert.ToInt32(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["FREQUENCY_VALUE"]));
                                                        }
                                                    }
                                                }
                                                break;
                                            default:
                                                if (dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["ADMIN_TIME"] != DBNull.Value
                                                    && dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["ADMIN_TIME"].ToString().Trim().Length > 0)
                                                {
                                                    string[] var = (dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["ADMIN_TIME"].ToString()).Split(',');
                                                    DateTime dtStart = Convert.ToDateTime(Convert.ToDateTime(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["START_DATE"]).ToShortDateString());
                                                    DateTime dtEnd = Convert.ToDateTime(Convert.ToDateTime(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["END_DATE"]).ToShortDateString());

                                                    bool IsOPStartTimeZero = false;
                                                    if ((Convert.ToDateTime(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["START_DATE"]).TimeOfDay == TimeSpan.Zero) && enumEpisodeStatus == (int)Infologics.Medilogics.Enumerators.Visit.EpisodeStatus.OP)
                                                    {
                                                        IsOPStartTimeZero = true;
                                                        DateTime dateVal1 = Convert.ToDateTime(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["START_DATE"]);
                                                        dttime = Convert.ToDateTime(Convert.ToDateTime(dateVal1.Date.Add((TimeSpan.Parse(var[0])))).ToString("dd-MMM-yyyy HH:mm"));
                                                    }
                                                    else
                                                    {
                                                        DateTime dateVal1 = Convert.ToDateTime(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["START_DATE"]);
                                                        dttime = Convert.ToDateTime(dateVal1.ToString("dd-MMM-yyyy HH:mm"));
                                                    }
                                                    //if( IsScheduledataAvailable == true )
                                                    //    {
                                                    //    if( obj.IsAdminDateinScheduledDays(dtschedulefreq , dttime.Date) == false )
                                                    //        {
                                                    //        dttime = DateTime.MinValue;
                                                    //        }
                                                    //    }

                                                    if (dttime != null && dttime != DateTime.MinValue && dttime.Date <= MaxAdminDays && MedEndDateToCompare != DateTime.MinValue && SetDate(dttime) <= SetDate(MedEndDateToCompare))
                                                    {
                                                        dr = dtEMR_PAT_DTLS_PH_ORDER_TIME.NewRow();
                                                        dr["EMR_PAT_DTLS_PH_ORDER_TIME_ID"] = Emr_PatDtlsOrderTimeID;
                                                        DateTime dtStarttimeforOP = DateTime.MinValue;
                                                        dr["TIME"] = dttime;

                                                        if ((Convert.ToDateTime(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["START_DATE"]).TimeOfDay == TimeSpan.Zero) && enumEpisodeStatus == (int)Infologics.Medilogics.Enumerators.Visit.EpisodeStatus.OP)
                                                        {
                                                            dtStarttimeforOP = dttime;
                                                        }
                                                        dr["EMR_PAT_DTLS_PH_ORDER_ID"] = dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["EMR_PAT_DTLS_PH_ORDER_ID"];
                                                        dtEMR_PAT_DTLS_PH_ORDER_TIME.Rows.Add(dr);
                                                        Emr_PatDtlsOrderTimeID = Emr_PatDtlsOrderTimeID - 1;
                                                    }
                                                    TimeSpan ttmaxDuration = TimeSpan.Zero;
                                                    if (drSelectedFrq[0]["FIELD3"].ToString() != String.Empty)
                                                    {
                                                        ttmaxDuration = TimeSpan.FromMinutes(Convert.ToDouble(drSelectedFrq[0]["FIELD3"]));
                                                    }
                                                    TimeSpan tTimeGap;
                                                    if (IsOPStartTimeZero == true)
                                                    {
                                                        tTimeGap = TimeSpan.Parse(var[0]) + ttmaxDuration;
                                                    }
                                                    else
                                                    {
                                                        tTimeGap = StartTime + ttmaxDuration;
                                                    }
                                                    bool canAdd = false;
                                                    for (int i = 1; i < var.Length; i++)
                                                    {
                                                        if (canAdd == false)
                                                        {
                                                            if (i == 1)
                                                            {
                                                                tTimeGap = StartTime + ttmaxDuration;
                                                            }
                                                            //------- modfied on 06 dec 2012
                                                            TimeSpan tt = TimeSpan.Parse(Convert.ToDateTime(var[i]).ToString("HH:mm"));
                                                            if (tTimeGap.Days == 0)
                                                            {
                                                                tTimeGap = TimeSpan.Parse(Convert.ToDateTime(tTimeGap.ToString()).ToString("HH:mm"));
                                                                //
                                                                // TimeSpan tt = TimeSpan.Parse(var[i].ToString("HH:mm"));
                                                                // if (TimeSpan.Parse(var[i]) >= tTimeGap)
                                                                if (tt >= tTimeGap)
                                                                {
                                                                    canAdd = true;
                                                                }
                                                                else
                                                                {

                                                                    canAdd = false;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //canAdd = true;//COMMENTED FOR BUGID 11345  26JUL20132.8  
                                                                canAdd = false;
                                                            }
                                                        }
                                                        if (canAdd == true)
                                                        {
                                                            DateTime dateVal2 = Convert.ToDateTime(dtStart.ToShortDateString() + " " + var[i]);
                                                            dttime = Convert.ToDateTime(dateVal2.ToString("dd-MMM-yyyy HH:mm"));
                                                            //if( IsScheduledataAvailable == true )
                                                            //    {
                                                            //    if( obj.IsAdminDateinScheduledDays(dtschedulefreq , dttime.Date) == false )
                                                            //        {
                                                            //        dttime = DateTime.MinValue;
                                                            //        }
                                                            //    }
                                                            if (dttime != null && dttime != DateTime.MinValue && dttime.Date <= MaxAdminDays && MedEndDateToCompare != DateTime.MinValue && SetDate(dttime) <= SetDate(MedEndDateToCompare))
                                                            {
                                                                dr = dtEMR_PAT_DTLS_PH_ORDER_TIME.NewRow();
                                                                dr["EMR_PAT_DTLS_PH_ORDER_TIME_ID"] = Emr_PatDtlsOrderTimeID;
                                                                dr["TIME"] = dttime;
                                                                dr["EMR_PAT_DTLS_PH_ORDER_ID"] = dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["EMR_PAT_DTLS_PH_ORDER_ID"];
                                                                dtEMR_PAT_DTLS_PH_ORDER_TIME.Rows.Add(dr);
                                                                Emr_PatDtlsOrderTimeID = Emr_PatDtlsOrderTimeID - 1;
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
                                                                    if (TimeSpan.Parse(Convert.ToDateTime(dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["END_DATE"]).ToString("HH:mm:ss")) >= TimeSpan.Parse(var[i]))
                                                                    {
                                                                        DateTime dateVal3 = Convert.ToDateTime(dtStart.ToShortDateString() + " " + var[i]);
                                                                        dttime = Convert.ToDateTime(dateVal3.ToString("dd-MMM-yyyy HH:mm"));
                                                                        //if( IsScheduledataAvailable == true )
                                                                        //    {
                                                                        //    if( obj.IsAdminDateinScheduledDays(dtschedulefreq , dttime.Date) == false )
                                                                        //        {
                                                                        //        dttime = DateTime.MinValue;
                                                                        //        }
                                                                        //    }
                                                                        if (dttime != null && dttime != DateTime.MinValue && dttime.Date <= MaxAdminDays && MedEndDateToCompare != DateTime.MinValue && SetDate(dttime) <= SetDate(MedEndDateToCompare))
                                                                        {

                                                                            dr = dtEMR_PAT_DTLS_PH_ORDER_TIME.NewRow();
                                                                            dr["EMR_PAT_DTLS_PH_ORDER_TIME_ID"] = Emr_PatDtlsOrderTimeID;
                                                                            dr["TIME"] = dttime;
                                                                            dr["EMR_PAT_DTLS_PH_ORDER_ID"] = dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["EMR_PAT_DTLS_PH_ORDER_ID"];
                                                                            dtEMR_PAT_DTLS_PH_ORDER_TIME.Rows.Add(dr);
                                                                            Emr_PatDtlsOrderTimeID = Emr_PatDtlsOrderTimeID - 1;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    DateTime dateVal4 = Convert.ToDateTime(dtStart.ToShortDateString() + " " + var[i]);
                                                                    dttime = Convert.ToDateTime(dateVal4.ToString("dd-MMM-yyyy HH:mm"));
                                                                    //if( IsScheduledataAvailable == true )
                                                                    //    {
                                                                    //    if( obj.IsAdminDateinScheduledDays(dtschedulefreq , dttime.Date) == false )
                                                                    //        {
                                                                    //        dttime = DateTime.MinValue;
                                                                    //        }
                                                                    //    }
                                                                    if (dttime != null && dttime != DateTime.MinValue && dttime.Date <= MaxAdminDays && MedEndDateToCompare != DateTime.MinValue && SetDate(dttime) <= SetDate(MedEndDateToCompare))
                                                                    {

                                                                        dr = dtEMR_PAT_DTLS_PH_ORDER_TIME.NewRow();
                                                                        dr["EMR_PAT_DTLS_PH_ORDER_TIME_ID"] = Emr_PatDtlsOrderTimeID;
                                                                        dr["TIME"] = dttime;
                                                                        dr["EMR_PAT_DTLS_PH_ORDER_ID"] = dsData.Tables["EMR_PAT_DTLS_PH_ORDER"].Rows[0]["EMR_PAT_DTLS_PH_ORDER_ID"];
                                                                        dtEMR_PAT_DTLS_PH_ORDER_TIME.Rows.Add(dr);
                                                                        Emr_PatDtlsOrderTimeID = Emr_PatDtlsOrderTimeID - 1;
                                                                    }
                                                                }
                                                            }
                                                            dtStart = dtStart.AddDays(1);
                                                        }
                                                    }

                                                }
                                                break;
                                        }

                                        #endregion
                                    }
                                }
                            }
                        }
                    }
                }
                return dtEMR_PAT_DTLS_PH_ORDER_TIME;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Reads the smart card data.
        /// </summary>
        /// <returns></returns>
        public DataTable ReadIDCardData(int IsPhoneReadFromSmartCard, bool IsEnableSmartCard)
        {
            DataTable dtCardData = null;

            try
            {
                string cardType = ConfigurationSettings.AppSettings["SmartCardType"].ToString().ToUpper();
                if (cardType == "OMNIKEY")
                {
                    bool is64BitProcess = (IntPtr.Size == 8);
                    if (is64BitProcess == true)
                    {
                        object[] objret = new object[3];
                        object[] param = new object[2];
                        param[0] = Convert.ToInt16(IsPhoneReadFromSmartCard);
                        param[1] = Convert.ToBoolean(IsEnableSmartCard);
                        object obj = new object();
                        string[] Control = null;
                        string DllName = string.Empty;
                        DllName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SmartCardDll64Bit"]);
                        Control = DllName.Split(',');
                        string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "64BitDlls\\");
                        //  Assembly asm = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + Control[0]);
                        Assembly asm = Assembly.LoadFrom(FilePath + Control[0]);
                        MethodInfo Minfo = null;
                        string NameSpace = Control[0];
                        NameSpace = NameSpace.Substring(0, NameSpace.Length - 4);
                        string ClassName = Control[1];
                        obj = asm.CreateInstance(NameSpace + "." + ClassName);
                        Minfo = obj.GetType().GetMethod("ReadOmniKeyCard");
                        // IsEnableSmartCard = true;
                        objret = (object[])Minfo.Invoke(obj, param);
                        dtCardData = (DataTable)objret[0];
                        IsEnableSmartCard = Convert.ToBoolean(objret[1]);
                        IsPhoneReadFromSmartCard = Convert.ToInt16(objret[2]);
                    }
                    else
                    {
                        object[] objret = new object[3];
                        object[] param = new object[2];
                        param[0] = Convert.ToInt16(IsPhoneReadFromSmartCard);
                        param[1] = Convert.ToBoolean(IsEnableSmartCard);
                        object obj = new object();
                        string[] Control = null;
                        string DllName = string.Empty;
                        DllName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SmartCardDll32Bit"]);
                        Control = DllName.Split(',');
                        string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "32BitDlls\\");
                        //  Assembly asm = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + Control[0]);
                        Assembly asm = Assembly.LoadFrom(FilePath + Control[0]);
                        MethodInfo Minfo = null;
                        string NameSpace = Control[0];
                        NameSpace = NameSpace.Substring(0, NameSpace.Length - 4);
                        string ClassName = Control[1];
                        obj = asm.CreateInstance(NameSpace + "." + ClassName);
                        Minfo = obj.GetType().GetMethod("ReadOmniKeyCard");
                        // IsEnableSmartCard = true;
                        objret = (object[])Minfo.Invoke(obj, param);
                        dtCardData = (DataTable)objret[0];
                        IsEnableSmartCard = Convert.ToBoolean(objret[1]);
                        IsPhoneReadFromSmartCard = Convert.ToInt16(objret[2]);
                    }
                    //object[] objret = new object[3];
                    //object[] param = new object[2];
                    //param[0] = Convert.ToInt16(IsPhoneReadFromSmartCard);
                    //param[1] = Convert.ToBoolean(IsEnableSmartCard);
                    //object obj = new object();
                    //string[] Control = null;
                    //string DllName = string.Empty;
                    //DllName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SmartCardDll"]);
                    //Control = DllName.Split(',');
                    //Assembly asm = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + Control[0]);
                    //MethodInfo Minfo = null;
                    //string NameSpace = Control[0];
                    //NameSpace = NameSpace.Substring(0, NameSpace.Length - 4);
                    //string ClassName = Control[1]; 
                    //obj = asm.CreateInstance(NameSpace + "." + ClassName);
                    //Minfo = obj.GetType().GetMethod("ReadOmniKeyCard");
                    //// IsEnableSmartCard = true;
                    //objret = (object[])Minfo.Invoke(obj, param);
                    //dtCardData = (DataTable)objret[0];
                    //IsEnableSmartCard = Convert.ToBoolean(objret[1]);
                    //IsPhoneReadFromSmartCard = Convert.ToInt16(objret[2]);
                    // IsEnableSmartCard = Convert.ToBoolean(objret[2]);



                    //dtCardData = ReadOmniKeyCard();
                    // dtCardData=(DataTable)
                }
                else if (cardType == "SMARTCARD")
                {
                    dtCardData = ReadSmartCardData(IsPhoneReadFromSmartCard,IsEnableSmartCard);
                }
                return dtCardData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable ReadSmartCardData(int IsPhoneReadFromSmartCard, bool IsEnableSmartCard)
        {
            DataTable dtSmartCard = null;
            try
            {
                dtSmartCard = this.CreateSmartCardTable();
                dtSmartCard.Rows.Add(dtSmartCard.NewRow());

                KOBSC.SDK.SmartCardManagerLegacy objSmartCard = new KOBSC.SDK.SmartCardManagerLegacy();
                bool status = objSmartCard.InitateSmartCardManagerLibrary();
                bool samStatus = false;
                bool cardStatus = false;

                if (status == true)
                {
                    status = objSmartCard.ReadDataFromCard(ref samStatus, ref cardStatus);
                    if (status == true)
                    {
                        if (objSmartCard.DataFromSmartCard.CPR0104_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["ID_NUMBER"] = objSmartCard.DataFromSmartCard.CPR0104_ElementData.ToString();    //CPR No
                        }
                        if (objSmartCard.DataFromSmartCard.FullNameEn_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["FULL_NAME_EN"] = objSmartCard.DataFromSmartCard.FullNameEn_ElementData.ToString();    //English Full Name
                        }
                        if (objSmartCard.DataFromSmartCard.FullNameAr_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["FULL_NAME_AR"] = objSmartCard.DataFromSmartCard.FullNameAr_ElementData.ToString();    //Arabic Full Name
                        }
                        if (objSmartCard.DataFromSmartCard.CPR0117_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["GENDER"] = objSmartCard.DataFromSmartCard.CPR0117_ElementData.ToString();    //Gender
                        }
                        if (objSmartCard.DataFromSmartCard.CPR0118_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["DOB"] = objSmartCard.DataFromSmartCard.CPR0118_ElementData.ToString();    //DOB
                        }
                        if (objSmartCard.DataFromSmartCard.CPR0119_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["EXPIRY_DATE"] = objSmartCard.DataFromSmartCard.CPR0119_ElementData.ToString();    //Expiry Date
                        }
                        if (objSmartCard.DataFromSmartCard.CorrenpondenceAddress_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["CORRESPONDANCE_ADDR"] = objSmartCard.DataFromSmartCard.CorrenpondenceAddress_ElementData.ToString();    //Correspondence Address
                        }
                        if (objSmartCard.DataFromSmartCard.CPR0105_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["FIRST_NAME"] = objSmartCard.DataFromSmartCard.CPR0105_ElementData.ToString();    //First Name
                        }
                        if (objSmartCard.DataFromSmartCard.CPR0106_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["MIDDLE_NAME1"] = objSmartCard.DataFromSmartCard.CPR0106_ElementData.ToString();    //Middle Name1
                        }
                        if (objSmartCard.DataFromSmartCard.CPR0107_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["MIDDLE_NAME2"] = objSmartCard.DataFromSmartCard.CPR0107_ElementData.ToString();    //Middle Name2
                        }
                        if (objSmartCard.DataFromSmartCard.CPR0108_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["MIDDLE_NAME3"] = objSmartCard.DataFromSmartCard.CPR0108_ElementData.ToString();    //Middle Name3
                        }
                        if (objSmartCard.DataFromSmartCard.CPR0109_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["MIDDLE_NAME4"] = objSmartCard.DataFromSmartCard.CPR0109_ElementData.ToString();    //Middle Name4
                        }
                        if (objSmartCard.DataFromSmartCard.CPR0110_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["LAST_NAME"] = objSmartCard.DataFromSmartCard.CPR0110_ElementData.ToString();    //Last Name
                        }
                        if (objSmartCard.DataFromSmartCard.CPR0308_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["FLAT_NUMBER"] = objSmartCard.DataFromSmartCard.CPR0308_ElementData.ToString();    //Flat No
                        }
                        if (objSmartCard.DataFromSmartCard.CPR0309_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["BUILDING_NUMBER"] = objSmartCard.DataFromSmartCard.CPR0309_ElementData.ToString();    //Building No
                        }
                        if (objSmartCard.DataFromSmartCard.CPR0310_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["BUILDING_NAME"] = objSmartCard.DataFromSmartCard.CPR0310_ElementData.ToString();    //Building Name
                        }
                        if (objSmartCard.DataFromSmartCard.CPR0312_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["ROAD_NUMBER"] = objSmartCard.DataFromSmartCard.CPR0312_ElementData.ToString();    //Road No
                        }
                        if (objSmartCard.DataFromSmartCard.CPR0313_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["ROAD_NAME"] = objSmartCard.DataFromSmartCard.CPR0313_ElementData.ToString();    //Road Name
                        }
                        if (objSmartCard.DataFromSmartCard.CPR0315_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["BLOCK_NUMBER"] = objSmartCard.DataFromSmartCard.CPR0315_ElementData.ToString();    //Block No
                        }
                        if (objSmartCard.DataFromSmartCard.CPR0316_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["GOVERNORATE_NAME"] = objSmartCard.DataFromSmartCard.CPR0316_ElementData.ToString();    //Governorate Name
                        }
                        if (objSmartCard.DataFromSmartCard.CPR0318_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["BLOCK_NAME"] = objSmartCard.DataFromSmartCard.CPR0316_ElementData.ToString();    //Governorate Name
                            //in smart card sw governorate name is shown as block name
                            //  dtSmartCard.Rows[0]["BLOCK_NAME"] = objSmartCard.DataFromSmartCard.CPR0318_ElementData.ToString();    //Block Name
                        }
                        //if (objSmartCard.DataFromSmartCard.CPR0318_ElementData != null)
                        //{
                        //    dtSmartCard.Rows[0]["BLOCK_NAME"] = objSmartCard.DataFromSmartCard.CPR0318_ElementData.ToString();    //Block Name
                        //}
                        if (objSmartCard.DataFromSmartCard.CPR0303_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["EMAIL"] = objSmartCard.DataFromSmartCard.CPR0303_ElementData.ToString();    //Email Address
                        }
                        //if (objSmartCard.DataFromSmartCard.CPR0304_ElementData != null && IsPhoneReadFromSmartCard == 1)
                        //{
                        //    dtSmartCard.Rows[0]["MOBILE_PH_NO"] = objSmartCard.DataFromSmartCard.CPR0304_ElementData.ToString();    //Contact no
                        //}
                        //if (objSmartCard.DataFromSmartCard.CPR0305_ElementData != null)
                        //{
                        //    dtSmartCard.Rows[0]["RESIDENCE_PH_NO"] = objSmartCard.DataFromSmartCard.CPR0305_ElementData.ToString();    //Residence no
                        //}
                        if (IsPhoneReadFromSmartCard == 1)
                        {
                            if (objSmartCard.DataFromSmartCard.CPR0304_ElementData != null)
                            {
                                dtSmartCard.Rows[0]["MOBILE_PH_NO"] = objSmartCard.DataFromSmartCard.CPR0304_ElementData.ToString();    //Contact no
                            }
                            if (objSmartCard.DataFromSmartCard.CPR0305_ElementData != null)
                            {
                                dtSmartCard.Rows[0]["RESIDENCE_PH_NO"] = objSmartCard.DataFromSmartCard.CPR0305_ElementData.ToString();    //Residence no
                            }
                        }
                        if (objSmartCard.DataFromSmartCard.CPR0306_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["FAX_NO"] = objSmartCard.DataFromSmartCard.CPR0306_ElementData.ToString();    //Residence no
                        }
                        if (objSmartCard.DataFromSmartCard.CPR0307_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["PO_BOX"] = objSmartCard.DataFromSmartCard.CPR0307_ElementData.ToString();    //P.O.Box No
                        }
                        if (objSmartCard.DataFromSmartCard.CPR1203_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["OCCUPATION"] = objSmartCard.DataFromSmartCard.CPR1203_ElementData.ToString();    //Occupation
                        }
                        if (objSmartCard.DataFromSmartCard.GDNPR0203_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["PASSPORT_NO"] = objSmartCard.DataFromSmartCard.GDNPR0203_ElementData.ToString();    //Nationality
                        }
                        if (objSmartCard.DataFromSmartCard.CPR0203_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["PHOTO"] = objSmartCard.DataFromSmartCard.CPR0203_ElementData;
                            string imagePath = Directory.GetCurrentDirectory() + "\\PicFromCard.jpeg";
                            if (objSmartCard.DataFromSmartCard.SavePhotoImageToFile(imagePath))
                            {
                                dtSmartCard.Rows[0]["PHOTO_PATH"] = imagePath;
                            }
                        }
                        if (objSmartCard.DataFromSmartCard.GDNPR0103_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["NATIONALITY"] = objSmartCard.DataFromSmartCard.GDNPR0103_ElementData.ToString();    //Nationality
                        }
                        if (objSmartCard.DataFromSmartCard.GDNPR0106_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["COUNTRY"] = objSmartCard.DataFromSmartCard.GDNPR0106_ElementData;
                        }
                        if (objSmartCard.DataFromSmartCard.GDNPR0104_ElementData != null)
                        {
                            dtSmartCard.Rows[0]["PLACEOFBIRTH"] = objSmartCard.DataFromSmartCard.GDNPR0104_ElementData;
                        }
                    }
                    else
                    {
                        giMessageBox.Show(Infologics.Medilogics.CommonClient.Controls.StaticData.CommonData.MESSAGEHEADER, "Smart Card read error", MessageBoxButtonType.OK, MessageBoxImages.Information);
                        IsEnableSmartCard = true;
                    }
                }
                else
                {
                    giMessageBox.Show(Infologics.Medilogics.CommonClient.Controls.StaticData.CommonData.MESSAGEHEADER, "Error reading Smart Card. Please re-insert and try again", MessageBoxButtonType.OK, MessageBoxImages.Information);
                    IsEnableSmartCard = true;
                }
                return dtSmartCard;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Creates the smart card table.
        /// </summary>
        /// <returns></returns>
        private DataTable CreateSmartCardTable()
        {
            try
            {
                DataTable SMART_CARD = new DataTable("SMART_CARD");
                SMART_CARD.Columns.Add("FULL_NAME_EN", typeof(string));
                SMART_CARD.Columns.Add("FULL_NAME_AR", typeof(string));
                SMART_CARD.Columns.Add("GENDER", typeof(string));
                SMART_CARD.Columns.Add("DOB", typeof(string));
                SMART_CARD.Columns.Add("EXPIRY_DATE", typeof(string));
                SMART_CARD.Columns.Add("CORRESPONDANCE_ADDR", typeof(string));
                SMART_CARD.Columns.Add("FIRST_NAME", typeof(string));
                SMART_CARD.Columns.Add("MIDDLE_NAME1", typeof(string));
                SMART_CARD.Columns.Add("MIDDLE_NAME2", typeof(string));
                SMART_CARD.Columns.Add("MIDDLE_NAME3", typeof(string));
                SMART_CARD.Columns.Add("MIDDLE_NAME4", typeof(string));
                SMART_CARD.Columns.Add("LAST_NAME", typeof(string));
                SMART_CARD.Columns.Add("FLAT_NUMBER", typeof(string));
                SMART_CARD.Columns.Add("BUILDING_NUMBER", typeof(string));
                SMART_CARD.Columns.Add("BUILDING_NAME", typeof(string));
                SMART_CARD.Columns.Add("ROAD_NUMBER", typeof(string));
                SMART_CARD.Columns.Add("ROAD_NAME", typeof(string));
                SMART_CARD.Columns.Add("BLOCK_NUMBER", typeof(string));
                SMART_CARD.Columns.Add("BLOCK_NAME", typeof(string));
                SMART_CARD.Columns.Add("GOVERNORATE_NAME", typeof(string));
                SMART_CARD.Columns.Add("EMAIL", typeof(string));
                SMART_CARD.Columns.Add("RESIDENCE_PH_NO", typeof(string));
                SMART_CARD.Columns.Add("FAX_NO", typeof(string));
                SMART_CARD.Columns.Add("PO_BOX", typeof(string));
                SMART_CARD.Columns.Add("OCCUPATION", typeof(string));
                SMART_CARD.Columns.Add("PASSPORT_NO", typeof(string));
                SMART_CARD.Columns.Add("PHOTO", typeof(byte[]));
                SMART_CARD.Columns.Add("PHOTO_PATH", typeof(string));
                SMART_CARD.Columns.Add("COUNTRY_CODE", typeof(string));
                SMART_CARD.Columns.Add("NATIONALITY", typeof(string));
                SMART_CARD.Columns.Add("PLACEOFBIRTH", typeof(string));
                SMART_CARD.Columns.Add("COUNTRY", typeof(string));

                SMART_CARD.Columns.Add("ID_NUMBER", typeof(string));
                SMART_CARD.Columns.Add("AREA_CODE", typeof(string));
                SMART_CARD.Columns.Add("AREA_DESCRIPTION", typeof(string));
                SMART_CARD.Columns.Add("CITY_CODE", typeof(string));
                SMART_CARD.Columns.Add("CITY_DESCRIPTION", typeof(string));
                SMART_CARD.Columns.Add("EMIRATE_CODE", typeof(string));
                SMART_CARD.Columns.Add("LOCATION_CODE", typeof(string));
                SMART_CARD.Columns.Add("MOBILE_PH_NO", typeof(string));
                SMART_CARD.Columns.Add("STREET", typeof(string));

                return SMART_CARD;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
