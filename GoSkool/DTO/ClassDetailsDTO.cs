namespace GoSkool.DTO
{
    public class ClassDetailsDTO
    {
        public int ClassId { get; set; }
        public string Name { get; set; }
        public int StudentsCount { get; set; }
        public string NameSortParam { get; set; } = "";
        public string FeeSortParam { get; set; } = "";
        public string SortParam { get; set; } = "";
        public List<Tuple<string, int>> StudentFeeDetials { get; set; } = new List<Tuple<string, int>>();
    }
}
