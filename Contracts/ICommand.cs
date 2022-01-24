using Gateways.NET.Core;
using System.Collections.Generic;
using System.Net;

namespace Gateways.NET.Contracts
{
    /// <summary>
    /// General command interface
    /// </summary>
    public interface ICommand
    {
    }

    public interface IEntityUpdateCommand<TKey> : ICommand
    {
        TKey Id { get; }
    }

    /// <summary>
    /// ICommand extension methods
    /// </summary>
    public static class ICommandExtenssion
    {
        /// <summary>
        /// Generates an OK response using command data
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="command"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static ICommandResponse OkResponse<TObject>(this ICommand command, TObject body) => new CommandResponse<TObject>(HttpStatusCode.OK, body);


        /// <summary>
        /// Generates an Error response using command data and an error message
        /// </summary>
        /// <param name="command"></param>
        /// <param name="error"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static ICommandResponse ErrorResponse(this ICommand command, string error, HttpStatusCode code = HttpStatusCode.InternalServerError) => new CommandResponse<string>(code, error, new string[] { error });

        /// <summary>
        /// Generate a Not Found request response using command data and an error message
        /// </summary>
        /// <param name="command"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static ICommandResponse NotFoundResponse(this ICommand command, string error) => new CommandResponse<string>(HttpStatusCode.NotFound, null, new string[] { error });


        /// <summary>
        /// Generates a BadRequest response using command data and an error message
        /// </summary>
        /// <param name="command"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static ICommandResponse BadResponse(this ICommand command, string error) => new CommandResponse<string>(HttpStatusCode.BadRequest, null, new string[] { error });

        /// <summary>
        /// Generates a BadRequest response using command data and a list of errors messages
        /// </summary>        
        /// <param name="command"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static ICommandResponse BadResponse(this ICommand command, IEnumerable<string> errors) => new CommandResponse<string>(HttpStatusCode.BadRequest, null, errors);

        /// <summary>
        /// Generates a redirect response using command data.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="url"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static ICommandResponse RedirectResponse(this ICommand command, string url, HttpStatusCode code = HttpStatusCode.Redirect) => new CommandResponse<string>(
            300 < (int)code && (int)code < 400 ? code : HttpStatusCode.Redirect, url);

        /// <summary>
        /// Generates a generic response using command data.
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="command"></param>
        /// <param name="code"></param>
        /// <param name="body"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static ICommandResponse Response<TObject>(this ICommand command, HttpStatusCode code, TObject body, IEnumerable<string> errors) => new CommandResponse<TObject>(code, body, errors);
    }
}
