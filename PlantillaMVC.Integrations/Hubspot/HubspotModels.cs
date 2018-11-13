using HubSpot.NET.Api.Deal.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaMVC.Integrations.Hubspot
{
    public class HubspotDealModel : DealHubSpotModel
    {
        
        public string linea_de_negocio { set; get; }
        //public string associations { set; get; }
        

        public int RelatedCompanies { set; get; }

        public int RelatedContacts { set; get; }
    }

    public class HubspotDealsResult
    {
        public long PortalId { set; get; }

        public long DealId { set; get; }
        public List<HubspotDeal> Deals { set; get; }
    }

    public class HubspotDeal
    {
        public List<HubspotDataEntity> Properties { set; get; }

        //public string Associations { set; get; }
    }

    public class HubspotDataEntity
    {
        //public HubspotDataEntityProp Dealname { get; set; }
        //public HubspotDataEntityProp Amount { get; set; }
        //public HubspotDataEntityProp linea_de_negocio { get; set; }
    }

    public class HubspotDataEntityProp
    {
        public string Property { get; set; }

        public object Value { get; set; }

        public object Name { get; set; }
    }
}
