
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.ComponentModel;
using System.Collections;

namespace OpenMcdf.Extensions.Formats.Structures
{
    /// <summary>
    /// first two bytes header
    /// folowing every bytes must be reversed bits
    /// bit ordering same as folowing
    /// 7 6 5 4 3 2 1 0 - 15 14 13 12 11 10 9 8 - 23 22 21 20 19 18 17 16 
    /// </summary>
    public class FeatureMask
    {
        public List<string> DataBits { get; set; }
        public FeatureMask()
        {
            DataBits = new List<string>();
        }

        public void Read(BinaryReader br)
        {
            byte[] A=br.ReadBytes(2);
            

            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                string yourByteString = Convert.ToString(br.ReadByte(), 2).PadLeft(8, '0');

                DataBits.Add(yourByteString);

                
            }
            var a=1;
        }


    }
}