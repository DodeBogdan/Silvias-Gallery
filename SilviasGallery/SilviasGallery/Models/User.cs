using SQLite;

namespace SilviasGallery
{
    public class User
    {
        #region Properties and Indexers
        public string Discount => (NumberOfPersons * 5) + "%";
        public string FirstName { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public string LastName { get; set; }
        public int NumberOfPersons { get; set; }
        public string Token { get; set; }
        #endregion

        #region Constructors
        private User(int id, string firstName, string lastName, int numberOfPersons, string token)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            NumberOfPersons = numberOfPersons;
            Token = token;
        }

        public User()
        {
        }
        #endregion

        #region Public members
        public User Clone()
        {
            return new User(Id, FirstName, LastName, NumberOfPersons, Token);
        }
        #endregion
    }
}
