namespace EFCore.ReadMultiResultSetFromSP.Models
{
    public class MainResult
    {
        public List<ResultSet1> ResultSet1 { get; set; }
        public List<ResultSet2> ResultSet2 { get; set; }
        public List<int> ResultSet3 { get; set; }
        public List<ResultSet4> ResultSet4 { get; set; }
    }
}