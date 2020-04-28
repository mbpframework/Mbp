using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EMS.Application.Utility
{
    public static class AttachmentHelper
    {
        public static void ClearUserTemp(string tempDirectory)
        {
            foreach (var file in Directory.GetFiles(tempDirectory))
            {
                File.Delete(file);
            }
        }
    }
}
