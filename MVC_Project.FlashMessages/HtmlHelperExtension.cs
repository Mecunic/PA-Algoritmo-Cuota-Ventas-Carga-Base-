using System.Web.Mvc;

namespace MVC_Project.FlashMessages
{
    public static class HtmlHelperExtension
    {
        public static bool TieneMensaje(this HtmlHelper html)
        {
            return MensajeFlashHandler.RecibioMensajeFlash();
        }
        public static MensajeFlash ObtenerMensaje(this HtmlHelper html)
        {
            return MensajeFlashHandler.ObtenerMensaje();
        }
    }
}
