using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Utility.Geometry;

namespace ASAPData
{
    //=======================================================================
    /// Public Class for data access to project settings.
    public partial class SystemSettingsData
    {
        enum BackingStore
        {
            DB,
            XML,
        }

        private DataSet dsData = new DataSet();

        //data base helper
        Data.DBHelper m_dbHelper;

        private String m_currentProjectDB;
        private BackingStore m_BackingStore = BackingStore.DB;
        private Dictionary<string, string> m_settings { get; set; }

        //===================================================================
        private String tblName;

        private DataTable TblData;

        //===================================================================
        private SystemSettings m_sysSettings = new SystemSettings();

        /// Gets or sets the proj settings.
        public SystemSettings SysSettings
        {
            get { return m_sysSettings; }

            set { m_sysSettings = value; }
        }

        //===================================================================
        /// Holder for an instance of a SystemSettings
        private static SystemSettings m_CurrentSettings = null;

        public SystemSettings CurrentSettings
        {
            get { return m_CurrentSettings; }
         
            set { m_CurrentSettings = value; }
        }

        //===================================================================
        public SystemSettingsData()
        {
            tblName = "ProjectSettings";

            m_dbHelper = Data.DBHelper.Instance;
            m_settings = new Dictionary<string, string>();
        }

        // Keep track of system setting in use for error tracking purposes
        String m_curSysSetting = "";


        //===================================================================
        /// Returns the value for the specified Project Setting
        public String GetSettingByName( String Name )
        {
            String ret = "";
            m_curSysSetting = Name;
            try
            {
                tblName = "ProjectSettings";

                //create adapter 
                DbDataAdapter da = null;

                m_dbHelper.CreateProjectSelectCommand("SELECT * FROM " + tblName + " WHERE Name = '" + Name + "'", ref da);

                dsData.Clear();

                if (da.Fill(dsData, tblName) > 0)
                {
                    ret = dsData.Tables["ProjectSettings"].Rows[0]["Value"].ToString();
                }
            }
            catch (Exception e)
            {
                //Utility.Diagnostics.ErrorLogger.Instance.LogException(e);
            }

            return ret;
        }

