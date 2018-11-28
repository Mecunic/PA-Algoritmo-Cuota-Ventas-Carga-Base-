using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlantillaMVC.Domain.Models;
using System.Data.SqlClient;
using System.Data;

namespace PlantillaMVC.Domain.Services
{
    public interface IDBService
    {
        void CreateDeal(DBDealModel deal);
    }

    public class DBService : IDBService
    {
        System.Data.SqlClient.SqlConnection cnn;
        public DBService()
        {
            string connetionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            cnn = new System.Data.SqlClient.SqlConnection(connetionString);
            cnn.Open();
        }

        public void CreateDeal(DBDealModel deal)
        {
            string sql = "[dbo].[SP_HS_INSERT_DEAL]";
            SqlCommand command = new SqlCommand(sql, cnn);
            SqlDataReader rdr = null;
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@procesoId", SqlDbType.Int).Value = 1;
                command.Parameters.Add("@dealId", SqlDbType.Int).Value = deal.DealId;
                command.Parameters.Add("@dealName", SqlDbType.VarChar).Value = deal.DealName;
                command.Parameters.Add("@domainName", SqlDbType.VarChar).Value = deal.CompanyDomain;
                command.Parameters.Add("@amount", SqlDbType.Decimal).Value = deal.Amount;
                command.Parameters.Add("@contact", SqlDbType.VarChar).Value = deal.ContactName;
                command.Parameters.Add("@companyName", SqlDbType.VarChar).Value = deal.CompanyName;
                command.Parameters.Add("@owner", SqlDbType.VarChar).Value = string.Empty;
                command.Parameters.Add("@stage", SqlDbType.VarChar).Value = deal.Stage;
                command.Parameters.Add("@productLine", SqlDbType.VarChar).Value = string.Empty;
                rdr = command.ExecuteReader();
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
        }
    }
}
