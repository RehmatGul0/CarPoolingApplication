
namespace CarPoolingApp.DataTransferObjects
{
    public class GetUserResponseDTO
    {
        public string name { get; }
        public string phone { get; }
        public int rating { get; }
        public string gender { get; }

        public bool isDriver { get; }

        public GetUserResponseDTO(string name, string phone, int rating, string gender,bool isDriver)
        {
            this.name = name;
            this.phone = phone;
            this.rating = rating;
            this.gender = gender;
            this.isDriver = isDriver;
        }
    }
}
