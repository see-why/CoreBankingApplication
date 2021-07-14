using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Core
{
    public class CardTransaction : Entity
    {
        public virtual string CardHolderName { get; set; }
        public virtual string AccountNumber { get; set; }
        public virtual string AccountNumber2 { get; set; }
        public virtual string CardPan { set; get; }
        public virtual string MTI { get; set; }
        public virtual string STAN { set; get; }
        public virtual DateTime TransactionDate { set; get; }
        public virtual string TransactionType { set; get; }
        public virtual double Amount { set; get; }
        public virtual double Charge { get; set; }
        public virtual string ResponseCode { set; get; }
        public virtual string ResponseDescription { set; get; }
        public virtual bool IsReversed { get; set; }
        public virtual string OriginalDataElement { get; set; }      
    }
}
