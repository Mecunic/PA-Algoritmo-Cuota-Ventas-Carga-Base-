using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MVC_Project.Utils
{
    public class Constants
    {
        public static readonly string AUTHENTICATED_USER_PROFILE = "ddc30589-80ba-48e6-88ec-6454350f2cd7_USER_PROFILE";

        //ROLE CODES
        public static readonly string ROLE_COMPANY_ADMIN = "SUPER_ADMIN";
        public static readonly string ROLE_COMPANY_RESOURCE = "ADMIN";
        public static readonly string ROLE_GLOBAL_PUBLISHER = "COMPANY";

        //public static string STORAGE_MAIN_CONTAINER = ConfigurationManager.AppSettings["StorageMainContainer"];

        public static string DATE_FORMAT_DAY_MONTH = "dd/MM";
        public static string DATE_FORMAT = "dd/MM/yyyy";
        public static string TIMEZONE_UTC = "UTC";
       

        // Uuids de templates sendgrid
        public static string NOT_TEMPLATE_WELCOME = "51d34567-1960-4501-8e89-e349c18275e6";
        public static string NOT_TEMPLATE_ACTIVATION = "534f930b-d06f-4aad-a30a-ff42346e7c56";
        public static string NOT_TEMPLATE_PASSWORDRESET = "d12f6fbe-cab5-485f-95ec-6875d196d49c";
        
        // Encabezados de archivo Excel
        public static string HEADER_USERIDENTIFIER = "Identificador";
        public static string HEADER_FIRSTNAME = "Nombre";
        public static string HEADER_LASTNAME = "Apellido Paterno";
        public static string HEADER_EMAIL = "Correo Electrónico";
        public static string HEADER_POSITION = "Puesto";

        //Mensajes
        public static string View_Message = "View.Message";
        public static string View_Error = "View.Error";
        
        // Status de notificaciones
        public static int PENDING = 0;
        public static int SENT = 1;
        public static int ERROR = 2;

        // URLs varias
        public static string DEFAULT_AVATAR = "https://bootdey.com/img/Content/avatar/avatar1.png";
       
    }
}
