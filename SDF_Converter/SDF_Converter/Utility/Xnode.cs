using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace xnode
{
    //=======================================================================
    // Delegates for callback's for elements
    // used for reciving notifications/callbacks on the xml parsing.

    public delegate void OnElementStartD(String name, String ns, Int32 numAttribs, Array attribs);
    public delegate void OnElementEndD(String name);
    public delegate void OnElementDataD(String name, String CData);

    //=======================================================================
    public class XMLNode : MarshalByRefObject
    {
        int mCDataLength;
        XMLNode mParent;
        List<XMLNode> mNodes;
        Dictionary<string, string> mAttributes;
        string mNS;
        string mTag;
        string mTag2;
        string mText;
        bool isComment = false;

        //===================================================================
        public bool UseCDATA { get; set; }

        //===================================================================
        public XMLNode(string tag = "",
                        Dictionary<string, string> attrib = null,
                        string text = "",
                        string ns = "",
                        XMLNode parent = null)
        {
            UseCDATA = false;
            mParent = parent;

            if (attrib != null)
            {
                mAttributes = new Dictionary<string, string>(attrib);
            }
            else
            {
                mAttributes = new Dictionary<string, string>();
            }

            mCDataLength = 0;
            mNS = ns;
            mTag = tag;
            mTag2 = tag;
            mText = text;

            if (tag == "!--")
            {
                isComment = true;
            }

            mNodes = new List<XMLNode>();

            if (parent != null)
            {
                parent.addNode(this);
            }

            // Namespace may be inside tag
            if (tag.Length > 0)
            {
                // Right trim
                tag = tag.TrimEnd();
                mTag = tag;

                if (tag.IndexOf(':') >= 0)
                {
                    string[] names = tag.Split(new char[] { ':' });
                    mNS = names[0];
                    mTag2 = names[1];
                }
                else
                {
                    mTag2 = tag;
                }
            }
        }

        //===================================================================
        // User-defined conversion from Digit to double 
        public static implicit operator string(XMLNode node)
        {
            return node.toxml();
        }

        //===================================================================
        public XMLNode this[int nIndex]
        {
            get { return mNodes[nIndex]; }
        }

        //===================================================================
        public object this[string attr]
        {
            get
            {
                string outVal = "";

                mAttributes.TryGetValue(attr, out outVal);

                return outVal;
            }

            set { mAttributes[attr] = value.ToString(); }
        }

        //===================================================================
        public XMLNode iter()
        {
            XMLNode node = null;

            return node;
        }

        //===================================================================
        public int cmp(XMLNode other)
        {
            return 0;
        }

        //===================================================================
        public XMLNode getCopy(XMLNode parent)
        {
            XMLNode node = null;

            node = new XMLNode(getTag(), mAttributes, mText, mNS, parent);

            for (int ii = 0; ii < mNodes.Count; ii++)
            {
                mNodes[ii].getCopy(node);
            }

            return node;
        }

        //===================================================================
        public XMLNode getParent()
        {
            return mParent;
        }

        //===================================================================
        public void copyAttrs(XMLNode node)
        {
            try
            {
                foreach (var kvp in node.mAttributes)
                {
                    setAttr(kvp.Key, kvp.Value);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("AUXN1: " + e.Message);
            }
        }

        //===================================================================
        public void setAttr(string attr, string val)
        {
            mAttributes[attr] = val;
        }

        //===================================================================
        public void setAttr(string attr, double val)
        {
            mAttributes[attr] = val.ToString();
        }

        //===================================================================
        public void setAttr(string attr, float val)
        {
            mAttributes[attr] = val.ToString();
        }

        //===================================================================
        public void setAttr(string attr, int val)
        {
            mAttributes[attr] = val.ToString();
        }

        //===================================================================
        public void setAttr(string attr, bool val)
        {
            mAttributes[attr] = val.ToString();
        }

        //===================================================================
        public string getAttr(string attr, string defVal = "", bool bRecurse = false)
        {
            string ret = defVal;

            if (mAttributes.ContainsKey(attr))
            {
                ret = mAttributes[attr];
            }
            return ret;
        }

        //===================================================================
        public double getAttr(string attr, double defVal = 0, bool bRecurse = false)
        {
            double ret = defVal;

            if (mAttributes.ContainsKey(attr))
            {
                bool bTryParse = double.TryParse(mAttributes[attr], out ret);

                if (bTryParse)
                {
                    // Do nothing
                }
                else
                {
                    ret = defVal;
                }
            }
            return ret;
        }

        //===================================================================
        public float getAttr(string attr, float defVal = 0.0f, bool bRecurse = false)
        {
            float ret = defVal;

            if (mAttributes.ContainsKey(attr))
            {
                bool bTryParse = float.TryParse(mAttributes[attr], out ret);

                if (bTryParse)
                {
                    // Do nothing

                }
                else
                {
                    ret = defVal;
                }
            }
            return ret;
        }

        //===================================================================
        public string getAttr(string attr, string defVal = "")
        {
            string ret = defVal;

            if (mAttributes.ContainsKey(attr))
            {
                ret = mAttributes[attr];
            }

            return ret;
        }

        //===================================================================
        public int getAttr(string attr, int defVal = 0, bool bRecurse = false)
        {
            int ret = defVal;

            if (mAttributes.ContainsKey(attr))
            {
                bool bTryParse = int.TryParse(mAttributes[attr], out ret);

                if (bTryParse)
                {
                    // Do nothing
                }
                else
                {
                    ret = defVal;
                }
            }
            return ret;
        }

        //===================================================================
        public uint getAttr(string attr, uint defVal = 0, bool bRecurse = false)
        {
            uint ret = defVal;

            if (mAttributes.ContainsKey(attr))
            {
                bool bTryParse = uint.TryParse(mAttributes[attr], out ret);

                if (bTryParse)
                {
                    // Do nothing
                }
                else
                {
                    ret = defVal;
                }
            }
            return ret;
        }

        //===================================================================
        public ulong getAttr(string attr, ulong defVal = 0, bool bRecurse = false)
        {
            ulong ret = defVal;

            if (mAttributes.ContainsKey(attr))
            {
                bool bTryParse = ulong.TryParse(mAttributes[attr], out ret);

                if (bTryParse)
                {
                    // Do nothing
                }
                else
                {
                    ret = defVal;
                }
            }
            return ret;
        }

        //===================================================================
        public bool getAttr(string attr, bool defVal = false, bool bRecurse = false)
        {
            bool ret = defVal;

            if (mAttributes.ContainsKey(attr))
            {
                string val = mAttributes[attr];

                if (val == "0" || val == "1")
                {
                    ret = (val == "1");
                }
                else
                {
                    bool bTryParse = bool.TryParse(mAttributes[attr], out ret);

                    if (bTryParse)
                    {
                        // Do nothing
                    }
                    else
                    {
                        ret = defVal;
                    }
                }
            }
            return ret;
        }

        //===================================================================
        public bool hasAttr(string attr)
        {
            return mAttributes.ContainsKey(attr);
        }

        //===================================================================
        public void setTag(string tag, string ns = "")
        {
            mTag = tag;

            mNS = ns;

            if (tag.IndexOf(":") >= 0)
            {
                string[] nameList = tag.Split(new char[] { ':' });

                mNS = nameList[0];

                mTag2 = nameList[1];
            }
            else
            {
                mTag2 = mTag;
            }
        }

        //===================================================================
        public string getTag(bool bFull = false)
        {
            string tag = "";

            if (bFull)
            {
                tag = mTag;
            }
            else
            {
                tag = mTag2;
            }
            return (tag);
        }

        //===================================================================
        public string getNamespace()
        {
            return (mNS);
        }

        //===================================================================
        public void setNamespace(string ns)
        {
            mNS = ns;
        }

        //===================================================================
        public void addText(string text)
        {
            mText += text.Trim();
        }

        //===================================================================
        public static string base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.Default.GetBytes(plainText);

            return System.Convert.ToBase64String(plainTextBytes);
        }

        //===================================================================
        public void setText(string text, bool bEncode = false)
        {
            if (bEncode == true)
            {
                setAttr("Encoded", "1");
            }
            else
            {
                if (mAttributes.ContainsKey("Encoded"))
                {
                    mAttributes.Remove("Encoded");
                }
            }
            mText = text;
        }

        //===================================================================
        public void setCData(byte[] data)
        {
            string mimeStr = System.Convert.ToBase64String(data, 0, data.Length, Base64FormattingOptions.InsertLineBreaks);

            mCDataLength = mimeStr.Length;

            mText = mimeStr;

            setAttr("Encoded", "1");
        }

        //===================================================================
        public string toxml(int level = 0, bool bPretty = false, bool bNamespace = false, bool bComment = false, bool bDecl = false, char cAttr = '"')
        {
            string s = "";

            if (isComment)
            {
                s = string.Format("<!-- {0} -->", mText);

                return s;
            }

            if (mParent == null && bDecl)
            {
                s += "<?xml version=" + cAttr + "1.0" + cAttr + "encoding=" + cAttr + "UTF-8" + cAttr + "?>\r\n";
            }

            if (bPretty)
            {
                for (int ii = 0; ii < level; ii++)
                {
                    s += "  ";
                }
            }

            // Print tag
            if (bNamespace && mNS != "")
            {
                s += "<" + mNS + ":" + getTag();
            }
            else
            {
                s += "<" + getTag();
            }

            // Print attributes
            foreach (var kvp in mAttributes)
            {
                string value = kvp.Value;

                value = value.Replace("&", "&amp;");

                s += " " + kvp.Key + "=" + cAttr + value + cAttr;
            }

            if (mNodes.Count == 0 && mText.Length == 0)
            {
                // Print end tag
                if (bPretty)
                {
                    s += " />\r\n";
                }
                else
                {
                    s += " />";
                }
            }
            else
            {
                // Print child nodes
                s += ">";

                if (mText.Length > 0)
                {
                    //<![CDATA[  ]]>
                    bool useCDATA = false;

                    if (UseCDATA || mCDataLength > 0 || mText.Contains(">") || mText.Contains("<"))
                    {
                        useCDATA = true;
                    }

                    if (useCDATA)
                    {
                        s += "<![CDATA[";
                    }

                    s += mText;

                    if (useCDATA)
                    {
                        s += "]]>";
                    }
                }

                if (bPretty && mNodes.Count > 0)
                {
                    s += "\r\n";
                }

                // Add child nodes
                foreach (XMLNode sub in mNodes)
                {
                    s += sub.toxml(level + 1, bPretty, bNamespace, bComment);
                }

                // endShow
                if (bPretty && mNodes.Count > 0)
                {
                    for (int ii = 0; ii < level; ii++)
                    {
                        s += "  ";
                    }
                }

                if (bNamespace && mNS.Length > 0)
                {
                    s += "</" + getNamespace() + ":" + getTag() + ">";
                }
                else
                {
                    s += "</" + getTag() + ">";
                }

                if (bPretty)
                {
                    s += "\r\n";
                }
            }
            return s;
        }

        //===================================================================
        public List<XMLNode> getElementsByTagName(string tag)
        {
            List<XMLNode> ret = new List<XMLNode>();

            foreach (XMLNode node in mNodes)
            {
                if (node.getTag() == tag)
                {
                    ret.Add(node);
                }
            }
            return ret;
        }

        //===================================================================
        public XMLNode getNode(string tag, bool nonNull = false)
        {
            XMLNode ret = null;

            foreach (XMLNode node in mNodes)
            {
                if (node.getTag() == tag)
                {
                    ret = node;

                    break;
                }
            }

            // Just to handle when we may want to not check for null...
            if (nonNull && ret == null)
            {
                ret = new XMLNode(tag);
            }
            return ret;
        }

        //===================================================================
        public string getText(bool bDecode = false)
        {
            string text = mText;

            if (bDecode || getAttr("Encoded", 0) != 0)
            {
                try
                {
                    byte[] binaryData = System.Convert.FromBase64String(text);

                    text = System.Text.Encoding.UTF8.GetString(binaryData);
                }
                catch (System.ArgumentNullException)
                {
                    System.Console.WriteLine("Base 64 string is null.");
                }
                catch (System.FormatException)
                {
                    System.Console.WriteLine("Base 64 string length is not " + "4 or is not an even multiple of 4.");
                }
            }

            return text;
        }

        //===================================================================
        public byte[] getData(bool bDecode = false)
        {
            byte[] binaryData = null;

            string text = mText;

            if (bDecode || getAttr("Encoded", 0) != 0)
            {
                try
                {
                    binaryData = System.Convert.FromBase64String(text);
                }
                catch (System.ArgumentNullException)
                {
                    System.Console.WriteLine("Base 64 string is null.");
                }
                catch (System.FormatException)
                {
                    System.Console.WriteLine("Base 64 string length is not " + "4 or is not an even multiple of 4.");
                }
            }
            return binaryData;
        }

        //===================================================================
        public void removeNode(XMLNode node)
        {
            mNodes.Remove(node);
        }

        //===================================================================
        public void orphan()
        {
            if (mParent != null)
            {
                mParent.removeNode(this);
            }

            mParent = null;
        }

        //===================================================================
        public void reparent(XMLNode parent)
        {
            if (mParent != null)
            {
                mParent.removeNode(this);
            }

            if (parent != null)
            {
                parent.addNode(this);
            }
            else
            {
                mParent = null;
            }
        }

        //===================================================================
        public void addNode(XMLNode node, XMLNode after = null)
        {
            if (after != null)
            {
                foreach (XMLNode iter in mNodes)
                {
                    if (iter == after)
                    {
                        mNodes.Insert(mNodes.IndexOf(iter), node);

                        break;
                    }
                }
            }
            else
            {
                mNodes.Add(node);
            }

            node.mParent = this;
        }

        //===================================================================
        public List<XMLNode> getNodeList(string tag = "")
        {
            List<XMLNode> ret = null;

            if (tag.Length > 0)
            {
                ret = getElementsByTagName(tag);
            }
            else
            {
                ret = mNodes;
            }

            return ret;
        }

        //===================================================================
        public Dictionary<string, string> getAttrList()
        {
            return mAttributes;
        }
    }

    /*
    //=======================================================================
    public class XMLParser : MarshalByRefObject
    {
        private XMLNode mRoot = null;

        private XMLNode mCurrent = null;

        private Stack mStack = null;

        private XmlTextReader reader = null;


        //===================================================================
        /// <summary>
        /// </summary>
        public bool DontDecode { get; set; }


        //===================================================================
        /// <summary>
        /// </summary>
        public XMLParser()
        {
            DontDecode = false;

            reader = null;

            mStack = new Stack();
        }


        //===================================================================
        /// <summary>
        /// </summary>
        public XMLNode ParseFile(String filename)
        {
            XMLNode ret = null;

            try
            {
                reader = new XmlTextReader(filename);

                ret = parse();

                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("AUXN2: " + e.Message);

                //Utility.Diagnostics.ErrorLogger.Instance.LogException(e);
            }

            return ret;
        }


        //===================================================================
        /// <summary>
        /// </summary>
        public XMLNode Parse(String xml)
        {
            XMLNode ret = null;

            try
            {
                reader = new XmlTextReader(new StringReader(xml));

                ret = parse();

                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("AUXN3: " + e.Message);

                //Utility.Diagnostics.ErrorLogger.Instance.LogException(e);
            }

            return ret;
        }


        //===================================================================
        // fill this node and then go to the next node
        private XMLNode startElement(string tag, string ns, Dictionary<string, string> attrs)
        {
            XMLNode node = new XMLNode(tag, attrs, "", ns, mCurrent);

            if (mRoot == null)
            {
                mRoot = node;
            }

            mCurrent = node;

            mStack.Push(node);

            return node;
        }


        //===================================================================
        // go back to the previous node
        private void endElement(string name)
        {
            if (name == mCurrent.getTag())
            {
                if (DontDecode == false)
                {
                    mCurrent.setText(mCurrent.getText());
                }

                mStack.Pop();

                if (mStack.Count > 0)
                {
                    mCurrent = (XMLNode)mStack.Peek();
                }
                else
                {
                    mCurrent = null;
                }
            }
        }


        //===================================================================
        // add the data to the current node
        void charData(string text, bool fromCDATA = false)
        {
            mCurrent.addText(text);

            mCurrent.UseCDATA = fromCDATA;
        }


        //===================================================================
        //returns 1 on success, 0 on failed
        private XMLNode parse()
        {
            mRoot = null;

            mCurrent = null;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Attribute:

                        mCurrent.setAttr(reader.Name, reader.Value);

                        break;

                    case XmlNodeType.Element:

                        Dictionary<string, string> attrs = new Dictionary<string, string>();

                        string tag = reader.Name;

                        string ns = reader.NamespaceURI;

                        bool bEndElement = false;

                        if (reader.IsEmptyElement)
                        {
                            bEndElement = true;
                        }

                        if (reader.HasAttributes)
                        {
                            for (int i = 0; i < reader.AttributeCount; i++)
                            {
                                reader.MoveToAttribute(i);

                                attrs[reader.Name] = reader.Value;
                            }
                        }

                        startElement(tag, ns, attrs);

                        if (bEndElement)
                        {
                            endElement(tag);
                        }

                        break;

                    case XmlNodeType.EndElement:

                        endElement(reader.Name);

                        break;

                    case XmlNodeType.Text:

                        charData(reader.Value);

                        break;

                    case XmlNodeType.CDATA:

                        charData(reader.Value, true);

                        break;
                }
            }

            return mRoot;
        }
    }*/
}
