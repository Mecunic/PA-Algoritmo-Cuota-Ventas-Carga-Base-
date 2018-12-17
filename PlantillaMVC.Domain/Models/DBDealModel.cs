﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaMVC.Domain.Models
{
    public class DBDealModel
    {
        public int ProcesoDetalleId { set; get; }
        public int DealId { set; get; }
        public string DealName { set; get; }
        public string CompanyRFC { set; get; }
        public string CompanyName { set; get; }

        public string ContactName { set; get; }

        public decimal Amount { set; get; }
        public string Stage { set; get; }

        public string ProductLine { set; get; }

        public string OwnerName { set; get; }
        

    }
}
