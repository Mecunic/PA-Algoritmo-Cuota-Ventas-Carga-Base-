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
}
