namespace Library.WebApi.Repositories
{
    public interface IBookAuthorRepository
    {
        Task AddAuthorToBook(int bookId, int authorId);
    }
}
