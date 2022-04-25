using System;

namespace Model.DTO_Model
{
    public class DtoProductComment
    {
        public string _id { get; set; }

        public DateTime DateTimeComment { get; set; }

        public string FullName { get; set; }

        public string Content { get; set; }

        public string ProductId { get; set; }
    }
}
