using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using practica3.Integration;
using practica3.Integration.dto;

namespace practica3.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly ListarUsuarios _listUsers;
        private readonly ListarUsuario _unUser;

        private readonly CrearUsuario _createUser;

        public UsuarioController(ILogger<UsuarioController> logger,
        ListarUsuarios listUsers,
        ListarUsuario unUser,
        //AGREGAMOS CREAR USUARIO
        CrearUsuario createUser)
        {
            _logger = logger;
            _listUsers = listUsers;
            _unUser = unUser;
             _createUser = createUser;
        }   

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Usuario> users = await _listUsers.GetAllUser();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Perfil(int Id)
        {
            Usuario user = await _unUser.GetUser(Id);
            return View(user);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create(string name, string job)
        {
            try
            {
                var response = await _createUser.CreateUser(name, job);
                            if (response != null)
                {
                    TempData["SuccessMessage"] = "";
                }
                else
                {
                    ModelState.AddModelError("", "ERROR AL CREAR USUARIO");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR AL CREAR USUARIO: {ex.Message}");
                ModelState.AddModelError("", "ERROR AL CREAR USUARIO");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}