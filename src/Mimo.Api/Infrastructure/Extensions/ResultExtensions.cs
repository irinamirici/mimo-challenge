using Mimo.Api.Dtos;
using System;

namespace Mimo.Api.Infrastructure.Extensions
{
    public static class ResultExtensions
    {
        public static Result OnSuccess(this Result result, Func<Result> func)
        {
            if (result.Failure)
                return result;

            return func();
        }

        public static Result OnSuccess(this Result result, Action action)
        {
            if (result.Failure)
                return result;

            action();

            return Result.Ok();
        }

        public static Result OnSuccess<T>(this Result<T> result, Action<T> action)
        {
            if (result.Failure)
                return result;

            action(result.Value);

            return Result.Ok();
        }

        public static Result<T> OnSuccess<T>(this Result result, Func<T> func)
        {
            if (result.Failure)
                return Result.Fail<T>(result.Errors, result.StatusCode);

            return Result.Ok(func());
        }

        public static Result<T> OnSuccess<T>(this Result result, Func<Result<T>> func)
        {
            if (result.Failure)
                return Result.Fail<T>(result.Errors, result.StatusCode);

            return func();
        }

        public static Result<R> OnSuccess<T, R>(this Result<T> result, Func<T, Result<R>> func)
        {
            if (result.Failure)
                return Result.Fail<R>(result.Errors, result.StatusCode);

            return func(result.Value);
        }

        public static Result<T> OnSuccess<T>(this Result<T> result, Func<Result<T>, Result<T>> func)
        {
            if (result.Failure)
                return Result.Fail<T>(result.Errors, result.StatusCode);

            return func(result);
        }

        public static Result OnFailure(this Result result, Action action)
        {
            if (result.Failure)
            {
                action();
            }

            return result;
        }

        public static Result<T> OnFailure<T>(this Result<T> result, Func<Result<T>, Result<T>> func)
        {
            if (result.Failure)
                return func(result);

            return result;
        }

        public static T OnBoth<T>(this Result result, Func<Result, T> func)
        {
            return func(result);
        }
        public static R OnBoth<T, R>(this Result<T> result, Func<Result<T>, R> func)
        {
            return func(result);
        }
    }
}
