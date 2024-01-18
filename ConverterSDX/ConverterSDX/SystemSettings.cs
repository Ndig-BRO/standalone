using System;
using System.Drawing;
using System.Diagnostics;
using Utility.Geometry;

namespace ASAPData
{
    //=======================================================================
    public class AmbientMedia
    {
        //===================================================================
        private string m_AmbientMediaName;

        /// Gets or sets the ambient media ID.
        public string AmbientMediaName
        {
            get { return m_AmbientMediaName; }

            set { m_AmbientMediaName = value; }
        }

        //===================================================================
        /// initializes Ambient media object with defaults
        public AmbientMedia()
        {
            m_AmbientMediaName = "Air";
        }

    }

    //=======================================================================
    /// handles fresnel reflection/transmission coefficients settings
    public class Fresnel
    {
        //===================================================================
        /// the  fresnel options  that may be  used
        public enum FresnelOption
        {
            OFF,
            AVE,
            BOTH,
            TIR
        } ;

        //===================================================================    
        protected FresnelOption m_CurrentFresnelOption;

        /// fresnel option desired
        public FresnelOption CurrentFresnelOption
        {
            get { return m_CurrentFresnelOption; }

            set
            {
                if ( Enum.GetName( typeof( FresnelOption ), value ) != null )
                {
                    m_CurrentFresnelOption = value;
                }
                else
                {
                   // Debug.WriteLine( "FresnelOption value is invalid!" );
                }
            }
        }

        //===================================================================
        /// initializes Fresnel object with defaults
        public Fresnel()
        {
        }        
    }

    //=======================================================================
    public class GeneralSettings
    {
        //===================================================================
        /// the  Axis options
        public enum AxisOptions
        {
            Off,
            NEGX,
            NEGY,
            NEGZ,
            X,
            Y,
            Z,
            Local
        } ;

        //===================================================================
        protected int m_WarningsThreshold;
        public int WarningsThreshold
        {
            get { return m_WarningsThreshold; }

            set { m_WarningsThreshold = value; }
        }

        //===================================================================
        protected String m_LocalObject;

        public String LocalObject
        {
            get { return m_LocalObject; }

            set { m_LocalObject = value; }
        }

        //===================================================================
        protected bool m_ListWarnings;

        public bool ListWarnings
        {
            get { return m_ListWarnings; }

            set { m_ListWarnings = value; }
        }

        //===================================================================
        protected AxisOptions m_CurrentAxis;

        public AxisOptions CurrentAxis
        {
            get { return m_CurrentAxis; }

            set
            {
                if ( Enum.GetName( typeof( AxisOptions ), value ) != null )
                {
                    m_CurrentAxis = value;
                }
                else
                {
                    //Debug.WriteLine( "AxisOptions value is invalid!" );
                }
            }
        }

        //=================================================================== 
        /// initializes general Settings object with defaults
        public GeneralSettings()
        {
        }
    }

    //=======================================================================
    /// Wrapper class for all project settings.
    public class SystemSettings
    {
        //===================================================================
        private AmbientMedia m_AmbientMediaProperties = new AmbientMedia();
            
        /// Gets or sets the ambient media properties.
        public AmbientMedia AmbientMediaProperties
        {
            get { return m_AmbientMediaProperties; }
            set { m_AmbientMediaProperties = value; }
        }

        //===================================================================
        Coherence m_CoherenceProperties = new Coherence();

        /// Gets or sets the coherence properties.    
        public Coherence CoherenceProperties
        {
            get { return m_CoherenceProperties; }
            set { m_CoherenceProperties = value; }
        }

        //===================================================================
        private Fresnel m_FresnelProperties = new Fresnel();
    
        /// Gets or sets the fresnel properties.
        public Fresnel FresnelProperties
        {
            get { return m_FresnelProperties; }
            set { m_FresnelProperties = value; }
        }

        //===================================================================
        private GeneralSettings m_GeneralSettingsProperties = new GeneralSettings();
    
        /// Gets or sets the general settings properties.
        public GeneralSettings GeneralSettingsProperties
        {
            get { return m_GeneralSettingsProperties; }
            set { m_GeneralSettingsProperties = value; }
        }

        //===================================================================
        private MissedRayPlotting m_MissedRayPlottingProperties = new MissedRayPlotting();
    
        /// Gets or sets the missed ray plotting properties.
        public MissedRayPlotting MissedRayPlottingProperties
        {
            get { return m_MissedRayPlottingProperties; }
            set { m_MissedRayPlottingProperties = value; }
        }

