using ASAPData;
using System;
using System.Collections.Generic;
using xnode;

namespace ConverterSDX
{
    class ProjectSDF
    {
        string db_path = "";
        public string DB_path
        {
            get
            {
               return db_path;
            }
            set
            {
                db_path = value;
            }
        }

        //===================================================================
        /// Handles the saving of Optics Manager files in sdf and sdx format
        public void FileSaveHandler(string filePath)
        {
            try
            {
               // from path to name
                string fileToSave = "";
                DB_path = filePath;

                if (fileToSave == "GlobalOpticalData.sdf")
                    GlobalSDF.saveGlobal(filePath);
                else
                    saveSDX(filePath);
            }
            catch (Exception exc)
            {
                // save failed
            }
        }
        private void saveSDX(string filename)
        {
            SystemSettingsData ssd = new SystemSettingsData();
            ssd.GetDBSettings();

            XMLNode doc = new XMLNode("ASAPOpticalSystem", null, "", "", null);

            XMLNode sysNode = new XMLNode("SystemSettings", null, "", "", doc);

            Dictionary<string, string> vals = ssd.GetSettings();
            foreach (KeyValuePair<string, string> kvp in vals)
            {
                XMLNode node = new XMLNode("SysParam", null, "", "", sysNode);
                node.setAttr("Name", kvp.Key);
                node.setText(XMLNode.base64Encode(kvp.Value), true);
            }
            List<SourceWavelengthPair> waveList = new List<SourceWavelengthPair>();
            SourceWavelengthData wvData = new SourceWavelengthData();
            waveList = wvData.ReturnAllWavelengthData();

            XMLNode waveNode = new XMLNode("Wavelengths", null, "", "", doc);
            foreach (SourceWavelengthPair swp in waveList)
            {
                XMLNode node = new XMLNode("Wavelength", null, "", "", waveNode);

                node.setAttr("ID", Guid.NewGuid().ToString());
                node.setAttr("Wavelength", swp.Wavelength);
                node.setAttr("Enabled", swp.Status);
            }

            // Save variables
            XMLNode varNode = new XMLNode("Variables", null, "", "", doc);
            Dictionary<string, string> varList = new Dictionary<string, string>(treeOpticalSystem.GetVariables());

            foreach (KeyValuePair<string, string> info in varList)
            {
                XMLNode node = new XMLNode("Variable", null, "", "", varNode);

                node.setAttr("Name", info.Key);
                node.setText(XMLNode.base64Encode(info.Value), true);
            }

            // Save geometry
            XMLNode geoNode = new XMLNode("Geometry", null, "", "", doc);

            foreach (Node subNode in m_geomNode.Nodes)
            {
                saveGeometry(subNode, geoNode, "GeometryNode", null);
            }

            // Save sources
            XMLNode srcNode = new XMLNode("Sources", null, "", "", doc);

            foreach (Node subNode in m_sourcesNode.Nodes)
            {
                saveGeometry(subNode, srcNode, "SourceNode", null);
            }

            // Save sources analysis
            XMLNode saNode = new XMLNode("SourceAnalysis", null, "", "", doc);

            foreach (Node subNode in m_sourcesAnalysisNode.Nodes)
            {
                saveGeometry(subNode, saNode, "SourceAnalysisNode", null);
            }

            // Save trace
            XMLNode traceNode = new XMLNode("Trace", null, "", "", doc);

            foreach (Node subNode in m_traceNode.Nodes)
            {
                saveGeometry(subNode, traceNode, "TraceNode", null);
            }

            // Save trace analysis
            XMLNode taNode = new XMLNode("TraceAnalysis", null, "", "", doc);

            foreach (Node subNode in m_analysisNode.Nodes)
            {
                saveGeometry(subNode, taNode, "TraceAnalysisNode", null);
            }

            File.WriteAllText(filename, doc.toxml(0, true));
        }

        //===================================================================
        /// Private recursively called routine for storing geometry nodes
        private void saveGeometry(Node node, XMLNode parent, string nodeTag, Node widget)
        {
            String id = node.Tag.ToString();

            XMLNode xNode = new XMLNode(nodeTag, null, "", "", parent);

            var name = node.Text;
            var nodeType = treeOpticalSystem.GetNodeType(id);
            var commandType = treeOpticalSystem.GetCommandType(id);
            var nodeName = treeOpticalSystem.GetNodeName(id);

            xNode.setAttr("Name", name);
            xNode.setAttr("NodeType", nodeType.ToString());
            xNode.setAttr("CommandType", commandType.ToString());
            xNode.setAttr("NodeName", nodeName.ToString());
            xNode.setAttr("ID", id);

            // Save the command parameters
            IBaseCommand cmdBase = treeOpticalSystem.GetCommandInfo(id);

            if (cmdBase != null)
            {
                foreach (KeyValuePair<string, string> paramPair in cmdBase.ParameterList)
                {
                    // If we need to have all referenced ID's local to the main widget we are saving...
                    if (widget != null && isGuid(paramPair.Value))
                    {
                        if (idSearch(widget, paramPair.Value) == false)
                        {
                            throw new Exception("Reference not found");
                        }
                    }

                    XMLNode param = new XMLNode("Param", null, "", "", xNode);

                    param.setAttr("Key", paramPair.Key);
                    param.setText(XMLNode.base64Encode(paramPair.Value), true);
                }

                if (commandType == BROCommand.CommandType.LightSourceInstance)
                {
                    cmdSource src = cmdBase as cmdSource;
                    if (src != null)
                    {
                        foreach (LightSourceInstance lsi in src.SourceList)
                        {
                            XMLNode instanceNode = new XMLNode("SourceData", null, "", "", xNode);
                            instanceNode.setAttr("AxisX", lsi.AxisX);
                            instanceNode.setAttr("AxisY", lsi.AxisY);
                            instanceNode.setAttr("AxisZ", lsi.AxisZ);
                            instanceNode.setAttr("PrimaryAxis", lsi.PrimaryAxis);
                            instanceNode.setAttr("SecondaryAxis", lsi.SecondaryAxis);
                            instanceNode.setAttr("INRIndex", lsi.GetINRindex());
                            instanceNode.setAttr("IgnoreGeometry", lsi.IgnoreGeometry);
                            instanceNode.setAttr("ImmerseMediaName", lsi.ImmerseMediaName);
                            instanceNode.setAttr("ImmerseMediaNameINR", lsi.ImmerseMediaNameINR);
                            instanceNode.setAttr("Name", lsi.Name);
                            instanceNode.setAttr("SourceID", lsi.SourceID ?? "");
                            instanceNode.setAttr("Type", lsi.Type.ToString());

                            foreach (LightSourceRaysetFileProperty rayset in lsi.RaysetFiles)
                            {
                                XMLNode raysetNode = new XMLNode("RaysetData", null, "", "", instanceNode);
                                raysetNode.setAttr("Filename", rayset.Filename);
                                raysetNode.setAttr("Status", rayset.Status);
                                raysetNode.setAttr("Wavelength", rayset.Wavelength);

                            }
                        }
                    }
                }
            }

            foreach (Node subNode in node.Nodes)
            {
                saveGeometry(subNode, xNode, nodeTag, widget);
            }
        }


    }
}
