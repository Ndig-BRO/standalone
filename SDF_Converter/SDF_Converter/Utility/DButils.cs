using System;
using System.Data;
using System.Data.Common;
using System.IO;

namespace DatabaseUtility
{
    //=======================================================================
    /// Database functions
    public class DBfunctions
    {
        // Global
        private static DbConnection m_dbConnection;
        private static DbCommand m_command;


        //===================================================================
        /// Close the current project database
        public void CloseDatabase()
        {
            if (m_dbConnection.State == ConnectionState.Open)
            {
                m_dbConnection.Close();
            }
        }

        //===================================================================
        /// Open the current project database
        public void OpenDatabase(String projectFile)
        {
            m_dbConnection.ConnectionString = "data source=" + projectFile;
            m_dbConnection.Open();
        }

        //===================================================================
        /// Database orientation for getting the database preped for reading
        public bool PerformDBStartupTasks(Data.DBSyncProvider theDBSync)
        {
            bool ret = true;
            string ErrorMessage = "";

            try
            {
                // Checks to see that the master GlobalOpticalData and Project Template file exists, and is not readonly
                if (FilesCheck(ref ErrorMessage) == false)
                {
                    Utility.Diagnostics.ErrorLogger.Instance.LogMessage(ErrorMessage + ASAP.Properties.Resources.CannotStartASAP);
                    Utility.MessageUtils.ShowMessage(ErrorMessage);

                    return false;
                }

                // Check to see if the master GlobalOpticalData file needs to be upgraded
                string ConnectionString = "DataSource = " + Utility.Paths.ASAPPaths.AllUserDBFile + "\\GlobalOpticalData.sdf; " + "password = " + DBHelper.Instance.Password + ";";
                DBHelper.FileStatus status = Data.DBHelper.UpgradeDatabase(ref ErrorMessage, ConnectionString, Utility.Paths.ASAPPaths.AllUserDBFile + "\\GlobalOpticalData.sdf");

                if (status == DBHelper.FileStatus.Damaged || status == DBHelper.FileStatus.Missing)
                {
                    Utility.Diagnostics.ErrorLogger.Instance.LogMessage(ErrorMessage + ASAP.Properties.Resources.CannotStartASAP);
                    Utility.MessageUtils.ShowMessage(ErrorMessage + ASAP.Properties.Resources.CannotStartASAP);

                    return false;
                }

                //  Check for the existence of and readonly status of Global Optical file in user area.
                DBHelper.FileStatus fs = new DBHelper.FileStatus();
                fs = Data.DBHelper.UserDataFileCheck(ref ErrorMessage, Utility.Paths.ASAPPaths.UserDBPath_GlobalOpticalData, "GlobalOpticalData.sdf");

                if (fs == DBHelper.FileStatus.Missing)
                {
                    // Make sure BRO user local directory exists to put .sdf files into
                    if (Directory.Exists(Utility.Paths.ASAPPaths.UserDBFile) != true)
                    {
                        Directory.CreateDirectory(Utility.Paths.ASAPPaths.UserDBFile);
                    }

                    string PreviousInstallDir = "";

                    // Need to look for a previous version of the product.  
                    // If found this method returns the location to the GlobalOpticalData file                   
                    if (PreviousInstallVersion(ref PreviousInstallDir) == true && File.Exists(PreviousInstallDir + "\\GlobalOpticalData.sdf") == true)
                    {
                        // The file was missing and there is a previous version.  
                        // Attempt to copy from the past location to the new User Location
                        File.Copy(PreviousInstallDir + "\\GlobalOpticalData.sdf", ASAPPaths.UserDBPath_GlobalOpticalData, false);
                    }
                    else if (File.Exists(Utility.Paths.ASAPPaths.AllUserDBFile + "\\GlobalOpticalData.sdf") == true)
                    {
                        //  No previous installation and no user copy of file  Copy file from current install location to user area.   
                        File.Copy(Utility.Paths.ASAPPaths.AllUserDBFile + "\\GlobalOpticalData.sdf", Utility.Paths.ASAPPaths.UserDBPath_GlobalOpticalData, false);
                    }
                    else
                    {
                        Utility.Diagnostics.ErrorLogger.Instance.LogMessage(PreviousInstallDir + "\\GlobalOpticalData.sdf does not exist");
                        Utility.Diagnostics.ErrorLogger.Instance.LogMessage(Utility.Paths.ASAPPaths.AllUserDBFile + "\\GlobalOpticalData.sdf does not exist");
                        Utility.Diagnostics.ErrorLogger.Instance.LogMessage(ErrorMessage + ASAP.Properties.Resources.CannotStartASAP);
                        Utility.MessageUtils.ShowMessage(ErrorMessage + System.Environment.NewLine + ASAP.Properties.Resources.CannotStartASAP);

                        return false;
                    }
                }
                else if (fs == DBHelper.FileStatus.Read_Only)
                {
                    File.SetAttributes(ASAPPaths.UserDBPath_GlobalOpticalData, FileAttributes.Normal);
                }

                // Check to see if the global database file in the user area needs to be upgraded
                ConnectionString = "DataSource = " + Utility.Paths.ASAPPaths.UserDBPath_GlobalOpticalData + "; password = " + DBHelper.Instance.Password + ";";
                status = Data.DBHelper.UpgradeDatabase(ref ErrorMessage, ConnectionString, ASAPPaths.UserDBPath_GlobalOpticalData);

                if (status == DBHelper.FileStatus.Damaged || status == DBHelper.FileStatus.Missing)
                {
                    Utility.Diagnostics.ErrorLogger.Instance.LogMessage(ErrorMessage + ASAP.Properties.Resources.CannotStartASAP);
                    Utility.MessageUtils.ShowMessage(ErrorMessage + System.Environment.NewLine + ASAP.Properties.Resources.CannotStartASAP);

                    return false;
                }

                theDBSync.ShowProgress = true;

                //  Always check to see if the user file is up to date with master by checking schema version.
                theDBSync.SynchronizeGlobalDB();
                fs = Data.DBHelper.UserDataFileCheck(ref ErrorMessage, ASAPPaths.UserDBPath_ASAPProjectOpticalData, "ASAPProjectOpticalData.sdf");

                if (fs == DBHelper.FileStatus.Missing)
                {
                    // Make sure BRO user local directory exists to put .sdf files into
                    if (Directory.Exists(Utility.Paths.ASAPPaths.ASAPUserDataPath) != true)
                    {
                        Directory.CreateDirectory(Utility.Paths.ASAPPaths.ASAPUserDataPath);
                    }

                    string PreviousInstallDir = "";

                    //  Need to look for a previous version of the product.  If found this method returns the location to the GlobalOpticalData file                   
                    if (PreviousInstallVersion(ref PreviousInstallDir) == true && File.Exists(PreviousInstallDir + "\\ASAPProjectOpticalData.sdf") == true)
                    {
                        //  The file was missing and there is a previous version.  Attempt to copy from the past location to the new User Location
                        File.Copy(PreviousInstallDir + "\\ASAPProjectOpticalData.sdf", ASAPPaths.UserDBPath_ASAPProjectOpticalData, false);
                    }
                    else if (File.Exists(Utility.Paths.ASAPPaths.AllUserDBFile + "\\ASAPProjectOpticalData.sdf") == true)
                    {
                        //  No previous installation and no user copy of file  Copy file from current install location to user area. 
                        File.Copy(Utility.Paths.ASAPPaths.AllUserDBFile + "\\ASAPProjectOpticalData.sdf", ASAPPaths.UserDBPath_ASAPProjectOpticalData, false);
                    }
                    else
                    {
                        Utility.Diagnostics.ErrorLogger.Instance.LogMessage(PreviousInstallDir + "\\ASAPProjectOpticalData.sdf does not exist");
                        Utility.Diagnostics.ErrorLogger.Instance.LogMessage(Utility.Paths.ASAPPaths.ASAPUserDataPath + "\\ASAPProjectOpticalData.sdf does not exist");
                        Utility.Diagnostics.ErrorLogger.Instance.LogMessage(ErrorMessage + ASAP.Properties.Resources.CannotStartASAP);

                        return false;
                    }
                }
                else if (fs == DBHelper.FileStatus.Read_Only)
                {
                    File.SetAttributes(ASAPPaths.UserDBPath_ASAPProjectOpticalData, FileAttributes.Normal);
                }

                // Check to see if the master project database file needs to be upgraded
                ConnectionString = "DataSource = " + Utility.Paths.ASAPPaths.AllUserDBFile + "\\ASAPProjectOpticalData.sdf";

                status = Data.DBHelper.UpgradeDatabase(ref ErrorMessage, ConnectionString, Utility.Paths.ASAPPaths.AllUserDBFile + "\\ASAPProjectOpticalData.sdf");

                if (status == DBHelper.FileStatus.Damaged || status == DBHelper.FileStatus.Missing)
                {
                    Utility.Diagnostics.ErrorLogger.Instance.LogMessage(ErrorMessage + ASAP.Properties.Resources.CannotStartASAP);

                    return false;
                }

                // Check to see if the project database file in the user area needs to be upgraded
                ConnectionString = "DataSource = " + ASAPPaths.UserDBPath_ASAPProjectOpticalData;

                status = Data.DBHelper.UpgradeDatabase(ref ErrorMessage, ConnectionString, ASAPPaths.UserDBPath_ASAPProjectOpticalData);

                if (status == DBHelper.FileStatus.Damaged || status == DBHelper.FileStatus.Missing)
                {
                    Utility.Diagnostics.ErrorLogger.Instance.LogMessage(ErrorMessage + ASAP.Properties.Resources.CannotStartASAP);

                    return false;
                }

                // Synchronize the project templates
                theDBSync.SynchronizeProjectDB("datasource = " + Utility.Paths.ASAPPaths.ASAPUserDataPath + "\\ASAPProjectOpticalData.sdf", "DataSource = " + Utility.Paths.ASAPPaths.UserDBPath_ProjectSettings);
                theDBSync.ShowProgress = false;

                //  After completing sync, set file to use for the global to the users area
                DBHelper.Instance.DbConnection.ConnectionString = "DataSource = " + Utility.Paths.ASAPPaths.UserDBPath_GlobalOpticalData + "; password = " + DBHelper.Instance.Password + ";";
            }
            catch (Exception excep)
            {
                ret = false;
                Utility.Diagnostics.ErrorLogger errLog = Utility.Diagnostics.ErrorLogger.Instance;
                errLog.LogException(excep);
                errLog.LogMessage("Problem setting up ASAP database.");

                if (ErrorMessage != "")
                {
                    errLog.LogMessage("Last error message = " + ErrorMessage);
                }

                Utility.MessageUtils.ShowMessage("A problem has occurred initializing ASAP project database files.\r\n Cannot start ASAP application.");
            }

            return ret;
        }


