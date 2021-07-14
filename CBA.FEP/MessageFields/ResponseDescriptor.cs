using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.FEP.MessageFields
{
    public class ResponseDescriptor
    {
        private const string responseDescriptions = @"<ResponseCodes>
		<add key='00' value='Approved by Financial Institution'></add>
		<add key='01' value='Refer to Financial Institution'></add>
		<add key='02' value='Refer to Financial Institution, Special Condition'></add>
		<add key='03' value='Invalid Merchant'></add>
		<add key='04' value='Pick-up card'></add>
		<add key='05' value='Do Not Honor'></add>
		<add key='06' value='Error'></add>
		<add key='07' value='Pick-Up Card, Special Condition'></add>
		<add key='08' value='Honor with Identification'></add>
		<add key='09' value='Request in Progress'></add>
		<add key='10' value='Approved by Financial Institution, Partial'></add>
		<add key='11' value='Approved by Financial Institution, VIP'></add>
		<add key='12' value='Invalid Transaction'></add>
		<add key='13' value='Invalid Amount'></add>
		<add key='14' value='Invalid Card Number'></add>
		<add key='15' value='No Such Financial Institution'></add>
		<add key='16' value='Approved by Financial Institution, Update Track 3'></add>
		<add key='17' value='Customer Cancellation'></add>
		<add key='18' value='Customer Dispute'></add>
		<add key='19' value='Re-enter Transaction'></add>
		<add key='20' value='Invalid Response from Financial Institution'></add>
		<add key='21' value='No Action Taken by Financial Institution'></add>
		<add key='22' value='Suspected Malfunction'></add>
		<add key='23' value='Unacceptable Transaction Fee'></add>
		<add key='24' value='File Update not Supported'></add>
		<add key='25' value='Unable to Locate Record'></add>
		<add key='26' value='Duplicate Record'></add>
		<add key='27' value='File Update Field Edit Error'></add>
		<add key='28' value='File Update File Locked'></add>
		<add key='29' value='File Update Failed'></add>
		<add key='30' value='Format Error'></add>
		<add key='31' value='Bank Not Supported'></add>
		<add key='32' value='Completed Partially by Financial Institution'></add>
		<add key='33' value='Expired Card, Pick-Up'></add>
		<add key='34' value='Suspected Fraud, Pick-Up'></add>
		<add key='35' value='Contact Acquirer, Pick-Up'></add>
		<add key='36' value='Restricted Card, Pick-Up'></add>
		<add key='37' value='Call Acquirer Security, Pick-Up'></add>
		<add key='38' value='PIN Tries Exceeded, Pick-Up'></add>
		<add key='39' value='No Credit Account'></add>
		<add key='40' value='Function not Supported'></add>
		<add key='41' value='Lost Card, Pick-Up'></add>
		<add key='42' value='No Universal Account'></add>
		<add key='43' value='Stolen Card, Pick-Up'></add>
		<add key='44' value='No Investment Account'></add>
		<add key='51' value='Insufficient Funds'></add>
		<add key='52' value='No Check Account'></add>
		<add key='53' value='No Savings Account'></add>
		<add key='54' value='Expired Card'></add>
		<add key='55' value='Incorrect PIN'></add>
		<add key='56' value='No Card Record'></add>
		<add key='57' value='Transaction not Permitted to Cardholder'></add>
		<add key='58' value='Transaction not Permitted on Terminal'></add>
		<add key='59' value='Suspected Fraud'></add>
		<add key='60' value='Contact Acquirer'></add>
		<add key='61' value='Exceeds Withdrawal Limit'></add>
		<add key='62' value='Restricted Card'></add>
		<add key='63' value='Security Violation'></add>
		<add key='64' value='Original Amount Incorrect'></add>
		<add key='65' value='Exceeds withdrawal frequency'></add>
		<add key='66' value='Call Acquirer Security'></add>
		<add key='67' value='Hard Capture'></add>
		<add key='68' value='Response Received Too Late'></add>
		<add key='75' value='PIN tries exceeded'></add>
		<add key='76' value='Reserved for Future Postilion Use'></add>
		<add key='77' value='Intervene, Bank Approval Required'></add>
		<add key='78' value='Intervene, Bank Approval Required for Partial Amount'></add>
		<add key='90' value='Cut-off in Progress'></add>
		<add key='91' value='Issuer or Switch Inoperative'></add>
		<add key='92' value='Routing Error'></add>
		<add key='93' value='Violation of law'></add>
		<add key='94' value='Duplicate Transaction'></add>
		<add key='95' value='Reconcile Error'></add>
		<add key='96' value='System Malfunction'></add>
		<add key='98' value='Exceeds Cash Limit'></add>
	</ResponseCodes>";

        public static string GetResponseDescription(string responseCode)
        {
            string result = string.Empty;
            StringReader sReader = new StringReader(responseDescriptions);
            DataSet ds = new DataSet();
            try
            {
                ds.ReadXml(sReader);

                if (ds.Tables.Count > 0)
                {
                    DataTable dtMessage = ds.Tables["add"];
                    if (dtMessage.Rows.Count > 0)
                    {
                        DataRow[] results = dtMessage.Select(string.Format("key='{0}'", responseCode));
                        if (results.Length > 0)
                        {
                            result = results[0]["value"].ToString();
                        }
                        else
                        {
                            result = "Unknown Error";
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

            return result;
        }
    }
}
