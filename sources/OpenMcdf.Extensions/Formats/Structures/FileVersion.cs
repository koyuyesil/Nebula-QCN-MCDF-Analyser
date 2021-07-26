
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.ComponentModel;

namespace OpenMcdf.Extensions.Formats.Structures
{
   
    public class FileVersion
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Revision { get; set; }
        public void Read(BinaryReader br)
        {

            Major = br.ReadUInt16();
            Minor = br.ReadUInt16();
            Revision = br.ReadUInt16();

        }


    }
}