        //===================================================================
        /// <summary>
        /// Finds "legacy" directories created by installer of previously installed versions of ASAP.
        /// </summary>
        /// <param name="InstallDir">The install dir.</param>
        /// <returns></returns>
        private bool PreviousInstallVersion(ref string InstallDir)
        {
            //  With each new release after 2014, the previous version should be added here (specific user area). 
            //  It should be added to the beginning of the list.
            //  The current version should not be listed here.
            string[] saVersions = { @"\Breault Research Organization\ASAP 2014 V1" };
            string LocalAppData = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);

            //  Versions keys are checked from most recent to oldest.
            for (int i = 0; i < saVersions.Length; i++)
            {
                if (Directory.Exists(LocalAppData + saVersions[i].ToString()))
                {
                    InstallDir = LocalAppData + saVersions[i].ToString();

                    return true;
                }
            }

            // Pre ASAP 2015 Location check
            string LocalCommonData = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
            LocalCommonData += @"\Breault Research Organization\ASAP 2014 V1";

            if (Directory.Exists(LocalCommonData))
            {
                InstallDir = LocalCommonData;

                return true;
            }

            return false;
        }


        //===================================================================
        /// <summary>
        /// Checks for the presence of and read only status of the project db file and the global dB file.
        /// </summary>
        /// <returns>If both files are available and not readonly true is returned.</returns>
        private bool FilesCheck(ref string ErrorMessage)
        {
            bool Status = true;

            //  This is the master Global Optical File typically in C:\ProgramData\Breault Research Orgranization
            string filePath = Utility.Paths.ASAPPaths.AllUserDBFile + "\\GlobalOpticalData.sdf";

            if (System.IO.File.Exists(filePath) == false)
            {
                ErrorMessage += "The ASAP global optical master database file is missing." + System.Environment.NewLine;
                ErrorMessage += "  (" + filePath + ")" + System.Environment.NewLine;
                Status = false;
            }
            else
            {
                FileInfo file = new FileInfo(filePath);

                if ((file.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    ErrorMessage += "The ASAP global optical master database file is read-only." + System.Environment.NewLine;
                    ErrorMessage += "  (" + filePath + ")" + System.Environment.NewLine;
                    Status = false;
                }
            }

            return Status;
        }
    }
}
