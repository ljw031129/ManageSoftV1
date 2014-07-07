using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialGoal.Model.Models;
using SocialGoal.Models;

namespace SocialGoal.Controllers
{   
    public class EquipmentController : Controller
    {
		private readonly IEquipmentRepository equipmentRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public EquipmentController() : this(new EquipmentRepository())
        {
        }

        public EquipmentController(IEquipmentRepository equipmentRepository)
        {
			this.equipmentRepository = equipmentRepository;
        }

        //
        // GET: /Equipment/

        public ViewResult Index()
        {
            return View(equipmentRepository.All);
        }

        //
        // GET: /Equipment/Details/5

        public ViewResult Details(string id)
        {
            return View(equipmentRepository.Find(id));
        }

        //
        // GET: /Equipment/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Equipment/Create

        [HttpPost]
        public ActionResult Create(Equipment equipment)
        {
            if (ModelState.IsValid) {
                equipmentRepository.InsertOrUpdate(equipment);
                equipmentRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /Equipment/Edit/5
 
        public ActionResult Edit(string id)
        {
             return View(equipmentRepository.Find(id));
        }

        //
        // POST: /Equipment/Edit/5

        [HttpPost]
        public ActionResult Edit(Equipment equipment)
        {
            if (ModelState.IsValid) {
                equipmentRepository.InsertOrUpdate(equipment);
                equipmentRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /Equipment/Delete/5
 
        public ActionResult Delete(string id)
        {
            return View(equipmentRepository.Find(id));
        }

        //
        // POST: /Equipment/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            equipmentRepository.Delete(id);
            equipmentRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                equipmentRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