        //===================================================================
        private RayArrows m_RayArrowsProperties = new RayArrows();
    
        /// Gets or sets the ray arrows properties.
        public RayArrows RayArrowsProperties
        {
            get { return m_RayArrowsProperties; }
            set { m_RayArrowsProperties = value; }
        }

        //===================================================================
        private RaySplitting m_RaySplittingProperties = new RaySplitting();
    
        /// Gets or sets the ray splitting properties.
        public RaySplitting RaySplittingProperties
        {
            get { return m_RaySplittingProperties; }
            set { m_RaySplittingProperties = value; }
        }

        //===================================================================
        private RayTermination m_RayTerminationProperties = new RayTermination();
    
        /// Gets or sets the ray termination properties.
        public RayTermination RayTerminationProperties
        {
            get { return m_RayTerminationProperties; }
            set { m_RayTerminationProperties = value; }
        }

        //===================================================================
        private RayGeneration m_RayGenerationProperties = new RayGeneration();

        /// Gets or sets the ray generation properties.
        public RayGeneration RayGenerationProperties
        {
            get { return m_RayGenerationProperties; }
            set { m_RayGenerationProperties = value; }
        }

        //===================================================================
        private RayTraceAttributes m_RayTraceAttributeProperties = new RayTraceAttributes();
    
        /// Gets or sets the ray trace attribute properties.
        public RayTraceAttributes RayTraceAttributeProperties
        {
            get { return m_RayTraceAttributeProperties; }
            set { m_RayTraceAttributeProperties = value; }
        }

        //===================================================================
        private RayTraceAccuracy m_RayTraceAccuracyProperties = new RayTraceAccuracy();
    
        /// Gets or sets the ray trace accuracy properties.
        public RayTraceAccuracy RayTraceAccuracyProperties
        {
            get { return m_RayTraceAccuracyProperties; }
            set { m_RayTraceAccuracyProperties = value; }
        }

        //===================================================================
        private TracePlotting m_TracePlottingProperties = new TracePlotting();
    
        /// Gets or sets the trace plotting properties.
        public TracePlotting TracePlottingProperties
        {
            get { return m_TracePlottingProperties; }
            set { m_TracePlottingProperties = value; }
        }

        //===================================================================
        private TraceStatistics m_TraceStatisticsProperties = new TraceStatistics();
    
        /// Gets or sets the trace statistics properties.
        public TraceStatistics TraceStatisticsProperties
        {
            get { return m_TraceStatisticsProperties; }
            set { m_TraceStatisticsProperties = value; }
        }

        //===================================================================
        private WavelengthSettings m_WaveLengthSettingsProperties = new WavelengthSettings();
    
        /// Gets or sets the wave length settings properties.
        public WavelengthSettings WaveLengthSettingsProperties
        {
            get { return m_WaveLengthSettingsProperties; }
            set { m_WaveLengthSettingsProperties = value; }
        }

        //===================================================================
        private PowerUnits m_PowerUnitsProperties = new PowerUnits();
    
        /// Gets or sets the power units properties.
        public PowerUnits PowerUnitsProperties
        {
            get { return m_PowerUnitsProperties; }
            set { m_PowerUnitsProperties = value; }
        }

        //===================================================================
        /// Initializes a new instance of the <see cref="SystemSettings"/> class.
        public SystemSettings()
        {
        }
    }

    //=======================================================================
    /// handles missed ray plotting settings
    public class MissedRayPlotting
    {
        //===================================================================
        /// the  Missed Ray Plot Style options  that may be  used
        public enum MissedRayPlotStyleOption
        {
            OFF,
            ARROWS,
            LINES
        } ;

        //===================================================================    
        /// THe user may choose to display missed rays or not.  If so, then 
        /// the rays may be drawn as arrows or lines
        protected MissedRayPlotStyleOption m_MissedRayPlotStyle;

        public MissedRayPlotStyleOption MissedRayPlotStyle
        {
            get { return m_MissedRayPlotStyle; }

            set
            {
                if ( Enum.GetName( typeof( MissedRayPlotStyleOption ), value ) != null )
                {
                    m_MissedRayPlotStyle = value;
                }
                else
                {
                    //Debug.WriteLine( "MissedRayPlotStyleOption value is invalid!" );
                }
            }
        }

        //===================================================================
        protected double m_Distance;
            
