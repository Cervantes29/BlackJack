using System.ComponentModel.DataAnnotations;

namespace ModelsEL
{
    /// <summary>
    /// User designed to keep track of balance account, username, decisionmaking.
    /// Is separate from "Player" since player is more gameoriented
    /// </summary>
    public class User
    {
        public User()
        {
            Username = "default name";
            Balance = 0;
            AllTimeBalance = 0;
            Decision = 0;
        }

        public User(string username, int balance, int id) : this()
        {
            Username = username;
            Balance = balance;
            Id = id;
        }

        [Key]
        public int Id { get; set; }

        public string Username { get; set; }
        public int Balance { get; set; }
        public int AllTimeBalance { get; set; }
        public float Decision { get; set; }
    }
}