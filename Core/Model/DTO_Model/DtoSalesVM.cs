using System;
using System.Collections.Generic;

namespace Model.DTO_Model
{
    public class DtoSalesVM
    {
        public DateTime Date { get; set; }

        public List<DtoDayTotalVM> Days { get; set; }

        public int Year { get; set; }   

        public int Month { get; set; }  
    }
}