        /// Distance for Missed Ray Plotting
        public double Distance
        {
            get { return m_Distance; }
            set { m_Distance = value; }
        }

        //===================================================================
        /// initializes MissedRayPlotting object with defaults
        public MissedRayPlotting()
        {
        }
    }

    //=======================================================================
    /// handles ray arrows settings
    public class RayArrows
    {
        //===================================================================
        /// indicates whether to display arrowheads on rays
        protected bool m_DisplayArrows;
            
        public bool DisplayArrows
        {
            get { return m_DisplayArrows; }
            set { m_DisplayArrows = value; }
        }

        //===================================================================
        /// the scaling factor desired
        protected double m_ScalingFactor;
        public double ScalingFactor
        {
            get { return m_ScalingFactor; }
            set { m_ScalingFactor = value; }
        }

        //===================================================================
        /// initialize the rayarrows object with default values
        public RayArrows()
        {
        }
    }

    //=======================================================================
    public class RaySplitting
    {
        //===================================================================
        /// the  Ray Splitting options.
        public enum RaySplitTraceOptions
        {
            Norm,
            Trans,
            Refl,
            MonteCarlo,
        } ;

        //===================================================================
        protected int m_ScatterLevels;
        
        public int ScatterLevels
        {
            get { return m_ScatterLevels; }
            set { m_ScatterLevels = value; }
        }

        //===================================================================
        protected double m_DiffuseRayFluxThreshold;

        public double DiffuseRayFluxThreshold
        {
            get { return m_DiffuseRayFluxThreshold; }
            set { m_DiffuseRayFluxThreshold = value; }
        }

        //===================================================================    
        ///  true if the user wants ray scattering, 
        /// false turns it off ***Note that this is not really used ***
        protected bool m_ScatterRaySplittingLevelOn;
    
        public bool ScatterRaySplittingLevelOn
        {
            get { return m_ScatterRaySplittingLevelOn; }
            set { m_ScatterRaySplittingLevelOn = value; }
        }

        //===================================================================
        ///  true if the user wants scatter rays generated for 
        /// each specular child ray
        protected bool m_ScatterRaysAllChildrenOn;

        public bool ScatterRaysAllChildrenOn
        {
            get { return m_ScatterRaysAllChildrenOn; }
            set { m_ScatterRaysAllChildrenOn = value; }
        }

        //===================================================================    
        /// m_MaxSplits indicates the maximum number of times 
        /// a ray may be split into specular components.  0 is the default
        protected int m_MaxSplits;
    
        public int MaxSplits
        {
            get { return m_MaxSplits; }
            set { m_MaxSplits = value; }
        }

        //===================================================================
        ///  the fractional energy cutoff for ray splitting. 
        /// The default for c is 1.E-6.   
        protected double m_EnergyCutoff;

        public double EnergyCutoff
        {
            get { return m_EnergyCutoff; }
            set { m_EnergyCutoff = value; }
        }

        //===================================================================
        ///  true if the user wants ray splitting on, 
        /// false turns it off
        protected bool m_RaySplittingOn;

        public bool RaySplittingOn
        {
            get { return m_RaySplittingOn; }
            set { m_RaySplittingOn = value; }
        }

        //===================================================================    
        protected bool m_RaySplittingMonteCarlo;

        public bool RaySplittingMonteCarlo
        {
            get { return m_RaySplittingMonteCarlo; }
            set { m_RaySplittingMonteCarlo = value; }
        }

        //===================================================================
        /// The currently selected split trace option
        protected RaySplitTraceOptions m_CurrentSplitTraceOption;

        public RaySplitTraceOptions CurrentTraceOption
        {
            get { return m_CurrentSplitTraceOption; }

            set
            {
                if ( Enum.GetName( typeof( RaySplitTraceOptions ), value ) != null )
                {
                    m_CurrentSplitTraceOption = value;
                }
                else
                {
                    //Debug.WriteLine( "RaySplitTraceOptions value is invalid!" );
                }
            }
        }

        //===================================================================
        /// initializes ray splitting object with defaults
        public RaySplitting()
        {
            MaxSplits = 0;
            EnergyCutoff = 0;
        }
    }

    //=======================================================================
    /// Class to handle ray generation settings. 
    public class RayGeneration
    {
        //===================================================================
        ///  The random seed. 
        protected int m_RandomSeed;
    
        ///  The random seed. 
        public int RandomSeed
        {
            get { return m_RandomSeed; }
            set { m_RandomSeed = value; }
        }

