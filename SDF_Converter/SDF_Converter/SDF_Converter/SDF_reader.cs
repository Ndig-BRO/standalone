using System;
using System.Data;
using System.Data.Common;
using System.IO;


namespace SDF_Converter
{
    class SDF_reader
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
            m_dbConnection.ConnectionString = "data source=" + projectFile ;
            m_dbConnection.Open();
        }

        //===================================================================
        /// Extract information from the SDF
        public void ExtractSDF(String projectFile)
        {
            OpenDatabase(projectFile);

            // Read each table


            // Select from the list

            // Info saved in each cell


            CloseDatabase();

        }
    }
}
