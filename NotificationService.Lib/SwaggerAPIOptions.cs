using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationService.Lib
{
    public class SwaggerAPIOptions
    {
        public SwaggerAPIOptions()
        {

        }
        public bool Enabled { get; set; }
        public string DocumentTitle { get; set; }
        public string VersionName { get; set; }
        public string RoutePrefix { get; set; }
        public string Path { get; set; }

    }
}
