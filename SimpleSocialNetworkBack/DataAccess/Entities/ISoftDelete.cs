namespace DataAccess.Entities
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
