using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using KeyValueDatabaseApi.Http.Requests;
using KeyValueDatabaseApi.Exceptions;
using KeyValueDatabaseApi.Parsers;
using KeyValueDatabaseApi.Validators;
using KeyValueDatabaseApi.Commands;
using KeyValueDatabaseApi.Context;

namespace KeyValueDatabaseApi.Controllers
{
    [Route("api/execute-command")]
    [ApiController]
    public class HttpController
    {
        [HttpGet("{id}", Name = "GetCommand")]
        public ActionResult<string> Get(long id)
        {
            return string.Empty;
        }

        [HttpPost]
        public ActionResult<string> Post(ExecuteCommandRequest commandRequest)
        {
            var command = commandRequest.Command.ToLower();

            try
            {
                ICommandValidator commandValidator = new CommandValidator();
                commandValidator.ValidateCommand(command);
            }
            catch (InvalidCommandException e)
            {
                return $"{e.GetType()} occured. Message: {e.Message}";
            }

            try
            {

                ICommandParser commandParser = new CommandParser();
                ICommand parsedCommand;
                if (commandParser.TryParse(command, out parsedCommand))
                {
                    parsedCommand.Execute();
                }
                else
                {
                    return $"Could not parse command: {command}";
                }
                return "SUCCESS";
            }
            catch (Exception exception)
            {
                return $"Failure with exception {exception}";
            }
        }
    }
}