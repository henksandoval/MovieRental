namespace MovieRentalApi.Data.Entities
{
    public class CategoryEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<MovieEntity> Movies { get; set; }
    }
}
