namespace HelloBlazor.Models
{
    public class CouleurDTO
    {
        public string Name { get; set; } = null!;
        public string HexCode { get; set; } = null!;
    }

    public class ListeCouleurDTO
    {
        public List<CouleurDTO> Results { get; set; } = null!;
    }

}
