// <copyright file="Resources.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

#if DOTNETFRAMEWORK

namespace Useful.UI.Resources
{
    using System.Drawing;
    using System.Reflection;

    /// <summary>
    /// Retrieves resources.
    /// </summary>
    public static class Resources
    {
        /// <summary>
        /// Gets the application icon.
        /// </summary>
        /// <returns>The application icon.</returns>
        public static Icon GetAppIcon()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var names = executingAssembly.GetManifestResourceNames();

            Icon icon;

            using (var fs = executingAssembly.GetManifestResourceStream("Useful.UI.app.ico"))
            {
                icon = new Icon(fs);
            }

            return icon;
        }
    }
}

#endif