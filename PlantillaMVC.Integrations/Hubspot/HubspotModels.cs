using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaMVC.Integrations.Hubspot
{
    public class HubspotDealModel
    {
        public long? Id { set; get; }

        public long? OwnerId { set; get; }
        public string Dealname { set; get; }

        public double? Amount { set; get; }

        public string Stage { set; get; }

        public string CloseDate { set; get; }

        public string Pipeline { set; get; }

        public string DealType { set; get; }

        public int RelatedCompanies { set; get; }

        public int RelatedContacts { set; get; }
    }
}
