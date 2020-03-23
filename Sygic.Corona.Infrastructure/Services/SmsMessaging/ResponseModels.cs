namespace Sygic.Corona.Infrastructure.Services.SmsMessaging.Models
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2003/05/soap-envelope")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2003/05/soap-envelope", IsNullable = false)]
    public partial class Envelope
    {

        private EnvelopeBody bodyField;

        /// <remarks/>
        public EnvelopeBody Body
        {
            get
            {
                return this.bodyField;
            }
            set
            {
                this.bodyField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2003/05/soap-envelope")]
    public partial class EnvelopeBody
    {

        private SendMessageResponse sendMessageResponseField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schema.minv.sk/IP/ESISPZ-SMS/SmsEXT-v1")]
        public SendMessageResponse SendMessageResponse
        {
            get
            {
                return this.sendMessageResponseField;
            }
            set
            {
                this.sendMessageResponseField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schema.minv.sk/IP/ESISPZ-SMS/SmsEXT-v1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schema.minv.sk/IP/ESISPZ-SMS/SmsEXT-v1", IsNullable = false)]
    public partial class SendMessageResponse
    {

        private SendMessageResponseSendMessageResult sendMessageResultField;

        /// <remarks/>
        public SendMessageResponseSendMessageResult SendMessageResult
        {
            get
            {
                return this.sendMessageResultField;
            }
            set
            {
                this.sendMessageResultField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schema.minv.sk/IP/ESISPZ-SMS/SmsEXT-v1")]
    public partial class SendMessageResponseSendMessageResult
    {

        private string errMsgField;

        private string messageIdField;

        private string resultStatusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/SmsExtService.InterfaceDef")]
        public string ErrMsg
        {
            get
            {
                return this.errMsgField;
            }
            set
            {
                this.errMsgField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/SmsExtService.InterfaceDef")]
        public string MessageId
        {
            get
            {
                return this.messageIdField;
            }
            set
            {
                this.messageIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/SmsExtService.InterfaceDef")]
        public string ResultStatus
        {
            get
            {
                return this.resultStatusField;
            }
            set
            {
                this.resultStatusField = value;
            }
        }
    }



}