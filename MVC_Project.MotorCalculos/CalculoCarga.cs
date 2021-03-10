using Microsoft.Analytics.Interfaces;
using Microsoft.Analytics.Interfaces.Streaming;
using Microsoft.Analytics.Types.Sql;
using MVC_Project.MotorCalculos.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MVC_Project.MotorCalculos
{
    public class Cargas
    {
        public static CargaDiaria CalcularCargaTotal(ConfiguracionCalculoCarga configuracion)
        {
            if(DateTime.ParseExact(configuracion.FechaACalcular, Utils.FORMAT_DATE, Utils.CULTURE_INFO).CompareTo(DateTime.Now) < 0)
            {
                throw new CargaException("La fecha a calcular no puede ser menor a la fecha actual");
            }
            //OBTENER SEMANAS ATRAS
            List<string> fechasAtras = Utils.ObtenerSemanasAtras(configuracion.FechaACalcular, configuracion.Semanas);
            //OBTENER INVENTARIOS DE CARGA
            List<Inventario> inventarios = configuracion.Cargas.Where(i => fechasAtras.Contains(i.Fecha) && i.Ruta.Equals(configuracion.Ruta)).ToList();
            //OBTENER VENTAS
            List<Venta> ventas = configuracion.Ventas.Where(v => fechasAtras.Contains(v.Fecha)).ToList();
            //VALIDAR NUMERO DE INVENTARIOS VS SEMANAS
            if(inventarios.Count < configuracion.Semanas)
            {
                throw new CargaException("El numero de Semanas a Calcular no se encuentran en el inventario");
            }
            //LLENAR VENTAS/PRODUCTOS EN CERO
            LlenadoProductosVentasCero(configuracion.PortafolioPredefinido, fechasAtras, ventas, configuracion.Ruta);
            //CALCULAR PROMEDIO INVENTARIO
            List<Producto> productosCarga = new List<Producto>();
            foreach (ProductoPortafolioPredefinido producto in configuracion.PortafolioPredefinido)
            {
                double PromedioInventario = inventarios.SelectMany(i => i.Productos).Where(p => p.SKU.Equals(producto.SKU)).Average(p => p.Cantidad);
                double PromedioVentas = ventas.SelectMany(v => v.Productos).Where(p => p.SKU.Equals(producto.SKU)).Average(p => p.Cantidad);
                //CUAL ES EL PRODUCTO CARGADO (ES INVENTARIOS) Y CUAL ES EL PRODUCTO VENDIDO (PRODUCTO VENDIDO SUPONGO QUE ES EL VENTAS)
            }

            return new CargaDiaria
            {
                Productos = productosCarga,
                Ruta = configuracion.Ruta,
                Fecha = configuracion.FechaACalcular
            };
        }

        private static void LlenadoProductosVentasCero(List<ProductoPortafolioPredefinido> PortafolioPredefinido, List<string> Fechas, List<Venta> Ventas,string Ruta)
        {
            foreach(string fecha in Fechas)
            {
                Venta venta = Ventas.Where(v => v.Fecha.Equals(fecha)).FirstOrDefault();
                if(venta == null)
                {
                    List<ProductoVenta> productos = new List<ProductoVenta>();
                    foreach(ProductoPortafolioPredefinido productoPortafolio in PortafolioPredefinido)
                    {
                        productos.Add(new ProductoVenta
                        {
                            Cantidad = productoPortafolio.Cantidad,
                            Precio = productoPortafolio.PrecioUnitario,
                            Nombre = productoPortafolio.Nombre,
                            SKU = productoPortafolio.SKU
                        });
                    }
                    venta = new Venta
                    {
                        Fecha = fecha,
                        Ruta = Ruta,
                        Productos = productos
                    };
                    Ventas.Add(venta);
                }
                else
                {
                    foreach (ProductoPortafolioPredefinido productoPortafolio in PortafolioPredefinido)
                    {
                        ProductoVenta productoVenta = venta.Productos.Where(p => p.SKU.Equals(productoPortafolio.SKU)).FirstOrDefault();
                        if(productoVenta == null)
                        {
                            productoVenta = new ProductoVenta
                            {
                                Cantidad = productoPortafolio.Cantidad,
                                Nombre = productoPortafolio.Nombre,
                                Precio = productoPortafolio.PrecioUnitario,
                                SKU = productoPortafolio.SKU
                            };
                            venta.Productos.Add(productoVenta);
                        }
                    }
                }
            }
        }



    }
}