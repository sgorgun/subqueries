namespace AutocodeDB.SQLTemplates
{
    public static class CommentEntity
    {
        public const string Comments = @"\/\*[\s\S]*?\*\/|\-\-.*$";//"([\s;]|^)(/\*[\s\S]*?(\*/))";
    }
}
