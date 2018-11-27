using HubSpot.NET.Api.Deal.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
        [DataMember(Name = "associations")]
        public HubSpotAssociationsDeals AssociationsModel { get; set; }
    }
    public class HubSpotAssociationsDeals : DealHubSpotAssociations
    {
        [DataMember(Name = "associatedDealIds")]
        public long[] AssociatedDealIds { get; set; }
    }
    [DataContract]
    public class HubspotDealsResult
    {
        public long PortalId { set; get; }

        public long DealId { set; get; }
        [DataMember(Name = "deals")]
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
    [DataContract]
    public class Associations
    {
        [DataMember(Name = "associatedVids")]
        public List<long> AssociatedVids { get; set; }
        [DataMember(Name = "associatedCompanyIds")]
        public List<long> associatedCompanyIds { get; set; }
        [DataMember(Name = "associatedDealIds")]
        public List<long> AssociatedDealIds { get; set; }
    }
    [DataContract]
    public class Version
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "value")]
        public string Value { get; set; }
        [DataMember(Name = "timestamp")]
        public long Timestamp { get; set; }
        [DataMember(Name = "source")]
        public string source { get; set; }
        [DataMember(Name = "sourceVid")]
        public List<long> SourceVid { get; set; }
    }
    [DataContract]
    public class Property
    {
        [DataMember(Name = "value")]
        public string Value { get; set; }
        [DataMember(Name = "timestamp")]
        public string Timestamp { get; set; }
        [DataMember(Name = "source")]
        public string source { get; set; }
        [DataMember(Name = "sourceId")]
        public string SourceId { get; set; }
        [DataMember(Name = "versions")]
        public List<Version> Versions { get; set; }
    }
   
    [DataContract]
    public class Properties
    {
        [DataMember(Name = "dealname")]
        public Property Dealname { get; set; }
        [DataMember(Name = "amount")]
        public Property Amount { get; set; }
        [DataMember(Name = "hubspot_owner_id")]
        public Property HubspotOwnerId { get; set; }
        [DataMember(Name = "closedate")]
        public Property CloseDate { get; set; }
        [DataMember(Name = "linea_de_negocio")]
        public Property LineaDeNegocio { get; set; }
        [DataMember(Name = "dealstage")]
        public Property DealStage { get; set; }
        [DataMember(Name = "hs_object_id")]
        public Property HsObjectId { get; set; }
    }
    [DataContract]
    public class Deal
    {
        [DataMember(Name = "portalId")]
        public int PortalId { get; set; }
        [DataMember(Name = "dealId")]
        public int DealId { get; set; }
        [DataMember(Name = "isDeleted")]
        public bool IsDeleted { get; set; }
        [DataMember(Name = "associations")]
        public Associations Associations { get; set; }
        [DataMember(Name = "properties")]
        public Properties Properties { get; set; }
        [DataMember(Name = "imports")]
        public List<object> Imports { get; set; }
    }

    [DataContract]
    public class DealHubSpot
    {
        [DataMember(Name = "deals")]
        public List<Deal> Deals { get; set; }
        [DataMember(Name = "hasMore")]
        public bool HasMore { get; set; }
        [DataMember(Name = "offset")]
        public int Offset { get; set; }
    }
}
