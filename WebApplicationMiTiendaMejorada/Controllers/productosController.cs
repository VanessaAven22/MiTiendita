using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplicationMiTiendaMejorada.Contexto;

namespace WebApplicationMiTiendaMejorada.Controllers
{
    public class productosController : Controller
    {
        private MiTiendaMejoradaEntities db = new MiTiendaMejoradaEntities();

        // GET: productos
        public ActionResult Index()
        {
            var productoes = db.Productoes.Include(p => p.Proveedor);
            return View(productoes.ToList());
        }

        // GET: productos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productoes.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: productos/Create
        public ActionResult Create()
        {
            ViewBag.id_proveedor = new SelectList(db.Proveedors, "id_proveedor", "nombre_proveedor");
            return View();
        }

        // POST: productos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_producto,producto_nombre,producto_precio,producto_cantidad,producto_descripcion,id_proveedor")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Productoes.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_proveedor = new SelectList(db.Proveedors, "id_proveedor", "nombre_proveedor", producto.id_proveedor);
            return View(producto);
        }

        // GET: productos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productoes.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_proveedor = new SelectList(db.Proveedors, "id_proveedor", "nombre_proveedor", producto.id_proveedor);
            return View(producto);
        }

        // POST: productos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_producto,producto_nombre,producto_precio,producto_cantidad,producto_descripcion,id_proveedor")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_proveedor = new SelectList(db.Proveedors, "id_proveedor", "nombre_proveedor", producto.id_proveedor);
            return View(producto);
        }

        // GET: productos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productoes.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Productoes.Find(id);
            db.Productoes.Remove(producto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: productos/Edit/5
        public ActionResult ventaProductos(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productoes.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_proveedor = new SelectList(db.Proveedors, "id_proveedor", "nombre_proveedor", producto.id_proveedor);
            return View(producto);
        }

        // POST: productos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ventaProductos([Bind(Include = "id_producto,producto_nombre,producto_precio,producto_cantidad,producto_descripcion,id_proveedor")] Producto producto, int cantidadProducto, int precioVenta)
        {
            if (ModelState.IsValid)
            {
                Venta ventas = new Venta();
                ventas.id_ventas++;
                ventas.venta_total = precioVenta;

                db.Ventas.Add(ventas);

                producto.producto_cantidad = producto.producto_cantidad - cantidadProducto;
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_proveedor = new SelectList(db.Proveedors, "id_proveedor", "nombre_proveedor", producto.id_proveedor);
            return View(producto);
        }


        public ActionResult precioDeVenta(int id_Producto, int cantidad_producto)
        {
            var valorProducto = (from producto in db.Productoes
                                 where producto.id_producto == id_Producto
                                 select producto.producto_precio).FirstOrDefault();

            int valor = int.Parse(valorProducto.ToString());

            return Content((valor * cantidad_producto).ToString());
        }
    }

}
