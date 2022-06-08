using FinalExamRea.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdminBlog.Controllers
{
    public class NoteController : Controller
    {
        private readonly ILogger<NoteController> _logger;
        private readonly NoteContext _context;

        public NoteController(ILogger<NoteController> logger, NoteContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.Categories = _context.Categories.Select(w =>
            new SelectListItem
            {
                Text = w.Name,
                Value = w.Id.ToString()
            }
            ).ToList();
            var list = _context.Notes.ToList();

            return View(list);

        }
        public IActionResult Publish(int Id)
        {
            var note = _context.Notes.Find(Id);
            note.IsPublish = true;
            _context.Update(note);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Note()
        {
            ViewBag.Categories = _context.Categories.Select(w =>
            new SelectListItem
            {
                Text = w.Name,
                Value = w.Id.ToString()
            }
            ).ToList();
            ViewBag.Tags = _context.Tags.Select(w =>
            new SelectListItem
            {
                Text = w.Name,
                Value = w.Id.ToString()
            }
            ).ToList();
            return View();

        }
        public IActionResult UpdateNote(int? id)
        {
            var note = _context.Notes.Find(id);
            ViewBag.Categories = _context.Categories.Select(w =>
            new SelectListItem
            {
                Text = w.Name,
                Value = w.Id.ToString()
            }
            ).ToList();
            ViewBag.Tags = _context.Tags.Select(w =>
            new SelectListItem
            {
                Text = w.Name,
                Value = w.Id.ToString()
            }
            ).ToList();
            ViewBag.Note = note;
            return View();

        }

        public  IActionResult DeleteNote(int? id)
        {
            var note = _context.Notes.Find(id);
            _context.Remove(note);
            _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Save(Note model)
        {
            if (model != null)
            {
                model.AuthorId = (int)HttpContext.Session.GetInt32("id");
                await _context.AddAsync(model);
                await _context.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }
        public async Task<IActionResult> Update(Note model)
        {
            if (model != null)
            {
                model.AuthorId = (int)HttpContext.Session.GetInt32("id");
                _context.Update(model);
                await _context.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

    }
}
