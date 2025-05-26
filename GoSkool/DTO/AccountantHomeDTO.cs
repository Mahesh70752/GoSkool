namespace GoSkool.DTO
{
    public class AccountantHomeDTO
    {
        public string Name {  get; set; }
        public List<List<Tuple<int, string>>> Classes { get; set; } = new List<List<Tuple<int, string>>>();
    }
}
