using System.Collections.Generic;
using System.Composition;

using KeyValueDatabaseApi.Commands;

namespace KeyValueDatabaseApi.Parsers
{
    public class CommandParser : ICommandParser
    {
        private IEnumerable<ICommandSpecification> KnownCommands = new List<ICommandSpecification> 
        {
            new CreateDatabaseCommandSpecification(),
            new DropDatabaseCommandSpecification(),
            new UseDatabaseCommandSpecification(),
            new CreateTableCommandSpecification(),
            new DropTableCommandSpecification(),
            new CreateIndexCommandSpecification(),
            new InsertIntoCommandSpecification()
        };

        public bool TryParse(string command, out ICommand parsedCommand) 
        {
            string lowercaseCommand = command.ToLower();
            foreach (var knownCommand in KnownCommands)
            {
                if (knownCommand.TryParse(command, out parsedCommand)) 
                {
                    return true;
                }
            }
            parsedCommand = null;
            return false;
        }
    }
}