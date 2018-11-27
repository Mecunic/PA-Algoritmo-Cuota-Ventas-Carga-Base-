using PlantillaMVC.Integrations.Hubspot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaMVC.Integrations.Mapper
{
    public class HubspotModelMapper : IMapper<DealHubSpot, DealListModel>
    {
        public DealListModel Map(DealHubSpot objToMap)
        {
            DealListModel listModel = new DealListModel {
                HasMore = objToMap.HasMore,
               
            };
            listModel.Deal = objToMap.Deals.Select(x => new DealModel {
                Amount = x.Properties.Amount == null ? null : (double?)Double.Parse(x.Properties.Amount.Value),
                CloseDate = x.Properties.CloseDate.Value,
                Name = x.Properties.Dealname.Value,
                Stage = x.Properties.DealStage.Value,
                Id = x.DealId,
                OwnerId = x.Properties.Amount == null ? null : (long?)Int64.Parse(x.Properties.HubspotOwnerId.Value)
            }).ToList();
            return listModel;
        }
    }
}
