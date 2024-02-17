// <copyright file="HomeController.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

using System;
using Microsoft.AspNetCore.Mvc;
using Useful.Security.Cryptography.UI.Models;
using Useful.Security.Cryptography.UI.Services;

namespace UsefulWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => RedirectToAction("Index", "Cryptography");

        public IActionResult Error() => View();
    }
}