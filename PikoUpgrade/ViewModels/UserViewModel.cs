namespace PikoUpgrade.ViewModels
{
    public class UserViewModel
    {
        public string Name { get; set; }

        public UserViewModel(string name)
        {
            Name = name;
        }
    }
}