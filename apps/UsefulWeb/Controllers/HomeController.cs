// <copyright file="HomeController.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;

namespace UsefulWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => RedirectToAction("Index", "Cryptography");

        public IActionResult Error() => View();
    }
}