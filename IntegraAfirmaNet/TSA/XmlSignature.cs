namespace IntegraAfirmaNet.TSA
{
    /// <summary>
    /// Constantes para uso dentro del SignatureFramework
    /// </summary>
    class XmlSignatureConstants
    {

        public class ElementNames
        {
            internal const string KeyInfo = "KeyInfo";
            internal const string KeyName = "KeyName";
            internal const string Signature = "Signature";

            internal const string CanonicalizationMethod = "CanonicalizationMethod";
            internal const string HMACOutputLength = "HMACOutputLength";
            internal const string Reference = "Reference";
            internal const string SignatureMethod = "SignatureMethod";
            internal const string SignedInfo = "SignedInfo";
            internal const string Transform = "Transform";
            internal const string Transforms = "Transforms";
            internal const string DigestMethod = "DigestMethod";
            internal const string DigestValue = "DigestValue";
            internal const string SignatureValue = "SignatureValue";
            internal const string Object = "Object";
        }

        internal class AttributeNames
        {
            internal const string Algorithm = "Algorithm";
            internal const string Id = "Id";
            internal const string URI = "URI";
            internal const string Type = "Type";
        }

        public const string NamespaceURI = "http://www.w3.org/2000/09/xmldsig#";
        public const string Prefix = "ds";

        public const string XmlDsigExcC14NTransformUrl = "http://www.w3.org/2001/10/xml-exc-c14n#";
        public const string XmlDsigExcC14NWithCommentsTransformUrl = XmlDsigExcC14NTransformUrl+"WithComments";
        public const string XmlDsigC14NTransformUrl = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315#";
        public const string XmlDsigC14NWithCommentsTransformUrl = XmlDsigC14NTransformUrl + "WithComments";
        public const string XmlDsigSHA1Url = NamespaceURI + "sha1";

    }
}
