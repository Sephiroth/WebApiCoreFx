using System;
using System.Collections.Generic;

namespace DBModel.Entity
{
    public partial class TbChargingStation
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Address { get; set; }
        public uint PriceTypeId { get; set; }
        public string PriceUnit { get; set; }
        public string PowerUnit { get; set; }
        public string Owner { get; set; }
        public string Worktime { get; set; }
        public double ParkingFee { get; set; }
        public string Photo { get; set; }
        public int State { get; set; }
        public string Desc { get; set; }
        public DateTime CreateDate { get; set; }
        public int Flag { get; set; }
    }
}
