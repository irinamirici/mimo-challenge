using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Mimo.Api.Dtos
{
    public class Result
    {
        public bool Success { get; private set; }
        public IEnumerable<ResultError> Errors { get; private set; }


        public HttpStatusCode? StatusCode { get; private set; }

        public string ErrorsDescription
        {
            get { return string.Join(" ", Errors.Select(error => error.ErrorDescription)); }
        }


        public bool Failure
        {
            get { return !Success; }
        }

        protected Result(bool success, IEnumerable<ResultError> errors)
        {
            Success = success;
            Errors = errors;
        }

        protected Result(bool success, IEnumerable<ResultError> errors, HttpStatusCode? statusCode) : this(success, errors)
        {
            StatusCode = statusCode;
        }

        public static Result Fail(ResultError error)
        {
            return new Result(false, new List<ResultError>
            {
               error
            });
        }

        public static Result Fail(ResultError error, HttpStatusCode statusCode)
        {
            return new Result(false, new List<ResultError>
            {
               error
            },
            statusCode);
        }

        public static Result Fail(IEnumerable<ResultError> errors)
        {
            return new Result(false, errors);
        }
        
        public static Result<T> Fail<T>(IEnumerable<ResultError> errors)
        {
            return new Result<T>(default(T), false, errors);
        }

        public static Result<T> Fail<T>(IEnumerable<ResultError> errors, HttpStatusCode statusCode)
        {
            return new Result<T>(default(T), false, errors, statusCode);
        }

        public static Result<T> Fail<T>(ResultError error)
        {
            return new Result<T>(default(T), false, new List<ResultError>
            {
               error
            });
        }

        public static Result<T> Fail<T>(ResultError error, HttpStatusCode statusCode)
        {
            return new Result<T>(default(T), false, new List<ResultError>
            {
               error
            }, statusCode);
        }

        public static Result<T> Fail<T>(IEnumerable<ResultError> errors, HttpStatusCode? httpStatusCode)
        {
            return new Result<T>(default(T), false, errors, httpStatusCode);
        }

        public static Result Ok()
        {
            return new Result(true, null);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, null);
        }

        public static Result<T> Ok<T>(T value, HttpStatusCode httpStatusCode)
        {
            return new Result<T>(value, true, null, httpStatusCode);
        }

        public static Result Combine(params Result[] results)
        {
            foreach (Result result in results)
            {
                if (result.Failure)
                    return result;
            }

            return Ok();
        }
    }

    public class Result<T> : Result
    {
        public T Value { get; set; }


        protected internal Result(T value, bool success, IEnumerable<ResultError> errors)
            : base(success, errors)
        {
            Value = value;
        }

        protected internal Result(T value, bool success, IEnumerable<ResultError> errors, HttpStatusCode? httpStatusCode)
            : base(success, errors, httpStatusCode)
        {
            Value = value;
        }

    }
}
