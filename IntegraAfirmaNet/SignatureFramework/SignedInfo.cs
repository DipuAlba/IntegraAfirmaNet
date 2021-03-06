using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Security.Cryptography;
using System.Xml;

namespace IntegraAfirmaNet.SignatureFramework
{
    /// <summary>
    /// Clase que representa el objeto <code>SignedIndo</code>.
    /// </summary>
    public class SignedInfo :  ICollection, IEnumerable
    {
        private ArrayList references;
        private string c14nMethod;
        private string inclusiveNamespaces;
        private string id;
        private string signatureMethod;
        private string signatureLength;

        public SignedInfo()
        {
            references = new ArrayList();
            c14nMethod = XmlSignatureConstants.XmlDsigC14NTransformUrl;
        }

        public string CanonicalizationMethod
        {
            get { return c14nMethod; }
            set { c14nMethod = value; }
        }

        public string InclusiveNamespaces
        {
            get { return inclusiveNamespaces; }
            set { inclusiveNamespaces = value; }
        }

        public ArrayList References
        {
            get { return references; }
        }

        public string SignatureLength
        {
            get { return signatureLength; }
            set { signatureLength = value; }
        }

        public string SignatureMethod
        {
            get { return signatureMethod; }
            set { signatureMethod = value; }
        }

        public void AddReference(System.Security.Cryptography.Xml.Reference reference)
        {
            references.Add(reference);
        }

        public IEnumerator GetEnumerator()
        {
            return references.GetEnumerator();
        }

        /// <summary>
        /// Obtiene el elemento para SignedInfo precalculando la información relativa a los objetos <code>Reference</code>
        /// </summary>
        /// <returns></returns>
        public XmlElement GetXml()
        {
            if (signatureMethod == null)
                throw new CryptographicException("SignatureMethod");
            if (references.Count == 0)
                throw new CryptographicException("References empty");

            XmlDocument document = new XmlDocument();
            XmlElement xel = document.CreateElement(XmlSignatureConstants.Prefix + ":" + XmlSignatureConstants.ElementNames.SignedInfo, XmlSignatureConstants.NamespaceURI);
            if (id != null)
                xel.SetAttribute(XmlSignatureConstants.AttributeNames.Id, id);

            if (c14nMethod != null)
            {
                XmlElement c14n = document.CreateElement(XmlSignatureConstants.Prefix + ":" + XmlSignatureConstants.ElementNames.CanonicalizationMethod, XmlSignatureConstants.NamespaceURI);
                c14n.SetAttribute(XmlSignatureConstants.AttributeNames.Algorithm, c14nMethod);
                xel.AppendChild(c14n);
            }
            if (signatureMethod != null)
            {
                XmlElement sm = document.CreateElement(XmlSignatureConstants.Prefix + ":" + XmlSignatureConstants.ElementNames.SignatureMethod, XmlSignatureConstants.NamespaceURI);
                sm.SetAttribute(XmlSignatureConstants.AttributeNames.Algorithm, signatureMethod);
                if (signatureLength != null)
                {
                    XmlElement hmac = document.CreateElement(XmlSignatureConstants.Prefix + ":" + XmlSignatureConstants.ElementNames.HMACOutputLength, XmlSignatureConstants.NamespaceURI);
                    hmac.InnerText = signatureLength;
                    sm.AppendChild(hmac);
                }
                xel.AppendChild(sm);
            }

            foreach (System.Security.Cryptography.Xml.Reference r in references)
            {
                XmlNode xn = r.GetXml();
                XmlNode newNode = document.ImportNode(xn, true);
                xel.AppendChild(newNode);
            }

            return xel;
        }

        private string GetAttributeFromElement(XmlElement xel, string attribute, string element, string namespaceURI)
        {
            string result = null;
            XmlNodeList xnl = xel.GetElementsByTagName(element, namespaceURI);
            if ((xnl != null) && (xnl.Count > 0))
            {
                XmlAttribute xa = xnl[0].Attributes[attribute];
                if (xa != null)
                    result = xa.InnerText;
            }
            return result;
        }

        private string GetAttribute(XmlElement xel, string attribute)
        {
            XmlAttribute xa = xel.Attributes[attribute];
            return ((xa != null) ? xa.InnerText : null);
        }

        /// <summary>
        /// Carga y parsea un objeto SignedInfo desde un elemento proporcionado
        /// </summary>
        /// <param name="value">Elemento SignedInfo a parsear.</param>
        public void LoadXml(XmlElement value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if ((value.LocalName != XmlSignatureConstants.ElementNames.SignedInfo) || (value.NamespaceURI != XmlSignatureConstants.NamespaceURI))
                throw new CryptographicException();

            id = GetAttribute(value, XmlSignatureConstants.AttributeNames.Id);
            c14nMethod = GetAttributeFromElement(value, XmlSignatureConstants.AttributeNames.Algorithm, XmlSignatureConstants.ElementNames.CanonicalizationMethod, XmlSignatureConstants.NamespaceURI);

            XmlNodeList xnlCn = value.GetElementsByTagName(XmlSignatureConstants.ElementNames.CanonicalizationMethod, XmlSignatureConstants.NamespaceURI);
            if (xnlCn.Count > 0)
            {
                XmlNode xn = xnlCn[0];

                var items = ((XmlElement)xn).GetElementsByTagName("InclusiveNamespaces");

                if (items != null && items.Count > 0)
                {
                    inclusiveNamespaces = GetAttribute((XmlElement)items[0], "PrefixList");
                }                
            }

            signatureMethod = GetAttributeFromElement(value, XmlSignatureConstants.AttributeNames.Algorithm, XmlSignatureConstants.ElementNames.SignatureMethod, XmlSignatureConstants.NamespaceURI);
            XmlNodeList xnl = value.GetElementsByTagName(XmlSignatureConstants.ElementNames.Reference, XmlSignatureConstants.NamespaceURI);
            foreach (XmlNode xn in xnl)
            {
                System.Security.Cryptography.Xml.Reference r = new System.Security.Cryptography.Xml.Reference();
                r.LoadXml((XmlElement)xn);
                AddReference(r);
            }
        }

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Count
        {
            get{return References.Count;}
        }

        public bool IsSynchronized
        {
           get{return false;}
        }

        public object SyncRoot
        {
           get{return null;}
        }

        #endregion
    }
}
