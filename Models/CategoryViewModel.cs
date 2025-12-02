namespace Test.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();
    }
}
