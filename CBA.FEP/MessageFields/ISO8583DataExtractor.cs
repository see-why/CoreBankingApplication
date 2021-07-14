using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trx.Messaging.Iso8583;

namespace CBA.FEP.MessageFields
{
    public class ISO8583DataExtractor
    {
        public Iso8583Message IsoMessage { get; set; }

        public ISO8583DataExtractor(Iso8583Message isoMessage)
        {
            IsoMessage = isoMessage;
        }


        public long SystemTraceAuditNumber
        {
            get
            {
                if (IsoMessage.Fields.Contains(FieldNos.F11_Trace))
                {
                    return Convert.ToInt64(IsoMessage.Fields[FieldNos.F11_Trace].Value);
                }
                return 0;
            }
        }

        public DateTime TransactionDateTime
        {
            get
            {
                if (IsoMessage.Fields.Contains(FieldNos.F7_TransDateTime))
                {
                    return DateTime.ParseExact(string.Format("{0:yyyy}", DateTime.Today) + Convert.ToString(IsoMessage.Fields[FieldNos.F7_TransDateTime].Value), "yyyyMMddHHmmss", CultureInfo.CurrentCulture);
                }
                return DateTime.Now;
            }
        }
        public byte[] PIN
        {
            get
            {
                if (IsoMessage.Fields.Contains(FieldNos.F52_PinData))
                {
                    return (byte[])IsoMessage.Fields[FieldNos.F52_PinData].Value;
                }
                return new byte[48];

            }
        }
        public string CardAcceptorID
        {
            get
            {
                if (IsoMessage.Fields.Contains(FieldNos.F42_CardAcceptorIDCode))
                {
                    return Convert.ToString(IsoMessage.Fields[FieldNos.F42_CardAcceptorIDCode].Value);
                }
                return string.Empty;
            }
        }

        public string TerminalID
        {
            get
            {
                if (IsoMessage.Fields.Contains(FieldNos.F41_CardAcceptorTerminalCode))
                {
                    return Convert.ToString(IsoMessage.Fields[FieldNos.F41_CardAcceptorTerminalCode].Value);
                }
                return string.Empty;
            }
        }

        public long AcquiringInstitutionID
        {
            get
            {
                if (IsoMessage.Fields.Contains(FieldNos.F32_AcquiringInstitutionIDCode))
                {
                    return Convert.ToInt64(IsoMessage.Fields[FieldNos.F32_AcquiringInstitutionIDCode].Value);
                }
                return 0;
            }
        }

        public String CardPAN
        {
            get
            {
                if (IsoMessage.Fields.Contains(FieldNos.F2_PAN))
                {
                    return IsoMessage.Fields[FieldNos.F2_PAN].Value.ToString();
                }
                return string.Empty;
            }
        }

        public String ReversalOriginalDataElements
        {
            get
            {
                if (IsoMessage.Fields.Contains(FieldNos.F90_OriginalDataElements))
                {
                    return IsoMessage.Fields[FieldNos.F90_OriginalDataElements].Value.ToString();
                }
                return string.Empty;
            }
        }

        public double Amount
        {
            get
            {
                if (IsoMessage.Fields.Contains(FieldNos.F4_TransAmount))
                {
                    return Convert.ToDouble(IsoMessage.Fields[FieldNos.F4_TransAmount].Value);
                }
                return 0;
            }
        }

        public long Surcharge
        {
            get
            {
                if (IsoMessage.Fields.Contains(FieldNos.F28_Surcharge))
                {
                    long _surcharge = Convert.ToInt64(IsoMessage.Fields[FieldNos.F28_Surcharge].Value.ToString().Substring(1, IsoMessage.Fields[FieldNos.F28_Surcharge].Value.ToString().Length - 1));
                    return _surcharge * (IsoMessage.Fields[FieldNos.F28_Surcharge].Value.ToString().Substring(0, 1).Equals("C", StringComparison.CurrentCultureIgnoreCase) ? -1 : 1);
                }
                return 0;
            }
        }

