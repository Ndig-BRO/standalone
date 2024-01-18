using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Utility.Geometry;

namespace ASAPData
{
    //=======================================================================
    /// Provide class for Variables
    public class VariableTable
    {

    }

    //=======================================================================
    public class VariableData
    {
        private Data.DBHelper m_dbHelper = Data.DBHelper.Instance;

        private DataSet m_dataSet = new DataSet();

        private Dictionary<String, String> m_variableList = new Dictionary<string, string>();

        //===================================================================
        // Return the Dictionary of all the variables in current project DB
        public Dictionary<string,string> ReturnAllVariableData()
        {
            
            try
            {
                DbDataAdapter da = null;

                String tableName = "Parameter";
                // from the parameter table there is a column "type" and the Variable is a type
                // save the name encode the value which can vary bool double float string

                m_dbHelper.CreateProjectSelectCommand("SELECT * FROM " + tableName + " WHERE Type ='Variable'", ref da);

                DataSet myDataSet = new DataSet();
                myDataSet.Clear();

                da.Fill(myDataSet, tableName);
                

            }
            catch(Exception e)
            {
                // errorlog
            }
            return m_variableLists;
        }
    }

    //=======================================================================
    /// Provides class for wavelength and status pairs
    public class SourceWavelengthPair
    {        
        private double m_wavelength;

        public double Wavelength
        {
            get { return m_wavelength; }
            set { m_wavelength = value; }
        }

        //===================================================================
        /// Wavelength status
        private bool m_status;

        public bool Status
        {
            get { return m_status; }
            set { m_status = value; }
        }
    }

    //=======================================================================
    public class SourceWavelengthData
    {
        private Data.DBHelper m_dbHelper = Data.DBHelper.Instance;
        
        private DataSet m_dataSet = new DataSet();

        //===================================================================
        /// Returns the list of all wavelengths and in current project DB
        public List<SourceWavelengthPair> ReturnAllWavelengthData()
        {
            List<SourceWavelengthPair> wvPairList = new List<SourceWavelengthPair>();

            try
            {
                DbDataAdapter da = null;
         
                String tableName = "Wavelengths";
         
                m_dbHelper.CreateProjectSelectCommand( "SELECT * FROM " + tableName, ref da );

                //get the data
                m_dataSet.Clear();
         
                da.Fill( m_dataSet, tableName );
         
                DataRowCollection drcWavelength = m_dataSet.Tables[tableName].Rows;

                foreach ( DataRow drWavelength in drcWavelength )
                {

                    if ( drWavelength["Enabled"] != DBNull.Value )
                    {
                        SourceWavelengthPair wvPair = new SourceWavelengthPair();

                        wvPair.Wavelength = Convert.ToDouble( drWavelength["Wavelength"], StringUtils.CultureFormat );
                        
                        wvPair.Status = Convert.ToBoolean( drWavelength["Enabled"] );
                        
                        wvPairList.Add( wvPair );
                    }
                }
            }
            catch ( Exception e )
            {
                /*Utility.Diagnostics.ErrorLogger errLog = Utility.Diagnostics.ErrorLogger.Instance;

                errLog.LogException( e );*/
            }
            return wvPairList;
        }
    }
}
