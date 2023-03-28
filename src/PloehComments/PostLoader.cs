namespace PloehComments;

public class PostLoader
{
    private string filePath;
    public PostLoader(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            throw new ArgumentNullException(nameof(filePath));

        this.filePath = filePath;
    }

    public async Task<string> PostToStringAsync()
    {
        return await File.ReadAllTextAsync(filePath);
    }
}