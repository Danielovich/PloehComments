using DiffPlex.DiffBuilder;

namespace Tests
{
    internal class PostDiff
    {
        /// <summary>
        /// Asserts anchor(s) on updatedPost are the only inserted and different content from the original content
        /// </summary>
        /// <param name="originalPost"></param>
        /// <param name="updatedPost"></param>
        /// <returns></returns>
        public static async Task AssertAnchors(string originalPost, string updatedPost)
        {
            var diff = InlineDiffBuilder.Diff(originalPost, updatedPost);

            // what has been inserted to the updated post ?
            var inserted = diff.Lines
                .Where(d => d.Type == DiffPlex.DiffBuilder.Model.ChangeType.Inserted)
                .AsEnumerable();

            foreach (var line in inserted)
            {
                if(line.Text.IndexOf(">#</a>") == -1)
                {
                    Assert.False(true);
                }
            }

            await Task.CompletedTask;
        }
    }
}
