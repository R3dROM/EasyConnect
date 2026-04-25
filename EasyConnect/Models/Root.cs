using System;
using System.Collections.Generic;
using System.Text;

namespace EasyConnect.Models
{
    public class Root
    {
        public SheetData sheetData { get; set; }
    }

    public class SheetData
    {
        public List<Row> row { get; set; }
    }

    public class Row
    {
        public List<Cell> c { get; set; }
    }

    public class Cell
    {
        public string v { get; set; }
    }
    public class DeviceList
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public string Number { get; set; }
    }
}
