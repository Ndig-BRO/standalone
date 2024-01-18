using System;
using System.Data;

namespace SDF_Converter
{
    class XML_writer
    {
        // Global

        //===================================================================
        /// Close the created SDX file
        public void CloseSDX(String projectFile)
        {
            if (m_SDXfile.State == ConnectionState.Open)
            {
                m_SDXfile.Close();
            }
        }

        //===================================================================
        /// Create the SDX file
        public void CreateSDX(String projectFile)
        {
            if (m_SDXfile.State == ConnectionState.Open)
            {
                m_SDXfile.Close();
            }
        }

        //===================================================================
        /// Write the info into an SDX
        public void WriteSDX(String projectFile)
        {
            CreateSDX(projectFile);

            //Create the nodes

            // write the info into each node

            CloseSDX(projectFile);
        }

    }
}
