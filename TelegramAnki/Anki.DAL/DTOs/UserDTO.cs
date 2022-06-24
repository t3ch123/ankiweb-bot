namespace Anki.DAL.DTOs
{

    public class UserDTO
    {
        public int ChatID { get; set; }
        public string Cookie { get; set; } = "";
        public int State { get; set; }
    }
}