namespace KeyValueDatabaseApi.Matchers
{
    public static class RegexStrings
    {
        public static string CreateCommandRegex = @"\s*create\s*";
        public static string DropCommandRegex = @"\s*drop\s*";
        public static string UseCommandRegex = @"\s*use\s*";
        public static string DatabaseReservedWordRegex = @"\s*database\s*";
        public static string TableReservedWordRegex = @"\s*table\s*";
        public static string IndexReservedWordRegex = @"\s*index\s*";
        internal static string OnReservedWordRegex = @"\s*on\s*";
        public static string IdentifierRegex = "[A-Za-z]+";
        public static string IdentifierRegexWithSpaces = @"\s*[A-Za-z]+\s*";
        public static string ParameterListWithoutParanthesisRegex = $@"{IdentifierRegexWithSpaces}(,{IdentifierRegexWithSpaces})*";
        public static string ParameterListRegex = $@"\s*\({ParameterListWithoutParanthesisRegex}\)\s*";
        public static string ColumnTypeRegex = @"\s*((string)|(int)|(double)|(float))\s*";
        public static string ColumnNameAndTypeRegex = $@"\s*{IdentifierRegex}\s*{ColumnTypeRegex}\s*";
        public static string TableColumnsRegex = $@"\s*\({ColumnNameAndTypeRegex}(,{ColumnNameAndTypeRegex})*\)";
        public static string TableColumnsWithoutParanthesisRegex = $@"{ColumnNameAndTypeRegex}(,{ColumnNameAndTypeRegex})+?";
    }
}