        //===================================================================
        public Dictionary<string, string> GetSettings()
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            try
            {
                tblName = "ProjectSettings";

                //create adapter 
                DbDataAdapter da = null;

                m_dbHelper.CreateProjectSelectCommand("SELECT * FROM " + tblName, ref da);
                dsData.Clear();

                if (da.Fill(dsData, tblName) > 0)
                {
                    foreach (DataRow row in dsData.Tables["ProjectSettings"].Rows)
                    {
                        ret[row["Name"].ToString()] = row["Value"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                //Utility.Diagnostics.ErrorLogger.Instance.LogException(e);
            }

            return ret;
        }

        //===================================================================
        /// Gets the DB settings.
        public bool GetDBSettings()
        {
            try
            {
                // Get settings for Accuracy
                m_sysSettings.RayTraceAccuracyProperties.Degrees = Convert.ToDouble( GetSettingByName( "AccuracyLevel-Degrees" ), StringUtils.CultureFormat );
                m_sysSettings.RayTraceAccuracyProperties.ParallelOn = Convert.ToBoolean( GetSettingByName( "AccuracyLevel-ParallelOn" ) );

                //  Get Ambient Media
                m_sysSettings.AmbientMediaProperties.AmbientMediaName = GetSettingByName( "AmbientMedia" );

                ////  Get General Settings
                m_sysSettings.GeneralSettingsProperties.ListWarnings = Convert.ToBoolean( GetSettingByName( "GeneralSettings-ListWarnings" ) );
                m_sysSettings.GeneralSettingsProperties.LocalObject = GetSettingByName( "GeneralSettings-LocalObject" );
                m_sysSettings.GeneralSettingsProperties.WarningsThreshold = Convert.ToInt32( GetSettingByName( "GeneralSettings-WarningsThreshold" ) );

                // Get MissedRayPlotting
                m_sysSettings.MissedRayPlottingProperties.Distance = Convert.ToDouble( GetSettingByName( "MissedRayPlotting-Distance" ), StringUtils.CultureFormat );

                //  Get RayArrows
                m_sysSettings.RayArrowsProperties.DisplayArrows = Convert.ToBoolean( GetSettingByName( "RayArrows-Display" ) );
                m_sysSettings.RayArrowsProperties.ScalingFactor = Convert.ToDouble( GetSettingByName( "RayArrows-ScalingFactor" ), StringUtils.CultureFormat );

                // Get Ray Splitting
                m_sysSettings.RaySplittingProperties.DiffuseRayFluxThreshold = Convert.ToDouble( GetSettingByName( "RaySplitting-DiffuseRayFluxThreshold" ), StringUtils.CultureFormat );
                m_sysSettings.RaySplittingProperties.EnergyCutoff = Convert.ToDouble( GetSettingByName( "RaySplitting-EnergyCutoff" ), StringUtils.CultureFormat );
                m_sysSettings.RaySplittingProperties.MaxSplits = Convert.ToInt32( GetSettingByName( "RaySplitting-MaxSplits" ) );
                m_sysSettings.RaySplittingProperties.RaySplittingOn = Convert.ToBoolean( GetSettingByName( "RaySplitting-RaySplittingOn" ) );
                m_sysSettings.RaySplittingProperties.ScatterLevels = Convert.ToInt32( GetSettingByName( "RaySplitting-ScatterLevels" ) );
                m_sysSettings.RaySplittingProperties.ScatterRaysAllChildrenOn = Convert.ToBoolean( GetSettingByName( "RaySplitting-ScatterRaysAllChildrenOn" ) );
                m_sysSettings.RaySplittingProperties.ScatterRaySplittingLevelOn = Convert.ToBoolean( GetSettingByName( "RaySplitting-ScatterRaysSplittingLevelOn" ) );
                m_sysSettings.RaySplittingProperties.RaySplittingMonteCarlo = Convert.ToBoolean( GetSettingByName( "Raysplitting-MonteCarlo" ) );

                // Get Ray Termination            
                m_sysSettings.RayTerminationProperties.AbsFluxThreshold = Convert.ToDouble( GetSettingByName( "RayTermination-AbsFluxThreshold" ), StringUtils.CultureFormat );
                m_sysSettings.RayTerminationProperties.CurrentFluxRatio = Convert.ToDouble( GetSettingByName( "RayTermination-CurrentFluxRatio" ), StringUtils.CultureFormat );
                m_sysSettings.RayTerminationProperties.MaxS = Convert.ToInt32( GetSettingByName( "RayTermination-MaxS" ) );
                m_sysSettings.RayTerminationProperties.MaxT = Convert.ToInt32( GetSettingByName( "RayTermination-MaxT" ) );
                m_sysSettings.RayTerminationProperties.Warnings = Convert.ToInt32( GetSettingByName( "RayTermination-Warnings" ) );
                m_sysSettings.RayTerminationProperties.PropogationDirectionsToSuppress = (RayTermination.PropogationDirections)TryParse( typeof( RayTermination.PropogationDirections ), "RayTermination-Direction" );


                // Get Ray Trace Attributes
                m_sysSettings.RayTraceAttributeProperties.SaveRayFile = Convert.ToBoolean( GetSettingByName( "RayTraceAttributes-SaveFile" ) );
                m_sysSettings.RayTraceAttributeProperties.RayFilename = Convert.ToString(GetSettingByName( "RayTraceAttributes-Filename" ));

                // Get Ray Generation
                m_sysSettings.RayGenerationProperties.RandomSeed = Convert.ToInt32( GetSettingByName( "RayGeneration-RandomSeed" ) );
                m_sysSettings.RayGenerationProperties.RandomSeedGeneration = Convert.ToBoolean( GetSettingByName( "RayGeneration-RandomSeedGeneration" ) );
                m_sysSettings.RayGenerationProperties.UseRandomNumber = Convert.ToBoolean( GetSettingByName( "RayGeneration-UseRandomSeedNumber" ) );

                //  Get Plotting                
                m_sysSettings.TracePlottingProperties.PlotRays = Convert.ToBoolean( GetSettingByName( "TracePlot-PlotRays" ) );
                m_sysSettings.TracePlottingProperties.PlotColor = System.Drawing.Color.FromArgb( Convert.ToInt32( GetSettingByName( "TracePlot-PlotColor" ) ) );
                m_sysSettings.TracePlottingProperties.NthRay = Convert.ToInt32( GetSettingByName( "TracePlot-NthRay" ) );
                m_sysSettings.TracePlottingProperties.Seed = Convert.ToDouble( GetSettingByName( "TracePlot-Seed" ), StringUtils.CultureFormat );
                m_sysSettings.TracePlottingProperties.TotalRays = Convert.ToInt32( GetSettingByName( "TracePlot-TotalRays" ) );
                m_sysSettings.TracePlottingProperties.PlotTotalRays = Convert.ToBoolean( GetSettingByName( "TracePlot-PlotTotalRays" ) );

                //  Get Stats                
                m_sysSettings.TraceStatisticsProperties.AccumulateAll = Convert.ToBoolean( GetSettingByName( "TraceStats-AccumulateAll" ) );
                m_sysSettings.TraceStatisticsProperties.AccumulateStatistics = Convert.ToBoolean( GetSettingByName( "TraceStats-AccumulateStatistics" ) );
                m_sysSettings.TraceStatisticsProperties.Keep = Convert.ToBoolean( GetSettingByName( "TraceStats-Keep" ) );
                m_sysSettings.TraceStatisticsProperties.PrintStatistics = Convert.ToBoolean( GetSettingByName( "TraceStats-PrintStatistics" ) );
                m_sysSettings.TraceStatisticsProperties.IdentifyCriticalObjects = Convert.ToBoolean( GetSettingByName( "TraceStats-IdentifyCriticalObjects" ) );
                m_sysSettings.TraceStatisticsProperties.DirectObjects = Convert.ToBoolean( GetSettingByName( "TraceStats-DirectObjects" ) );

                //  wavelength & Power Units Settings               
                m_sysSettings.WaveLengthSettingsProperties.Wavelength = Convert.ToDouble( GetSettingByName( "Wavelength-Wavelength" ), StringUtils.CultureFormat );
                m_sysSettings.WaveLengthSettingsProperties.WavelengthUnits = (ASAPUnits)Enum.Parse( typeof( ASAPUnits ), GetSettingByName( "WaveLength-WavelengthUnits" ), true );
                m_sysSettings.PowerUnitsProperties.PowerUnitsValue = Convert.ToString( GetSettingByName( "PowerUnits-PowerUnitsValue" ) );
                m_sysSettings.PowerUnitsProperties.UseTotalSystemPower = Convert.ToBoolean( GetSettingByName( "Power-TotalSystemPower" ) );
                m_sysSettings.PowerUnitsProperties.TotalSystemPowerValue = Convert.ToDouble( GetSettingByName( "Power-TotalSystemPowerValue" ) );
                m_sysSettings.WaveLengthSettingsProperties.SystemUnits = Convert.ToString( GetSettingByName( "WaveLength-SystemUnits" ) );
                
                //  Get all enum settings.
                m_sysSettings.MissedRayPlottingProperties.MissedRayPlotStyle = (MissedRayPlotting.MissedRayPlotStyleOption)TryParse( typeof( MissedRayPlotting.MissedRayPlotStyleOption ), "MissedRayPlotting-PlottingStyle" );
                m_sysSettings.RaySplittingProperties.CurrentTraceOption = (RaySplitting.RaySplitTraceOptions)TryParse( typeof( RaySplitting.RaySplitTraceOptions ), "RaySplitting-TraceOption" );
                m_sysSettings.RayTraceAccuracyProperties.AccuracyLevel = (RayTraceAccuracy.Level)TryParse( typeof( RayTraceAccuracy.Level ), "AccuracyLevel" );
                
                //  Get Fresnel settings            
                m_sysSettings.FresnelProperties.CurrentFresnelOption = (Fresnel.FresnelOption)TryParse( typeof( Fresnel.FresnelOption ), "FresnelOption" );
                m_sysSettings.GeneralSettingsProperties.CurrentAxis = (GeneralSettings.AxisOptions)TryParse( typeof( GeneralSettings.AxisOptions ), "GeneralSettings-Axis" );
                m_sysSettings.CoherenceProperties.WavelengthUnits = (ASAPUnits)Enum.Parse( typeof( ASAPUnits ), GetSettingByName( "CoherenceWavelengthUnits" ), true );
                m_sysSettings.CoherenceProperties.CurrentCoherenceOption = (Coherence.CoherenceOption)TryParse( typeof( Coherence.CoherenceOption ), "CoherenceOption" );
                m_sysSettings.CoherenceProperties.CurrentPropagationOption = (Coherence.PropagationOption)TryParse( typeof( Coherence.PropagationOption ), "CoherencePropagationOption" );
                m_sysSettings.CoherenceProperties.Rays = Convert.ToDouble(GetSettingByName("Coherence-Rays"), StringUtils.CultureFormat);
                m_sysSettings.CoherenceProperties.BeamScaleFactor = Convert.ToDouble(GetSettingByName("Coherence-BeamScaleFactor"), StringUtils.CultureFormat);
                m_sysSettings.CoherenceProperties.HeightScaleFactor = Convert.ToDouble(GetSettingByName("Coherence-HeightScaleFactor"), StringUtils.CultureFormat);

            }
            catch ( Exception e )
            {
               /* MessageBox.Show( "When retreiving the project settings " + m_curSysSetting + ", the following error occured: " + 
                                 System.Environment.NewLine + 
                                 e.Message, 
                                 StringUtils.ProductInfo.ASAPName );

                Utility.Diagnostics.ErrorLogger errLog = Utility.Diagnostics.ErrorLogger.Instance;
                
                errLog.LogException( e );*/
            }

            return true;
        }


        //===================================================================
        private object TryParse( Type EnumT, String Setting )
        {
            try
            {
                return Enum.Parse( EnumT, GetSettingByName( Setting ), true );
            }
            catch ( Exception e )
            {
               /* MessageBox.Show( "The current database setting for " + 
                                 Setting + 
                                 " is invalid" + 
                                 System.Environment.NewLine + 
                                 e.Message, 
                                 StringUtils.ProductInfo.ASAPName );

                Utility.Diagnostics.ErrorLogger errLog = Utility.Diagnostics.ErrorLogger.Instance;
                
                errLog.LogException( e );*/
            }

            return "";
        }
    }
}
