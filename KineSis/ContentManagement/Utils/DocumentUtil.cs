﻿/*
   Copyright 2012 Alexandru Albu - sandu.albu@gmail.com

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KineSis.ContentManagement.Utils
{

    /// <summary>
    /// Document Utility class
    /// </summary>
    class DocumentUtil
    {

        /// <summary>
        /// Create a directory name based on current date and time
        /// </summary>
        /// <returns></returns>
        public static String GenerateDirectoryName()
        {
            DateTime dateTime = DateTime.Now;
            return "" + dateTime.ToString("yyyyMMddHHmmss");
        }
    }
}
