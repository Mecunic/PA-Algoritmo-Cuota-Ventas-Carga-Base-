﻿using System;
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

        void ClearDeals();

        DBProceso GetProcessInfo(string codigo);
        void ActualizarEstatusProceso(DBProceso procesoInfo);

        int CreateProcesoEjecucion(DBProcesoEjecucion detalle);

        void ActualizarProcesoEjecucion(DBProcesoEjecucion detalle);
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
                command.Parameters.Add("@procesoId", SqlDbType.Int).Value = deal.ProcesoDetalleId;
                command.Parameters.Add("@dealId", SqlDbType.Int).Value = deal.DealId;
                command.Parameters.Add("@dealName", SqlDbType.VarChar).Value = deal.DealName;
                command.Parameters.Add("@domainName", SqlDbType.VarChar).Value = deal.CompanyDomain;
                command.Parameters.Add("@amount", SqlDbType.Decimal).Value = deal.Amount;
                command.Parameters.Add("@contact", SqlDbType.VarChar).Value = deal.ContactName;
                command.Parameters.Add("@companyName", SqlDbType.VarChar).Value = deal.CompanyName;
                command.Parameters.Add("@owner", SqlDbType.VarChar).Value = deal.OwnerName;
                command.Parameters.Add("@stage", SqlDbType.VarChar).Value = deal.Stage;
                command.Parameters.Add("@productLine", SqlDbType.VarChar).Value = deal.ProductLine;
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

        public void ClearDeals()
        {
            string sql = "[dbo].[SP_HS_CLEAR_DEALS]";
            SqlCommand command = new SqlCommand(sql, cnn);
            SqlDataReader rdr = null;
            try
            {
                command.CommandType = CommandType.StoredProcedure;
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
        
        public DBProceso GetProcessInfo(string codigo)
        {
            DBProceso proceso = new DBProceso();

            string sql = "SELECT * FROM dbo.Proceso_Herramental WHERE codigo = @codigo";
            SqlCommand cmd = new SqlCommand(sql, cnn);
            SqlDataReader reader;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@codigo", SqlDbType.VarChar).Value = codigo;

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                proceso.ProcesoId = Convert.ToInt32(reader["ProcesoHerramentalId"]);
                proceso.Codigo = Convert.ToString(reader["Codigo"]);
                proceso.EstatusProceso = Convert.ToBoolean(reader["EstatusProceso"]);
                proceso.EstatusEjecucion = Convert.ToBoolean(reader["EstatusEjecucion"]);
            }
            reader.Close();
            cmd.Dispose();
            
            return proceso;
        }

        
        public void ActualizarEstatusProceso(DBProceso procesoInfo)
        {
            string sql = "UPDATE dbo.Proceso_Herramental SET EstatusEjecucion = @EstatusEjecucion, UltimaEjecucion = @UltimaEjecucion WHERE codigo = @codigo";
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@EstatusEjecucion", SqlDbType.Bit).Value = procesoInfo.EstatusEjecucion;
            cmd.Parameters.Add("@UltimaEjecucion", SqlDbType.DateTime2).Value = procesoInfo.UltimaEjecucion;
            cmd.Parameters.Add("@codigo", SqlDbType.VarChar).Value = procesoInfo.Codigo;
            cmd.ExecuteNonQuery();
        }

        public int CreateProcesoEjecucion(DBProcesoEjecucion detalle)
        {
            string sql = "[dbo].[SP_HS_INSERT_PROCESS_DETAIL]";
            SqlCommand command = new SqlCommand(sql, cnn);
            SqlDataReader rdr = null;
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@procesoId", SqlDbType.Int).Value = detalle.ProcesoId;
                command.Parameters.Add("@estatus", SqlDbType.Int).Value = detalle.Estatus;
                command.Parameters.Add("@resultado", SqlDbType.VarChar).Value = detalle.Resultado;
                
                var returnParameter = command.Parameters.Add("@ProcesoDetalleId", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                command.ExecuteNonQuery();
                detalle.ProcesoEjecucionId = Convert.ToInt32(returnParameter.Value);
                
                command.Dispose();
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
            return detalle.ProcesoEjecucionId;
        }

        public void ActualizarProcesoEjecucion(DBProcesoEjecucion detalle)
        {
            string sql = "UPDATE dbo.ProcesoEjecucion_Herramental SET Estatus = @Estatus, FechaFin = @FechaFin, Resultado = @Resultado WHERE ProcesoEjecucionHerramentalId = @id";
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = detalle.ProcesoEjecucionId;
            cmd.Parameters.Add("@Estatus", SqlDbType.Bit).Value = detalle.Estatus;
            cmd.Parameters.Add("@FechaFin", SqlDbType.DateTime2).Value = detalle.FechaFin;
            cmd.Parameters.Add("@Resultado", SqlDbType.VarChar).Value = detalle.Resultado;
            cmd.ExecuteNonQuery();
        }

    }
}