        //===================================================================
        /// The random seed generation flag
        protected bool m_RandomSeedGeneration;
   
        /// Sets random seed generation 
        public bool RandomSeedGeneration
        {
            get { return m_RandomSeedGeneration; }
            set { m_RandomSeedGeneration = value; }
        }

        //===================================================================
        /// The use a quasi random seed number flag
        protected bool m_UseRandomNumber;
    
        /// Sets the use quasi random number sequence 
        public bool UseRandomNumber
        {
            get { return m_UseRandomNumber; }
            set { m_UseRandomNumber = value; }
        }

        //===================================================================
        /// initializes ray generation object with defaults  
        public RayGeneration()
        {
        }
    }

    //=======================================================================
    /// handles ray termination settings
    public class RayTermination
    {
        //===================================================================    
        /// the propogation directions to suppress
        public enum PropogationDirections
        {
            OFF,
            NEGX,
            NEGY,
            NEGZ,
            X,
            Y,
            Z,
            NEG,
            POS
        } ;

        //===================================================================
        static PropogationDirections DefaultDirections = PropogationDirections.OFF;

        //===================================================================
        ///  the maximum number of times (maxS) a ray can 
        /// consecutively intersect the same non-LENS object. 
        protected int m_MaxS;

        public int MaxS
        {
            get { return m_MaxS; }
            set { m_MaxS = value; }
        }

        //===================================================================
        /// the maximum number of total object intersections 
        protected int m_MaxT;

        public int MaxT
        {
            get { return m_MaxT; }
            set { m_MaxT = value; }
        }

        //===================================================================
        /// sets the absolute flux threshold (t)- below this 
        /// value, the system ignores the rays to the decimal 
        /// number t (default 1.E-18).
        protected double m_AbsFluxThreshold;

        public double AbsFluxThreshold
        {
            get { return m_AbsFluxThreshold; }
            set { m_AbsFluxThreshold = value; }
        }

        //===================================================================
        /// the current flux/initial flux threshold ratio.
        /// The default for the decimal number is 1.E-6
        protected double m_CurrentFluxRatio;

        public double CurrentFluxRatio
        {
            get { return m_CurrentFluxRatio; }
            set { m_CurrentFluxRatio = value; }
        }

        //===================================================================
        /// The number of ray warning messages
        protected int m_NumberOfWarnings;

        public int Warnings
        {
            get { return m_NumberOfWarnings; }
            set { m_NumberOfWarnings = value; }
        }

        //===================================================================
        /// The user may specifies OFF, which turns off the 
        /// global coordinate or surface normal direction.  Or 
        /// the user may specify a literal entry (-X, -Y, -Z, 
        /// X, Y, Z, -, or +)  for the undesirable propagation direction. 
        /// There is no default.  
        protected PropogationDirections m_PropogationDirectionsToSuppress;

        public PropogationDirections PropogationDirectionsToSuppress
        {
            get { return m_PropogationDirectionsToSuppress; }

            set
            {
                if ( Enum.GetName( typeof( PropogationDirections ), value ) != null )
                {
                    m_PropogationDirectionsToSuppress = value;
                }
                else
                {
                   // Debug.WriteLine( "PropogationDirections value is invalid!" );
                }
            }
        }

        //==================================================================
        /// initializes ray termination object with defaults
        public RayTermination()
        {
        }
    }
    //=======================================================================
    /// handles coherence settings
    public class Coherence
    {
        //===================================================================
        /// Default coherence options
        public enum CoherenceOption
        {
            Coherent,
            Incoherent
        } ;

        //===================================================================
        /// Default propagation options
        public enum PropagationOption
        {        
            Geometric,
            Diffract
        } ;

        //===================================================================
        protected PropagationOption m_propagationOption;
    
        /// Coherence propagation option
        public PropagationOption CurrentPropagationOption
        {
            get { return m_propagationOption; }
            set { m_propagationOption = value; }
        }

        //===================================================================
        protected CoherenceOption m_coherenceOption;

        public CoherenceOption CurrentCoherenceOption
        {
            get { return m_coherenceOption; }

            set
            {
                if ( Enum.GetName( typeof( CoherenceOption ), value ) != null )
                {
                    m_coherenceOption = value;
                }
                else
                {
                   // Debug.WriteLine( "Coherence option value is invalid!" );
                }
            }
        }

