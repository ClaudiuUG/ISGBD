using System.Collections.Generic;
using System.Text.RegularExpressions;
using KeyValueDatabaseApi.Matchers;
using KeyValueDatabaseApi.Models;

namespace KeyValueDatabaseApi.Commands
{
    public class CreateTableCommandSpecification : ICommandSpecification
    {
        private const int TableNamePosition = 2;

        private string _insertTableRegex = "^" + RegexStrings.CreateCommandRegex + RegexStrings.TableReservedWordRegex + RegexStrings.IdentifierRegex + RegexStrings.TableColumnsRegex + "$";

        public bool TryParse(string command, out ICommand parsedCommand)
        {
            var match = Regex.Match(command, _insertTableRegex);
            if (!match.Success)
            {
                parsedCommand = null;
                return false;
            }

            var commandRegex = new Regex(RegexStrings.IdentifierRegex);
            var componentMatch = commandRegex.Matches(command);
            var tableName = componentMatch[TableNamePosition].Value;

            var tableColumnsMatch = Regex.Match(command, RegexStrings.TableColumnsWithoutParanthesisRegex);
            var tableColumnsAndNamesStringed = tableColumnsMatch.Value;
            // tableColumnsAndNamesStringed = tableColumnsAndNamesStringed.Replace("(", "").Replace(")", "").Replace(" ", string.Empty);
            var identifierRegex = RegexStrings.IdentifierRegex + "|" + RegexStrings.ColumnTypeRegex;
            var tableColumnTypesAndNames = Regex.Match(tableColumnsAndNamesStringed, identifierRegex);
            
            var attributes = new List<AttributeModel>();
            while (tableColumnTypesAndNames.Success)
            {
                var columnName = tableColumnTypesAndNames.Value.Replace(" ", string.Empty);
                tableColumnTypesAndNames = tableColumnTypesAndNames.NextMatch();
                var columnType = tableColumnTypesAndNames.Value.Replace(" ", string.Empty);
                tableColumnTypesAndNames = tableColumnTypesAndNames.NextMatch();
                attributes.Add(new AttributeModel(columnName, columnType));
            }

            parsedCommand = new CreateTableCommand(tableName, attributes);
            return true;
        }
    }
}