namespace Anki.DAL.DTOs
{

    public class UserDTO
    {
        public long ChatID { get; set; }
        public string Cookie { get; set; } = "";
        public int State { get; set; }
        public string CsrfToken { get; set; } = "";
    }
}