        //===================================================================
        /// The user may choose a value for the units desired
        protected ASAPUnits m_WavelengthUnits;

        /// wavelength units
        public ASAPUnits WavelengthUnits
        {
            get { return m_WavelengthUnits; }

            set
            {
                if ( Enum.GetName( typeof( ASAPUnits ), value ) != null )
                {
                    m_WavelengthUnits = value;
                }
                else
                {
                   // Debug.WriteLine( "WavelengthUnitsChoices value is invalid!" );
                }
            }
        }

        //===================================================================
        protected double m_Rays;
    
        /// Number of parabasal rays
        public double Rays
        {
            get { return m_Rays; }
            set { m_Rays = value; }
        }

        //===================================================================
        // width scale factor
        protected double m_beamScaleFactor;
    
        /// Gaussian beam overlap scale factor
        public double BeamScaleFactor
        {
            get { return m_beamScaleFactor; }
            set { m_beamScaleFactor = value; }
        }

        //===================================================================
        protected double m_heightScaleFactor;
    
        /// Parabasal ray height scale factor
        public double HeightScaleFactor
        {
            get { return m_heightScaleFactor; }
            set { m_heightScaleFactor = value; }
        }

        //===================================================================
        /// initializes coherence object with defaults    
        public Coherence()
        {
            m_coherenceOption = CoherenceOption.Incoherent;
            m_propagationOption = PropagationOption.Geometric;
            m_Rays = 4;
            m_beamScaleFactor = 1.6;
            m_heightScaleFactor = 1;
        }
    }

    //======================================================================
    /// handles ray trace accuracy settings
    public class RayTraceAccuracy
    {
        //===================================================================
        public enum Level
        {
            High,
            Medium,
            Low
        } ;

        //===================================================================
        /// Was level of accuracy desired. Now controls geometry healing in INR generation.
        protected Level m_AccuracyLevel;
    
        public Level AccuracyLevel
        {
            get { return m_AccuracyLevel; }

            set
            {
                if ( Enum.GetName( typeof( Level ), value ) != null )
                {
                    m_AccuracyLevel = value;
                }
                else
                {
                    //Debug.WriteLine( "AccuracyLevel value is invalid!" );
                }
            }
        }

        //===================================================================
        protected bool m_ParallelOn;

        public bool ParallelOn
        {
            get { return m_ParallelOn; }
            set { m_ParallelOn = value; }
        }

        //===================================================================
        protected double m_Degrees;

        public double Degrees
        {
            get { return m_Degrees; }
            set { m_Degrees = value; }
        }

        //===================================================================
        /// initializes raytraceAccuracy object with defaults
        public RayTraceAccuracy()
        {
            m_AccuracyLevel = Level.Low;
            m_Degrees = 0.0;
            m_ParallelOn = false;
        }
    }

    //======================================================================
    /// Class to handle Trace Plotting settings. 
    /// The Trace Plotting settings apply for tracing all 
    public class TracePlotting
    {
        //===================================================================
        protected int m_NthRay;

        public int NthRay
        {
            get { return m_NthRay; }
            set { m_NthRay = value; }
        }

        //===================================================================
        /// The user may enter a seed that is used for the 
        /// random number generator when using roughness, random,
        /// scattor random/towards, grid random, and emitting commands
        protected double m_Seed;

        public double Seed
        {
            get { return m_Seed; }
            set { m_Seed = value; }
        }

        //===================================================================
        /// Total number of rays to plot. This is alternative to user specifying m_NthRay
        /// value.  In this case, we need to calculate m_NthRay value for user.
        protected int m_TotalRays;

        public int TotalRays
        {
            get { return m_TotalRays; }
            set { m_TotalRays = value; }
        }

        //===================================================================
        /// If true, only display m_TotalRays of ray tracks.
        protected bool m_PlotTotalRays;

        public bool PlotTotalRays
        {
            get { return m_PlotTotalRays; }
            set { m_PlotTotalRays = value; }
        }

        //===================================================================
        /// The user may choose to plot every nth ray.  
        /// If this boolean is true, the user 
        /// has indicated that they wish to plot every nth ray,    
        protected bool m_PlotRays;
    
        public bool PlotRays
        {
            get { return m_PlotRays; }
            set { m_PlotRays = value; }
        }

        //===================================================================
        /// The user may specify the color  
        /// in which the rays should be plotted    
        protected Color m_PlotColor;
    
        public Color PlotColor
        {
            get { return m_PlotColor; }
            set { m_PlotColor = value; }
        }

