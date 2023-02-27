namespace EF_Study.Encje
{
    public class State
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<WorkItem> WorkItems { get; set; }

    }
}
