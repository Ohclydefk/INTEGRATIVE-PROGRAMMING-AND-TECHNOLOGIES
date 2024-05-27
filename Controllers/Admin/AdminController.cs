using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IT15_Project.Areas.Identity.Data;
using IT15_Project.Models;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace IT15_Project.Controllers.CRUD
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddDoctorsPage()
        {
            return View("AddDoctor");
        }

        // GET: Appointment
        public async Task<IActionResult> Appointment()
        {
            var appointments = await _context.Appointments.ToListAsync();
            foreach (var appointment in appointments)
            {
                if (appointment.AppointmentDate.AddYears(3) <= DateTime.Now)
                {
                    appointment.Validity = "Archived";
                }
            }
            _context.SaveChanges();
            return View("Appointment", appointments);
        }

        // GET: Appointment/Edit/{id}
        public async Task<IActionResult> EditAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }


        // GET: DoctorModels
        public async Task<IActionResult> DoctorsPage()
        {
            var doctors = await _context.Doctors.ToListAsync();
            return View(doctors);
        }

        // GET: DoctorModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorModel = await _context.Doctors
                .FirstOrDefaultAsync(m => m.DoctorId == id);
            if (doctorModel == null)
            {
                return NotFound();
            }

            return RedirectToAction("AddDoctorsPage", "Admin");
        }


        //Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorModelDto doctorsDto)
        {
            if (doctorsDto.ImageFile == null || doctorsDto.ImageFile.Length == 0)
            {
                ModelState.AddModelError("ImageFile", "The image file is required");
            }

            if (!ModelState.IsValid)
            {
                return View("AddDoctor", doctorsDto);
            }

            string uploadsFolder = Path.Combine(_env.WebRootPath, "uploaded_images");
            string uniqueFileName = $"{Guid.NewGuid()}_{doctorsDto.ImageFile!.FileName}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await doctorsDto.ImageFile.CopyToAsync(fileStream);
            }

            DoctorModel doctorModel = new DoctorModel
            {
                FirstName = doctorsDto.FirstName,
                LastName = doctorsDto.LastName,
                Department = doctorsDto.Department,
                BirthDate = doctorsDto.BirthDate,
                PhoneNumber = doctorsDto.PhoneNumber,
                Email = doctorsDto.Email,
                Gender = doctorsDto.Gender,
                ImageFileName = uniqueFileName
            };

            _context.Doctors.Add(doctorModel);
            await _context.SaveChangesAsync();

            return RedirectToAction("DoctorsPage");
        }

        //Edit Function 
        public IActionResult EditDoctor(int id)
        {
            var doctor = _context.Doctors.Find(id);
            if (doctor == null)
            {
                return NotFound();
            }

            var doctorDto = new DoctorModelDto
            {
                DoctorId = doctor.DoctorId,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Department = doctor.Department,
                BirthDate = doctor.BirthDate,
                PhoneNumber = doctor.PhoneNumber,
                Email = doctor.Email,
                Gender = doctor.Gender
            };

            return View("EditDoctor", doctorDto);
        }

        //Update function
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(DoctorModelDto doctorsDto)
        {
            if (doctorsDto.DoctorId == 0 || doctorsDto == null)
            {
                return BadRequest();
            }

            var doctor = await _context.Doctors.FindAsync(doctorsDto.DoctorId);
            if (doctor == null)
            {
                return NotFound();
            }

            if (doctorsDto.ImageFile != null && doctorsDto.ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "uploaded_images");
                string uniqueFileName = $"{Guid.NewGuid()}_{doctorsDto.ImageFile.FileName}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await doctorsDto.ImageFile.CopyToAsync(fileStream);
                }

                if (!string.IsNullOrEmpty(doctor.ImageFileName))
                {
                    var oldFilePath = Path.Combine(uploadsFolder, doctor.ImageFileName);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                doctor.ImageFileName = uniqueFileName;
            }

            doctor.FirstName = doctorsDto.FirstName;
            doctor.LastName = doctorsDto.LastName;
            doctor.Department = doctorsDto.Department;
            doctor.BirthDate = doctorsDto.BirthDate;
            doctor.PhoneNumber = doctorsDto.PhoneNumber;
            doctor.Email = doctorsDto.Email;
            doctor.Gender = doctorsDto.Gender;

            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();

            return RedirectToAction("DoctorsPage");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetDepartments()
        {
            var departments = _context.Doctors.Select(d => d.Department).Distinct().ToList();
            return Json(departments);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetDoctors(string department)
        {
            var doctors = _context.Doctors.Where(d => d.Department == department).ToList();
            return Json(doctors);
        }

        [HttpPost]
        public IActionResult Archive(int id)
        {
            var appointment = _context.Appointments.Find(id);
            if (appointment != null)
            {
                appointment.Validity = "Archived";
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAppointment(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                // Fetch the existing appointment from the database
                var existingAppointment = await _context.Appointments.FindAsync(appointment.AppointmentId);

                if (existingAppointment == null)
                {
                    return NotFound();
                }

                try
                {
                    // Update appointment properties
                    existingAppointment.AssignedDept = appointment.AssignedDept;
                    existingAppointment.AssignedDoctor = appointment.AssignedDoctor;
                    existingAppointment.AppointmentDate = appointment.AppointmentDate;
                    existingAppointment.Status = appointment.Status;

                    // Save changes to the database
                    _context.Appointments.Update(existingAppointment);
                    await _context.SaveChangesAsync();

                    // Add a success message to TempData for displaying after redirect
                    TempData["SuccessMessage"] = "Appointment updated successfully.";

                    return RedirectToAction(nameof(Appointment));
                }
                catch (Exception)
                {
                    // Log the exception or handle it as needed
                    TempData["ErrorMessage"] = "An error occurred while updating the appointment.";
                    return RedirectToAction(nameof(Appointment));
                }
            }

            // If model state is not valid, return to the form with validation errors
            return View(appointment);
        }
    }
}

