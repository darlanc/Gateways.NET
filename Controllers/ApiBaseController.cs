using Gateways.NET.Contracts;
using Gateways.NET.Core;
using Gateways.NET.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Gateways.NET.Controllers
{
	/// <summary>
	/// Base Api Controller
	/// </summary>
	[ApiController]
	[Produces("application/json")]
	[Route("[controller]")]
	public abstract class ApiBaseController : ControllerBase
	{
		/// <summary>
		/// Set response headers
		/// </summary>
		/// <param name="headers"></param>
		/// <param name="contentType"></param>
		/// <param name="status"></param>
		private void SetHeaders(HeaderDictionary headers, string contentType, int status)
		{
			Response.Headers.Add("Content-Type", contentType);
			Response.StatusCode = status;
			Response.Headers.Add("Status", status.ToString());

			if (headers != null && headers.Count > 0)
			{
				foreach (var header in headers)
					Response.Headers.Add(header.Key, header.Value);
			}
		}

		/// <summary>
		/// Create an API Response
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="payload">Respose Data</param>
		/// <param name="status">Http Status Code</param>
		/// <param name="errors">Error List</param>
		/// <param name="contentType">Reponse Content Type</param>
		/// <param name="headers">Response Headers <see cref="HeaderDictionary"/></param>
		/// <param name="paging">Pagination Info. <see cref="Pagination"/></param>		
		/// <returns><see cref="ApiResponse"/></returns>
		protected ApiResponse<T> Respond<T>(
			object payload = null,
			int status = StatusCodes.Status200OK,
			IEnumerable<Error> errors = null,
			string contentType = "application/json",
			HeaderDictionary headers = null,
			Pagination paging = null)
		{
			var res = new ApiResponse<T>
			{
				Status = status,
			};

			if (errors != null)
				res.Errors = errors;			

			if (payload != null)
				res.Payload = (T)payload;			

			SetHeaders(headers, contentType, status);

			return res;
		}

		protected PaginatedApiResponse<T> RespondPaginated<T>(object payload = null,
			int status = StatusCodes.Status200OK,
			IEnumerable<Error> errors = null,
			string contentType = "application/json",
			HeaderDictionary headers = null,
			Pagination paging = null)
        {
			var res = new PaginatedApiResponse<T>
			{
				Status = status,
			};

			if (errors != null)
				res.Errors = errors;

			if (payload != null)
				res.Payload = (T)payload;

			if (paging != null)
				res.Paging = paging;

			SetHeaders(headers, contentType, status);

			return res;
		}

		/// <summary>
		/// Create an Api Response
		/// </summary>
		/// <param name="status">Http Status Code</param>
		/// <param name="errors">Error List</param>
		/// <param name="contentType">Response Content Type</param>
		/// <param name="headers">Response Headers <see cref="HeaderDictionary"/></param>
		/// <returns><see cref="ApiResponse"/></returns>
		protected ApiResponse Respond(
			int status = StatusCodes.Status200OK,
			IEnumerable<Error> errors = null,
			string contentType = "application/json",
			HeaderDictionary headers = null)
		{
			var res = new ApiResponse
			{
				Status = status
			};

			if (errors != null)
				res.Errors = errors;

			SetHeaders(headers, contentType, status);

			return res;
		}		

		/// <summary>
		/// Returns a Generic Paginated Response.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <returns></returns>
		protected ApiResponse<IEnumerable<T>> PaginatedResponse<T>(PaginatedList<T> items)
		{
			return RespondPaginated<IEnumerable<T>>(				
				payload: items,
				paging: items.Paging);
		}

		/// <summary>
		/// Return an Error response from an error messages collection.
		/// </summary>
		/// <param name="errors"></param>
		/// <param name="status"></param>
		/// <returns></returns>
		protected ApiResponse Error(IEnumerable<string> errors, int status = StatusCodes.Status400BadRequest)
		{
			return Respond(status: status, errors: errors?.Select(e => new Error
			{
				Message = e
			}));
		}

		/// <summary>
		/// Return an Error response from an error message.
		/// </summary>
		/// <param name="errors"></param>
		/// <param name="status"></param>
		/// <returns></returns>
		protected ApiResponse Error(string error, int status = StatusCodes.Status400BadRequest)
		{
			return Respond(status: status, errors: new List<Error> { new Error { Message = error } });
		}

		/// <summary>
		/// Return a Generic Error response from an error messages collection.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="errors"></param>
		/// <param name="status"></param>
		/// <returns></returns>
		protected ApiResponse<T> Error<T>(IEnumerable<string> errors, int status = StatusCodes.Status400BadRequest)
		{
			return Respond<T>(status: status, errors: errors?.Select(e => new Error
			{
				Message = e
			}));
		}

		/// <summary>
		/// Return a Generic Error response from an Error collection.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="errors"></param>
		/// <param name="status"></param>
		/// <returns></returns>
		protected ApiResponse<T> Error<T>(IEnumerable<Error> errors, int status = StatusCodes.Status400BadRequest) => Respond<T>(status: status, errors: errors);

		/// <summary>
		/// Return a Generic Error response from an error message
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="error"></param>
		/// <param name="status"></param>
		/// <returns></returns>
		protected ApiResponse<T> Error<T>(string error, int status = StatusCodes.Status400BadRequest)
		{
			return Respond<T>(status: status, errors: new List<Error> { new Error { Message = error } });
		}				
	}
}
