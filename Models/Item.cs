namespace WEBAPI_Demo.Models
{
    public class Item
    {
        public int ID { get; set; }
        public int? EmpID { get; set; }
        public string? EmpName { get; set; }
        public string? Task_Description { get; set; }
        public string? Critical_Level { get; set; }
    }

}