        public DateTime CardExpiryDate
        {
            get
            {
                if (IsoMessage.Fields.Contains(FieldNos.F14_CardExpiryDate))
                {
                    string expDate = IsoMessage.Fields[FieldNos.F14_CardExpiryDate].Value.ToString();
                    return new DateTime(2000 + Convert.ToInt32(expDate.Substring(0, 2)), Convert.ToInt32(expDate.Substring(2, 2)), 1);
                }
                return DateTime.MinValue;
            }
        }

        public String SourceAccountType
        {
            get
            {
                if (IsoMessage.Fields.Contains(FieldNos.F3_ProcCode))
                {
                    return IsoMessage.Fields[FieldNos.F3_ProcCode].Value.ToString().Substring(2, 2);
                }
                return string.Empty;
            }
        }
        public String SourceAccount
        {
            get
            {
                if (IsoMessage.Fields.Contains(FieldNos.F102_Account1))
                {
                    return IsoMessage.Fields[FieldNos.F102_Account1].Value.ToString();
                }
                return string.Empty;
            }
        }
        public String DestinationAccountType
        {
            get
            {
                if (IsoMessage.Fields.Contains(FieldNos.F3_ProcCode))
                {
                    return IsoMessage.Fields[FieldNos.F3_ProcCode].Value.ToString().Substring(4, 2);
                }
                return string.Empty;
            }
        }
        public String DestinationAccount
        {
            get
            {
                if (IsoMessage.Fields.Contains(FieldNos.F103_Account2))
                {
                    return IsoMessage.Fields[FieldNos.F103_Account2].Value.ToString();
                }
                return string.Empty;
            }
        }
        public String TransactionType
        {
            get
            {
                if (IsoMessage.Fields.Contains(FieldNos.F3_ProcCode))
                {
                    return IsoMessage.Fields[FieldNos.F3_ProcCode].Value.ToString().Substring(0, 2);
                }
                return string.Empty;
            }
        }

        public long FowardingInstitutionID
        {
            get
            {
                if (IsoMessage.Fields.Contains(FieldNos.F33_ForwardingInstitutionIDCode))
                {
                    return Convert.ToInt64(IsoMessage.Fields[FieldNos.F33_ForwardingInstitutionIDCode].Value);
                }
                return 0;
            }
        }

        public string OriginalDataElements
        {
            get
            {
                return string.Format("{0:0000}{1:000000}{2:MMddHHmmss}{3:00000000000}{4:00000000000}", IsoMessage.MessageTypeIdentifier, SystemTraceAuditNumber, TransactionDateTime, AcquiringInstitutionID, FowardingInstitutionID);
            }
        }

        public string CardAcceptorNameLocation
        {
            get
            {
                if (IsoMessage.Fields.Contains(FieldNos.F43_CardAcceptorNameLocation))
                {
                    return IsoMessage.Fields[FieldNos.F43_CardAcceptorNameLocation].Value.ToString();
                }
                return string.Empty;
            }
        }

        public string ResponseCode
        {
            get
            {
                if (IsoMessage.Fields.Contains(FieldNos.F39_ResponseCode))
                {
                    return IsoMessage.Fields[FieldNos.F39_ResponseCode].Value.ToString();
                }
                return string.Empty;
            }
        }

        public string ResponseDescription
        {
            get
            {
                if (IsoMessage.Fields.Contains(FieldNos.F39_ResponseCode))
                {
                    return ResponseDescriptor.GetResponseDescription(IsoMessage.Fields[FieldNos.F39_ResponseCode].Value.ToString());
                }
                return string.Empty;
            }
        }

        public string TransactionID
        {
            get
            {
                if (IsoMessage.Fields.Contains(FieldNos.F37_RetrievalReference))
                {
                    return IsoMessage.Fields[FieldNos.F37_RetrievalReference].Value.ToString();
                }
                return string.Empty;
            }
        }

        public string MaskedPAN
        {
            get
            {
                string mask = "";
                if (!string.IsNullOrEmpty(CardPAN))
                {
                    char[] charArray = CardPAN.ToCharArray();
                    for (int i = 0; i < charArray.Length; i++)
                    {
                        mask += i > 5 && i < charArray.Length - 6 ? "*" : charArray[i].ToString();
                    }
                }
                return mask;
            }
        }
    }
}
