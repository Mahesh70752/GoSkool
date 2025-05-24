namespace GoSkool.DTO
{
    public class DriverDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BusNumber {get; set; }
        public bool driving {get; set; } = false;

        public List<string> lats { get; set; }
        public List<string> lngs { get; set; }
    }
}
