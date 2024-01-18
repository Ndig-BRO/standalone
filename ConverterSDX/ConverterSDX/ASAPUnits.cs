using System;
using System.Globalization;
using System.Windows.Forms;

namespace Utility.Geometry
{
    //=======================================================================
    /// Unit types

    public enum ASAPUnits
    {
        Angstroms,    
        Nanometers,
        Microns,
        Millimeters,
        Centimeters,
        Meters,
        Feet,
        Inches,
        MicroInches
    }

    //=======================================================================
    /// This class is a convenience class that holds utilities for processing strings.
    public partial class StringUtils
    {
        //===================================================================
        /// Project Info Structure
        public struct ProjectInfo
        {
            public static decimal DBSchemaVersion;

            public static string Comments;
        }

        //===================================================================
        /// Product info Sructure
        public struct ProductInfo
        {
            public static string APEXName;
        
            public static string ASAPName;
        
            public static decimal Version;
        
            public static string Comments;
        
            public static string APEXDisplayVersion;
        
            public static string ASAPDisplayVersion;
        
            public static string BuildNumber;
        
            public static decimal DBSchemaVersion;
        }

        //===================================================================
        private static CultureInfo m_APEX_Culture = CultureInfo.CreateSpecificCulture( "" );

        public static CultureInfo CultureFormat
        {
            get { return m_APEX_Culture; }
        }
    }

    //=======================================================================
    /// Utilities for displaying messages to user.
    public class MessageUtils
    {
        //===================================================================
        /// Display a simple string.
        public static DialogResult ShowMessage( String theMessage )
        {
            DialogResult ret = DialogResult.Cancel;

            using ( Form tmpForm = new Form() )  // use the "owner" arg to make sure message box is a topmost window
            {
                tmpForm.TopMost = true;

                ret = MessageBox.Show( theMessage, StringUtils.ProductInfo.ASAPName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
            }
            return ret;
        }

        //===================================================================
        /// Shows the message.
        public static DialogResult ShowMessage( String theMessage, MessageBoxIcon mbIcon )
        {
            DialogResult ret = DialogResult.Cancel;

            using ( Form tmpForm = new Form() )  // use the "owner" arg to make sure message box is a topmost window
            {
                tmpForm.TopMost = true;

                ret = MessageBox.Show( theMessage, StringUtils.ProductInfo.ASAPName, MessageBoxButtons.OK, mbIcon );
            }
            return ret;
        }


        //===================================================================
        /// Display a string and specified buttons
        public static DialogResult ShowMessage( String theMessage, MessageBoxButtons theButtons )
        {
            DialogResult ret = DialogResult.Cancel;

            MessageBoxIcon showIcon = MessageBoxIcon.Exclamation;

            if ( theButtons == MessageBoxButtons.YesNo || theButtons == MessageBoxButtons.YesNoCancel )
            {
                showIcon = MessageBoxIcon.Question;
            }

            using ( Form tmpForm = new Form() )  // use the "owner" arg to make sure message box is a topmost window
            {
                tmpForm.TopMost = true;

                ret = MessageBox.Show( tmpForm, theMessage, StringUtils.ProductInfo.ASAPName, theButtons, showIcon );
            }

            return ret;
        }
    }
}