        //===================================================================
        /// initializes TracePlotting object with defaults
        public TracePlotting()
        {
        }
    }

    //=======================================================================
    public class TraceStatistics
    {
        //===================================================================
        /// If this boolean is true, the user 
        ///  wishes to print the statistics
        protected bool m_PrintStatistics;

        public bool PrintStatistics
        {
            get { return m_PrintStatistics; }
            set { m_PrintStatistics = value; }
        }

        //===================================================================
        protected bool m_AccumulateStatistics;

        public bool AccumulateStatistics
        {
            get { return m_AccumulateStatistics; }
            set { m_AccumulateStatistics = value; }
        }

        //===================================================================    
        protected bool m_AccumulateAll;

        public bool AccumulateAll
        {
            get { return m_AccumulateAll; }
            set { m_AccumulateAll = value; }
        }

        //===================================================================
        protected bool m_Keep;

        public bool Keep
        {
            get { return m_Keep; }
            set { m_Keep = value; }
        }

        //===================================================================
    
        protected bool m_IdentifyCriticalObjects;

        public bool IdentifyCriticalObjects
        {
            get { return m_IdentifyCriticalObjects; }
            set { m_IdentifyCriticalObjects = value; }
        }

        //===================================================================
        protected bool m_DirectObjects;

        public bool DirectObjects
        {
            get { return m_DirectObjects; }
            set { m_DirectObjects = value; }
        }

        //===================================================================
        public TraceStatistics()
        {
        }
    }

    //=======================================================================
    /// Class to handle  Wavelength settings. 
    public class WavelengthSettings
    {
        //===================================================================
        protected double m_Wavelength;

        public double Wavelength
        {
            get { return m_Wavelength; }
            set { m_Wavelength = value; }
        }

        //===================================================================
        /// The user may choose a value for the units desired
        protected ASAPUnits m_WavelenthUnits;

        public ASAPUnits WavelengthUnits
        {
            get { return m_WavelenthUnits; }
         
            set
            {
                if ( Enum.GetName( typeof( ASAPUnits ), value ) != null )
                {
                    m_WavelenthUnits = value;
                }
                else
                {
                   // Debug.WriteLine( "WavelengthUnitsChoices value is invalid!" );
                }
            }
        }

        //===================================================================    
        /// The user may choose a value for the system units desired
        private String m_SystemUnits;
    
        public String SystemUnits
        {
            get { return m_SystemUnits; }
            set
            {
                m_SystemUnits = value;
            }
        }

        //===================================================================    
        /// initializes WavelengthSettings object with defaults
        public WavelengthSettings()
        {
        }
    }

    //=======================================================================
    /// Public class for all properties related to the Units of Power (Power Units)
    public class PowerUnits
    {
        //===================================================================
        private double m_TotalSystemPowerValue;
    
        public double TotalSystemPowerValue
        {
            get { return m_TotalSystemPowerValue; }
            set { m_TotalSystemPowerValue = value; }
        }

        //===================================================================
        private bool m_UseTotalSystemPower;
   
        public bool UseTotalSystemPower
        {
            get { return m_UseTotalSystemPower; }
            set { m_UseTotalSystemPower = value; }
        }

        //===================================================================
        private string m_PowerUnitsValue;

        public string PowerUnitsValue
        {
            get { return m_PowerUnitsValue; }
            set { m_PowerUnitsValue = value; }
        }

        //===================================================================    
        /// Initializes a new instance of the <see cref="PowerUnits"/> class.
        public PowerUnits()
        {
        }
    }

    //=======================================================================
    public class RayPathAnalysis
    {
        //===================================================================
        public RayPathAnalysis()
        {
        }

    }

    //=======================================================================
    public class RayFluenceAndAbsorbedAnalysis
    {
        //===================================================================
        public RayFluenceAndAbsorbedAnalysis()
        {
        }
    }

    //=======================================================================
    /// Class for ray trace attributes such as the SAVE command
    public class RayTraceAttributes
    {
        //===================================================================
        private bool m_SaveRayFile;

        public bool SaveRayFile
        {
            get { return m_SaveRayFile; }
            set { m_SaveRayFile = value; }
        }

        //===================================================================
        private string m_RayFilename;

        public string RayFilename
        {
            get { return m_RayFilename; }
            set { m_RayFilename = value; }
        }

        //===================================================================
        public RayTraceAttributes()
        {
        }
    }
}
