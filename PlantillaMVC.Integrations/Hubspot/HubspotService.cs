using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HubSpot.NET.Api.Contact.Dto;
using HubSpot.NET.Api.Deal.Dto;
using HubSpot.NET.Api.Engagement.Dto;
using HubSpot.NET.Core;
using PlantillaMVC.Integrations.Hubspot;

namespace PlantillaMVC.Integrations
{
    public interface IHubspotService
    {
        object CreateContact();

        List<HubspotDealModel> ReadDeals();


    }
    public class HubspotService : IHubspotService
    {
        HubSpotApi apiService;

        public HubspotService()
        {
            apiService = new HubSpotApi("3b609872-906e-42ae-92da-2e18ef841210");
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
            List<HubspotDealModel> list = new List<HubspotDealModel>();
            var dealList = apiService.Deal.List<DealHubSpotModel>(true,
                    new ListRequestOptions(250) { PropertiesToInclude = new List<string> { "hubspot_owner_id", "dealname", "amount", "associations", "closedate", "dealstage", "pipeline", "dealtype" } });

            foreach (var deal in dealList.Deals)
            {
                DealHubSpotAssociations associations = deal.Associations;
                int RelatedCompanies = associations.AssociatedCompany!=null?associations.AssociatedCompany.Length:0;
                int RelatedContacts = associations.AssociatedContacts!=null?associations.AssociatedContacts.Length:0;
                list.Add(new HubspotDealModel()
                {
                    Id = deal.Id,
                    OwnerId = deal.OwnerId,
                    Dealname = deal.Name,
                    Amount = deal.Amount,
                    CloseDate = deal.CloseDate,
                    Stage = deal.Stage,
                    Pipeline = deal.Pipeline,
                    DealType = deal.DealType,
                    RelatedCompanies = RelatedCompanies,
                    RelatedContacts = RelatedContacts
                });
            }

            return list;
        }
    }
}
