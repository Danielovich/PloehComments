
namespace PloehComments;

public class PostsNameRetriever
{
    private string lookInDirectory;

    public PostsNameRetriever(string lookInDirectory)
    {
        if (string.IsNullOrEmpty(lookInDirectory))
            throw new ArgumentNullException(nameof(lookInDirectory));

        this.lookInDirectory = lookInDirectory;
    }

    public async Task<IEnumerable<string>?> PostsInDirectory()
    {
        var directory = new DirectoryInfo(lookInDirectory);

        var filenames = directory.EnumerateFileSystemInfos().Select(file => file.Name);

        return await Task.FromResult(filenames);
    }
}