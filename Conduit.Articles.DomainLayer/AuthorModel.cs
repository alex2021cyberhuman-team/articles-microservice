namespace Conduit.Articles.DomainLayer;

public class AuthorModel
{
    public AuthorModel(
        string username,
        string bio,
        string image,
        bool following)
    {
        Username = username;
        Bio = bio;
        Image = image;
        Following = following;
    }

    public string Username { get; set; }

    public string Bio { get; set; }

    public string Image { get; set; }

    public bool Following { get; set; }
}