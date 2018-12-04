using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HubSpot.NET.Api.Contact.Dto;
using HubSpot.NET.Api.Deal.Dto;
using HubSpot.NET.Api.Engagement.Dto;
using HubSpot.NET.Core;
using HubSpot.NET.Core.Interfaces;
using Newtonsoft.Json;
using PlantillaMVC.Integrations.Hubspot;
using RestSharp;

namespace PlantillaMVC.Integrations
{
    public interface IHubspotService
    {
        object CreateContact();

        List<HubspotDealModel> ReadDeals2();

        DealHubSpotResult ReadDeals(int limit, long offset);

        CompanyHubSpotResult GetCompanyById(long id);
        ContactHubSpotResult GetContactById(long id);

        string CreateTicketToCompany();

    }
    public class HubspotService : IHubspotService
    {
        private IHubSpotApi apiService;
        private string apiKey;
        private string apiKeyMetrolab;
        private RestClient client;
        public HubspotService()
        {
            apiService = new HubSpotApi(System.Configuration.ConfigurationManager.AppSettings["HubspotApiKey"]);
            apiKey = System.Configuration.ConfigurationManager.AppSettings["HubspotApiKey"];
            apiKeyMetrolab = System.Configuration.ConfigurationManager.AppSettings["HubspotApiKey_Metrolab"];
            client = new RestClient("https://api.hubapi.com");
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


        public List<HubspotDealModel> ReadDeals2()
        {
            //https://api.hubapi.com/deals/v1/deal/paged?hapikey=<apikey>&includeAssociations=true&properties=dealname&properties=linea_de_negocio&properties=dealtype

            List<DealHubSpotModel> list = new List<DealHubSpotModel>();
            var dealList = apiService.Deal.List<HubspotDealModel>(true);

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

            return null;
        }

        public DealHubSpotResult ReadDeals(int limit, long offset)
        {
            RestRequest request = new RestRequest("/deals/v1/deal/paged", Method.GET);
            request.AddParameter("hapikey", apiKey);
            request.AddParameter("includeAssociations", true);
            if (limit > 0) request.AddParameter("limit", limit);
            if (offset > 0) request.AddParameter("offset", offset);
            request.AddParameter("properties", "hubspot_owner_id");
            request.AddParameter("properties", "amount");
            request.AddParameter("properties", "closedate");
            request.AddParameter("properties", "dealstage");
            request.AddParameter("properties", "dealname");
            request.AddParameter("properties", "linea");
            request.AddParameter("properties", "dealtype");

            IRestResponse response = client.Execute(request);
            DealHubSpotResult result = JsonConvert.DeserializeObject<DealHubSpotResult>(response.Content);
            return result;
        }
        public CompanyHubSpotResult GetCompanyById(long id)
        {

            RestRequest request = new RestRequest("/companies/v2/companies/{id}", Method.GET);
            request.AddParameter("hapikey", apiKey);
            request.AddUrlSegment("id", id.ToString());

            IRestResponse response = client.Execute(request);

            CompanyHubSpotResult result = JsonConvert.DeserializeObject<CompanyHubSpotResult>(response.Content);

            return result;
        }
        public ContactHubSpotResult GetContactById(long id)
        {

            RestRequest request = new RestRequest("/contacts/v1/contact/vid/{id}/profile", Method.GET);
            request.AddParameter("hapikey", apiKey);
            request.AddUrlSegment("id", id.ToString());

            IRestResponse response = client.Execute(request);

            ContactHubSpotResult result = JsonConvert.DeserializeObject<ContactHubSpotResult>(response.Content);

            return result;



        }

        public string CreateTicketToCompany()
        {
            //https://developers.hubspot.com/docs/methods/crm-associations/crm-associations-overview
            //Ticket to company 	26
            
            List<TicketProperty> ticketProperties = new List<TicketProperty>();
            ticketProperties.Add(new TicketProperty() { Name = "subject", Value = "Ticket de prueba" });
            ticketProperties.Add(new TicketProperty() { Name = "content", Value = "Contenido del ticket de prueba" });
            ticketProperties.Add(new TicketProperty() { Name = "hs_pipeline", Value = "0" });
            ticketProperties.Add(new TicketProperty() { Name = "hs_pipeline_stage", Value = "1" });
            string jsonToSend = JsonConvert.SerializeObject(ticketProperties);

            var request = new RestRequest("/crm-objects/v1/objects/tickets?hapikey=" + apiKeyMetrolab);
            request.Method = Method.POST;
            request.AddHeader("Accept", "application/json");
            request.Parameters.Clear();
            request.AddParameter("application/json", jsonToSend, ParameterType.RequestBody);

            var response = client.Execute(request);

            TicketResponse ticketResponse = JsonConvert.DeserializeObject<TicketResponse>(response.Content);

            if (ticketResponse.ObjectId > 0)
            {
                Association association = new Association()
                {
                    FromObjectId = ticketResponse.ObjectId,
                    ToObjectId = 1060806363,
                    Category = "HUBSPOT_DEFINED",
                    DefinitionId = 26
                };
                jsonToSend = JsonConvert.SerializeObject(association);
                request = new RestRequest("/crm-associations/v1/associations?hapikey=" + apiKeyMetrolab);
                request.Method = Method.PUT;
                request.AddParameter("application/json", jsonToSend, ParameterType.RequestBody);
                request.AddHeader("Content-Type", "application/json");
                response = client.Put(request);
            }
            //response.StatusCode

            return response.Content;
        }
        
        //public int GetCompanyByRFC()
        //{

        //}

    }
}
