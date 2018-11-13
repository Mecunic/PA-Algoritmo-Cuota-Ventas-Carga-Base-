using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HubSpot.NET.Api.Contact.Dto;
using HubSpot.NET.Api.Deal.Dto;
using HubSpot.NET.Api.Engagement.Dto;
using HubSpot.NET.Core;
using Newtonsoft.Json;
using PlantillaMVC.Integrations.Hubspot;
using RestSharp;

namespace PlantillaMVC.Integrations
{
    public interface IHubspotService
    {
        object CreateContact();

        List<HubspotDealModel> ReadDeals();

        HubspotDealsResult ReadDeals2();


    }
    public class HubspotService : IHubspotService
    {
        HubSpotApi apiService;

        public HubspotService()
        {
            apiService = new HubSpotApi(System.Configuration.ConfigurationManager.AppSettings["HubspotApiKey"]);
        }

        public object CreateContact()
        {
            // Create a contact
            var contact = apiService.Contact.Create(new ContactHubSpotModel()
            {
                Email = "john@gmail.com",
                FirstName = "John",
                LastName = "Smith",
                Phone = "00000 000000",
                Company = "Test Company"
            });

            return contact;
        }


        public List<HubspotDealModel> ReadDeals()
        {
            //https://api.hubapi.com/deals/v1/deal/paged?hapikey=<apikey>&includeAssociations=true&properties=dealname&properties=linea_de_negocio&properties=dealtype

            List<HubspotDealModel> list = new List<HubspotDealModel>();
            var dealList = apiService.Deal.List<HubspotDealModel>(true,
                    new ListRequestOptions(250) { PropertiesToInclude = new List<string> { "hubspot_owner_id", "dealname", "amount", "closedate", "dealstage", "pipeline", "dealtype", "linea_de_negocio" } });

            foreach (var deal in dealList.Deals)
            {
                DealHubSpotAssociations associations = deal.Associations;
                int RelatedCompanies = associations.AssociatedCompany!=null?associations.AssociatedCompany.Length:0;
                int RelatedContacts = associations.AssociatedContacts!=null?associations.AssociatedContacts.Length:0;
                list.Add(new HubspotDealModel()
                {
                    Id = deal.Id,
                    OwnerId = deal.OwnerId,
                    Name = deal.Name,
                    Amount = deal.Amount,
                    CloseDate = deal.CloseDate,
                    Stage = deal.Stage,
                    Pipeline = deal.Pipeline,
                    DealType = deal.DealType,
                    linea_de_negocio = deal.linea_de_negocio,
                    RelatedCompanies = RelatedCompanies,
                    RelatedContacts = RelatedContacts
                });
            }

            return list;
        }

        public HubspotDealsResult ReadDeals2()
        {
            var client = new RestClient("https://api.hubapi.com");

            var request = new RestRequest("/deals/v1/deal/paged", Method.GET);
            request.AddParameter("hapikey", "bdb3a514-f38f-466d-a1cb-94fd69a76a84");
            request.AddParameter("includeAssociations", true);
            request.AddParameter("properties", "hubspot_owner_id");
            request.AddParameter("properties", "amount");
            request.AddParameter("properties", "closedate");
            request.AddParameter("properties", "dealstage");
            request.AddParameter("properties", "dealname");
            request.AddParameter("properties", "linea_de_negocio");
            request.AddParameter("properties", "dealtype");

            IRestResponse response = client.Execute(request);
            HubspotDealsResult result = JsonConvert.DeserializeObject<HubspotDealsResult>(response.Content);
            return result;
        }
    }
}
