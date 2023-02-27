namespace EF_Study.Encje
{
    public class Tag
    {
        public int Id { get; set; }
        public string value { get; set; }
        public List<WorkItem> WorkItems { get; set; }
    }
}
