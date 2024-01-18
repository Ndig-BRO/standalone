using ConverterSDX;
using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using Utility.Geometry;

namespace Data
{

    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase
    {

        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("System.Data.SqlServerCe.3.5")]
        public string DBProvider
        {
            get
            {
                return ((string)(this["DBProvider"]));
            }
        }
    }

    //=======================================================================
    /// Class for gathering info from Global Database for product info (i.e. Product Name, version etc.)
    public class ProductInfo
    {
        private DataSet dsData = new DataSet();

        DBHelper m_dbHelper;

        //===================================================================
        public ProductInfo()
        {
            m_dbHelper = DBHelper.Instance;
        }

        //===================================================================
        /// Gets the product info from the global database.
        public bool GetInfo( StringUtils.ProductInfo pi )
        {
            bool ret = false;

            try
            {
                m_dbHelper.DbConnection.Open();

                DbDataAdapter da = null;

                string theSelectCmd = "SELECT * FROM ProductInfo";

                m_dbHelper.CreateGlobalSelectCommand( theSelectCmd, ref da );

                dsData.Clear();

                if ( da.Fill( dsData, "ProductInfo" ) > 0 )
                {
                    foreach ( DataRow drProductInfo in dsData.Tables[0].Rows )
                    {
                        string tmp = (string)drProductInfo["Name"].ToString();

                        switch ( tmp )
                        {
                            case "ASAPName":
                
                                StringUtils.ProductInfo.ASAPName = drProductInfo["Value"].ToString();

                                Debug.WriteLine( "ProductInfo.Name = " + StringUtils.ProductInfo.ASAPName );
                
                            break;
                
                            case "Version":

                                StringUtils.ProductInfo.Version = Convert.ToDecimal( drProductInfo["Value"], StringUtils.CultureFormat );

                                Debug.WriteLine( "ProductInfo.Version = " + StringUtils.ProductInfo.Version );

                            break;

                            case "ASAPDisplayVersion":

                                StringUtils.ProductInfo.ASAPDisplayVersion = drProductInfo["Value"].ToString();

                                Debug.WriteLine( "ProductInfo.DisplayVersion = " + StringUtils.ProductInfo.ASAPDisplayVersion );

                            break;

                            case "Comments":

                                StringUtils.ProductInfo.Comments = drProductInfo["Value"].ToString();

                                Debug.WriteLine( "ProductInfo.Comments = " + StringUtils.ProductInfo.Comments );

                            break;

                            case "BuildNumber":

                                StringUtils.ProductInfo.BuildNumber = drProductInfo["Value"].ToString();

                                Debug.WriteLine( "ProductInfo.BuildNumber = " + StringUtils.ProductInfo.BuildNumber );

                            break;

                            case "DBSchemaVersion":

                                StringUtils.ProductInfo.DBSchemaVersion = Convert.ToDecimal( drProductInfo["Value"], StringUtils.CultureFormat );

                                Debug.WriteLine( "ProductInfo.DBSchemaVersion = " + StringUtils.ProductInfo.DBSchemaVersion );

                            break;

                            default:
                            break;
                        }
                    }
                    ret = true;
                }
            }
            catch ( Exception e )
            {
                /*Utility.Diagnostics.ErrorLogger errLog = Utility.Diagnostics.ErrorLogger.Instance;
                
                errLog.LogMessage( "Problem reading Product Info.  ProductInfo.GetInfo()" );
                
                errLog.LogException( e );*/
                
                // APEX add-in won't load due to this problem
                ret = false;
            }
            finally
            {
                m_dbHelper.DbConnection.Close();
            }
            return ret;
        }
    }
    
    //=======================================================================
    /// Data class to hold all infor realted to the current project.
    public class ProjectInfo
    {
        private DataSet dsData = new DataSet();

        DBHelper m_dbHelper;

        //===================================================================
        /// Initializes a new instance of the <see cref="ProjectInfo"/> class.
        public ProjectInfo()
        {
            m_dbHelper = DBHelper.Instance;
        }

        //===================================================================
        /// Adds the DB schema.
        public bool AddDBSchema( string SchemaVersion )
        {
            bool ret = false;

            try
            {
                // open connection to DB
                m_dbHelper.ProjectDbConnection.Open();

                //create adapter 
                DbDataAdapter da = null;
            
                string theSelectCmd = "SELECT * FROM ProjectInfo";

                m_dbHelper.CreateProjectSelectCommand( theSelectCmd, ref da );

                dsData.Clear();

                da.Fill( dsData, "ProjectInfo" );

                DbCommandBuilder cbVersion = m_dbHelper.DbFactory.CreateCommandBuilder();
                
                cbVersion.DataAdapter = da;

                //  Add the row
                DataRow drDBSchemaVersion = dsData.Tables["ProjectInfo"].NewRow();
                
                drDBSchemaVersion["ID"] = System.Guid.NewGuid().ToString();
                drDBSchemaVersion["Name"] = "DBSchemaVersion";
                drDBSchemaVersion["Value"] = SchemaVersion;

                dsData.Tables["ProjectInfo"].Rows.Add( drDBSchemaVersion );

                da.Update( dsData, "ProjectInfo" );
                
                ret = true;
            }
            catch ( Exception e )
            {
               // Utility.Diagnostics.ErrorLogger.Instance.LogException( e );
            }
            // close connection to DB
            m_dbHelper.ProjectDbConnection.Close();

            return ret;
        }
    }

    //=======================================================================
    public class DBHelper
    {
        //===================================================================
        private static DbConnection m_dbConnection;

        private static DbConnection m_dbProjConnection;

        private static DbProviderFactory m_dbFactory;

        private static DBHelper m_dbHelperInstance = null;

        private String m_dbCurrentProjectFile = null;

        private String m_ErrorString;

        private string m_Password = "Br3ault12345";

        //===================================================================
        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; }
        }

        //===================================================================
        private DBHelper()
        {
            try
            {
                m_ErrorString = null;

                String dbProvider = null;

                Settings sSettings = Settings.Default;

                try
                {
                    dbProvider = sSettings.DBProvider;
                }
                catch ( Exception ex )
                {
                   /* Utility.Diagnostics.ErrorLogger.Instance.LogException( ex );
                    Utility.Diagnostics.ErrorLogger.Instance.LogMessage( "In DBHelper constructor: Data.Properties.Settings.DBProvider not set: machine.config may be missing SQL provider information." );
                */
                    return;
                }
                try
                {
                    m_dbFactory = DbProviderFactories.GetFactory( dbProvider );
                }
                catch ( Exception ex )
                {/*
                    m_ErrorString = ex.Message + " " + dbProvider;
                    Utility.Diagnostics.ErrorLogger.Instance.LogException( ex );
                    */
                    return;
                }
                
                //create connection as set the connection String
                m_dbConnection = m_dbFactory.CreateConnection();

                ProjectSDF projectSDF = new ProjectSDF();

                m_dbConnection.ConnectionString = "data source=" + projectSDF.DB_path + ";password=" + Password;

                // create and set the project connection String
                m_dbProjConnection = m_dbFactory.CreateConnection();

                m_dbProjConnection.ConnectionString = "data source=" + projectSDF.DB_path;
            }
            catch ( Exception e )
            {
                //Utility.Diagnostics.ErrorLogger.Instance.LogException( e );
            }
        }

        //===================================================================
        /// create single instance of the class
        public static DBHelper Instance
        {
            get
            {
                if ( m_dbHelperInstance == null )
                {
                    m_dbHelperInstance = new DBHelper();
                }
                return m_dbHelperInstance;
            }
            set
            {
                m_dbHelperInstance = null;
            }
        }

        //===================================================================
        /// Create a select command for the global database using the command text and add it to a db adapter.
        public void CreateGlobalSelectCommand( String theCommandText, ref DbDataAdapter adapter )
        {
            adapter = m_dbFactory.CreateDataAdapter();

            DbCommand dc = m_dbFactory.CreateCommand();

            dc.CommandText = theCommandText;
         
            dc.Connection = m_dbConnection;

            adapter.SelectCommand = dc;
        }

        //===================================================================
        /// Create a select command for the project database using the command text
        public void CreateProjectSelectCommand( String theCommandText, ref DbDataAdapter adapter )
        {
            adapter = m_dbFactory.CreateDataAdapter();

            DbCommand dc = m_dbFactory.CreateCommand();

            dc.CommandText = theCommandText;

            dc.Connection = m_dbProjConnection;

            adapter.SelectCommand = dc;
        }

        //===================================================================
        /// Get db connection
        public DbConnection DbConnection
        {
            get { return m_dbConnection; }
        }

        //===================================================================
        /// Get project db connection
        public DbConnection ProjectDbConnection
        {
            get { return m_dbProjConnection; }

            set { m_dbProjConnection = value; }
        }

        //===================================================================
        /// Get the database factory
        public DbProviderFactory DbFactory
        {
            get { return m_dbFactory; }
        }

        //===================================================================
        /// Set the name of the project database
        public void SetProjectDatabase( String projectFile )
        {
            m_dbCurrentProjectFile = projectFile;

            String projectFilePath = "data source=" + projectFile;

            m_dbProjConnection.ConnectionString = projectFilePath;
        }

        //===================================================================
        /// Close the current project database
        public void CloseProjectDatabase( String projectFile )
        {
            if ( m_dbProjConnection.State == ConnectionState.Open )
            {
                m_dbProjConnection.Close();
            }
        }
        /*
        //===================================================================
        /// Upgrades SQL Server CE SDF database file
        public static FileStatus UpgradeDatabase( ref String Message, String SourceConnectionString, String theDatabaseFile )
        {
            String majVersion = null;

            String minVersion = null;

            String theDatabaseLogFile = "";
            
            FileStatus status = FileStatus.Ready;

            try
            {
                // Get the current DB version
                System.Data.SqlServerCe.SqlCeEngine engine = new System.Data.SqlServerCe.SqlCeEngine( SourceConnectionString );

                // This method will dump version information to a text file in your current working directory
                if ( engine.Verify( System.Data.SqlServerCe.VerifyOption.Default ) )
                {
                    theDatabaseLogFile = theDatabaseFile.Replace( ".sdf", ".log" );
                
                    // Read version information from text file
                    if ( File.Exists( theDatabaseLogFile ) )
                    {
                        using ( StreamReader sr = new StreamReader( theDatabaseLogFile ) )
                        {
                            String line;

                            try
                            {
                                // Read lines from the file 
                                while ( ( line = sr.ReadLine() ) != null )
                                {
                                    // split line, using space as delimeter
                                    String[] tokens = line.Split( new char[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries );

                                    // There should be a line containing the major and minor version information
                                    if ( tokens.Length > 0 )
                                    {
                                        if ( tokens[0].Contains( "verMajor:" ) )
                                        {
                                            majVersion = tokens[0].Substring( tokens[0].LastIndexOf( ":" ) + 1 );

                                            minVersion = tokens[1].Substring( tokens[1].LastIndexOf( ":" ) + 1 );

                                            break;
                                        }
                                    }
                                }
                            }
                            catch ( Exception e )
                            {
                                //Utility.Diagnostics.ErrorLogger errLog = Utility.Diagnostics.ErrorLogger.Instance;
                                //errLog.LogException( e );

                                Message += "The file " + theDatabaseLogFile + " is missing or is invalid.\r\n";

                                return FileStatus.Missing;
                            }
                        }
                    }
                    else
                    {
                        Message += "The file " + theDatabaseFile + " may be damaged.\r\n";

                        return FileStatus.Damaged;
                    }
                    // Upgrade if version earlier than SQL Server CE 3.5.  
                    if ( !String.IsNullOrEmpty( minVersion ) && !String.IsNullOrEmpty( majVersion ) )
                    {
                        int majorVersion = 0;

                        int minorVersion = 0;

                        majorVersion = Convert.ToInt32( majVersion );

                        minorVersion = Convert.ToInt32( minVersion );

                        if ( majorVersion <= 3 && minorVersion < 5 )
                        {
                            // Upgrade database using SQLEe Engine
                            engine.Upgrade();

                            status = FileStatus.Upgraded;
                        }
                        // Delete the database log file 
                        File.Delete( theDatabaseLogFile );
                    }
                    else
                    {
                        Message += "The file " + theDatabaseFile + " may be damaged.\r\n";

                        return FileStatus.Damaged;
                    }
                }
            }
            catch ( Exception e )
            {
                //Utility.Diagnostics.ErrorLogger errLog = Utility.Diagnostics.ErrorLogger.Instance;
                //errLog.LogException( e );

                Message += "The file " + theDatabaseFile + " may be damaged.\r\n";

                return FileStatus.Damaged;
            }
            return status;
        }
        */
        //===================================================================
        /// Gets the bool value.
        public static bool GetBoolValue( DataRow drRow, string Field )
        {
            if ( drRow[Field] == DBNull.Value )
            {
                return false;
            }
            else if ( (bool)drRow[Field] == true )
            {
                return true;
            }
            else if ( (bool)drRow[Field] == false )
            {
                return false;
            }
            return false;
        }

        public static ASAPUnits GetASAPUnit( DataRow drRow, string Field )
        {
            ASAPUnits au = new ASAPUnits();
            
            if ( drRow[Field] == DBNull.Value )
            {
                return au;
            }

            return (ASAPUnits)drRow[Field];
        }

        //===================================================================
        /// Check for the existence and the readonly status of the users global app data file.
        public static FileStatus UserDataFileCheck( ref string Message, string PathAndFileName, string fileName )
        {
            if ( File.Exists( PathAndFileName ) == false )
            {
                Message += "The file " + PathAndFileName + " is missing.\r\n";

                return FileStatus.Missing;
            }
            else
            {
                ProjectSDF projectSDF = new ProjectSDF();

                FileInfo file = new FileInfo( projectSDF.DB_path + "\\" + fileName );

                if ( ( file.Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly )
                {
                    Message += "The file " + PathAndFileName + " is read-only.\r\n";

                    return FileStatus.Read_Only;
                }
            }
            return 0;
        }

        //===================================================================
        /// File Status Possibilities
        public enum FileStatus
        {
            Ready,
            Missing,
            Read_Only,
            Out_Of_Date,
            Damaged,
            Upgraded
        }

        //===================================================================
        /// Checks to see if specified name exist in given table
        private bool Exists( string Name, string table )
        {
            try
            {
                DbDataAdapter da = null;

                CreateGlobalSelectCommand( "SELECT * FROM " + table + " WHERE Name ='" + Name + "'", ref da );

                DataSet dsExistsData = new DataSet();

                if ( da.Fill( dsExistsData, table ) > 0 )
                {
                    return true;
                }
                return false;
            }
            catch ( Exception e )
            {
                //Utility.Diagnostics.ErrorLogger errLog = Utility.Diagnostics.ErrorLogger.Instance;
                //errLog.LogException( e );
                
                return false;
            }
        }

        //===================================================================
        /// Checks to see if specified name exist in given table
        public string FindValidName( string Name, string Table )
        {
            for ( int i = 0; i < 9999; i++ )
            {
                if ( Exists( Name + " - " + i.ToString(), Table ) == false )
                {
                    return Name + " - " + i.ToString();
                }
            }
            return "-1";
        }
    }
}
